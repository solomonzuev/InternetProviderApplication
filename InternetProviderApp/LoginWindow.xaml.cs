using InternetProviderApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = TextUsername.Text;
            string password = GetPasswordHash(TextPassword.Password);

            string usernameErrors = ValidateUsername(username);

            if (usernameErrors.Length > 0)
            {
                MessageBox.Show(usernameErrors, Title, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Пароль не может быть пустым!", Title, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var user = UserRepository.GetUsers().SingleOrDefault(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                var mainWindow = new MainWindow(user);
                mainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Пользователь не существует!");
            }
        }
    }
}
