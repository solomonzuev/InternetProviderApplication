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
    /// Логика взаимодействия для AdditionalServicePage.xaml
    /// </summary>
    public partial class AdditionalServicePage : Page
    {
        public AdditionalServicePage()
        {
            InitializeComponent();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new AdditionalServiceAddEditPage());
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                DGridServices.ItemsSource = AdditionalServiceRepository.GetAdditionalServices();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is AdditionalService service)
            {
                FrameManager.MainFrame.Navigate(new AdditionalServiceAddEditPage(service));
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedServices = DGridServices.SelectedItems.Cast<AdditionalService>().ToList();

            if (selectedServices.Count > 0)
            {
                var result = MessageBox.Show($"Вы уверены, что хотите удалить выбранные {selectedServices.Count} элементов?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        AdditionalServiceRepository.DeleteAdditionalServices(selectedServices);
                        DeleteFromItemsSource(selectedServices);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void DeleteFromItemsSource(List<AdditionalService> selectedServices)
        {
            if (DGridServices.ItemsSource is List<AdditionalService> services)
            {
                services.RemoveAll(x => selectedServices.Contains(x));
                DGridServices.ItemsSource = null;
                DGridServices.ItemsSource = services;
            }
        }
    }
}
