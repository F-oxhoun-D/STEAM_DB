using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data;
using System.Windows;

namespace STEAM_DB
{
    internal static class DBHelper
    {
        internal static IEnumerable<dynamic> GetListOfGames() // получение списка игр
        {
            IEnumerable<dynamic> games = [];
            SteamContext context = PostgreSqlConnection.Context;
            try
            {
                games = context.Games.Join(context.Developers, g => g.DeveloperId, d => d.DeveloperId,
                    (g, d) => new
                    {
                        g.Title,
                        g.Description,
                        d.Developername
                    }).ToList();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { }
            return games;
        }

        internal static DataView GetListOfPurchases(in int id) // список покупок
        {
            DataTable dt = new();
            NpgsqlConnection con = PostgreSqlConnection.Connection;
            try
            {
                con.Open();
                NpgsqlCommand cmd = new()
                {
                    Connection = con,
                    CommandText = $"select g.title, p.date from games g join purchases p on g.game_id = p.game_id and p.user_id = {id};"
                };
                cmd.ExecuteNonQuery();
                NpgsqlDataAdapter dataAdapter = new(cmd);
                dt = new("Games");
                dataAdapter.Fill(dt);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { con.Close(); }

            return dt.DefaultView;
        }

        internal static IEnumerable<dynamic> GetListOfPurchasee(int id) // список покупок
        {
            List<Purchase>? purchases = [];
            try
            {
                SteamContext context = PostgreSqlConnection.Context;
                purchases = context.Purchases.Include(p => p.Game.GameId).OrderBy(p => p.PurchaseId).Where(p => p.UserId == id).ToList();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

            return purchases;
        }

        internal static List<string> GetListOfWishlist(in int id) // список избранного
        {
            List<string> list = [];
            NpgsqlConnection con = PostgreSqlConnection.Connection;
            try
            {
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
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { con.Close(); }

            return list;
        }

        private static void GetID(in string gameName, ref int gameId)
        {
            NpgsqlConnection con = PostgreSqlConnection.Connection;
            try
            {
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
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { con.Close(); }
        }

        internal static bool CheckThePurchase(in string gameName, ref int gameId) // проверка покупки (куплена ли данная игра или нет) 
        {
            object? obj = null;
            GetID(gameName, ref gameId);

            NpgsqlConnection con = PostgreSqlConnection.Connection;
            try
            {
                int userId = Global.user.UserId;
                con.Open();
                NpgsqlCommand cmd = new()
                {
                    Connection = con,
                    CommandText = $"select count(*) from purchases where game_id = {gameId} and user_id = {userId};"
                };

                obj = cmd.ExecuteScalar();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { con.Close(); }

            int count = -1;
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

            Purchase purchase = new() { PurchaseId = Id, PurchaseDate = dateString, GameId = gameId, UserId = userId };

            try
            {
                SteamContext context = PostgreSqlConnection.Context;
                context.Purchases.Add(purchase);
                context.SaveChanges();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        internal static bool CheckTheWishlist(in string gameName, ref int gameId) // проверка избранного
        {
            GetID(gameName, ref gameId);
            int userId = Global.user.UserId;

            object? obj = null;
            NpgsqlConnection con = PostgreSqlConnection.Connection;
            try
            {
                con.Open();
                NpgsqlCommand cmd = new()
                {
                    Connection = con,
                    CommandText = $"select count(*) from wishlist where game_id = {gameId} and user_id = {userId};"
                };
                
                obj = cmd.ExecuteScalar();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { con.Close(); }

            int count = -1;
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
            Wishlist wishlist = new() { WishlistId = Id, UserId = userId, GameId = gameId };

            try
            {
                SteamContext context = PostgreSqlConnection.Context;
                context.Wishlists.Add(wishlist);
                context.SaveChanges();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        internal static void RemoveFromWishlist(in string gameName, ref int gameID) // убрать из избранного
        {
            GetID(gameName, ref gameID);
            int gameId = gameID;
            int userId = Global.user.UserId;

            try
            {
                SteamContext context = PostgreSqlConnection.Context;
                var wishlist = context.Wishlists.SingleOrDefault(x => x.GameId == gameId && x.UserId == userId);
                if (wishlist != null)
                {
                    context.Wishlists.Remove(wishlist);
                    context.SaveChanges();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private static int GetNextID(in string sql) // получаем следующее айди 
        {
            int prevId = 0;

            NpgsqlConnection con = PostgreSqlConnection.Connection;
            try
            {
                con.Open();

                NpgsqlCommand cmd = new()
                {
                    Connection = con,
                    CommandText = sql
                };

                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                    prevId = reader.GetInt32(0);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { con.Close(); }

            return prevId + 1;
        }

        internal static bool CheckToReturnTheGame(string gameName, ref int gameId) // проверка возможности возврата игры
        {
            GetID(gameName, ref gameId);
            int userId = Global.user.UserId;

            TimeSpan timeDif;
            // количество дней со дня покупки
            int days = -1;

            NpgsqlConnection con = PostgreSqlConnection.Connection;
            try
            {
                con.Open();
                NpgsqlCommand cmd = new()
                {
                    Connection = con,
                    CommandText = $"select date from purchases where game_id = {gameId} and user_id = {userId};"
                };
                NpgsqlDataReader reader = cmd.ExecuteReader();

                string data = string.Empty;
                while (reader.Read())
                    data = reader.GetString(0);

                // сегодняшняя дата
                DateTime timeToday = DateTime.Today;
                // дата покупки
                DateTime timePurchase = Convert.ToDateTime(data);
                // разность дат
                timeDif = timeToday.Subtract(timePurchase);
                days = timeDif.Days;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { con.Close(); }
            
            
            if (days <= 14 && days != -1) // если прошло меньше двух недель, даём добро на возврат
                return true;
            else
                return false;
        }

        internal static void ReturnGame(int gameId) // возврат игры
        {
            int userId = Global.user.UserId;

            try
            {
                SteamContext context = PostgreSqlConnection.Context;
                // получаем объект из базы данных
                var purchase = context.Purchases.SingleOrDefault(x => x.GameId == gameId && x.UserId == userId);
                if (purchase != null)
                {
                    // удаляем и сохраняем изменения
                    context.Purchases.Remove(purchase);
                    context.SaveChanges();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        internal static DataView SQLQuery(string sql) // обработка любого запроса админа
        {
            DataTable dt = new();

            NpgsqlConnection con = PostgreSqlConnection.Connection;
            try
            {
                con.Open();

                NpgsqlCommand cmd = new()
                {
                    Connection = con,
                    CommandText = sql
                };
                cmd.ExecuteNonQuery();
                NpgsqlDataAdapter dataAdapter = new(cmd);
                dataAdapter.Fill(dt);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { con.Close(); }

            return dt.DefaultView;
        }
    }
}
