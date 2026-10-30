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
    /// Логика взаимодействия для SignUpPage.xaml
    /// </summary>
    public partial class SignUpPage : Page
    {
        private Service _currentService = new Service();
        public SignUpPage(Service SelectedService)
        {
            InitializeComponent();
            if (SelectedService != null)
                this._currentService = SelectedService;

            DataContext = _currentService;

            var _currentClient = Kuznecov_DBEntities.GetContext().Client.ToList();
            ComboClient.ItemsSource = _currentClient;

        }

        private ClientService _currentClientService = new ClientService();

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (ComboClient.SelectedIndex == -1)
                errors.AppendLine("Укажите ФИО клиента");
            if (StartDate.Text == "")
                errors.AppendLine("Укажите дату услуги");
            if (TBStart.Text == "")
                errors.AppendLine("Укжаите начало услуги");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
            }

            _currentClientService.ClientID = ComboClient.SelectedIndex + 1;
            _currentClientService.ServiceID = _currentService.ID;
            _currentClientService.StartTime = Convert.ToDateTime(StartDate.Text + " " + TBStart.Text);
            if (_currentClientService.ID == 0)
                Kuznecov_DBEntities.GetContext().ClientService.Add(_currentClientService);

            try
            {
                Kuznecov_DBEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена");
                Mananger.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }


        }

        private void TBStart_TextChanged(object sender, TextChangedEventArgs e)
        {
            string s = TBStart.Text;

            if (s.Length < 4 || !s.Contains(':'))
            {
                TBEnd.Text = "";
                return;
            }

            string[] start = s.Split(':');

            int hour;
            int minute;

            if (start.Length != 2 ||
                !int.TryParse(start[0], out hour) ||
                !int.TryParse(start[1], out minute) ||
                hour < 0 || hour > 23 ||
                minute < 0 || minute > 59)
            {
                TBEnd.Text = "";
                return;
            }

            int startHour = hour * 60;
            int startMin = minute;

            int sum = startHour + startMin + _currentService.DurationIn;

            // Ограничиваем время в пределах суток
            int EndHour = (sum / 60) % 24;
            int EndMin = sum % 60;

            // Всегда выводим часы и минуты двумя цифрами
            TBEnd.Text = EndHour.ToString("D2") + ":" + EndMin.ToString("D2");


        }

        private void TBStart_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.All(c => (c >= '0' && c <= '9') || c == ':');
        }

        private void TBStart_Copying(object sender, DataObjectCopyingEventArgs e)
        {
            if (!e.DataObject.GetDataPresent(DataFormats.UnicodeText))
            {
                e.CancelCommand();
                return;
            }

            string text =
                e.DataObject.GetData(DataFormats.UnicodeText) as string;

            if (string.IsNullOrEmpty(text) ||
                text.Any(c => (c < '0' || c > '9') && c != ':'))
            {
                e.CancelCommand();
            }
        }
    }
}
