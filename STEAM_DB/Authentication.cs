using Microsoft.EntityFrameworkCore;

namespace STEAM_DB
{
    internal class Authentication
    {
        public static bool CheckThePassword(string name, string password)
        {
            using SteamContext context = new (ConnectionToDataBase.ConnectionStringOptions);
            bool result = context.Users.Any(x => x.Password == password && x.Username == name);
            return result;
        }

        public static void GetUser(string name, string password)
        {
            using SteamContext context = new(ConnectionToDataBase.ConnectionStringOptions);
            User user = context.Users.FirstOrDefault(x => x.Password == password && x.Username == name) ?? null!;
            Global.user = new();
            Global.user = user;
        }
    }
}
