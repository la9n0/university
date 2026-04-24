using System;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Text;
using System.Linq;

namespace lab06
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
        public bool tg => list.Any(a => a.GetType() == typeof(Game));
        public bool vwp => list.Any(a => a.GetType() == typeof(WordProcessor));
    }
    
    public class PC : PO
    {
        public void AddObj(object obj)
        {
            if (obj.GetType().IsValueType)
                throw new PCException("Неверный тип данных");
            list.Add(obj);
        }

        public void DelObj(object obj)
        {
            if (!list.Remove(obj))
                throw new PCException("Объект не найден");
        }

        public void Print()
        {
            if (list.Count == 0)
                throw new PCException("В списке нет объектов");
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
            if (list.Count == 0)
                throw new PCException("В списке нет объектов");
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
            Debug.Assert(!list.tg, "Не найден объект с типом Game");
            
            if (!list.tg)
                throw new PCException("Не найден объект с типом Game");
                
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
            if (!list.vwp)
                throw new PCException("Не найден объект с типом WordProcessor");
            
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