using System.Text.Json;
using System.IO;
using System.Reflection;


namespace lab05
{
    public class GunStore
    {
        private List<Gun> Guns = new List<Gun>();
        private string path = "D:/university/4_sem/oop/4-5_lab/Guns.json";
        
        public void SaveJSON()
        {
            string json = JsonSerializer.Serialize(Guns);
            File.WriteAllText(path, json);
        }
        
        public void LoadJSON()
        {
            if (!File.Exists(path)) return;
            string json = File.ReadAllText(path);
        
            var restored = JsonSerializer.Deserialize<List<Gun>>(json);
            Guns = restored;
        }
        
        public List<Gun> GetGuns()
        {
            return Guns;
        }

        public bool AddGun(string name, string fullName, string description, 
            string fullDescription, string category, List<string> imagePath,
            float price, int quantity, string country, float discount)
        {
            if (Guns.Contains(Guns.Find(g => g.name == name))) return false;
            
            Gun gun = new Gun();
            gun.name = name;
            gun.fullName = fullName;
            gun.description = description;
            gun.fullDescription = fullDescription;
            gun.category = category;
            gun.price = price;
            gun.quantity = quantity;
            gun.country = country;
            gun.discount = discount;
            gun.imagePath = imagePath;
            Guns.Add(gun);
            return true;
        }

        public void DelGun(string name)
        {
            Guns.Remove(Guns.Find(g => g.name == name));
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