using InternetProviderApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private User _user;
        public MainWindow(User user)
        {
            InitializeComponent();
            _user = user;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame = this.MainFrame;
            FrameManager.MainFrame.Navigate(new DashboardPage());
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            if (FrameManager.MainFrame.CanGoBack)
            {
                FrameManager.MainFrame.GoBack();
            }
        }

        private void MainFrame_ContentRendered(object sender, EventArgs e)
        {
            if (FrameManager.MainFrame.CanGoBack)
            {
                BtnBack.Visibility = Visibility.Visible;
            }
            else
            {
                BtnBack.Visibility = Visibility.Hidden;
            }
        }
    }
}
