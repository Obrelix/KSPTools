using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for PageMain.xaml
    /// </summary>
    public partial class PageMain : Page
    {
        public PageMain()
        {
            InitializeComponent();
        }

        private string ExecuteCommands(string command1, string command2)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe");
            processStartInfo.RedirectStandardInput = true;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.UseShellExecute = false;

            Process process = Process.Start(processStartInfo);

            if (process != null)
            {
                process.StandardInput.WriteLine(command1);
                process.StandardInput.WriteLine(command2);
                process.StandardInput.WriteLine("exit");

                process.StandardInput.Close(); // line added to stop process from hanging on ReadToEnd()

                string outputString = process.StandardOutput.ReadToEnd();
                return outputString;
            }

            return string.Empty;
        }
        private void Reddit_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.reddit.com/r/KerbalSpaceProgram");
        }
        private void Forum_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://forum.kerbalspaceprogram.com/");
        }
        private void Wiki_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://wiki.kerbalspaceprogram.com/wiki/Main_Page");
        }
        private void CurseMods_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://mods.curse.com/ksp-mods/kerbal?filter-project-sort=3");
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            ExecuteCommands("cd" + @" C:\Program Files (x86)\Steam", "start steam \"steam://rungameid/220200\"");
        }



        private void buttonMissonDVCalc_Click(object sender, RoutedEventArgs e)
        {

            MissionCalculator Window = new MissionCalculator();
            Window.Show();
        }

        private void buttonDVCalc_Click(object sender, RoutedEventArgs e)
        {
            DeltaVCalculator Stage = new DeltaVCalculator();
            Stage.Show();
        }

        private void buttonTerminology_Click(object sender, RoutedEventArgs e)
        {
            Terminology Window = new Terminology();
            Window.Show();

        }

        private void buttonBiomes_Click(object sender, RoutedEventArgs e)
        {
            Biomes Window = new Biomes();
            Window.Show();
        }

        private void buttonDeltaVMaps_Click(object sender, RoutedEventArgs e)
        {
            DeltaVMaps map = new DeltaVMaps();
            map.Show();
        }

        private void button_ClickCkan(object sender, RoutedEventArgs e)
        {
            ExecuteCommands("cd" + @" C:\Program Files (x86)\Steam\steamapps\common\Kerbal Space Program", "start ckan.exe");
        }

      
    }
}

