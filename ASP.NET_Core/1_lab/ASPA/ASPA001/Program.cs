using Microsoft.AspNetCore.HttpLogging; // Подключение библиотеки для логирования HTTP-запросов и ответов

namespace ASPA001 // Объявление пространства имён проекта
{
    public class Program // Главный класс приложения
    {
        public static void Main(string[] args) // Точка входа в программу
        {
            // Создаём объект builder для настройки приложения
            var builder = WebApplication.CreateBuilder(args);

            // Регистрируем сервис логирования HTTP в контейнере зависимостей
            builder.Services.AddHttpLogging(options =>
            {
                // Указываем, что нужно логировать ВСЁ (запросы, ответы, заголовки, тело и т.д.)
                options.LoggingFields = HttpLoggingFields.All;
            });

            // Строим (собираем) приложение на основе настроек builder
            var app = builder.Build();

            // Добавляем middleware для логирования HTTP-запросов и ответов
            app.UseHttpLogging();

            // Определяем маршрут для GET-запроса по адресу "/"
            // При обращении к корню сайта возвращается строка "Hello World!"
            app.MapGet("/", () => "Hello World!");

            // Запускаем веб-приложение и начинаем обработку запросов
            app.Run();
        }
    }
}