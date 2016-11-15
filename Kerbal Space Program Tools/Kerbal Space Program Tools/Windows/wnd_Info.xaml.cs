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

namespace Kerbal_Space_Program_Tools.Windows
{
    /// <summary>
    /// Interaction logic for wnd_Info.xaml
    /// </summary>
    /// 
    public static class isopen
    {
        public static bool open;
    }
    public partial class wnd_Info : Window
    {
        
        public wnd_Info()
        {
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            isopen.open = false;
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            isopen.open = true;
        }
    }
}
