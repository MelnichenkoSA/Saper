using Saper.View;
using Saper.ViewModel;
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

namespace Saper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
        private void Option_Click(object sender, RoutedEventArgs e)
        {
            Window1 passwordWindow = new Window1();

            if (passwordWindow.ShowDialog() == true)
            {
                MessageBox.Show("Сложность установлена");
            }
            else
            {
                MessageBox.Show("Авторизация не пройдена");
            }
        }
    }
}
