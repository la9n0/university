using System;
using System.Data.SqlTypes;
using System.Text;
using System.Linq;
using System.Text.Json;

namespace lab07
{
    public interface IOperations<T> //обобщённый интерфейс
    {
        public void Add(int position, T a);
        public void Del(int position);
        public void View();
    }
    
    class Aclass<T> : IOperations<T> where T : new()   //обобщённый класс
    {
        private T[] data;
        public Aclass(int size)
        {
            data = new T[size];
        }
        public T this[int i]
        {
            get => data[i];
            set => data[i] = value;
        }
        public int Length => data.Length;

        public void Add(int position, T a)
        {
            data[position] = a;
        }

        public void Del(int position)
        {
            data[position] = default(T);
        }

        public void View()
        {
            Console.WriteLine("Элементы массива: ");
            foreach (T i in data)
            {
                Console.WriteLine(i);
            }
        }
        
        public T[] Items
        {
            get => data;
            set => data = value ?? new T[0];
        }
        public void SaveToFile(string path)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true
            };

            string tmp = path + ".tmp";
            try
            {
                using (var fs = File.Create(tmp))
                {
                    JsonSerializer.Serialize(fs, this, this.GetType(), options);
                    fs.Flush(true);
                }

                if (File.Exists(path))
                    File.Replace(tmp, path, null);
                else
                    File.Move(tmp, path);
            }
            catch
            {
                try { if (File.Exists(tmp)) File.Delete(tmp); } catch { }
                throw;
            }
        }

        public Aclass<T> LoadFromFile(string path)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            using (var fs = File.OpenRead(path))
            {
                var obj = (Aclass<T>)JsonSerializer.Deserialize(fs, typeof(Aclass<T>), options);
                return obj ?? new Aclass<T>(0);
            }
        }
    }
    
    //пользовательский класс
    public interface FinalProduct
    {
        public void writeInf();
        public void IAmPrinting();
    }

    public abstract class Operations
    {
        protected SetOfOperation set_of_operation = new SetOfOperation();

        public override string ToString()
        {
            return $"{GetType().Name} : \nadminOperations={set_of_operation.adminOperations}, " +
                   $"\nuserOperations={set_of_operation.userOperations}";
        }

        public virtual void write_ooo()
        {
            Console.WriteLine("OOO");
        }
    }

    sealed class Software
    {
        public string core = "";
        public string frontend = "";
        public string backend = "";
    }

    public class SetOfOperation
    {
        public string userOperations = "";
        public string adminOperations = "";
    }

    class WordProcessor : Operations
    {
        protected string textEditor = "";

        public override void write_ooo()
        {
            Console.WriteLine("ooo");
        }

        public override string ToString()
        {
            return $"{GetType().Name} : \nadminOperations={set_of_operation.adminOperations}, " +
                   $"\nuserOperations={set_of_operation.userOperations},\ntextEditor={textEditor}";
        }
    }

    class Word : WordProcessor, FinalProduct
    {
        public string[] projects = { "" };
        public string information = "чепуха о ворде";
        Software WordSoftware = new Software();

        public void writeInf()
        {
            Console.WriteLine("Информация о Word: {0}", information);
        }

        public void IAmPrinting()
        {
            Console.WriteLine($"Тип объекта: {GetType().Name}");
            Console.WriteLine(this.ToString());
        }

        public override string ToString()
        {
            return $"{GetType().Name} : \ncore={WordSoftware.core},\nfrontend={WordSoftware.frontend},\nbackend={WordSoftware.backend}" +
                   $"\nadminOperations={set_of_operation.adminOperations}, " +
                   $"\nuserOperations={set_of_operation.userOperations},\ntextEditor={textEditor},\n" +
                   $"projects={string.Join(",", projects)},\ninformation={information}";
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Aclass<char> b = new Aclass<char>(5);
                Aclass<int> a = new Aclass<int>(5);
                for (int i = 0; i < a.Length; i++)
                {
                    a.Add(i, i*2);
                }
                a.View();
                a.Del(3);
                Console.WriteLine("\nПосле удаления:");
                a.View();
                a.SaveToFile("D:/university/3_sem/oop/7_lab/lab07/lab07/c.json");
            }
            catch (Exception)
            {
                Console.WriteLine("░░░░░██████████ ]▄▄▄▄▄▄▄▄▄ \n▂▄▅████ОШИБКА!████▅▄▃ \n████ОШИБКА!ОШИБКА!████] \n◥⊙▲⊙▲⊙▲⊙▲⊙▲⊙▲⊙▲⊙▲⊙▲⊙◤..");
            }
            finally
            {
                Console.WriteLine("Конец 2 задания\n -------------------------\n");
            }
            Aclass<Word> c = new Aclass<Word>(5);
            Word w = new Word();
            w.projects[0] = "bububb";
            c.Add(0, w);
        }
    }
}