using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.Pkcs;
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

        internal static DataView GetListOfGames() // получение списка игр
        { 
            // подключение к бд
            using NpgsqlConnection con = new(ConnectionToDataBase.ConnectionString);
            con.Open();
            // задаём команду
            NpgsqlCommand cmd = new()
            {
                Connection = con,
                CommandText = $"select g.title, g.description, d.developername from games g, developers d where  g.developer_id = d.developer_id;"
            };
            // выполняем запрос
            cmd.ExecuteNonQuery();
            // для создания таблицы
            NpgsqlDataAdapter dataAdapter = new (cmd);
            DataTable dt = new("Games");
            dataAdapter.Fill(dt);
            // закрываем подключение
            con.Dispose();
            con.Close();
            // возвращяем объект
            return dt.DefaultView;
        }

        internal static DataView GetListOfPurchase(in int id) // список покупок
        {
            List<string[]> list = [];
            using NpgsqlConnection con = new(connection);
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
            con.Dispose();
            con.Close();
            return dt.DefaultView;
        }

        internal static List<string> GetListOfWishlist(in int id) // список избранного
        {
            List<string> list = [];
            using NpgsqlConnection con = new(connection);
            con.Open();
            NpgsqlCommand cmd = new()
            {
                Connection = con,
                CommandText = $"select g.title from games g, wishlist w where w.user_id = {id} and g.game_id = w.game_id;"
            };
            // чтение данных
            NpgsqlDataReader reader = cmd.ExecuteReader();
            // пока данные есть выполняем запись
            while (reader.Read())
            {
                list.Add(reader.GetString(0));
            }
            con.Dispose();
            con.Close();
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

        private static void GetID(in string gameName, ref int gameId)
        {
            using NpgsqlConnection con = new(connection);
            con.Open();
            NpgsqlCommand cmd = new()
            {
                Connection = con,
                CommandText = $"select game_id from games where title = '{gameName}';"
            };
            NpgsqlDataReader reader = cmd.ExecuteReader();
            int userId = Global.user.UserId;
            while (reader.Read())
            {
                gameId = reader.GetInt32(0);
            }
            con.Dispose();
            con.Close();
        }

        internal static bool CheckThePurchase(in string gameName, ref int gameId) // проверка покупки (куплена ли данная игра или нет) 
        {
            GetID(gameName, ref gameId);
            int userId = Global.user.UserId;
            using NpgsqlConnection con = new(connection);
            con.Open();
            NpgsqlCommand cmd = new()
            {
                Connection = con,
                CommandText = $"select count(*) from purchases where game_id = {gameId} and user_id = {userId};"
            };
            int count = -1;
            object? obj = cmd.ExecuteScalar();

            con.Dispose();
            con.Close();

            if (obj != null)
                count = (int)(long)obj;
            if (count > 0)
                return true;
            else
                return false;
        }

        internal static void BuyGame(in int gameId) // покупка игры
        {
            // сегодняшняя дата
            DateTime date = DateTime.Today;
            // конвертируем в строку вида yyyy-MM-dd
            string dateString = date.ToString("yyyy-MM-dd");

            int userId = Global.user.UserId;

            Purchase purchase = new() { PurchaseDate =  dateString, GameId = gameId, UserId = userId };

            using SteamContext context = new(options);
            context.Purchases.Add(purchase);
            context.SaveChanges();
            context.Dispose();
        }

        internal static bool CheckTheWishlist(in string gameName, ref int gameId) // проверка избранного
        {
            GetID(gameName, ref gameId);
            int userId = Global.user.UserId;
            using NpgsqlConnection con = new(connection);
            con.Open();
            NpgsqlCommand cmd = new()
            {
                Connection = con,
                CommandText = $"select count(*) from wishlist where game_id = {gameId} and user_id = {userId};"
            };
            int count = -1;
            object? obj = cmd.ExecuteScalar();

            con.Dispose();
            con.Close();

            if (obj != null)
                count = (int)(long)obj;
            if (count > 0)
                return true;
            else
                return false;
        }

        internal static void AddToWishlist(in int gameId)
        {
            int userId = Global.user.UserId;
            Wishlist wishlist = new() { UserId = userId, GameId = gameId};

            using SteamContext context = new(options);
            context.Wishlists.Add(wishlist);
            context.SaveChanges();  
            context.Dispose();
        }
    }
}
