namespace STEAM_DB
{
    internal class AuthorizationDB: ConnectionToDataBase
    {
        public static bool QueryCheckAvailability(string param) // проверка наличия в базе данных пользователя с данной почтой
        {
            using SteamContext context = new (GetConnectionString());
            bool result = context.Users.Any(u => u.Email == param);
            return result;
        }

        public static void AddUserInDB(string name, string email, string password, string date)
        {
            User user = new() { Username = name, Email = email, Password = password, Registration = date};
            using SteamContext context = new (GetConnectionString());
            context.Users.Add(user);
            context.SaveChanges();
        }
    }
}
