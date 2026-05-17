using System.Data;
using System.Data.SqlClient;
using ENB_project.Controls;
using Microsoft.Data.SqlClient;

namespace ENB_project
{
    // ══════════════════════════════════════════════════════
    //  Доменные модели (без изменений, только перенесены сюда)
    // ══════════════════════════════════════════════════════

    public class User
    {
        public required string Login    { get; init; }
        public required string Password { get; set; }
        public required string Email    { get; set; }
        public required string Theme    { get; set; }
        public required string Language { get; set; }
        public byte[]?         Avatar   { get; set; }
        public FileSystemTree  Tree     { get; set; } = new();
    }

    public class Category
    {
        public int    Id    { get; set; }
        public string Name  { get; set; } = string.Empty;
        public string Color { get; set; } = "#7C6FCD";
    }

    public class SearchEfResult
    {
        public string  Name          { get; set; } = string.Empty;
        public string? CategoryColor { get; set; }
    }

    // ══════════════════════════════════════════════════════
    //  UserList — репозиторий поверх ADO.NET
    // ══════════════════════════════════════════════════════

    /// <summary>
    /// Полная замена EF-репозитория. Весь доступ к БД —
    /// через ADO.NET (SqlCommand, SqlDataReader, транзакции).
    /// Хранимые процедуры вызываются там, где это оговорено в ТЗ.
    /// </summary>
    public class UserList
    {
        // ── Пользователи ──────────────────────────────────

        /// <summary>
        /// Получить пользователя по логину вместе с деревом файлов.
        /// Использует хранимую процедуру sp_GetUser.
        /// </summary>
        public User? GetUser(string login)
        {
            using var conn = Database.OpenConnection();
            using var cmd  = new SqlCommand("sp_GetUser", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Login", login);

            using var reader = cmd.ExecuteReader();
            if (!reader.Read()) return null;

            var user = new User
            {
                Login    = reader.GetString(reader.GetOrdinal("Login")),
                Password = reader.GetString(reader.GetOrdinal("Password")),
                Email    = reader.GetString(reader.GetOrdinal("Email")),
                Theme    = reader.GetString(reader.GetOrdinal("Theme")),
                Language = reader.GetString(reader.GetOrdinal("Language")),
                Avatar   = reader.IsDBNull(reader.GetOrdinal("Avatar"))
                               ? null
                               : (byte[])reader["Avatar"]
            };
            var userId = reader.GetInt32(reader.GetOrdinal("Id"));
            reader.Close();

            user.Tree = LoadTree(conn, userId);
            return user;
        }

        /// <summary>
        /// Добавить пользователя. Использует хранимую процедуру sp_AddUser.
        /// Возвращает false если логин уже занят.
        /// </summary>
        public bool AddUser(string login, string password,
            string email, string theme, string language)
        {
            using var conn = Database.OpenConnection();
            using var cmd  = new SqlCommand("AddUser", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Login",    login);
            cmd.Parameters.AddWithValue("@Password", password);
            cmd.Parameters.AddWithValue("@Email",    email);
            cmd.Parameters.AddWithValue("@Theme",    theme);
            cmd.Parameters.AddWithValue("@Language", language);

            using var reader = cmd.ExecuteReader();
            if (!reader.Read()) return false;
            return reader.GetInt32(0) == 1;
        }

        /// <summary>
        /// Удалить пользователя. Каскадное удаление обрабатывается на уровне БД.
        /// </summary>
        public void DelUser(string login)
        {
            using var conn = Database.OpenConnection();
            using var tx   = conn.BeginTransaction();
            try
            {
                using var cmd = new SqlCommand(
                    "DELETE FROM Users WHERE Login = @Login", conn, tx);
                cmd.Parameters.AddWithValue("@Login", login);
                cmd.ExecuteNonQuery();
                tx.Commit();
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Изменить одно поле пользователя. Использует хранимую процедуру sp_EditUser.
        /// </summary>
        public bool EditUser(string login, string propertyName, string value)
        {
            using var conn = Database.OpenConnection();
            using var cmd  = new SqlCommand("sp_EditUser", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Login", login);
            cmd.Parameters.AddWithValue("@Field", propertyName);
            cmd.Parameters.AddWithValue("@Value", value);
            cmd.ExecuteNonQuery();
            return true;
        }

        /// <summary>
        /// Сохранить аватар пользователя (графическое поле).
        /// Использует хранимую процедуру sp_SaveAvatar + транзакцию.
        /// </summary>
        public void SaveAvatar(string login, byte[] avatarBytes)
        {
            using var conn = Database.OpenConnection();
            using var tx   = conn.BeginTransaction();
            try
            {
                using var cmd = new SqlCommand("sp_SaveAvatar", conn, tx)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Login",  login);
                var p = cmd.Parameters.AddWithValue("@Avatar", avatarBytes);
                p.SqlDbType = SqlDbType.VarBinary;
                cmd.ExecuteNonQuery();
                tx.Commit();
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

        // ── Дерево файлов ─────────────────────────────────

        public FileSystemTree GetTree(string login)
        {
            var userId = GetUserId(login, "UserList.GetTree");
            using var conn = Database.OpenConnection();
            return LoadTree(conn, userId);
        }

        public FileSystemNode AddNodeToRoot(string login, string nodeName,
            FileItemType type = FileItemType.File)
        {
            var userId    = GetUserId(login, "UserList.AddNodeToRoot");
            var sortOrder = CountRoots(userId);
            return CreateNode(userId, null, nodeName, type, sortOrder);
        }

        public FileSystemNode AddNodeToFolder(string login,
            FileSystemNode parent, string nodeName,
            FileItemType type = FileItemType.File)
        {
            var userId   = GetUserId(login, "UserList.AddNodeToFolder");
            var parentId = GetNodeId(parent.Name, userId, "UserList.AddNodeToFolder");
            var sortOrder = CountChildren(parentId);
            return CreateNode(userId, parentId, nodeName, type, sortOrder);
        }

        public FileSystemNode InsertNode(string login,
            FileSystemNode parent, int index, string nodeName,
            FileItemType type = FileItemType.File)
        {
            var userId   = GetUserId(login, "UserList.InsertNode");
            var parentId = GetNodeId(parent.Name, userId, "UserList.InsertNode");
            return CreateNode(userId, parentId, nodeName, type, index);
        }

        public bool RemoveNode(string login, FileSystemNode node)
        {
            var userId = GetUserId(login, "UserList.RemoveNode");
            using var conn = Database.OpenConnection();
            using var tx   = conn.BeginTransaction();
            try
            {
                // Триггер trg_DeleteFolderChildren выполнит рекурсивное удаление
                using var cmd = new SqlCommand(
                    "DELETE FROM FileSystemNodes WHERE Name = @Name AND UserId = @UserId",
                    conn, tx);
                cmd.Parameters.AddWithValue("@Name",   node.Name);
                cmd.Parameters.AddWithValue("@UserId", userId);
                var affected = cmd.ExecuteNonQuery();
                tx.Commit();
                return affected > 0;
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

        public void MoveNode(string login, FileSystemNode node, FileSystemNode? newParent)
        {
            var userId = GetUserId(login, "UserList.MoveNode");
            using var conn = Database.OpenConnection();
            using var tx   = conn.BeginTransaction();
            try
            {
                int? newParentId = null;
                int  sortOrder   = CountRoots(userId);

                if (newParent != null)
                {
                    newParentId = GetNodeId(newParent.Name, userId, "UserList.MoveNode");
                    sortOrder   = CountChildren(newParentId.Value);
                }

                using var cmd = new SqlCommand(
                    @"UPDATE FileSystemNodes
                      SET    ParentId  = @ParentId,
                             SortOrder = @SortOrder
                      WHERE  Name   = @Name
                        AND  UserId = @UserId",
                    conn, tx);

                cmd.Parameters.AddWithValue("@ParentId",  (object?)newParentId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@SortOrder", sortOrder);
                cmd.Parameters.AddWithValue("@Name",      node.Name);
                cmd.Parameters.AddWithValue("@UserId",    userId);
                cmd.ExecuteNonQuery();
                tx.Commit();
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

        public FileSystemNode? FindNode(string login, string nodeName)
        {
            var userId = GetUserId(login, "UserList.FindNode");
            using var conn = Database.OpenConnection();
            using var cmd  = new SqlCommand(
                @"SELECT n.Id, n.Name, n.ItemType, n.CreateTime, n.EditTime,
                         n.CategoryId, cat.Color AS CategoryColor,
                         nc.Content
                  FROM   FileSystemNodes n
                  LEFT   JOIN Categories cat ON cat.Id = n.CategoryId
                  LEFT   JOIN NoteContents nc ON nc.NodeId = n.Id
                  WHERE  n.Name = @Name AND n.UserId = @UserId",
                conn);
            cmd.Parameters.AddWithValue("@Name",   nodeName);
            cmd.Parameters.AddWithValue("@UserId", userId);

            using var reader = cmd.ExecuteReader();
            if (!reader.Read()) return null;

            return new FileSystemNode(
                reader.GetString(reader.GetOrdinal("Name")),
                reader.GetString(reader.GetOrdinal("ItemType")) == "Folder"
                    ? FileItemType.Folder : FileItemType.File)
            {
                Content       = reader.IsDBNull(reader.GetOrdinal("Content"))
                                    ? null : reader.GetString(reader.GetOrdinal("Content")),
                CategoryId    = reader.IsDBNull(reader.GetOrdinal("CategoryId"))
                                    ? null : reader.GetInt32(reader.GetOrdinal("CategoryId")),
                CategoryColor = reader.IsDBNull(reader.GetOrdinal("CategoryColor"))
                                    ? null : reader.GetString(reader.GetOrdinal("CategoryColor")),
                CreateTime    = DateOnly.FromDateTime(
                                    reader.GetDateTime(reader.GetOrdinal("CreateTime"))),
                EditTime      = DateOnly.FromDateTime(
                                    reader.GetDateTime(reader.GetOrdinal("EditTime")))
            };
        }

        public void ClearTree(string login)
        {
            var userId = GetUserId(login, "UserList.ClearTree");
            using var conn = Database.OpenConnection();
            using var tx   = conn.BeginTransaction();
            try
            {
                using var cmd = new SqlCommand(
                    "DELETE FROM FileSystemNodes WHERE UserId = @UserId AND ParentId IS NULL",
                    conn, tx);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.ExecuteNonQuery();
                tx.Commit();
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Асинхронное сохранение контента заметки в транзакции.
        /// Триггер trg_UpdateEditTime автоматически обновит EditTime.
        /// </summary>
        public async Task SaveNoteContentAsync(string login, string nodeName, string content)
        {
            var userId = GetUserId(login, "UserList.SaveNoteContentAsync");

            await using var conn = await Database.OpenConnectionAsync();
            await using var tx   = (SqlTransaction)await conn.BeginTransactionAsync();
            try
            {
                // Получаем nodeId
                int nodeId;
                await using (var cmd = new SqlCommand(
                    "SELECT Id FROM FileSystemNodes WHERE Name = @Name AND UserId = @UserId",
                    conn, tx))
                {
                    cmd.Parameters.AddWithValue("@Name",   nodeName);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    var result = await cmd.ExecuteScalarAsync();
                    if (result == null) { await tx.RollbackAsync(); return; }
                    nodeId = (int)result;
                }

                // UPSERT NoteContents
                await using (var cmd = new SqlCommand(
                    @"IF EXISTS (SELECT 1 FROM NoteContents WHERE NodeId = @NodeId)
                          UPDATE NoteContents SET Content = @Content WHERE NodeId = @NodeId
                      ELSE
                          INSERT INTO NoteContents (NodeId, Content) VALUES (@NodeId, @Content)",
                    conn, tx))
                {
                    cmd.Parameters.AddWithValue("@NodeId",  nodeId);
                    cmd.Parameters.AddWithValue("@Content", content);
                    await cmd.ExecuteNonQueryAsync();
                }

                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        public void RenameNode(string login, string oldName, string newName)
        {
            var userId = GetUserId(login, "UserList.RenameNode");
            using var conn = Database.OpenConnection();
            using var tx   = conn.BeginTransaction();
            try
            {
                using var cmd = new SqlCommand(
                    "UPDATE FileSystemNodes SET Name = @NewName WHERE Name = @OldName AND UserId = @UserId",
                    conn, tx);
                cmd.Parameters.AddWithValue("@NewName", newName);
                cmd.Parameters.AddWithValue("@OldName", oldName);
                cmd.Parameters.AddWithValue("@UserId",  userId);
                cmd.ExecuteNonQuery();
                tx.Commit();
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

        public void SetNodeCategory(string login, string nodeName, int? categoryId)
        {
            var userId = GetUserId(login, "UserList.SetNodeCategory");
            using var conn = Database.OpenConnection();
            using var tx   = conn.BeginTransaction();
            try
            {
                using var cmd = new SqlCommand(
                    "UPDATE FileSystemNodes SET CategoryId = @CatId WHERE Name = @Name AND UserId = @UserId",
                    conn, tx);
                cmd.Parameters.AddWithValue("@CatId",  (object?)categoryId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Name",   nodeName);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.ExecuteNonQuery();
                tx.Commit();
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Поиск заметок через хранимую процедуру sp_SearchNotes (асинхронный).
        /// </summary>
        public async Task<List<SearchEfResult>> SearchNotesAsync(string login, string query)
        {
            var results = new List<SearchEfResult>();

            await using var conn = await Database.OpenConnectionAsync();
            await using var cmd  = new SqlCommand("sp_SearchNotes", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Login", login);
            cmd.Parameters.AddWithValue("@Query", query.Trim());

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                results.Add(new SearchEfResult
                {
                    Name          = reader.GetString(reader.GetOrdinal("Name")),
                    CategoryColor = reader.IsDBNull(reader.GetOrdinal("CategoryColor"))
                                        ? null
                                        : reader.GetString(reader.GetOrdinal("CategoryColor"))
                });
            }

            return results;
        }

        // ── Категории ──────────────────────────────────────

        /// <summary>
        /// Получить категории через хранимую процедуру sp_GetCategories.
        /// </summary>
        public List<Category> GetCategories(string login)
        {
            var result = new List<Category>();

            using var conn = Database.OpenConnection();
            using var cmd  = new SqlCommand("sp_GetCategories", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Login", login);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new Category
                {
                    Id    = reader.GetInt32(reader.GetOrdinal("Id")),
                    Name  = reader.GetString(reader.GetOrdinal("Name")),
                    Color = reader.GetString(reader.GetOrdinal("Color"))
                });
            }

            return result;
        }

        public Category AddCategory(string login, string name, string color)
        {
            var userId = GetUserId(login, "UserList.AddCategory");
            using var conn = Database.OpenConnection();
            using var tx   = conn.BeginTransaction();
            try
            {
                using var cmd = new SqlCommand(
                    @"INSERT INTO Categories (UserId, Name, Color)
                      OUTPUT INSERTED.Id
                      VALUES (@UserId, @Name, @Color)",
                    conn, tx);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@Name",   name);
                cmd.Parameters.AddWithValue("@Color",  color);
                var newId = (int)cmd.ExecuteScalar()!;
                tx.Commit();
                return new Category { Id = newId, Name = name, Color = color };
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

        public bool EditCategory(int categoryId, string name, string color)
        {
            using var conn = Database.OpenConnection();
            using var tx   = conn.BeginTransaction();
            try
            {
                using var cmd = new SqlCommand(
                    "UPDATE Categories SET Name = @Name, Color = @Color WHERE Id = @Id",
                    conn, tx);
                cmd.Parameters.AddWithValue("@Name",  name);
                cmd.Parameters.AddWithValue("@Color", color);
                cmd.Parameters.AddWithValue("@Id",    categoryId);
                var affected = cmd.ExecuteNonQuery();
                tx.Commit();
                return affected > 0;
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

        public bool DeleteCategory(int categoryId)
        {
            using var conn = Database.OpenConnection();
            using var tx   = conn.BeginTransaction();
            try
            {
                using var cmd = new SqlCommand(
                    "DELETE FROM Categories WHERE Id = @Id",
                    conn, tx);
                cmd.Parameters.AddWithValue("@Id", categoryId);
                var affected = cmd.ExecuteNonQuery();
                tx.Commit();
                return affected > 0;
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

        // ── AdminPanel запросы ────────────────────────────

        /// <summary>
        /// Получить всех пользователей для таблицы AdminPanel.
        /// Использует хранимую процедуру sp_GetAllUsers с параметрами сортировки.
        /// </summary>
        public DataTable GetAllUsers(string sortColumn = "Login", string sortDir = "ASC")
        {
            using var conn = Database.OpenConnection();
            using var cmd  = new SqlCommand("sp_GetAllUsers", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@SortColumn", sortColumn);
            cmd.Parameters.AddWithValue("@SortDir",    sortDir);

            var dt     = new DataTable();
            using var adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dt);
            return dt;
        }

        /// <summary>
        /// Получить все узлы пользователя для AdminPanel.
        /// Использует хранимую процедуру sp_GetAllNodes.
        /// </summary>
        public DataTable GetAllNodes(string login,
            string sortColumn = "Name", string sortDir = "ASC")
        {
            using var conn = Database.OpenConnection();
            using var cmd  = new SqlCommand("sp_GetAllNodes", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Login",      login);
            cmd.Parameters.AddWithValue("@SortColumn", sortColumn);
            cmd.Parameters.AddWithValue("@SortDir",    sortDir);

            var dt = new DataTable();
            using var adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dt);
            return dt;
        }

        /// <summary>
        /// Получить аватар пользователя.
        /// </summary>
        public byte[]? GetAvatar(string login)
        {
            using var conn = Database.OpenConnection();
            using var cmd  = new SqlCommand(
                "SELECT Avatar FROM Users WHERE Login = @Login", conn);
            cmd.Parameters.AddWithValue("@Login", login);
            var result = cmd.ExecuteScalar();
            return result == DBNull.Value || result == null ? null : (byte[])result;
        }

        // ── Вспомогательные методы ────────────────────────

        private static int GetUserId(string login, string location)
        {
            using var conn = Database.OpenConnection();
            using var cmd  = new SqlCommand(
                "SELECT Id FROM Users WHERE Login = @Login", conn);
            cmd.Parameters.AddWithValue("@Login", login);
            var result = cmd.ExecuteScalar();
            if (result == null)
                throw MyExceptions.Navigation(
                    $"Пользователь «{login}» не найден", location);
            return (int)result;
        }

        private static int GetNodeId(string name, int userId, string location)
        {
            using var conn = Database.OpenConnection();
            using var cmd  = new SqlCommand(
                "SELECT Id FROM FileSystemNodes WHERE Name = @Name AND UserId = @UserId",
                conn);
            cmd.Parameters.AddWithValue("@Name",   name);
            cmd.Parameters.AddWithValue("@UserId", userId);
            var result = cmd.ExecuteScalar();
            if (result == null)
                throw MyExceptions.Navigation($"Узел «{name}» не найден", location);
            return (int)result;
        }

        private static int CountRoots(int userId)
        {
            using var conn = Database.OpenConnection();
            using var cmd  = new SqlCommand(
                "SELECT COUNT(*) FROM FileSystemNodes WHERE UserId = @UserId AND ParentId IS NULL",
                conn);
            cmd.Parameters.AddWithValue("@UserId", userId);
            return (int)cmd.ExecuteScalar()!;
        }

        private static int CountChildren(int parentId)
        {
            using var conn = Database.OpenConnection();
            using var cmd  = new SqlCommand(
                "SELECT COUNT(*) FROM FileSystemNodes WHERE ParentId = @ParentId",
                conn);
            cmd.Parameters.AddWithValue("@ParentId", parentId);
            return (int)cmd.ExecuteScalar()!;
        }

        /// <summary>
        /// Создаёт узел в БД внутри транзакции.
        /// Для файлов сразу создаёт NoteContent.
        /// </summary>
        private static FileSystemNode CreateNode(
            int userId, int? parentId, string name, FileItemType type, int sortOrder)
        {
            using var conn = Database.OpenConnection();
            using var tx   = conn.BeginTransaction();
            try
            {
                var today    = DateTime.Today;
                var itemType = type == FileItemType.Folder ? "Folder" : "File";

                int nodeId;
                using (var cmd = new SqlCommand(
                    @"INSERT INTO FileSystemNodes
                        (UserId, ParentId, Name, ItemType, SortOrder, CreateTime, EditTime)
                      OUTPUT INSERTED.Id
                      VALUES
                        (@UserId, @ParentId, @Name, @ItemType, @SortOrder, @Today, @Today)",
                    conn, tx))
                {
                    cmd.Parameters.AddWithValue("@UserId",    userId);
                    cmd.Parameters.AddWithValue("@ParentId",  (object?)parentId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Name",      name);
                    cmd.Parameters.AddWithValue("@ItemType",  itemType);
                    cmd.Parameters.AddWithValue("@SortOrder", sortOrder);
                    cmd.Parameters.AddWithValue("@Today",     today);
                    nodeId = (int)cmd.ExecuteScalar()!;
                }

                if (type == FileItemType.File)
                {
                    using var cmd2 = new SqlCommand(
                        "INSERT INTO NoteContents (NodeId, Content) VALUES (@NodeId, '')",
                        conn, tx);
                    cmd2.Parameters.AddWithValue("@NodeId", nodeId);
                    cmd2.ExecuteNonQuery();
                }

                tx.Commit();

                var dateOnly = DateOnly.FromDateTime(today);
                return new FileSystemNode(name, type)
                {
                    CreateTime = dateOnly,
                    EditTime   = dateOnly
                };
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Загружает FileSystemTree из БД, восстанавливает Parent-ссылки в памяти.
        /// Одним запросом получает узлы + контент + цвета категорий.
        /// </summary>
        private static FileSystemTree LoadTree(SqlConnection conn, int userId)
        {
            // Один запрос с JOIN — меньше round-trips
            using var cmd = new SqlCommand(
                @"SELECT n.Id, n.ParentId, n.Name, n.ItemType, n.SortOrder,
                         n.CreateTime, n.EditTime,
                         n.CategoryId, cat.Color AS CategoryColor,
                         nc.Content
                  FROM   FileSystemNodes n
                  LEFT   JOIN Categories cat ON cat.Id   = n.CategoryId
                  LEFT   JOIN NoteContents nc ON nc.NodeId = n.Id
                  WHERE  n.UserId = @UserId
                  ORDER  BY n.SortOrder",
                conn);
            cmd.Parameters.AddWithValue("@UserId", userId);

            var entities = new List<(int Id, int? ParentId, FileSystemNode Node)>();

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var node = new FileSystemNode(
                        reader.GetString(reader.GetOrdinal("Name")),
                        reader.GetString(reader.GetOrdinal("ItemType")) == "Folder"
                            ? FileItemType.Folder : FileItemType.File)
                    {
                        Content       = reader.IsDBNull(reader.GetOrdinal("Content"))
                                            ? null : reader.GetString(reader.GetOrdinal("Content")),
                        CategoryId    = reader.IsDBNull(reader.GetOrdinal("CategoryId"))
                                            ? null : reader.GetInt32(reader.GetOrdinal("CategoryId")),
                        CategoryColor = reader.IsDBNull(reader.GetOrdinal("CategoryColor"))
                                            ? null : reader.GetString(reader.GetOrdinal("CategoryColor")),
                        CreateTime    = DateOnly.FromDateTime(
                                            reader.GetDateTime(reader.GetOrdinal("CreateTime"))),
                        EditTime      = DateOnly.FromDateTime(
                                            reader.GetDateTime(reader.GetOrdinal("EditTime")))
                    };

                    int  id       = reader.GetInt32(reader.GetOrdinal("Id"));
                    int? parentId = reader.IsDBNull(reader.GetOrdinal("ParentId"))
                                        ? null : reader.GetInt32(reader.GetOrdinal("ParentId"));

                    entities.Add((id, parentId, node));
                }
            }

            var nodeMap = entities.ToDictionary(e => e.Id, e => e.Node);
            var tree    = new FileSystemTree();

            foreach (var (id, parentId, node) in entities)
            {
                if (parentId == null)
                    tree.Roots.Add(node);
                else if (nodeMap.TryGetValue(parentId.Value, out var parent))
                {
                    node.Parent = parent;
                    parent.Children.Add(node);
                }
            }

            return tree;
        }
    }
}