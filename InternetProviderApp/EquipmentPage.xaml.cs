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
    /// Логика взаимодействия для EquipmentPage.xaml
    /// </summary>
    public partial class EquipmentPage : Page
    {
        private List<Equipment> _equipments;

        public EquipmentPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _equipments = EquipmentRepository.GetEquipments();
                LVEquipments.ItemsSource = _equipments;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new EquipmentAddEditPage());
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedEquipments = LVEquipments.SelectedItems.Cast<Equipment>().ToList();

            if (selectedEquipments.Count > 0)
            {
                var result = MessageBox.Show($"Вы уверены, что хотите удалить выбранные {selectedEquipments.Count} элементов?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        EquipmentRepository.DeleteEquipments(selectedEquipments);
                        DeleteFromItemsSource(selectedEquipments);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void DeleteFromItemsSource(List<Equipment> selectedEquipments)
        {
            _equipments.RemoveAll(x => selectedEquipments.Contains(x));

            LVEquipments.ItemsSource = null;
            LVEquipments.ItemsSource = _equipments;
        }

        private void TextSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextSearch.Text))
            {
                LVEquipments.ItemsSource = null;
                LVEquipments.ItemsSource = _equipments;
            }
            else
            {
                var filteredList = _equipments.Where(x => x.Name.ToUpper()
                                                            .Contains(TextSearch.Text.ToUpper())).ToList();

                LVEquipments.ItemsSource = null;
                LVEquipments.ItemsSource = filteredList;
            }
        }
    }
}
