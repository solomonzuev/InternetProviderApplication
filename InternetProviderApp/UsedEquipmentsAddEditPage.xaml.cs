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
    /// Логика взаимодействия для UsedEquipmentsAddEditPage.xaml
    /// </summary>
    public partial class UsedEquipmentsAddEditPage : Page
    {
        private readonly UsedEquipment _usedEquipment;
        public UsedEquipmentsAddEditPage(UsedEquipment usedEquipment = null)
        {
            InitializeComponent();
            _usedEquipment = usedEquipment ?? new UsedEquipment();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (_usedEquipment.Contract == null)
            {
                ComboContracts.ItemsSource = ContractRepository.GetContracts();
                ComboEquipments.ItemsSource = EquipmentRepository.GetEquipments();
                BtnAddEdit.Content = "Добавить";
            }
            else
            {
                ComboContracts.ItemsSource = new List<Contract> { _usedEquipment.Contract };
                ComboContracts.SelectedIndex = 0;
                ComboContracts.IsReadOnly = true;

                ComboEquipments.ItemsSource = new List<Equipment> { _usedEquipment.Equipment };
                ComboEquipments.SelectedIndex = 0;
                ComboEquipments.IsReadOnly = true;
 
                PickerStartDate.SelectedDate = _usedEquipment.StartDate;
                BtnAddEdit.Content = "Редактировать";
            }
           
            
           
        }

        private void BtnAddEdit_Click(object sender, RoutedEventArgs e)
        {
            if (IsUsedEquipmentValid())
            {
                _usedEquipment.StartDate = PickerStartDate.SelectedDate.Value;

                try
                {
                    if (_usedEquipment.Contract == null)
                    {
                        _usedEquipment.Contract = ComboContracts.SelectedItem as Contract;
                        _usedEquipment.Equipment = ComboEquipments.SelectedItem as Equipment;
                        UsedEquipmentRepository.AddUsedEquipment(_usedEquipment);
                    }
                    else
                    {
                        UsedEquipmentRepository.UpdateUsedEquipment(_usedEquipment);
                    }
                    FrameManager.MainFrame.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool IsUsedEquipmentValid()
        {
            if (ComboContracts.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите договор из списка!");
                return false;
            }

            if (ComboEquipments.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите оборудование из списка!");
                return false;
            }

            if (PickerStartDate.SelectedDate == null)
            {
                MessageBox.Show("Указана некорректная дата!");
                return false;
            }

            return true;
        }
    }
}
