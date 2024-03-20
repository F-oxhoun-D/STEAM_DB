using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data;
using System.Data.Common;

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

        /*internal static DataView GetListOfGames() // получение списка игр
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
        }*/

        internal static IEnumerable<dynamic> GetListOfGames() // получение списка игр
        {
            using SteamContext context = new (options);
            var games = context.Games.Join(context.Developers, g => g.DeveloperId, d => d.DeveloperId,
                (g, d) => new
                {
                    g.Title,
                    g.Description,
                    d.Developername
                }).ToList();
            return games;
        }

        internal static DataView GetListOfPurchases(in int id) // список покупок
        {
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

        internal static IEnumerable<dynamic> GetListOfPurchasee(int id) // список покупок
        {
            using SteamContext context = new (options);
            /*var purchases = context.Purchases.Join(context.Games, p => p.GameId, g => g.GameId,
                (p, g) => new
                {
                    g.Title,
                    p.PurchaseDate
                }).ToList();*/
            var purchases = context.Purchases.Include(p => p.Game.GameId).OrderBy(p => p.PurchaseId).Where(p => p.UserId == id).ToList();    
            return purchases;
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
            string sql = "select purchase_id from purchases order by purchase_id desc limit 1;";
            int Id = GetNextID(sql);
            // сегодняшняя дата
            DateTime date = DateTime.Today;
            // конвертируем в строку вида yyyy-MM-dd
            string dateString = date.ToString("yyyy-MM-dd");

            int userId = Global.user.UserId;

            Purchase purchase = new() { PurchaseId = Id, PurchaseDate =  dateString, GameId = gameId, UserId = userId };

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

        internal static void AddToWishlist(in int gameId) // добавить в избранное
        {
            string sql = "select wishlist_id from wishlist order by wishlist_id desc limit 1;";
            int Id = GetNextID(sql);

            int userId = Global.user.UserId;
            Wishlist wishlist = new() { WishlistId = Id, UserId = userId, GameId = gameId};

            using SteamContext context = new(options);
            context.Wishlists.Add(wishlist);
            context.SaveChanges();  
            context.Dispose();
        }

        internal static void RemoveFromWishlist(in string gameName, ref int gameID) // убрать из избранного
        {
            GetID(gameName, ref gameID);
            int gameId = gameID;
            int userId = Global.user.UserId;    

            using SteamContext context = new(options);
            var wishlist = context.Wishlists.SingleOrDefault(x => x.GameId == gameId && x.UserId == userId);
            if (wishlist != null)
            {
                context.Wishlists.Remove(wishlist);
                context.SaveChanges();
            }
            context.Dispose();
        }

        private static int GetNextID(in string sql) // получаем следующее айди 
        {
            int prevId = 0;

            NpgsqlConnection con = new(connection);
            con.Open();

            NpgsqlCommand cmd = new()
            {
                Connection = con,
                CommandText = sql
            };

            NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
                prevId = reader.GetInt32(0);

            return prevId + 1;
        }

        internal static bool CheckToReturnTheGame(string gameName, ref int gameId) // проверка возможности возврата игры
        {
            GetID(gameName, ref gameId);
            int userId = Global.user.UserId;

            NpgsqlConnection con = new(connection); 
            con.Open();
            NpgsqlCommand cmd = new()
            {
                Connection = con,
                CommandText = $"select date from purchases where game_id = {gameId} and user_id = {userId};"
            };
            NpgsqlDataReader reader = cmd.ExecuteReader();

            string data = string.Empty;
            while(reader.Read())
                data = reader.GetString(0);

            // сегодняшняя дата
            DateTime timeToday = DateTime.Today;
            // дата покупки
            DateTime timePurchase = Convert.ToDateTime(data);
            // разность дат
            TimeSpan timeDif = timeToday.Subtract(timePurchase);
            // количество дней со дня покупки
            int days = timeDif.Days;
            if (days <= 14) // если прошло меньше двух недель, даём добро на возврат
                return true;
            else
                return false;
        }

        internal static void ReturnGame(int gameId) // возврат игры
        {
            int userId = Global.user.UserId;

            using SteamContext context = new(options);
            // получаем объект из базы данных
            var purchase = context.Purchases.SingleOrDefault(x => x.GameId == gameId && x.UserId == userId);
            if (purchase != null)
            {
                // удаляем и сохраняем изменения
                context.Purchases.Remove(purchase);
                context.SaveChanges();
            }
            context.Dispose();
        }

        internal static DataView SQLQuery(string sql) // обработка любого запроса админа
        {
            using NpgsqlConnection con = new (connection);
            con.Open();

            NpgsqlCommand cmd = new()
            {
                Connection = con,
                CommandText = sql
            };
            cmd.ExecuteNonQuery();
            NpgsqlDataAdapter dataAdapter = new(cmd);
            DataTable dt = new();
            dataAdapter.Fill(dt);
            con.Dispose();
            con.Close();
            return dt.DefaultView;
        }
    }
}
