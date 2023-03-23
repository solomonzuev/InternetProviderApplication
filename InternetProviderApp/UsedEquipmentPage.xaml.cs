using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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
    /// Логика взаимодействия для UsedEquipmentPage.xaml
    /// </summary>
    public partial class UsedEquipmentPage : Page
    {
        private readonly Models.Contract _contract;
        public UsedEquipmentPage(Models.Contract contract)
        {
            InitializeComponent();

            _contract = contract;
        }



        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new UsedEquipmentsAddEditPage());
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                DGridUsedEquipments.ItemsSource
                    = UsedEquipmentRepository.GetUsedEquipmentsFor(_contract);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is UsedEquipment usedEquipment)
            {
                FrameManager.MainFrame.Navigate(new UsedEquipmentsAddEditPage(usedEquipment));
            }
        }
    }
}
