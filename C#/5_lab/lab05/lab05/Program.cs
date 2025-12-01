using System;
using System.Data.SqlTypes;
using System.Text;
using System.Linq;

namespace lab05
{
    
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
        public string version = "";

        public override void write_ooo()
        {
            Console.WriteLine("ooo");
        }

        public override string ToString()
        {
            return $"{GetType().Name} : \nadminOperations={set_of_operation.adminOperations}, " +
                   $"\nuserOperations={set_of_operation.userOperations},\nversion={version}";
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
                   $"\nuserOperations={set_of_operation.userOperations},\nversion={version},\n" +
                   $"projects={string.Join(",", projects)},\ninformation={information}";
        }
    }

    class Virus : Operations
    {
        public string backend = "";

        public override void write_ooo()
        {
            Console.WriteLine("ooo");
        }

        public override string ToString()
        {
            return $"{GetType().Name} : \nadminOperations={set_of_operation.adminOperations}, " +
                   $"\nuserOperations={set_of_operation.userOperations},\nbackend={backend}";
        }
    }

    class Conficker : Virus, FinalProduct
    {
        public string information = "чепуха о сетевых червях";
        public string HTTP_server = "";

        public void writeInf()
        {
            Console.WriteLine("Информация о Conficker: {0}", information);
        }

        public void IAmPrinting()
        {
            Console.WriteLine($"Тип объекта: {GetType().Name}");
            Console.WriteLine(this.ToString());
        }

        public override string ToString()
        {
            return $"{GetType().Name} : " +
                   $"\nadminOperations={set_of_operation.adminOperations}, " +
                   $"\nuserOperations={set_of_operation.userOperations},\nbackend={backend},\n" +
                   $"HTTP_server={HTTP_server},\ninformation={information}";
        }
    }

    class Game : Operations
    {
        public string type = "";

        public override void write_ooo()
        {
            Console.WriteLine("ooo");
        }

        public override string ToString()
        {
            return $"{GetType().Name} : \nadminOperations={set_of_operation.adminOperations}, " +
                   $"\nuserOperations={set_of_operation.userOperations},\ntype={type}";
        }
    }

    class Sapper : Game, FinalProduct
    {
        public string information = "чепуха о сапёре";
        public string mine = "";
        Software SapperSoftware = new Software();

        public void writeInf()
        {
            Console.WriteLine("Информация о сапёре: {0}", information);
        }

        public void IAmPrinting()
        {
            Console.WriteLine(this.ToString());
        }

        public override string ToString()
        {
            return $"{GetType().Name} : \ncore={SapperSoftware.core},\nfrontend={SapperSoftware.frontend},\nbackend={SapperSoftware.backend}" +
                   $"\nadminOperations={set_of_operation.adminOperations}, " +
                   $"\nuserOperations={set_of_operation.userOperations},\ntype={type},\n" +
                   $"mine={mine},\ninformation={information}";
        }
    }

    class Developer
    {
        public string name = "";
        public string projects = "";
    }

    public abstract class AbstrTask4
    {
        public abstract void writeInf();
    }

    public partial class Task4 : AbstrTask4, FinalProduct
    {
        public override void writeInf()
        {
            Console.WriteLine("interface");
        }

        void FinalProduct.writeInf()
        {
            Console.WriteLine("abstract class");
        }
    }

    public class Printer
    {
        public void IAmPrinting(FinalProduct someobj)
        {
            someobj.IAmPrinting();
        }
    }
    
    class Program
    {
        static void Main()
        {
            // 4 задание
            /*{
                Console.WriteLine("4 задание");
                Task4 task4 = new Task4();
                task4.writeInf();
                FinalProduct t4 = task4;
                t4.writeInf();
                Console.WriteLine("---------------------------------------");
            }
            // 5 задание
            {
                Console.WriteLine("5 задание");

                Word word = new Word();
                Conficker conficker = new Conficker();
                Sapper sapper = new Sapper();

                FinalProduct w = word;
                Operations w1 = w as Operations;
                w1.write_ooo();

                Operations c = conficker;
                c.write_ooo();

                FinalProduct s = sapper;

                if (s is FinalProduct)
                    s.writeInf();
                Console.WriteLine("---------------------------------------");
            }
            // 7 задание
            {
                Console.WriteLine("7 задание");

                Printer printer = new Printer();

                FinalProduct[] products = new FinalProduct[]
                {
                    new Word(),
                    new Conficker(),
                    new Sapper(),
                };

                foreach (FinalProduct item in products)
                {
                    printer.IAmPrinting(item);
                }
                Console.WriteLine("---------------------------------------");
            }*/
            // 5 лаба
            {
                PC conteiner = new PC();
                IPC controller = new IPC(conteiner);
                conteiner.AddObj(new Word());
                conteiner.AddObj(new Conficker());
                conteiner.AddObj(new Sapper());
                
                Game obj1 = new Game();
                Game obj2 = new Game();
                obj1.type = "sus";
                obj2.type = "dad";
                conteiner.AddObj(obj1);
                conteiner.AddObj(obj2);
                
                WordProcessor obj3 = new WordProcessor();
                WordProcessor obj4 = new WordProcessor();
                obj3.version = "1.1v";
                obj4.version = "2.2v";
                conteiner.AddObj(obj3);
                conteiner.AddObj(obj4);
                
                conteiner.Print();
                Console.WriteLine("\n---------------------------------------\n");
                controller.AlphabetPrint();
                Console.WriteLine("\n---------------------------------------\n");
                controller.TGames("sus");
                Console.WriteLine("\n---------------------------------------\n");
                controller.VWordProcessor("2.2v");
            }
        }
    }
}
