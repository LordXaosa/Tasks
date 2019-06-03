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
using System.Windows.Shapes;

namespace DetegoRFID.Views
{
    /// <summary>
    /// Interaction logic for ExceptionWindow.xaml
    /// </summary>
    public partial class ExceptionWindow : Window
    {
        public ExceptionWindow()
        {
            InitializeComponent();
        }
        private void BT_InnerException(object sender, RoutedEventArgs e)
        {
            if (DataContext is Exception ex && ex.InnerException != null)
            {
                (new ExceptionWindow() { DataContext = ex.InnerException }).Show();
            }
        }

        private void BT_OK(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
