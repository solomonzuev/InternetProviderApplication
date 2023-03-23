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
    /// Логика взаимодействия для TariffAddEditPage.xaml
    /// </summary>
    public partial class TariffAddEditPage : Page
    {
        private readonly Tariff _tariff;
        public TariffAddEditPage(Tariff tariff = null)
        {
            InitializeComponent();

            _tariff = tariff ?? new Tariff();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void BtnAddEdit_Click(object sender, RoutedEventArgs e)
        {
            if (IsTariffValid())
            {
                _tariff.Name = TextTariffName.Text;
                _tariff.PricePerDay = decimal.Parse(TextPricePerPay.Text);
                _tariff.InternetSpeed = int.Parse(TextInternetSpeed.Text);

                try
                {
                    TariffRepository.AddUpdateTariff(_tariff);
                    FrameManager.MainFrame.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool IsTariffValid()
        {
            if (string.IsNullOrWhiteSpace(TextTariffName.Text))
            {
                MessageBox.Show("Укажите корректное название тарифа");
                return false;
            }

            if (!decimal.TryParse(TextPricePerPay.Text, out decimal res1) || res1 < 0)
            {
                MessageBox.Show($"Введите корректную цену тарифа за день");
                return false;
            }

            if (!int.TryParse(TextInternetSpeed.Text, out int res2) || res2 < 0)
            {
                MessageBox.Show($"Введите корректную скорость интернета в МБ");
                return false;
            }

            return true;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (_tariff.Id == 0)
            {
                BtnAddEdit.Content = "Добавить";
            }
            else
            {
                TextTariffName.Text = _tariff.Name;
                TextPricePerPay.Text = _tariff.PricePerDay.ToString();
                TextInternetSpeed.Text = _tariff.InternetSpeed.ToString();

                BtnAddEdit.Content = "Редактировать";
            }
        }
    }
}
