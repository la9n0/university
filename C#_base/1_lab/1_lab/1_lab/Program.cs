using System;
namespace _1_lab
{
    class Class1
    {
        [STAThread]
        static void Main()
        {
            Console.WriteLine("Введите номер задания:");
            int choice=int.Parse(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    Task1();
                    break;
                case 2:
                    Task2();
                    break;
                case 3:
                    Task3();
                    break;
                case 4:
                    Task4();
                    break;
            }
        }
        
        static void Task1()
        {
            void task_a()
            {
                bool b = true;
                byte by = 1;
                sbyte sb = 1;
                char ch = 'a';
                decimal dec = 1;
                double d = 1;
                float f = 1;
                int i = 1;
                uint ui = 1;
                long l = 1;
                ulong ul = 1;
                short sh = 1;
                ushort ush = 1;
                
                Console.Write("Введите bool (true/false): ");
                b = bool.Parse(Console.ReadLine());

                Console.Write("Введите byte: ");
                by = byte.Parse(Console.ReadLine());

                Console.Write("Введите sbyte: ");
                sb = sbyte.Parse(Console.ReadLine());

                Console.Write("Введите char: ");
                ch = char.Parse(Console.ReadLine());

                Console.Write("Введите decimal: ");
                dec = decimal.Parse(Console.ReadLine());

                Console.Write("Введите double: ");
                d = double.Parse(Console.ReadLine());

                Console.Write("Введите float: ");
                f = float.Parse(Console.ReadLine());

                Console.Write("Введите int: ");
                i = int.Parse(Console.ReadLine());

                Console.Write("Введите uint: ");
                ui = uint.Parse(Console.ReadLine());

                Console.Write("Введите long: ");
                l = long.Parse(Console.ReadLine());

                Console.Write("Введите ulong: ");
                ul = ulong.Parse(Console.ReadLine());

                Console.Write("Введите short: ");
                sh = short.Parse(Console.ReadLine());

                Console.Write("Введите ushort: ");
                ush = ushort.Parse(Console.ReadLine());
                
                Console.WriteLine("\nВывод:");
                Console.WriteLine($"bool = {b}");
                Console.WriteLine($"byte = {by}");
                Console.WriteLine($"sbyte = {sb}");
                Console.WriteLine($"char = {ch}");
                Console.WriteLine($"decimal = {dec}");
                Console.WriteLine($"double = {d}");
                Console.WriteLine($"float = {f}");
                Console.WriteLine($"int = {i}");
                Console.WriteLine($"uint = {ui}");
                Console.WriteLine($"long = {l}");
                Console.WriteLine($"ulong = {ul}");
                Console.WriteLine($"short = {sh}");
                Console.WriteLine($"ushort = {ush}");
            }

            void task_b()
            {
                //неявные
                int i = 1;
                long l = i;
                float f = i;
                double d = i;
                decimal dec = i;

                Console.WriteLine("Неявные приведения:");
                Console.WriteLine($"int -> long: {l}");
                Console.WriteLine($"int -> float: {f}");
                Console.WriteLine($"float -> double: {d}");
                Console.WriteLine($"int -> decimal: {dec}");
                Console.WriteLine($"float -> double: {d}");

                // явные
                double d2 = 1.1;
                int i2 = (int)d2;
                long l2 = 1234567891212;
                int i3 = (int)l2;
                float f2 = 1.1f;
                int i4 = (int)f2;
                decimal dec2 = 1.1m;
                int i5 = (int)dec2;
                ulong ul = 1;
                int i6 = (int)ul;

                Console.WriteLine("\nЯвные приведения:");
                Console.WriteLine($"double -> int: {i2}");
                Console.WriteLine($"long -> int: {i3}");
                Console.WriteLine($"float -> int: {i4}");
                Console.WriteLine($"decimal -> int: {i5}");
                Console.WriteLine($"ulong -> int: {i6}");
                //Convect - перевод из 1 типа в другой
            }

            void task_c()
            {
                int a = 5;
                float b = 8.5f;
                object obg1 = a;
                object obg2 = b;
                Console.WriteLine(obg1);
                Console.WriteLine(obg2);
                double c = (float)obg2;
                double d = (int)obg1;
                Console.WriteLine(d);
                Console.Write(c);
            }
            
            void task_d()   //неявнотипизированная переменная задаётся через var
            {
                var x = 1;
                var str = "huh";
                /*нельзя после присвоения переменной 1 типа задать ей новый тип, т.е.
                 теперь нельзя сделать так: x="huh";*/
                Console.WriteLine(x);
                Console.WriteLine(str);
            }
            
            void task_e()   //Nullable переменная - переменная которая может хранить null
            {
                Nullable<int> a = null;
                int? b = null;
                //a = b
                Console.WriteLine(a);
                Console.WriteLine(b);
                a = 5; b = 4;
                Console.WriteLine(a);
                Console.WriteLine(b);
            }
            
            void task_f()
            {
                var x = "huh";
                //x = 1; //раскомитить при показе задания
            }
            
            Console.WriteLine("Введите номер задания:");
            int choice=int.Parse(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    task_a();
                    break;
                case 2:
                    task_b();
                    break;
                case 3:
                    task_c();
                    break;
                case 4:
                    task_d();
                    break;
                case 5:
                    task_e();
                    break;
                case 6:
                    task_f();
                    break;
            }
        }
        
        static void Task2()
        {
            void task_a()   //в C# нельзя сравнивать строки обычным способом(>, =, <)
            {
                string str1 = "huh";
                string str2 = "tyt";
                int result = string.Compare(str1,str2);
                if (result > 0)
                {
                    Console.WriteLine(str1, "больше", str2);
                }
                else if (result < 0)
                {
                    Console.WriteLine(str1, "меньше", str2);
                }
                else
                {
                    Console.WriteLine("Строки равны");
                }
            }
            
            void task_b()
            {
                string str1 = "huh";
                string str2 = "tyt";
                string str3 = "pop";
                
            }
            
            void task_c()
            {
                
                
            }
            
            void task_d()
            {
                
                
            }
            
            Console.WriteLine("Введите номер задания:");
            int choice=int.Parse(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    task_a();
                    break;
                case 2:
                    task_b();
                    break;
                case 3:
                    task_c();
                    break;
                case 4:
                    task_d();
                    break;
            }
        }

        static void Task3()
        {
            
        }
        
        static void Task4()
        {
            
        }
    }
}