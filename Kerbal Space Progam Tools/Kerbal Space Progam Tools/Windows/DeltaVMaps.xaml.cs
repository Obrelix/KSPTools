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
using System.Windows.Resources;
using System.Windows.Shapes;

namespace Kerbal_Space_Progam_Tools
{
    /// <summary>
    /// Interaction logic for DeltaVMaps.xaml
    /// </summary>
    public partial class DeltaVMaps : Window
    {
        public DeltaVMaps()
        {
            InitializeComponent();

            string[] Maps = new string[] { "Normal", "Outer Planets Stock Planets", "Outer Planets", "Real Solar System" };
            //comboBoxMaps.ItemsSource = Maps;
        }

        private void comboBoxMaps_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*if (comboBoxMaps.SelectedIndex == 0)
            {
                
                BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(@"pack://application:,,/Images/DeltaVMaps/Normal.png");
                    logo.EndInit();
                Main.Source = logo;

            }
            if (comboBoxMaps.SelectedIndex == 1)
            {

                BitmapImage logo = new BitmapImage();
                logo.BeginInit();
                logo.UriSource = new Uri(@"pack://application:,,/Images/DeltaVMaps/DeltaVMap.png");
                logo.EndInit();
                Main.Source = logo;
            }
            if (comboBoxMaps.SelectedIndex == 2)
            {
                
                BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(@"pack://application:,,/Images/DeltaVMaps/OuterPlanteDeltaVMap.png");
                    logo.EndInit();
                Main.Source = logo;
            }
            if (comboBoxMaps.SelectedIndex == 3)
            {
                BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(@"pack://application:,,/Images/DeltaVMaps/RssDeltaVMap.png");
                    logo.EndInit();
                Main.Source = logo;
            }*/
        }
    }
}
