using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace STEAM_DB
{
    internal class ConnectionToDataBase
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
