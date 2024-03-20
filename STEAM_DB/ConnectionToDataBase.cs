using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace STEAM_DB
{
    // применение шаблона Singleton
    // ключевое слово sealed - запечатанный класс (нельзя наследовать)
    sealed class ConnectionToDataBase
    {
        public static string ConnectionString { get; set; } = null!;

        public static DbContextOptions<SteamContext> ConnectionStringOptions { get; set; } = null!;

        // экземпляр класса
        private static ConnectionToDataBase? _instance;

        // свойство
        public static ConnectionToDataBase Instance
        {
            // получаем значение
            get
            {
                // если экземпляр равен null, создаём новый объект
                _instance ??= new ConnectionToDataBase();
                // иначе возвращаем уже созданный
                return _instance;
                // это средство доступа - единственный способ получить доступ к Singleton экземпляру
                // защита от созданий множества экземпляров класса ConnectionToDataBase
            }
        }

        private ConnectionToDataBase()
        {
            Connection();
        }

        private static void Connection()
        {
            var builder = new ConfigurationBuilder();
            // установка пути к текущему каталогу
            builder.SetBasePath(Directory.GetCurrentDirectory());
            // получаем конфигурацию из файла
            builder.AddJsonFile("appsettings.json");
            // создаём конфигурацию
            var config = builder.Build();
            // получаем строку подключения
            string? connectionString = config.GetConnectionString("DefaultConnection");
            ConnectionString = connectionString ?? null!;
            var optionsBuilder = new DbContextOptionsBuilder<SteamContext>();
            ConnectionStringOptions = optionsBuilder.UseNpgsql(connectionString).Options;
        }
    }
}
