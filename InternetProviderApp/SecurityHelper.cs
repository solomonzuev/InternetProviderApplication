using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Windows;

namespace InternetProviderApp
{
    public static class SecurityHelper
    {
        public static string GetPasswordHash(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                return string.Empty;
            }

            var sb = new StringBuilder();

            // Начинаем работу с хешированием
            using (var sha256Hash = SHA256.Create())
            {
                // Рассчитываем хеш для пароля
                var data = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                
                // Преобразовываем массив byte в строку
                foreach (var b in data)
                {
                    sb.Append(b.ToString("x2"));
                }
            }

            // Возвращаем полученную строку с хешем пароля
            return sb.ToString();
        }

        public static string ValidateUsername(string username) 
        {
            var sb = new StringBuilder();

            if (string.IsNullOrWhiteSpace(username))
            {
                sb.AppendLine("Имя пользователя не может быть пустым!");
            }
            if (username.Length > 50)
            {
                sb.AppendLine("Длина имени пользователя должна быть меньше или равна 50!");
            }
            if (Regex.IsMatch(username, "[^0-9A-Za-z._]"))
            {
                sb.AppendLine("Имя пользователя может состоять только из цифр, букв латинского алфавита, символов '.' и '_'");
            }

            return sb.ToString();
        }

        public static string ValidateConfirmPassword(string password, string passwordConfirm)
        {
            var sb = new StringBuilder();

            if (string.IsNullOrWhiteSpace(password))
            {
               sb.Append("Пароль не может быть пустым!");
            }
            if (string.IsNullOrWhiteSpace(passwordConfirm))
            {
                sb.Append("Подтверждение пароля не может быть пустым!");
            }
            if (!string.Equals(password, passwordConfirm, StringComparison.OrdinalIgnoreCase))
            {
                sb.Append("Пароль и подтверждение пароля не совпадают!");
            }

            return sb.ToString();
        }
    }
}
