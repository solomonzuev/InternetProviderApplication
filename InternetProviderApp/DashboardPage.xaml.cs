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
    /// Логика взаимодействия для DashboardPage.xaml
    /// </summary>
    public partial class DashboardPage : Page
    {
        public DashboardPage()
        {
            InitializeComponent();
        }

        public void BtnTariffs_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new TariffPage());
        }

        private void BtnClients_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new ClientPage());
        }

        private void BtnContracts_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new ContractsPage());
        }

        private void BtnEquipments_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new EquipmentPage());
        }

        private void BtnAdditionalServices_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new AdditionalServicePage());
        }
    }
}
