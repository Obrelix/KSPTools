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

namespace Kerbal_Space_Progam_Tools
{
    /// <summary>
    /// Interaction logic for Terminology.xaml
    /// </summary>
    public partial class Terminology : Window
    {
        public Terminology()
        {
            InitializeComponent();
            string[] Combos = new string[] { "Delta-V", "Isp" };
            comboBox.ItemsSource = Combos;
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox.SelectedIndex == 0)
            {
                richTextBox.Visibility = Visibility.Visible;
                richTextBox_Copy.Visibility = Visibility.Hidden;
            }
            if (comboBox.SelectedIndex == 1)
            {
                richTextBox_Copy.Visibility = Visibility.Visible;
                richTextBox.Visibility = Visibility.Hidden;

            }
        }
    }
}
