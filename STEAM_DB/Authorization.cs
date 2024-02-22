namespace STEAM_DB
{
    internal class Authorization: ConnectionToDataBase
    {
        public static bool QueryCheckAvailability(string param) // проверка наличия в базе данных пользователя с данной почтой
        {
            // получаем строку подключения
            using SteamContext context = new (GetConnectionString());
            // проверяем наличия пользователя с заданной почтой
            bool result = context.Users.Any(u => u.Email == param);
            return result;
        }

        public static void AddUserInDB(string name, string email, string password, string date)
        {
            // создание пользователя
            User user = new() { Username = name, Email = email, Password = password, Registration = date};
            // создание объекта контекста данных
            using SteamContext context = new (GetConnectionString());
            // добавляем в бд
            context.Users.Add(user);
            // сохраняем изменения
            context.SaveChanges();
        }
    }
}
