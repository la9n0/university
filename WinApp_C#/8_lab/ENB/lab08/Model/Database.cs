// Database.cs

using System.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Windows;

namespace ENB_project
{
    public static class Database
    {
        private static string? _connectionString;

        public static string ConnectionString
        {
            get
            {
                if (_connectionString != null)
                    return _connectionString;

                var cs = ConfigurationManager
                    .ConnectionStrings["ENB"]?.ConnectionString;

                if (string.IsNullOrWhiteSpace(cs))
                    throw new InvalidOperationException(
                        "Строка подключения ENB не найдена");

                _connectionString = cs;
                return cs;
            }
        }

        private static string MasterConnectionString
        {
            get
            {
                var builder = new SqlConnectionStringBuilder(ConnectionString)
                {
                    InitialCatalog = "master"
                };

                return builder.ConnectionString;
            }
        }

        public static void EnsureCreated()
        {
            try
            {
                EnsureDatabase();
                EnsureTables();
                EnsureStoredProcedures();
                EnsureTriggers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ошибка инициализации БД:\n{ex.Message}",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                throw;
            }
        }

        private static void EnsureDatabase()
        {
            var dbName =
                new SqlConnectionStringBuilder(ConnectionString)
                    .InitialCatalog;

            using var conn = new SqlConnection(MasterConnectionString);
            conn.Open();

            using var cmd = conn.CreateCommand();

            cmd.CommandText = $@"
IF NOT EXISTS (
    SELECT * FROM sys.databases
    WHERE name = N'{dbName}'
)
BEGIN
    CREATE DATABASE [{dbName}]
END";

            cmd.ExecuteNonQuery();
        }

        private static void EnsureTables()
        {
            using var conn = new SqlConnection(ConnectionString);
            conn.Open();

            using var cmd = conn.CreateCommand();

            // USERS

            cmd.CommandText = @"
IF NOT EXISTS (
    SELECT * FROM sys.tables
    WHERE name = 'Users'
)
BEGIN
    CREATE TABLE Users
    (
        Id       INT IDENTITY(1,1) PRIMARY KEY,
        Login    NVARCHAR(25)  NOT NULL UNIQUE,
        Password NVARCHAR(30)  NOT NULL,
        Email    NVARCHAR(255) NOT NULL,
        Theme    NVARCHAR(20)  NOT NULL DEFAULT 'Dark',
        Language NVARCHAR(10)  NOT NULL DEFAULT 'ru',
        Avatar   VARBINARY(MAX) NULL
    )
END";

            cmd.ExecuteNonQuery();

            // CATEGORIES

            cmd.CommandText = @"
IF NOT EXISTS (
    SELECT * FROM sys.tables
    WHERE name = 'Categories'
)
BEGIN
    CREATE TABLE Categories
    (
        Id     INT IDENTITY(1,1) PRIMARY KEY,
        UserId INT NOT NULL,
        Name   NVARCHAR(50) NOT NULL,
        Color  NVARCHAR(7) NOT NULL DEFAULT '#7C6FCD',

        CONSTRAINT FK_Categories_Users
            FOREIGN KEY (UserId)
            REFERENCES Users(Id)
            ON DELETE CASCADE
    )
END";

            cmd.ExecuteNonQuery();

            // FILESYSTEMNODES

            cmd.CommandText = @"
IF NOT EXISTS (
    SELECT * FROM sys.tables
    WHERE name = 'FileSystemNodes'
)
BEGIN
    CREATE TABLE FileSystemNodes
    (
        Id         INT IDENTITY(1,1) PRIMARY KEY,

        UserId     INT NOT NULL,

        ParentId   INT NULL,

        CategoryId INT NULL,

        Name       NVARCHAR(255) NOT NULL,

        ItemType   NVARCHAR(10) NOT NULL DEFAULT 'File',

        SortOrder  INT NOT NULL DEFAULT 0,

        CreateTime DATE NOT NULL,

        EditTime   DATE NOT NULL,

        CONSTRAINT FK_FileSystemNodes_User
            FOREIGN KEY (UserId)
            REFERENCES Users(Id)
            ON DELETE CASCADE,

        CONSTRAINT FK_FileSystemNodes_Parent
            FOREIGN KEY (ParentId)
            REFERENCES FileSystemNodes(Id),

        CONSTRAINT FK_FileSystemNodes_Category
            FOREIGN KEY (CategoryId)
            REFERENCES Categories(Id)
    )
END";

            cmd.ExecuteNonQuery();

            // NOTECONTENTS

            cmd.CommandText = @"
IF NOT EXISTS (
    SELECT * FROM sys.tables
    WHERE name = 'NoteContents'
)
BEGIN
    CREATE TABLE NoteContents
    (
        Id      INT IDENTITY(1,1) PRIMARY KEY,

        NodeId  INT NOT NULL UNIQUE,

        Content NVARCHAR(MAX) NOT NULL DEFAULT '',

        CONSTRAINT FK_NoteContents_Node
            FOREIGN KEY (NodeId)
            REFERENCES FileSystemNodes(Id)
            ON DELETE CASCADE
    )
END";

            cmd.ExecuteNonQuery();
        }

        private static void EnsureStoredProcedures()
        {
            using var conn = new SqlConnection(ConnectionString);
            conn.Open();

            ExecuteScript(conn, @"
CREATE OR ALTER PROCEDURE sp_GetUser
    @Login NVARCHAR(25)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        Login,
        Password,
        Email,
        Theme,
        Language,
        Avatar
    FROM Users
    WHERE Login = @Login
END");

            ExecuteScript(conn, @"
CREATE OR ALTER PROCEDURE AddUser
    @Login NVARCHAR(25),
    @Password NVARCHAR(30),
    @Email NVARCHAR(255),
    @Theme NVARCHAR(20),
    @Language NVARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1
        FROM Users
        WHERE Login = @Login
    )
    BEGIN
        SELECT 0 AS Success;
        RETURN;
    END

    INSERT INTO Users
    (
        Login,
        Password,
        Email,
        Theme,
        Language
    )
    VALUES
    (
        @Login,
        @Password,
        @Email,
        @Theme,
        @Language
    );

    SELECT 1 AS Success;
END");

            ExecuteScript(conn, @"
CREATE OR ALTER PROCEDURE sp_EditUser
    @Login NVARCHAR(25),
    @Field NVARCHAR(50),
    @Value NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    IF @Field = 'Password'
        UPDATE Users
        SET Password = @Value
        WHERE Login = @Login;

    ELSE IF @Field = 'Email'
        UPDATE Users
        SET Email = @Value
        WHERE Login = @Login;

    ELSE IF @Field = 'Theme'
        UPDATE Users
        SET Theme = @Value
        WHERE Login = @Login;

    ELSE IF @Field = 'Language'
        UPDATE Users
        SET Language = @Value
        WHERE Login = @Login;
END");

            ExecuteScript(conn, @"
CREATE OR ALTER PROCEDURE sp_SaveAvatar
    @Login NVARCHAR(25),
    @Avatar VARBINARY(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Users
    SET Avatar = @Avatar
    WHERE Login = @Login;
END");
        }

        private static void EnsureTriggers()
        {
            using var conn = new SqlConnection(ConnectionString);
            conn.Open();

            ExecuteScript(conn, @"
IF OBJECT_ID('trg_UpdateEditTime', 'TR') IS NOT NULL
    DROP TRIGGER trg_UpdateEditTime");

            ExecuteScript(conn, @"
CREATE TRIGGER trg_UpdateEditTime
ON NoteContents
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE FileSystemNodes
    SET EditTime = CAST(GETDATE() AS DATE)
    WHERE Id IN (
        SELECT NodeId
        FROM inserted
    );
END");
        }

        private static void ExecuteScript(
            SqlConnection conn,
            string sql)
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
        }

        public static SqlConnection OpenConnection()
        {
            var conn = new SqlConnection(ConnectionString);
            conn.Open();
            return conn;
        }

        public static async Task<SqlConnection> OpenConnectionAsync()
        {
            var conn = new SqlConnection(ConnectionString);

            await conn.OpenAsync();

            return conn;
        }
    }
}