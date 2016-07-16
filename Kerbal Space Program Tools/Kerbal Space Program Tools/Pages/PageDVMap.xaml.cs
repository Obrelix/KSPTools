using Kerbal_Space_Program_Tools.Windows;
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

namespace Kerbal_Space_Progam_Tools.Pages
{
    /// <summary>
    /// Interaction logic for PageDVMap.xaml
    /// </summary>
    public partial class PageDVMap : Page
    {
        public PageDVMap()
        {
            InitializeComponent();
            string[] Maps = new string[] { "Normal", "Outer Planets Stock Planets", "Outer Planets", "Real Solar System" };
            // comboBoxMaps.ItemsSource = Maps;
        }

        private void comboBoxMaps_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*  if (comboBoxMaps.SelectedIndex == 0)
              {

                  BitmapImage logo = new BitmapImage();
                  logo.BeginInit();
                  logo.UriSource = new Uri(@"pack://application:,,/DeltaVMaps/Normal.png");
                  logo.EndInit();
                  Main.Source = logo;

              }
              if (comboBoxMaps.SelectedIndex == 1)
              {

                  BitmapImage logo = new BitmapImage();
                  logo.BeginInit();
                  logo.UriSource = new Uri(@"pack://application:,,/DeltaVMaps/DeltaVMap.png");
                  logo.EndInit();
                  Main.Source = logo;
              }
              if (comboBoxMaps.SelectedIndex == 2)
              {

                  BitmapImage logo = new BitmapImage();
                  logo.BeginInit();
                  logo.UriSource = new Uri(@"pack://application:,,/DeltaVMaps/OuterPlanteDeltaVMap.png");
                  logo.EndInit();
                  Main.Source = logo;
              }
              if (comboBoxMaps.SelectedIndex == 3)
              {
                  BitmapImage logo = new BitmapImage();
                  logo.BeginInit();
                  logo.UriSource = new Uri(@"pack://application:,,/DeltaVMaps/RssDeltaVMap.png");
                  logo.EndInit();
                  Main.Source = logo;
              }*/
        }

        private void buttonNormal_Click(object sender, RoutedEventArgs e)
        {
            DeltaVMaps window = new DeltaVMaps();
            //window.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,/Images/DeltaVMaps/Normal.png")));
            window.Show();
        }

        private void buttonOuterStock_Click(object sender, RoutedEventArgs e)
        {
            DeltaVMapsOuter1 window = new DeltaVMapsOuter1();
            //window.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,/Images/DeltaVMaps/DeltaVMap.png")));
            window.Show();
        }

        private void buttonOuter_Click(object sender, RoutedEventArgs e)
        {
            DeltaVMapOuter2 window = new DeltaVMapOuter2();
            //window.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,/Images/DeltaVMaps/OuterPlanteDeltaVMap.png")));
            window.Show();
        }

        private void buttonRss_Click(object sender, RoutedEventArgs e)
        {
            DeltaVMapsRSS window = new DeltaVMapsRSS();
            //window.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,/Images/DeltaVMaps/RssDeltaVMap.png")));
            window.Show();
        }
    }
}
