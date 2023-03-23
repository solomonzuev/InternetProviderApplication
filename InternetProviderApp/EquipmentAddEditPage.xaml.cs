using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.Win32;

namespace InternetProviderApp
{
    /// <summary>
    /// Логика взаимодействия для EquipmentAddEditPage.xaml
    /// </summary>
    public partial class EquipmentAddEditPage : Page
    {
        private readonly Equipment _equipment;

        public EquipmentAddEditPage(Equipment equipment = null)
        {
            InitializeComponent();
            _equipment = equipment ?? new Equipment();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (_equipment.Id == 0)
            {
                BtnAddEdit.Content = "Добавить";
            }
            else
            {
                TextEquipmentName.Text = _equipment.Name;
                TextPrice.Text = _equipment.Price.ToString();
                TextWarrantyInMonths.Text = _equipment.WarrantyInMonths.ToString();
                BtnAddEdit.Content = "Редактировать";
            }
            
            LoadImagePreviewOrDefault();
        }

        private void LoadImagePreviewOrDefault()
        {
            if (_equipment.ImagePreview == null)
            {
                ImagePreview.Source = 
                    new BitmapImage(new Uri("/Resources/item_picture.png", UriKind.Relative));
            }
            else
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = new MemoryStream(_equipment.ImagePreview);
                bitmap.EndInit();

                ImagePreview.Source = bitmap;
            }
        }

        private void BtnUpdateImagePreview_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                Filter = "Images |*.jpg;*.png"
            };

            if (ofd.ShowDialog() == true)
            {
                _equipment.ImagePreview = File.ReadAllBytes(ofd.FileName);
                LoadImagePreviewOrDefault();
            }
        }

        private void BtnAddEdit_Click(object sender, RoutedEventArgs e)
        {
            if (IsEquipmentValid())
            {
                _equipment.Name = TextEquipmentName.Text;
                _equipment.Price = decimal.Parse(TextPrice.Text);
                _equipment.WarrantyInMonths = short.Parse(TextWarrantyInMonths.Text);

                try
                {
                    EquipmentRepository.AddUpdateEquipment(_equipment);
                    FrameManager.MainFrame.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool IsEquipmentValid()
        {
            if (string.IsNullOrWhiteSpace(TextEquipmentName.Text))
            {
                MessageBox.Show("Название оборудование должно быть указано!");
                return false;
            }

            if (!decimal.TryParse(TextPrice.Text, out decimal res1) || res1 < 0) 
            {
                MessageBox.Show("Цена оборудования должна быть числом больше или равным 0!");
                return false;
            }

            if (!short.TryParse(TextWarrantyInMonths.Text, out short res2) || res2 < 0)
            {
                MessageBox.Show("Гарантийный период должен быть числом больше или равным 0!");
                return false;
            }

            return true;
        }
    }
}
