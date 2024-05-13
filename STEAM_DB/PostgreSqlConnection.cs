using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.IO;
using System.Windows.Navigation;

namespace STEAM_DB
{
    // применение шаблона Singleton
    // ключевое слово sealed - запечатанный класс (нельзя наследовать)
    sealed class PostgreSqlConnection
    { 
        // экземпляр класса
        private static NpgsqlConnection? connection;

        private static SteamContext? context;

        public static string ConnectionString { get; set; } = null!;

        public static DbContextOptions<SteamContext> ConnectionStringOptions { get; set; } = null!;

        static PostgreSqlConnection() => Connect();

        public static NpgsqlConnection Connection 
        { 
            get
            {
                connection ??= CreateConnection();
                return connection;
            }
        }

        public static SteamContext Context
        {
            get
            {
                context ??= CreateContext();
                return context;
            }
        }

        private static NpgsqlConnection CreateConnection() => new(ConnectionString);

        private static SteamContext CreateContext() => new(ConnectionStringOptions);

        private static void Connect()
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
