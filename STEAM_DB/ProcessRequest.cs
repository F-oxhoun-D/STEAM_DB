using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEAM_DB
{
    internal static class ProcessRequest
    {
        // строка подключения
        static readonly DbContextOptions<SteamContext> options = ConnectionToDataBase.GetConnectionString();

        public static List<Game> GetListOfGames()
        {
            // создаём подключение с базой данных
            using SteamContext context = new (options);
            // извлекаем из таблицы в базе данные об играх в виде списка
            List<Game> games = [.. context.Games]; // context.Games.ToList()
            // сортируем список по айди
            List<Game> gamesSort = [.. games.OrderBy(p => p.GameId)]; // games.OrderBy(p => p.GameId).ToList()
            // возврашяем отсортированный список
            return gamesSort; 
        }
    }
}
