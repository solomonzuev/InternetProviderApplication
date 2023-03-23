using InternetProviderApp.Models;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InternetProviderApp
{
    /// <summary>
    /// Логика взаимодействия для UsedAdditionalServiceAddEditPage.xaml
    /// </summary>
    public partial class UsedAdditionalServiceAddEditPage : Page
    {
        private readonly UsedAdditionalService _usedService;
        public UsedAdditionalServiceAddEditPage(UsedAdditionalService usedService = null)
        {
            InitializeComponent();
            _usedService = usedService ?? new UsedAdditionalService();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (_usedService.Contract == null)
            {
                ComboContracts.ItemsSource = ContractRepository.GetContracts();
                ComboServices.ItemsSource = AdditionalServiceRepository.GetAdditionalServices();
                BtnAddEdit.Content = "Добавить";
            }
            else
            {
                ComboContracts.ItemsSource = new List<Contract> { _usedService.Contract };
                ComboContracts.SelectedIndex = 0;
                ComboContracts.IsReadOnly = true;

                ComboServices.ItemsSource = new List<AdditionalService> { _usedService.Service };
                ComboServices.SelectedIndex = 0;
                ComboServices.IsReadOnly = true;

                CBServiceWork.IsChecked = _usedService.IsServiceWork;

                PickerDateOfLastPayment.SelectedDate = _usedService.DateOfLastPayment;
                BtnAddEdit.Content = "Редактировать";
            }
        }

        private void BtnAddEdit_Click(object sender, RoutedEventArgs e)
        {
            if (IsUsedServiceValid())
            {
                _usedService.IsServiceWork = CBServiceWork.IsChecked.Value;
                _usedService.DateOfLastPayment = PickerDateOfLastPayment.SelectedDate.Value;

                try
                {
                    if (_usedService.Contract == null)
                    {
                        _usedService.Contract = ComboContracts.SelectedItem as Contract;
                        _usedService.Service = ComboServices.SelectedItem as AdditionalService;
                        UsedAdditionalServiceRepository.AddUsedAdditionalService(_usedService);
                    }
                    else
                    {
                        UsedAdditionalServiceRepository.UpdateUsedAdditionalService(_usedService);
                    }
                    FrameManager.MainFrame.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool IsUsedServiceValid()
        {
            if (ComboContracts.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите договор из списка!");
                return false;
            }

            if (ComboServices.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите услугу из списка!");
                return false;
            }

            if (CBServiceWork.IsChecked == null)
            {
                MessageBox.Show("Отметьте, активна ли сейчас услуга!");
                return false;
            }

            if (PickerDateOfLastPayment.SelectedDate == null)
            {
                MessageBox.Show("Укажите корректную дату последнего платежа!");
                return false;
            }

            return true;
        }
    }
}
