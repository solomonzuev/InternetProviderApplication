using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
    /// Логика взаимодействия для TariffPage.xaml
    /// </summary>
    public partial class TariffPage : Page
    {
        public TariffPage()
        {
            InitializeComponent();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is Tariff tariff)
            {
                FrameManager.MainFrame.Navigate(new TariffAddEditPage(tariff));
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new TariffAddEditPage());
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                DGridTariffs.ItemsSource = TariffRepository.GetTariffs();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedTariffs = DGridTariffs.SelectedItems.Cast<Tariff>().ToList();

            if (selectedTariffs.Count > 0)
            {
                var result = MessageBox.Show($"Вы уверены, что хотите удалить выбранные {selectedTariffs.Count} элементов?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        TariffRepository.DeleteTariffs(selectedTariffs);
                        DeleteFromItemsSource(selectedTariffs);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void DeleteFromItemsSource(List<Tariff> selectedTariffs)
        {
            if (DGridTariffs.ItemsSource is List<Tariff> tariffs)
            {
                tariffs.RemoveAll(t => selectedTariffs.Contains(t));
                DGridTariffs.ItemsSource = null;
                DGridTariffs.ItemsSource = tariffs;
            }
        }
    }
}
