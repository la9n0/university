namespace lab06
{
    public class PCException : Exception
    {
        public PCException(string message) : base(message)
        {
            Console.WriteLine("Ошибка: {0}",message);
        }
    }
}