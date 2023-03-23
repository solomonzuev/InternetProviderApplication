using System;
using System.Collections.Generic;
using System.Configuration;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using InternetProviderApp.Models;
using InternetProviderApp.Repositories;

namespace InternetProviderApp
{
    /// <summary>
    /// Логика взаимодействия для ClientAddEditPage.xaml
    /// </summary>
    public partial class ClientAddEditPage : Page
    {
        private const int DAYS_IN_YEAR = 365;
        private const int MIN_AGE = 14;
        private readonly Client _client;
        public ClientAddEditPage(Client client = null)
        {
            InitializeComponent();
            if (client == null)
            {
                _client = new Client();
            }
            else
            {
                _client = client;
            }
        }

        private void BtnAddEdit_Click(object sender, RoutedEventArgs e)
        {
            if (IsClientValid())
            {
                _client.FullName = TextFullName.Text;
                _client.HomeAddress = TextHomeAddress.Text;
                _client.PassportNumber = TextPassportNumber.Text;
                _client.CodPorazdeleniya = TextCodPodrazdeleniya.Text;
                _client.DateOfBirth = PickerDateOfBirth.SelectedDate.Value;
                _client.PhoneNumber = TextPhoneNumber.Text == string.Empty ? null : TextPhoneNumber.Text;
                _client.Email = TextEmail.Text == string.Empty ? null : TextEmail.Text;

                try
                {
                    ClientRepository.AddUpdateClient(_client);
                    FrameManager.MainFrame.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool IsClientValid()
        {
            if (string.IsNullOrWhiteSpace(TextFullName.Text))
            {
                MessageBox.Show("ФИО клиента должно быть заполнено");
                return false;
            }

            if (string.IsNullOrWhiteSpace(TextHomeAddress.Text))
            {
                MessageBox.Show("Адрес клиента должен быть заполнен");
                return false;
            }

            if (!IsPassportNumberValid())
            {
                MessageBox.Show("Введите серию и номер паспорта в формате: xx xx xxxxxx");
                return false;
            }

            if (!IsCodPodrazdeleniyaValid())
            {
                MessageBox.Show("Введите код подразделения в формате: xxx-xxx");
                return false;
            }

            if (PickerDateOfBirth.SelectedDate == null ||
                (DateTime.Now.Subtract(PickerDateOfBirth.SelectedDate.Value).TotalDays / DAYS_IN_YEAR) < MIN_AGE)
            {
                MessageBox.Show("Дата не указана или ваш возраст меньше 14 лет и вы не можете пользоваться услугами нашей компании");
                return false;
            }

            if (!string.IsNullOrWhiteSpace(TextPhoneNumber.Text) 
                && TextPhoneNumber.Text.Length != 12)
            {
                MessageBox.Show("Номер телефона должен быть в формате: +79876543219");
                return false;
            }

            return true;
        }

        private bool IsPassportNumberValid()
        {
            var regex = new Regex(@"[0-9]{2} [0-9]{2} [0-9]{6}");
            return regex.IsMatch(TextPassportNumber.Text);
        }

        private bool IsCodPodrazdeleniyaValid()
        {
            var regex = new Regex(@"[0-9]{3}-[0-9]{3}");
            return regex.IsMatch(TextCodPodrazdeleniya.Text);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (_client.Id != 0)
            {
                TextFullName.Text = _client.FullName;
                TextHomeAddress.Text = _client.HomeAddress;
                TextPassportNumber.Text = _client.PassportNumber;
                TextCodPodrazdeleniya.Text = _client.CodPorazdeleniya;
                PickerDateOfBirth.SelectedDate = _client.DateOfBirth;
                TextPhoneNumber.Text = _client.PhoneNumber;
                TextEmail.Text = _client.Email;
            }
        }
    }
}
