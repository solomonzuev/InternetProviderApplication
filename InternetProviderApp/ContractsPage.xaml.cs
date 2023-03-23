using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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
    /// Логика взаимодействия для ContractsPage.xaml
    /// </summary>
    public partial class ContractsPage : Page
    {
        public ContractsPage()
        {
            InitializeComponent();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new ContractAddEditPage());
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedContracts = DGridContracts.SelectedItems.Cast<Contract>().ToList();

            if (selectedContracts.Count > 0)
            {
                var result = MessageBox.Show($"Вы уверены, что хотите удалить выбранные {selectedContracts.Count} элементов?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        ContractRepository.DeleteContracts(selectedContracts);
                        DeleteFromItemsSource(selectedContracts);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void DeleteFromItemsSource(List<Contract> selectedContracts)
        {
            if (DGridContracts.ItemsSource is List<Contract> contracts)
            {
                contracts.RemoveAll(c => selectedContracts.Contains(c));
                DGridContracts.ItemsSource = null;
                DGridContracts.ItemsSource = contracts;
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is Contract contract)
            {
                FrameManager.MainFrame.Navigate(new ContractAddEditPage(contract));
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                DGridContracts.ItemsSource = ContractRepository.GetContracts();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnEditUsedEquipments_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is Contract contract)
            {
                FrameManager.MainFrame.Navigate(new UsedEquipmentPage(contract));
            }
        }

        private void BtnUsedServices_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is Contract contract)
            {
                FrameManager.MainFrame.Navigate(new UsedAdditionalServicePage(contract));
            }
        }
    }
}
