using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using lab07.Controls;

namespace lab07
{
    /// <summary>
    /// Репозиторий пользователей. Загружает и сохраняет список в JSON,
    /// предоставляет методы для CRUD-операций над пользователями и их деревьями файлов.
    /// </summary>
    public class UserList
    {
        private List<User> _users = [];
        private readonly string _path;

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            WriteIndented = true,
        };

        public UserList()
        {
            var basePath = AppContext.BaseDirectory;
            _path = Path.Combine(basePath, "app_data", "userList.json");
        }

        public void SaveJson()
        {
            try
            {
                var dir = Path.GetDirectoryName(_path);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                var json = JsonSerializer.Serialize(_users, _jsonOptions);
                File.WriteAllText(_path, json);
            }
            catch (Exception ex)
            {
                throw MyExceptions.IO("Не удалось сохранить данные пользователей", "UserList.SaveJson", ex);
            }
        }

        /// <summary>
        /// Загружает список пользователей из JSON и восстанавливает Parent-ссылки
        /// в деревьях файлов (они не сериализуются).
        /// </summary>
        public void LoadJson()
        {
            try
            {
                var json     = File.ReadAllText(_path);
                var restored = JsonSerializer.Deserialize<List<User>>(json, _jsonOptions);

                _users = restored ?? throw MyExceptions.Data("Список пользователей пуст", "UserList.LoadJson");

                foreach (var user in _users)
                    RestoreParents(user.Tree.Roots, parent: null);
            }
            catch (FileNotFoundException ex)
            {
                throw MyExceptions.IO("Файл данных не найден", "UserList.LoadJson", ex);
            }
            catch (JsonException ex)
            {
                throw MyExceptions.Data("Некорректный формат JSON", "UserList.LoadJson", ex);
            }
        }

        /// <summary>
        /// Рекурсивно восстанавливает поле Parent у всех узлов дерева после десериализации.
        /// </summary>
        private static void RestoreParents(
            IEnumerable<FileSystemNode> nodes, FileSystemNode? parent)
        {
            foreach (var node in nodes)
            {
                node.Parent = parent;
                RestoreParents(node.Children, node);
            }
        }

        public User? GetUser(string username)
            => _users.Find(u => u?.Username == username);

        /// <summary>
        /// Добавляет нового пользователя. Возвращает false, если имя уже занято.
        /// </summary>
        public bool AddUser(string username, string password,
            string email, string theme, string language)
        {
            if (_users.Any(u => u?.Username == username))
                return false;

            _users.Add(new User
            {
                Username = username,
                Password = password,
                Email    = email,
                Theme    = theme,
                Language = language
            });
            return true;
        }

        public void DelUser(string name)
        {
            var user = _users.Find(u => u.Username == name);
            if (user != null)
                _users.Remove(user);
        }

        public bool EditUser(string name, string changedPropertyName, string changed)
        {
            if (GetUser(name) == null) return false;
            var index = _users.IndexOf(_users.FirstOrDefault(x => x.Username == name));

            switch (changedPropertyName)
            {
                case nameof(User.Password): _users[index].Password = changed; break;
                case nameof(User.Email):    _users[index].Email    = changed; break;
                case nameof(User.Theme):    _users[index].Theme    = changed; break;
                case nameof(User.Language): _users[index].Language = changed; break;
                default: return false;
            }
            return true;
        }

        public FileSystemTree GetTree(string username)
            => GetUser(username)?.Tree
               ?? throw MyExceptions.Navigation($"Пользователь «{username}» не найден", "UserList.GetTree");

        public FileSystemNode AddNodeToRoot(string username, string nodeName,
            FileItemType type = FileItemType.File)
            => GetUser(username)?.Tree.AddToRoot(nodeName, type)
               ?? throw MyExceptions.Navigation($"Пользователь «{username}» не найден", "UserList.AddNodeToRoot");

        public FileSystemNode AddNodeToFolder(string username,
            FileSystemNode parent, string nodeName,
            FileItemType type = FileItemType.File)
            => GetUser(username)?.Tree.AddChild(parent, nodeName, type)
               ?? throw MyExceptions.Navigation($"Пользователь «{username}» не найден", "UserList.AddNodeToFolder");

        public FileSystemNode InsertNode(string username,
            FileSystemNode parent, int index, string nodeName,
            FileItemType type = FileItemType.File)
            => GetUser(username)?.Tree.InsertChild(parent, index, nodeName, type)
               ?? throw MyExceptions.Navigation($"Пользователь «{username}» не найден", "UserList.InsertNode");

        public bool RemoveNode(string username, FileSystemNode node)
        {
            var user = GetUser(username)
                ?? throw MyExceptions.Navigation($"Пользователь «{username}» не найден", "UserList.RemoveNode");
            return user.Tree.Remove(node);
        }

        public void MoveNode(string username,
            FileSystemNode node, FileSystemNode? newParent)
        {
            var user = GetUser(username)
                ?? throw MyExceptions.Navigation($"Пользователь «{username}» не найден", "UserList.MoveNode");
            user.Tree.Move(node, newParent);
        }

        public FileSystemNode? FindNode(string username, string nodeName)
            => GetUser(username)?.Tree.FindByName(nodeName)
               ?? throw MyExceptions.Navigation($"Пользователь «{username}» не найден", "UserList.FindNode");

        public void ClearTree(string username)
        {
            var user = GetUser(username)
                ?? throw MyExceptions.Navigation($"Пользователь «{username}» не найден", "UserList.ClearTree");
            user.Tree.Clear();
        }
    }

    /// <summary>
    /// Модель пользователя. Хранит учётные данные, настройки и дерево файлов.
    /// </summary>
    public class User
    {
        public required string Username  { get; init; }
        public required string Password  { get; set; }
        public required string Email     { get; set; }
        public required string Theme     { get; set; }
        public required string Language  { get; set; }
        public FileSystemTree  Tree      { get; set; } = new();
    }

    /// <summary>
    /// Узел дерева файловой системы. Может быть папкой или файлом-заметкой.
    /// Содержит дочерние узлы и ссылку на родителя (не сериализуется).
    /// </summary>
    public class FileSystemNode
    {
        private string _name = string.Empty;

        public string Name
        {
            get => _name;
            set => _name = value ?? string.Empty;
        }

        public FileItemType ItemType { get; set; } = FileItemType.File;

        [JsonIgnore]
        public bool IsFolder => ItemType == FileItemType.Folder;

        /// <summary>
        /// Текстовое содержимое заметки. Заполняется только для файловых узлов.
        /// </summary>
        public string? Content { get; set; }

        public List<FileSystemNode> Children { get; set; } = new();

        /// <summary>
        /// Не сериализуется — восстанавливается вручную через RestoreParents после загрузки.
        /// </summary>
        [JsonIgnore]
        public FileSystemNode? Parent { get; internal set; }

        public FileSystemNode() { }

        public FileSystemNode(string name, FileItemType itemType = FileItemType.File)
        {
            Name     = name;
            ItemType = itemType;
        }
    }

    /// <summary>
    /// Дерево файловой системы пользователя. Управляет корневыми узлами и предоставляет
    /// методы для добавления, удаления, перемещения и обхода узлов.
    /// </summary>
    public class FileSystemTree
    {
        public List<FileSystemNode> Roots { get; set; } = new();

        public FileSystemNode AddToRoot(string name, FileItemType type = FileItemType.File)
        {
            var node = new FileSystemNode(name, type);
            Roots.Add(node);
            return node;
        }

        public FileSystemNode AddChild(FileSystemNode parent, string name,
            FileItemType type = FileItemType.File)
        {
            if (!parent.IsFolder)
                throw MyExceptions.Validation(
                    $"Узел «{parent.Name}» не является папкой", "FileSystemTree.AddChild");

            var node = new FileSystemNode(name, type) { Parent = parent };
            parent.Children.Add(node);
            return node;
        }

        public FileSystemNode InsertChild(FileSystemNode parent, int index,
            string name, FileItemType type = FileItemType.File)
        {
            if (!parent.IsFolder)
                throw MyExceptions.Validation(
                    $"Узел «{parent.Name}» не является папкой", "FileSystemTree.InsertChild");

            var node      = new FileSystemNode(name, type) { Parent = parent };
            int safeIndex = Math.Clamp(index, 0, parent.Children.Count);
            parent.Children.Insert(safeIndex, node);
            return node;
        }

        public FileSystemNode InsertToRoot(int index, string name,
            FileItemType type = FileItemType.File)
        {
            var node      = new FileSystemNode(name, type);
            int safeIndex = Math.Clamp(index, 0, Roots.Count);
            Roots.Insert(safeIndex, node);
            return node;
        }

        public bool Remove(FileSystemNode node)
        {
            if (node.Parent != null)
            {
                bool removed = node.Parent.Children.Remove(node);
                if (removed) node.Parent = null;
                return removed;
            }

            return Roots.Remove(node);
        }

        public void Clear()
        {
            foreach (var root in Roots)
                ClearRecursive(root);

            Roots.Clear();
        }

        private static void ClearRecursive(FileSystemNode node)
        {
            foreach (var child in node.Children)
            {
                child.Parent = null;
                ClearRecursive(child);
            }
        }

        /// <summary>
        /// Перемещает узел к новому родителю. Если newParent == null — переносит в корень.
        /// Бросает исключение, если newParent не является папкой.
        /// </summary>
        public void Move(FileSystemNode node, FileSystemNode? newParent)
        {
            Remove(node);

            if (newParent == null)
            {
                node.Parent = null;
                Roots.Add(node);
            }
            else
            {
                if (!newParent.IsFolder)
                    throw MyExceptions.Validation(
                        $"Узел «{newParent.Name}» не является папкой", "FileSystemTree.Move");

                node.Parent = newParent;
                newParent.Children.Add(node);
            }
        }

        public FileSystemNode? FindByName(string name)
            => FindByNameRecursive(Roots, name);

        private static FileSystemNode? FindByNameRecursive(
            IEnumerable<FileSystemNode> nodes, string name)
        {
            foreach (var node in nodes)
            {
                if (string.Equals(node.Name, name, StringComparison.OrdinalIgnoreCase))
                    return node;

                var found = FindByNameRecursive(node.Children, name);
                if (found != null) return found;
            }
            return null;
        }

        /// <summary>
        /// Обходит всё дерево в глубину и вызывает action для каждого узла.
        /// Второй параметр action — глубина узла (0 для корневых).
        /// </summary>
        public void Traverse(Action<FileSystemNode, int> action)
            => TraverseRecursive(Roots, action, depth: 0);

        private static void TraverseRecursive(IEnumerable<FileSystemNode> nodes,
            Action<FileSystemNode, int> action, int depth)
        {
            foreach (var node in nodes)
            {
                action(node, depth);
                TraverseRecursive(node.Children, action, depth + 1);
            }
        }
    }
}