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
using System.Windows.Navigation;
using System.Windows.Shapes;
using InternetProviderApp.Models;
using InternetProviderApp.Repositories;

namespace InternetProviderApp
{
    /// <summary>
    /// Логика взаимодействия для ContractAddEditPage.xaml
    /// </summary>
    public partial class ContractAddEditPage : Page
    {
        private readonly Contract _contract;

        public ContractAddEditPage(Contract contract = null)
        {
            InitializeComponent();

            _contract = contract ?? new Contract();
        }

        private void BtnAddEdit_Click(object sender, RoutedEventArgs e)
        {
            if (IsContractValid())
            {
                _contract.Client = (ComboClients.SelectedItem as Client);
                _contract.Tariff = (ComboTariffs.SelectedItem as Tariff);
                _contract.Balance = decimal.Parse(TextBalance.Text);
                _contract.Discount = short.Parse(TextDiscount.Text);
                _contract.IsTariffWork = CBTariffWork.IsChecked ?? false;
                _contract.DateOfLastPayment = PickerDateOfLastPayment.SelectedDate.Value;
                
                try
                {
                    ContractRepository.AddUpdateContract(_contract);
                    FrameManager.MainFrame.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool IsContractValid()
        {
            if (ComboClients.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите клиента из списка");
                return false;
            }

            if (ComboTariffs.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите тариф из списка");
                return false;
            }

            if (!decimal.TryParse(TextBalance.Text, out decimal res1) || res1 < 0)
            {
                MessageBox.Show("Баланс не может быть отрицательным");
                return false;
            }

            if (!short.TryParse(TextDiscount.Text, out short res2) || res2 < 0 || res2 > 100)
            {
                MessageBox.Show("Скидка должна быть в диапазоне от 0 до 100 %");
                return false;
            }

            if (CBTariffWork.IsChecked == null)
            {
                MessageBox.Show("Состояние работы тарифа не может быть неопределено");
                return false;
            }

            if (PickerDateOfLastPayment.SelectedDate == null 
                || PickerDateOfLastPayment.SelectedDate.Value > DateTime.Now)
            {
                MessageBox.Show("Дата последнего платежа не может быть позднее текущего дня");
                return false;
            }

            return true;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var clients = ClientRepository.GetClients();
            var tariffs = TariffRepository.GetTariffs();

            ComboClients.ItemsSource = clients;
            ComboTariffs.ItemsSource = tariffs;

            if (_contract.Number == 0)
            {
                BtnAddEdit.Content = "Добавить";
            }
            else
            {
                TextBalance.Text = _contract.Balance.ToString();
                TextDiscount.Text = _contract.Discount.ToString();
                CBTariffWork.IsChecked = _contract.IsTariffWork;
                PickerDateOfLastPayment.SelectedDate = _contract.DateOfLastPayment;
                ComboClients.SelectedItem = clients.First(x => x.Id == _contract.Client.Id);
                ComboTariffs.SelectedItem = tariffs.First(x => x.Id == _contract.Tariff.Id);
                BtnAddEdit.Content = "Редактировать";
            }
        }
    }
}
