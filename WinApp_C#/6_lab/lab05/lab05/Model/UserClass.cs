using System.IO;
using System.Text.Json;

namespace lab05
{
    public class UserList
    {
        private List<User> _users = new List<User>();
        private const string Path = "D:/university/4_sem/oop/6_lab/Users.json";

        public void SaveJson()
        {
            string json = JsonSerializer.Serialize(_users);
            File.WriteAllText(Path, json);
        }
        
        public void LoadJson()
        {
            if (!File.Exists(Path)) return;
            string json = File.ReadAllText(Path);
        
            var restored = JsonSerializer.Deserialize<List<User>>(json);
            _users = restored;
        }
        
        public User? GetUser(string name)
        {
            return _users?.Find(g => g.username == name);
        }

        public bool AddUser(string username, string password, string name, 
            string surname, string email, string theme, string language)
        {
            if (_users.Contains(_users.Find(g => g.name == name))) return false;
            
            User user = new User
            {
                username = username,
                password = password,
                name = name,
                surname = surname,
                email = email,
                theme = theme,
                language = language
            };
            _users.Add(user);
            return true;
        }

        public void DelUser(string name)
        {
            _users.Remove(_users.Find(g => g.username == name));
        }
    }
    
    public class User
    {
        public string username   {get; set;}
        public string password  {get; set;}
        public string name {get; set;}
        public string surname  {get; set;}
        public string email  {get; set;}
        public string theme {get; set;}
        public string language {get; set;}
    }
}