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
using System.IO;
using System.Reflection;

namespace Kerbal_Space_Progam_Tools
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            string[] TermiCombo = new string[] 
            { "----->Mathematics", "1) Elliptical", "2) Hyperbolic","3) Normal vector", "4) Scalar", "5) Vector",

             "----->Orbital Terms" , "1) Apsis", "2) Periapsis","3) Apoapsis",  "4) Ascending node" , "5) Descending node" ,
            "6) Eccentricity",   "7) Inclination" ,"8) Low orbit", "9) Orbital node" ,   "10) Orbit normal" ,    "11) Orbital plane" , "12) Orbital speed",
            "13) Prograde",  "14) Retrograde",  "15) Reference plane"  , "16) Semi-major axis",   "17) Sidereal period",
            "18) Sub-orbital","19) Synodic period" , "20) True Anomaly" ,
            

            "----->Ship Orientation" , "1) Attitude" , "2) Zenith", "3) Nadir", "4) Port" ,"5) Starboard", "6) Forward (Fore)", "7) Aft" ,

            "----->Space Maneuvers" , "1) Aerobraking", "2) Lithobraking", "3) Atmospheric entry" ,"4) Burn", "5) Circularizing", 
            "6) Gravity assist" , "7) Maneuver node" , "8) Radial-in burn" , "9) Radial-out burn", "10) Re-entry", "11) Retroburn ",

            "----->Physics", "1) Acceleration", "2) Ballistic trajectory", "3) Delta-v (Δv)", "4) Energy", "5) Escape Velocity", "6) g-force (g)",
            "7) Gravity", "8) Gravity well", "9) Orbit", "10) Specific Impulse (Isp)", "11) Sphere of influence" , "12) Tangential velocity",
            "13) Thrust-to-weight ratio", "14) Trajectory", "15) Velocity",

            "----->Interplanetary How-to Guide by Kosmo-not "} ;
            comboBoxTerminology.ItemsSource = TermiCombo;
            comboBoxTerminology.SelectedIndex = 0;
        }
        private void comboBoxTerminology_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxTerminology.SelectedIndex < 6)
            {
                frameTerminology.Source = new Uri("/Kerbal Space Progam Tools;component/Pages/PageTerminology.xaml", UriKind.Relative);
            }
            else if(comboBoxTerminology.SelectedIndex > 5 && comboBoxTerminology.SelectedIndex < 27)
            {
                frameTerminology.Source = new Uri("/Kerbal Space Progam Tools;component/Pages/PageTerminologyOrbitalTerms.xaml", UriKind.Relative);
            }
            else if(comboBoxTerminology.SelectedIndex > 26 && comboBoxTerminology.SelectedIndex < 35)
            {
                frameTerminology.Source = new Uri("/Kerbal Space Progam Tools;component/Pages/PageTerminologyShipOrientation.xaml", UriKind.Relative);
            }
            else if (comboBoxTerminology.SelectedIndex > 34 && comboBoxTerminology.SelectedIndex < 47)
            {
                frameTerminology.Source = new Uri("/Kerbal Space Progam Tools;component/Pages/PageTerminologySpaceManeuvers.xaml", UriKind.Relative);
            }
            else if (comboBoxTerminology.SelectedIndex > 46 && comboBoxTerminology.SelectedIndex < 63)
            {
                frameTerminology.Source = new Uri("/Kerbal Space Progam Tools;component/Pages/PageTerminologyPhysics.xaml", UriKind.Relative);
            }
            else
            {
                frameTerminology.Source = new Uri("/Kerbal Space Progam Tools;component/Pages/InterplanetaryHowToGuide.xaml", UriKind.Relative);
            }
        }

        private void RadioTerms(object sender, RoutedEventArgs e)
        {
            if (radioButtonMaths.IsChecked == true)
            {
                frameTerminology.Source = new Uri("/Kerbal Space Progam Tools;component/Pages/PageTerminology.xaml", UriKind.Relative);
            }
            else if (radioButtonOrbitTerms.IsChecked == true)
            {
                frameTerminology.Source = new Uri("/Kerbal Space Progam Tools;component/Pages/PageTerminologyOrbitalTerms.xaml", UriKind.Relative);
            }
            else if (radioButtonShipOrient.IsChecked == true)
            {
                frameTerminology.Source = new Uri("/Kerbal Space Progam Tools;component/Pages/PageTerminologyShipOrientation.xaml", UriKind.Relative);
            }
            else if (radioButtonSpaceManu.IsChecked == true)
            {
                frameTerminology.Source = new Uri("/Kerbal Space Progam Tools;component/Pages/PageTerminologySpaceManeuvers.xaml", UriKind.Relative);
            }
            else if (radioButtonPhysics.IsChecked == true)
            {
                frameTerminology.Source = new Uri("/Kerbal Space Progam Tools;component/Pages/PageTerminologyPhysics.xaml", UriKind.Relative);
            }
            else if (radioButtonInterplanetary.IsChecked == true)
            {
                frameTerminology.Source = new Uri("/Kerbal Space Progam Tools;component/Pages/InterplanetaryHowToGuide.xaml", UriKind.Relative);
            }
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
        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Created By Obrelix"+
                Environment.NewLine+ "Released : 13/7/16"
                + Environment.NewLine + "Version : Beta 0.8");
        }
        private void Credit_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Credits for the  Delta-V maps" +
                Environment.NewLine + "JellyCubes (Original concept)"
                + Environment.NewLine + "WAC (Original design)"
                + Environment.NewLine + "CuriousMetaphor (Original out-of-atmosphere numbers)"
                + Environment.NewLine + "Kowgan (Design, original in-atmosphere numbers)"
                + Environment.NewLine + "Swashlebucky (Design)"
                + Environment.NewLine + "AlexMoon (Time of flight)"
                + Environment.NewLine
                + Environment.NewLine + "Credits for the  Biome maps"
                + Environment.NewLine + "http://wiki.kerbalspaceprogram.com/wiki/Biome"
                + Environment.NewLine + "http://outer-planets.wikia.com/wiki/Outer_Planets_Wiki"
                + Environment.NewLine
                + Environment.NewLine + "Credits for the  Terminology"
                + Environment.NewLine + "http://wiki.kerbalspaceprogram.com/wiki/Main_Page"
                + Environment.NewLine + "http://www.braeunig.us/space/"
                + Environment.NewLine + "Kosmo-not for his amazing guide.");
        }



        private void buttonMissonDVCalc_Click(object sender, RoutedEventArgs e)
        {

            //MissionCalculator Window = new MissionCalculator();
            //Window.Show();
            tabControl.SelectedIndex = 1;
        }

        private void buttonDVCalc_Click(object sender, RoutedEventArgs e)
        {
            //DeltaVCalculator Stage = new DeltaVCalculator();
            //Stage.Show();
            tabControl.SelectedIndex = 2;
        }

        private void buttonTerminology_Click(object sender, RoutedEventArgs e)
        {
            //Terminology Window = new Terminology();
            //Window.Show();
            tabControl.SelectedIndex = 5;
        }

        private void buttonBiomes_Click(object sender, RoutedEventArgs e)
        {
            //Biomes Window = new Biomes();
            //Window.Show();
            tabControl.SelectedIndex = 3;
        }

        private void buttonDeltaVMaps_Click(object sender, RoutedEventArgs e)
        {
            // DeltaVMaps map = new DeltaVMaps();
            //map.Show();
            tabControl.SelectedIndex = 4;
        }

        private void button_ClickCkan(object sender, RoutedEventArgs e)
        {
            ExecuteCommands("cd" + @" C:\Program Files (x86)\Steam\steamapps\common\Kerbal Space Program", "start ckan.exe");
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }

        
    }
}
