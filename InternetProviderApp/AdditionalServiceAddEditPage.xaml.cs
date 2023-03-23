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
    /// Логика взаимодействия для AdditionalServiceAddEditPage.xaml
    /// </summary>
    public partial class AdditionalServiceAddEditPage : Page
    {
        private readonly AdditionalService _service;
        public AdditionalServiceAddEditPage(AdditionalService service = null)
        {
            InitializeComponent();
            _service = service ?? new AdditionalService();
        }

        private void BtnAddEdit_Click(object sender, RoutedEventArgs e)
        {
            if (IsServiceValid())
            {
                _service.Name = TextName.Text;
                _service.PricePerDay = decimal.Parse(TextPricePerDay.Text);
                _service.Description = TextDescription.Text;

                try
                {
                    AdditionalServiceRepository.AddUpdateAdditionalService(_service);
                    FrameManager.MainFrame.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool IsServiceValid()
        {
            if (string.IsNullOrWhiteSpace(TextName.Text))
            {
                MessageBox.Show("Название дополнительной услуги не может быть пустым!");
                return false;
            }

            if (!decimal.TryParse(TextPricePerDay.Text, out decimal res) || res < 0)
            {
                MessageBox.Show("Введите корректную цену за день в рублях!");
                return false;
            }

            return true;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (_service.Id == 0)
            {
                BtnAddEdit.Content = "Добавить";
            }
            else
            {
                TextName.Text = _service.Name;
                TextPricePerDay.Text = _service.PricePerDay.ToString();
                TextDescription.Text = _service.Description;
                BtnAddEdit.Content = "Редактировать";
            }
        }
    }
}
