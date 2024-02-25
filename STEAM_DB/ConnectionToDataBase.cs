using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace STEAM_DB
{
    abstract internal class ConnectionToDataBase
    {
        internal static string ConnectionString { get; set; } = null!;

        internal static DbContextOptions<SteamContext> ConnectionStringOptions { get; set; } = null!;

        public static void GetConnectionString()
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
