using System.Security.Cryptography;
using System.Text;

namespace STEAM_DB
{
    internal static class Hash
    {
        public static string GetHash(string password)
        {
            // преобразуем строку в массив байтов
            byte[] inputBytes = Encoding.ASCII.GetBytes(password);
            // получаем хэш в виде массива байтов
            byte[] hash1 = MD5.HashData(inputBytes);
            // преобразуем хэш из массива в строку, состоящую из шестнадцатеричных символов в верхнем регистре
            return Convert.ToHexString(hash1);
        }
    }
}
