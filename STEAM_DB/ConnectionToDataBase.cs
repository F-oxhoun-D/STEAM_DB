using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace STEAM_DB
{
    abstract internal class ConnectionToDataBase
    {
        public static DbContextOptions<SteamContext> GetConnectionString()
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

            var optionsBuilder = new DbContextOptionsBuilder<SteamContext>();
            return optionsBuilder.UseNpgsql(connectionString).Options;
        }
    }
}
