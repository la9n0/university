using System;
using System.Data.SqlTypes;
using System.Text;
using System.Linq;

namespace lab03
{
    static class extensions
    {
        public static bool cont(this string str, char a)
        {
            return str.Contains(a);
        }

        public static int[] delNeg(this int[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] < 0)
                    arr[i] = 0;
            }
            return arr;
        }
    }

    static class StatisticOperation
    {
        public static arrClass sum(arrClass a, arrClass b)
        {
            int size=a.Length;
            if (a.Length > b.Length)
                size = b.Length;
            arrClass result = new arrClass(size);
            for (int i = 0; i < size; i++)
            {
                result[i] = a[i] + b[i];
            }
            return result;
        }

        public static int dif(arrClass a)
        {
            int min = a[0];
            int max = a[0];
            for (int i = 1; i < a.Length; i++)
            {
                if (a[i] < min)
                    min = a[i];
                if (a[i] > max)
                    max = a[i];
            }
            return max - min;
        }
        
        public static int numOfEl(arrClass a)
        {
            return a.Length;
        }

        public static int FdifL(this arrClass a)
        {
            return a[0]-a[^1];
        }

        public static string delLast(this string str)
        {
            return str.Substring(0, str.Length - 1);
        }
    }
    
    class arrClass
    {
        private int[] data;
        public arrClass(int size)
        {
            data = new int[size];
        }
        public int this[int i]
        {
            get => data[i];
            set => data[i] = value;
        }
        public int Length => data.Length;
        
        public static arrClass operator *(arrClass a, arrClass b)
        {
            int size=a.Length;
            if (a.Length > b.Length)
                size = b.Length;
            arrClass result = new arrClass(size);
            for (int i = 0; i < size; i++)
            {
                result.data[i] = a.data[i] * b.data[i];
            }
            return result;
        }

        public static bool operator true(arrClass a)
        {
            for(int i=0;i<a.Length;i++)
            {
                if(a[i]<0)
                    return false;
            }
            return true;
        }
        
        public static bool operator false(arrClass a)
        {
            for(int i=0;i<a.Length;i++)
            {
                if(a[i]<0)
                    return true;
            }
            return false;
        }
        
        public static implicit operator int(arrClass a)
        {
            return a.Length;
        }
        
        
        public static bool operator ==(arrClass a, arrClass b)
        {
            if (a.Length != b.Length)
                return false;
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i])
                    return false;
            }
            return true;
        }
        public static bool operator !=(arrClass a, arrClass b)
        {
            if (a.Length != b.Length)
                return true;
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i])
                    return true;
            }
            return false;
        }

        public static bool operator <(arrClass a, arrClass b)
        {
            if (a.Length != b.Length)
                return a.Length < b.Length;
            else
            {
                return a.data.Sum() < b.data.Sum();
            }
        }
        public static bool operator >(arrClass a, arrClass b)
        {
            if (a.Length != b.Length)
                return a.Length > b.Length;
            else
            {
                return a.data.Sum() > b.data.Sum();
            }
        }

        public class Production
        {
            public int Id=52345;
            public string orgName="OOOOOOOOOOOOOOO";
        }
        public class Developer
        {
            public string FIO="Петров Пётр Попович";
            public int Id=52161;
            public int departament=5;
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            arrClass arr1 = new arrClass(5);
            arrClass arr2 = new arrClass(5);

            for (int i = 0; i < 5; i++)
            {
                arr1[i] = i + 1;
                arr2[i] = i + 2;
            }

            arrClass result = arr1 * arr2;
            Console.WriteLine("Результат умножения:");
            for (int i = 0; i < result.Length; i++)
            {
                Console.Write(result[i] + " ");
            }
            Console.WriteLine("\n-----------------------------------");
            
            if (arr1)
            {
                Console.WriteLine("В arr1 нет отрицательных чисел");
            }
            
            arr1[2] = -5;
            if (arr1)
            {
                Console.WriteLine("Теперь в arr1 есть отрицательное число");
            }
            Console.WriteLine("-----------------------------------");

            arrClass arr3 = new arrClass(3);
            Console.WriteLine($"arr2 < arr3 = {arr2 < arr3}");
            Console.WriteLine($"arr3 < arr1 = {arr3 < arr1}");
            Console.WriteLine("-----------------------------------");
            
            Console.WriteLine($"Проверка на равенство: {arr1==arr2}");
            
            arrClass a1 = new arrClass(5);
            arrClass a2 = new arrClass(5);
            for (int i = 0; i < 5; i++)
            {
                a1[i] = i + 1;
                a2[i] = i + 1;
            }
            
            Console.WriteLine($"Проверка на равенство х2: {a1==a2}");
            
            Console.WriteLine("-----------------------------------");
            Console.WriteLine("-----------------------------------");
            
            Console.WriteLine("Проверка методов:");
            string str = "lalalallabub";
            Console.WriteLine($"Проверка на нахождение элемента: {str.cont('l')}");
            
            int[] a=new int[6];
            for (int i = 0; i < 6; i++)
            {
                a[i] = i * (i + (-3));
            }
            Console.WriteLine("-----------------------------------");
            
            Console.WriteLine("Замена отр чисел на 0");
            Console.WriteLine(string.Join(" ", a));
            Console.WriteLine(string.Join(" ", a.delNeg()));
            
            Console.WriteLine("-----------------------------------");
            Console.WriteLine("-----------------------------------");
            
            arrClass.Production amam=new arrClass.Production();
            Console.WriteLine($"Инициализация Production:\n ID={amam.Id}, Название организации: {amam.orgName}");
            Console.WriteLine("-----------------------------------");
            
            arrClass.Developer amam2=new arrClass.Developer();
            Console.WriteLine($"Инициализация Production:\n ФИО: {amam2.FIO}, ID={amam2.Id}, отдел: {amam2.departament}");
            
            Console.WriteLine("-----------------------------------");
            Console.WriteLine("-----------------------------------");
            
            Console.WriteLine("Методы статического класса StaticOperation:");
            Console.WriteLine($"Сумма массивов: {StatisticOperation.sum(arr1,arr2)}");
            Console.WriteLine($"Разница между макс и мин элементом: {StatisticOperation.dif(arr1)}");
            Console.WriteLine($"Количество элементов: {StatisticOperation.numOfEl(arr1)}");
            
            Console.WriteLine("-----------------------------------");
            Console.WriteLine("-----------------------------------");
            
            Console.WriteLine("Методы расширения: \n"); 
            Console.WriteLine($"Разница между 1 и последним элементами массива: {arr1.FdifL()}");
            Console.WriteLine($"Удаление последнего элемента строки: {str.delLast()}");
        }
    }
}