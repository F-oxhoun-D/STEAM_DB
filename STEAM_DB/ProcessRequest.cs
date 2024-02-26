using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
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

        /*public static List<Game> GetListOfGames()
        {
            // создаём подключение с базой данных
            using SteamContext context = new(options);
            // извлекаем из таблицы в базе данные об играх в виде списка
            List<Game> games = [.. context.Games]; // context.Games.ToList()
            // сортируем список по айди
            List<Game> gamesSort = [.. games.OrderBy(p => p.GameId)]; // games.OrderBy(p => p.GameId).ToList()
            // возврашяем отсортированный список
            return gamesSort;
        }*/

        public static DataView GetListOfGames()
        {
            NpgsqlConnection con = new(ConnectionToDataBase.ConnectionString);
            con.Open();
            NpgsqlCommand cmd = new()
            {
                Connection = con,
                CommandText = $"select g.title, g.description, d.developername from games g, developers d where  g.developer_id = d.developer_id;"
            };
            cmd.ExecuteNonQuery();
            NpgsqlDataAdapter dataAdapter = new (cmd);
            DataTable dt = new("Games");
            dataAdapter.Fill(dt);
            con.Close();
            return dt.DefaultView;
        }

        public static DataView GetListOfPurchase(int id)
        {
            List<string[]> list = [];
            NpgsqlConnection con = new(connection);
            con.Open();
            NpgsqlCommand cmd = new()
            {
                Connection = con,
                CommandText = $"select g.title, p.date from games g join purchases p on g.game_id = p.game_id and p.user_id = {id};"
            };
            cmd.ExecuteNonQuery();
            NpgsqlDataAdapter dataAdapter = new (cmd);
            DataTable dt = new("Games");
            dataAdapter.Fill(dt);
            con.Close();
            return dt.DefaultView;
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
