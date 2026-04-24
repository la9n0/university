using System.Text.Json;
using System.IO;
using System.Reflection;


namespace lab05
{
    public class GunStore
    {
        private List<Gun> _guns = [];
        private const string Path = "D:/university/4_sem/oop/4-5_lab/Guns.json";

        public void SaveJson()
        {
            string json = JsonSerializer.Serialize(_guns);
            File.WriteAllText(Path, json);
        }
        
        public void LoadJson()
        {
            if (!File.Exists(Path)) return;
            string json = File.ReadAllText(Path);
        
            var restored = JsonSerializer.Deserialize<List<Gun>>(json);
            _guns = restored;
        }
        
        public List<Gun> GetGuns()
        {
            return _guns;
        }

        public bool AddGun(string name, string fullName, string description, 
            string fullDescription, string category, List<string> imagePath,
            float price, int quantity, string country, float discount)
        {
            if (_guns.Contains(_guns.Find(g => g.name == name))) return false;
            
            Gun gun = new Gun
            {
                name = name,
                fullName = fullName,
                description = description,
                fullDescription = fullDescription,
                category = category,
                price = price,
                quantity = quantity,
                country = country,
                discount = discount,
                imagePath = imagePath
            };
            _guns.Add(gun);
            return true;
        }

        public void DelGun(string name)
        {
            _guns.Remove(_guns.Find(g => g.name == name));
        }
    }
    
    public class Gun
    {
        public string name { get; set; }
        public string fullName { get; set; }
        public string description  { get; set; }
        public string fullDescription  { get; set; }
        public List<string> imagePath { get; set; } = new List<string>();
        public string category { get; set; }
        public float price { get; set; }
        public int quantity  { get; set; }
        public string country  { get; set; }
        public float discount  { get; set; }
    }
}