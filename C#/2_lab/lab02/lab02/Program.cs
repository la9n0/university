using System;
using System.Text;
using System.Linq;
using System.Runtime.CompilerServices;

namespace lab02
{
    partial class Vector
    {
        public Vector()
        {
            Console.WriteLine("Конструктор без параметров");
            ID = GetHashCode();
            obj_count++;
            count = 0;
        }

        public Vector(int a, int b)
        {
            Console.WriteLine($"Конструктор с параметрами: {a}, {b}");
            ID = GetHashCode();
            obj_count++;
            count = 2;
        }

        public Vector(int i, double a = 54.5, string b = "huh")
        {
            Console.WriteLine($"Конструктор с параметрами по умолчанию: {i}, {a}, {b}");
            ID = GetHashCode();
            obj_count++;
            count = 3;
        }
        
        static Vector()
        {
            Console.WriteLine("Статистический конструктор");
            classInfo();
        }
        
        private Vector(int a)
        {
            Console.WriteLine("Закрытый конструктор");
            ID = GetHashCode();
            obj_count++;
        }

        public static void us_private()
        {
            Vector vector = new Vector(5);
        }

        public void Sum(ref int value)
        {
            for (int i = 0; i < a.Length; i++)
            {
                a[i] += value;
            }
            value = a.Sum();
            obj_count++;
        }

        public void Multiplication(int multiplier, out int result)
        {
            result = 1;
            for (int i = 0; i < a.Length; i++)
            {
                a[i] *= multiplier;
                result *= a[i];
            }
            obj_count++;
        }

        public static void classInfo()
        {
            Console.WriteLine($"Информация о классе:");
            Console.WriteLine($"Число вызванных объектов: {obj_count}"); 
            Console.WriteLine($"Размер массива a по умолчанию: 5");
        }

        public override bool Equals(object obj)
        {
            if (obj is Vector other)
            {
                if (a.Length != other.a.Length)
                    return false;

                for (int i = 0; i < a.Length; i++)
                    if (a[i] != other.a[i])
                        return false;

                return count == other.count && state == other.state;
            }
            return false;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 23 + count.GetHashCode();
            hash = hash * 23 + state.GetHashCode();
            foreach (int val in a)
                hash = hash * 23 + val.GetHashCode();
            return hash;
        }

        public override string ToString()
        {
            return $"Vector(ID={ID}, count={count}, state={state}, elements=[{string.Join(", ", a)}])";
        }
    }

    partial class Vector
    {
        private int[] a = new int[5];
        private int count = 0;
        private bool state;
        private static int obj_count = 0;
        private readonly int ID;
        private const int c = 100;

        public bool State => state;

        public int[] A
        {
            get { return a; }
            set { a = value; }
        }

        public int Count
        {
            get { return count; }
            private set { count = value; }
        }

        public bool StateProp
        {
            get { return state; }
            set { state = value; }
        }

        public int IDProp => ID;
        public int ConstValue => c;

        public object this[string name]
        {
            get
            {
                switch (name)
                {
                    case "a": return a;
                    case "count": return count;
                    case "state": return state;
                    case "ID": return ID;
                    case "c": return c;
                    default:
                        Console.WriteLine($"Ошибка: поля \"{name}\" не существует!");
                        state = false;
                        return null;
                }
            }

            set
            {
                switch (name)
                {
                    case "a":
                        a = (int[])value;
                        state = true;
                        break;

                    case "count":
                        count = (int)value;
                        state = true;
                        break;

                    case "state":
                        state = (bool)value;
                        break;

                    case "ID":
                        Console.WriteLine("Ошибка: поле ID только для чтения!");
                        state = false;
                        break;

                    case "c":
                        Console.WriteLine("Ошибка: поле c — это константа, его нельзя изменить!");
                        state = false;
                        break;

                    default:
                        Console.WriteLine($"Ошибка: поля \"{name}\" не существует!");
                        state = false;
                        break;
                }
            }
        }
    }

    class Program
    {
        static void Main()
        {
            Vector v1 = new Vector();
            Vector v2 = new Vector(2, 3);
            Vector v3 = new Vector(1);

            int sumVal = 5;
            v1.Sum(ref sumVal);

            int product;
            v2.Multiplication(2, out product);

            Console.WriteLine(v1);
            Console.WriteLine(v2);
            Console.WriteLine(v3);
            Console.WriteLine($"Произведение v2 после умножения: {product}");

            Vector[] vectors = { v1, v2, v3 };

            Console.WriteLine("\nВекторы, содержащие 0:");
            foreach (var v in vectors)
                if (Array.Exists(v.A, x => x == 0))
                    Console.WriteLine(v);

            Console.WriteLine("\nВекторы с наименьшим модулем:");
            double min = double.MaxValue;
            foreach (var v in vectors)
            {
                double mod = Math.Sqrt(v.A.Sum(x => x * x));
                if (mod < min) min = mod;
            }

            foreach (var v in vectors)
            {
                double mod = Math.Sqrt(v.A.Sum(x => x * x));
                if (Math.Abs(mod - min) < 1e-6)
                    Console.WriteLine(v);
            }

            var anon = new { v1.IDProp, v1.Count, v1.StateProp };
            Console.WriteLine($"\nАнонимный тип: {anon}");
        }
    }
}
