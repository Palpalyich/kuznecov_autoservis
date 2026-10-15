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

namespace kuznecov
{
    /// <summary>
    /// Логика взаимодействия для ServisPage.xaml
    /// </summary>
    public partial class ServisPage : Page
    {
        public ServisPage()
        {
            InitializeComponent();

            var currentServices = Kuznecov_DBEntities.GetContext().Service.ToList();

            ServiceListView.ItemsSource = currentServices;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Mananger.MainFrame.Navigate(new AddEditPage());

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
