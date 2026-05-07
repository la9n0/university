using System;
using System.Text.Json;

namespace DAL004
{
    public record Celebrity(int Id, string Firstname, string Surname, string PhotoPath);

    public interface IRepository : IDisposable
    {
        string Basepath { get; }
        Celebrity[] getAllCelebrities();
        Celebrity? GetCelebrityById(int id);
        Celebrity[] GetCelebritiesBySurname(string Surname);
        string? getPhotoPathById(int id);
        int? addCelebrity(Celebrity celebrity);
        bool delCelebrityById(int id);
        int? updCelebrityById(int id, Celebrity celebrity);
        int SaveChanges();
    }

    public class Repository : IRepository
    {
        public static string JSONFileName { get; } = "Celebrities.json";
        public string Basepath { get; }
        private List<Celebrity> AllCelebrity { get; set; }

        private Repository(string directoryPath)
        {
            Basepath = "D:\\university\\4_sem\\tpi\\4-5_lab\\Celebrities\\Celebrities.json";

            if (File.Exists(Basepath))
            {
                var jsonData = File.ReadAllText(Basepath);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                AllCelebrity = JsonSerializer.Deserialize<List<Celebrity>>(jsonData, options)
                               ?? new List<Celebrity>();
            }
            else
            {
                AllCelebrity = new List<Celebrity>();
            }
        }

        public static Repository Create(string directoryName)
        {
            return new Repository(directoryName);
        }

        public Celebrity[] getAllCelebrities()
        {
            return AllCelebrity.ToArray();
        }

        public Celebrity? GetCelebrityById(int id)
        {
            return AllCelebrity.FirstOrDefault(x => x.Id == id);
        }

        public Celebrity[] GetCelebritiesBySurname(string Surname)
        {
            return AllCelebrity
                .Where(x => x.Surname.Equals(Surname, StringComparison.OrdinalIgnoreCase))
                .ToArray();
        }

        public string? getPhotoPathById(int id)
        {
            return AllCelebrity.FirstOrDefault(y => y.Id == id)?.PhotoPath;
        }

        public void Dispose() { }

        public int? addCelebrity(Celebrity celebrity)
        {
            if (celebrity?.Firstname == null || celebrity?.Surname == null || celebrity?.PhotoPath == null)
                return null;

            int newId = AllCelebrity.Any() ? AllCelebrity.Max(x => x.Id) + 1 : 1;

            var newCelebrity = celebrity with { Id = newId };
            AllCelebrity.Add(newCelebrity);

            return newId;
        }

        public bool delCelebrityById(int id)
        {
            var celeb = AllCelebrity.FirstOrDefault(x => x.Id == id);
            if (celeb == null)
                return false;

            AllCelebrity.Remove(celeb);
            return true;
        }

        public int? updCelebrityById(int id, Celebrity celebrity)
        {
            var existing = AllCelebrity.FirstOrDefault(x => x.Id == id);
            if (existing == null)
                return null;

            if (celebrity?.Firstname == null || celebrity?.Surname == null || celebrity?.PhotoPath == null)
                return null;

            var updated = celebrity with { Id = id };

            int index = AllCelebrity.IndexOf(existing);
            AllCelebrity[index] = updated;

            return id;
        }

        public int SaveChanges()
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };

                var json = JsonSerializer.Serialize(AllCelebrity, options);
                File.WriteAllText(Basepath, json);

                return AllCelebrity.Count;
            }
            catch
            {
                return -1;
            }
        }
    }
}