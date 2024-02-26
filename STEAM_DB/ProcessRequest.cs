using Microsoft.EntityFrameworkCore;
using Npgsql;
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
        static readonly DbContextOptions<SteamContext> options = ConnectionToDataBase.ConnectionStringOptions;
        static readonly string connection = ConnectionToDataBase.ConnectionString;

        public static List<Game> GetListOfGames()
        {
            // создаём подключение с базой данных
            using SteamContext context = new(options);
            // извлекаем из таблицы в базе данные об играх в виде списка
            List<Game> games = [.. context.Games]; // context.Games.ToList()
            // сортируем список по айди
            List<Game> gamesSort = [.. games.OrderBy(p => p.GameId)]; // games.OrderBy(p => p.GameId).ToList()
            // возврашяем отсортированный список
            return gamesSort;
        }

        public static List<string[]> GetListOfGamess()
        {
            // создаём подключение с базой данных
            using SteamContext context = new(options);
            var Info = context.Games.Join(context.Developers,
                g => g.DeveloperId,
                b => b.DeveloperId,
                (g, b) => new
                {
                    g.Title,
                    g.Description,
                    b.Developername
                });
            List<string[]> gamesInfo = [];
            int i = 0;
            foreach (var game in Info)
            {
                gamesInfo.Add(new string[3]);
                gamesInfo[i][0] = game.Title;
                gamesInfo[i][1] = game.Description;
                gamesInfo[i][2] = game.Developername;
                i++;
            }
            return gamesInfo;
        }

        public static List<string> GetListOfWishlist(int id)
        {
            List<string> list = [];
            NpgsqlConnection con = new(connection);
            con.Open();
            NpgsqlCommand cmd = new()
            {
                Connection = con,
                CommandText = $"select g.title from games g, wishlist w where w.user_id = {id} and g.game_id = w.game_id;"
            };
            NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(reader.GetString(0));
            }
            return list;
            /*var games = context.Games.Join(context.Wishlists, g => g.GameId,
                w => w.GameId,
                (g, w) => new
                {
                    g.Title
                });
            return (IQueryable<Game>)games;*/
            /*var games = context.Games.FromSqlRaw($"select g.title from games g, wishlist w" +
                $"where w.user_id = {id} and g.game_id = w.game_id").ToList();

            return games;*/
        }
    }
}
