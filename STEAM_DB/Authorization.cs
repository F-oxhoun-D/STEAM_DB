using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace STEAM_DB
{
    internal class Authorization: ConnectionToDataBase
    {
        public static bool CheckUserName(string userName)
        {
            /*using NpgsqlConnection con = new(ConnectionString);
            con.Open();
            NpgsqlCommand cmd = new()
            {
                Connection = con,
                CommandText = $"select count(username) from users where username = {userName};"
            };
            int count = -1;
            object? obj = cmd.ExecuteScalar();

            con.Dispose();
            con.Close();

            if (obj != null)
                count = (int)(long)obj;
            if (count > 0)
                return false;
            else
                return true;*/

            
            using SteamContext context = new(ConnectionStringOptions);
            bool result = context.Users.Any(u => u.Username == userName);
            context.Dispose();
            return result;
        }

        public static bool CheckEmail(string email) // проверка наличия в базе данных пользователя с данной почтой
        {
            // получаем строку подключения
            using SteamContext context = new (ConnectionStringOptions);
            // проверяем наличия пользователя с заданной почтой
            bool result = context.Users.Any(u => u.Email == email);
            return result;
        }

        public static void AddUserInDB(string name, string email, string password, string date)
        {
            int Id = GetNextID();
            // создание пользователя
            User user = new() { UserId = Id, Username = name, Email = email, Password = password, Registration = date};
            // создание объекта контекста данных
            using SteamContext context = new (ConnectionStringOptions);
            // добавляем в бд
            context.Users.Add(user);
            // сохраняем изменения
            context.SaveChanges();
        }

        private static int GetNextID() // получаем следующий айди для заполнения в бд
        {
            int prevId = 0;

            using NpgsqlConnection con = new(ConnectionString);
            con.Open();
            NpgsqlCommand cmd = new()
            {
                Connection = con,
                CommandText = "select user_id from users order by user_id desc limit 1;"
            };
            NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
                prevId = reader.GetInt32(0);

            con.Dispose();
            con.Close();

            return prevId + 1;
        }

        private static int GetNextId() // получаем следующий айди для заполнения в бд
        {
            int prevId = 0;

            

            return prevId + 1;
        }
    }
}
