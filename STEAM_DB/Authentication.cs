namespace STEAM_DB
{
    internal class Authentication: ConnectionToDataBase
    {
        public static bool CheckThePassword(string name, string password)
        {
            using SteamContext context = new (GetConnectionString());
            bool result = context.Users.Any(x => x.Password == password && x.Username == name);
            return result;
        }
    }
}
