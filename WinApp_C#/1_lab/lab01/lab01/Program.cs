namespace lab01;

static class Program
{
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new InitialForm());
    }
}