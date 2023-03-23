using InternetProviderApp.Models;
using InternetProviderApp.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static InternetProviderApp.SecurityHelper;

namespace InternetProviderApp
{
    /// <summary>
    /// Логика взаимодействия для SignupWindow.xaml
    /// </summary>
    public partial class SignupWindow : Window
    {
        public SignupWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private void SignupButton_Click(object sender, RoutedEventArgs e)
        {
            string usernameErrors = ValidateUsername(TextUsername.Text);

            if (usernameErrors.Length > 0)
            {
                MessageBox.Show(usernameErrors, Title, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string passwordErrors = ValidateConfirmPassword(TextPassword.Password, TextConfirmPassword.Password);

            if (passwordErrors.Length > 0)
            {
                MessageBox.Show(passwordErrors, Title, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var newUser = new User
            {
                Username = TextUsername.Text,
                Password = GetPasswordHash(TextPassword.Password),
                Role = "Клиент"
            };

            try
            {
                UserRepository.AddUser(newUser);
                MessageBox.Show("Пользователь успешно добавлен!", Title, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (SqlException)
            {
                MessageBox.Show("Пользователь с таким именем пользователя уже существует в базе данных!",
                    Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
