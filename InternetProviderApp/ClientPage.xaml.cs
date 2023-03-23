using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using InternetProviderApp.Models;
using InternetProviderApp.Repositories;

namespace InternetProviderApp
{
    /// <summary>
    /// Логика взаимодействия для ClientPage.xaml
    /// </summary>
    public partial class ClientPage : Page
    {
        public ClientPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                DGridClients.ItemsSource = ClientRepository.GetClients();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new ClientAddEditPage());
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is Client client)
            {
                FrameManager.MainFrame.Navigate(new ClientAddEditPage(client));
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedClients = DGridClients.SelectedItems.Cast<Client>().ToList();

            if (selectedClients.Count > 0)
            {
                var result = MessageBox.Show($"Вы уверены, что хотите удалить выбранные {selectedClients.Count} элементов?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        ClientRepository.DeleteClients(selectedClients);
                        DeleteFromItemsSource(selectedClients);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void DeleteFromItemsSource(List<Client> selectedClients)
        {
            if (DGridClients.ItemsSource is List<Client> clients)
            {
                clients.RemoveAll(e => selectedClients.Contains(e));
                DGridClients.ItemsSource = null;
                DGridClients.ItemsSource = clients;
            }
        }
    }
}
