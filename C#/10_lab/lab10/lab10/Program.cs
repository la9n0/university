
namespace lab10
{
    class task_1
    {
        public string[] months = {"January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"};
        
        public string MonthSequence(int n)
        {
            string[] str = months.Where(x => x.Length == n).ToArray();
            if (str.Length != 0)
                return string.Join(", ", str);
            return "Нет подходящих месяцев";
        }

        public string SumWin()
        {
            return string.Join(", ", months.Skip(11).Take(1).ToArray()) + ", " +
                   string.Join(", ", months.Take(2).ToArray()) + ", " +
                   string.Join(", ", months.Skip(5).Take(3).ToArray());
        }

        public string Alphabet()
        {
            return string.Join(", ", months.OrderBy(x => x[0]).ThenBy(x=>x[1]).ToArray());
        }

        public int UMonthes()
        {
            string[] str = months.Where(x => x.Length >= 4).ToArray();
            return str.Count(x => x.Any(y => y=='u'));
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Первое задание:\n");
            task_1 t = new task_1();
            Console.WriteLine(t.MonthSequence(5));
            Console.WriteLine(t.SumWin());
            Console.WriteLine(t.Alphabet());
            Console.WriteLine(t.UMonthes());
        }
    }
}