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
    /// Логика взаимодействия для UsedAdditionalServicePage.xaml
    /// </summary>
    public partial class UsedAdditionalServicePage : Page
    {
        private readonly Models.Contract _contract;
        public UsedAdditionalServicePage(Contract contract)
        {
            InitializeComponent();
            _contract = contract;
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new UsedAdditionalServiceAddEditPage());
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                DGridUsedServices.ItemsSource
                    = UsedAdditionalServiceRepository.GetUsedServicesFor(_contract);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is UsedAdditionalService usedService)
            {
                FrameManager.MainFrame.Navigate(new UsedAdditionalServiceAddEditPage(usedService));
            }
        }
    }
}
