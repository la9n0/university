using System;
using System.Data.SqlTypes;
using System.Diagnostics.Metrics;
using System.Text;
using System.Linq;

namespace lab05
{
    public partial class Task4 : AbstrTask4, FinalProduct
    {
        public void IAmPrinting(){}
    }
    
    enum aba
    {
        a,
        b,
        c,
        d
    }

    struct bab
    {
        public int a;
        public int b;
        public bab(int a, int b)
        {
            this.a = a;
            this.b = b;
        }
    }

    public abstract class PO
    {
        protected List<object> list = new List<object>();

        public object this[int index]
        {
            get { return list[index]; } 
            set { list[index] = value;}
        }

        public int Count => list.Count;
    }
    
    public class PC : PO
    {
        public void AddObj(object obj)
        {
            list.Add(obj);
        }

        public void DelObj(object obj)
        {
            list.Remove(obj);
        }

        public void Print()
        {
            Console.WriteLine("Объекты контейнера: ");
            foreach (object obj in list)
            {
                Console.WriteLine(obj); 
            }
        }
    }

    public class IPC 
    {
        private PC list;

        public object this[int index]
        {
            get { return list[index]; }
            set { list[index] = value; }
        }

        public IPC(PC list)
        {
            this.list = list;
        }
        
        public void AlphabetPrint()
        {
            List<object> lst = new List<object>();
            for (int i = 0; i < list.Count; i++)
            {
                lst.Add(list[i]);
            }
            
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < list.Count - i-1; j++)
                {
                    if (string.Compare(lst[j].ToString(), lst[j + 1].ToString()) > 0)
                    {
                        object temp = lst[j];
                        lst[j] = lst[j + 1];
                        lst[j + 1] = temp;
                    }
                }
            }
            
            Console.WriteLine("Объекты контейнера в алфавитном порядке:");
            foreach (object obj in lst)
            {
                Console.WriteLine(obj);
            }
        }

        public void TGames(string type)
        {
            Console.WriteLine("Все игры с типом: {0}", type);
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] is Game obj)
                {
                    if (obj.type == type)
                        Console.WriteLine(obj);
                }
            }
        }
        public void VWordProcessor(string version)
        {
            Console.WriteLine("Все текстовые редакторы версии: {0}", version);
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] is WordProcessor obj)
                {
                    if (obj.version == version)
                        Console.WriteLine(obj);
                }
                
            }
        }
    }
}