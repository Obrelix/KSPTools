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
    /// Interaction logic for PageBiomes.xaml
    /// </summary>
    public partial class PageBiomes : Page
    {
        public PageBiomes()
        {
            InitializeComponent();
            
            WPFWindow.MouseWheel += MainWindow_MouseWheel;

            image.MouseLeftButtonDown += image_MouseLeftButtonDown;
            image.MouseLeftButtonUp += image_MouseLeftButtonUp;
            image.MouseMove += image_MouseMove;
        }

        string[] Maps = new string[18] { "Bop", "Dress", "Duna", "Eeloo", "Eve", "Gilly", "Ike", "Kerbin", "Kerbin Old", "Laythe"
            , "Minmus", "Minmus old", "Moho", "Mun", "Mun Old", "Pol", "Tylo", "Vall"};
        string[] OuterMaps = new string[8] { "Hale", "Ovoc", "Polta", "Priax", "Slate", "Tal", "Tekto", "Wall" };
        string[] RSSMaps = new string[24] { "Callisto", "Charon", "Deimos", "Dione", "Earth", "Enceladus", "Europa", "Ganymede",
                "Iapetus", "Io", "Jupiter", "MarsBiomes", "MercuryBiomes", "Mimas", "Moon", "Phobos", "Pluto", "Rhea", "Saturn",
                "Tethys", "Titan", "Triton", "Uranus", "Venus"};
        bool btnchk = false;

        private void WPFWindow_Loaded(object sender, RoutedEventArgs e)
        {
            
            radioButtonNormal.IsChecked = true;
            
            
        }

        private void radioButtonNormal_Checked(object sender, RoutedEventArgs e)
        {
            if (radioButtonRss.IsChecked == true)
            {
                comboBoxMaps.ItemsSource = RSSMaps;
                comboBoxMaps.SelectedIndex = 4;
            }
            else if (radioButtonOuter.IsChecked == true)
            {
                comboBoxMaps.ItemsSource = OuterMaps;
                comboBoxMaps.SelectedIndex = 3;
            }
            else 
            {
                comboBoxMaps.ItemsSource = Maps;
                comboBoxMaps.SelectedIndex = 13;
            }

        }

        private Point origin;
        private Point start;
        private void image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            image.ReleaseMouseCapture();
        }

        private void image_MouseMove(object sender, MouseEventArgs e)
        {
            if (!image.IsMouseCaptured) return;
            Point p = e.MouseDevice.GetPosition(border);

            Matrix m = image.RenderTransform.Value;
            m.OffsetX = origin.X + (p.X - start.X);
            m.OffsetY = origin.Y + (p.Y - start.Y);

            image.RenderTransform = new MatrixTransform(m);
        }

        private void image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (image.IsMouseCaptured) return;
            image.CaptureMouse();

            start = e.GetPosition(border);
            origin.X = image.RenderTransform.Value.OffsetX;
            origin.Y = image.RenderTransform.Value.OffsetY;
        }

        private void MainWindow_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Point p = e.MouseDevice.GetPosition(image);

            Matrix m = image.RenderTransform.Value;
            if (e.Delta > 0)
                m.ScaleAtPrepend(1.1, 1.1, p.X, p.Y);
            else
                m.ScaleAtPrepend(1 / 1.1, 1 / 1.1, p.X, p.Y);

            image.RenderTransform = new MatrixTransform(m);
        }

        private string PlanetName = "";

        
        private void cmbxChange()
        {
            if (radioButtonRss.IsChecked == true)
            {
                if (comboBoxMaps.SelectedIndex == 0)
                {
                    PlanetName = "Callisto";
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(@"pack://application:,,/Images/Biomes/RSS/CallistoBiomes.png");
                    logo.EndInit();
                    image.Source = logo;
                }
                else if (comboBoxMaps.SelectedIndex == 1)
                {
                    PlanetName = "Charon";
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(@"pack://application:,,/Images/Biomes/RSS/CharonBiomes.png");
                    logo.EndInit();
                    image.Source = logo;
                }
                else if (comboBoxMaps.SelectedIndex == 2)
                {
                    PlanetName = "Deimos";
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(@"pack://application:,,/Images/Biomes/RSS/DeimosBiomes.png");
                    logo.EndInit();
                    image.Source = logo;
                }
                else if (comboBoxMaps.SelectedIndex == 3)
                {
                    PlanetName = "Dione";
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(@"pack://application:,,/Images/Biomes/RSS/DioneBiomes.png");
                    logo.EndInit();
                    image.Source = logo;
                }
                else if (comboBoxMaps.SelectedIndex == 4)
                {
                    PlanetName = "Earth";
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(@"pack://application:,,/Images/Biomes/RSS/EarthBiomes.png");
                    logo.EndInit();
                    image.Source = logo;
                }
                else if (comboBoxMaps.SelectedIndex == 5)
                {
                    PlanetName = "Enceladus";
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(@"pack://application:,,/Images/Biomes/RSS/EnceladusBiomes.png");
                    logo.EndInit();
                    image.Source = logo;
                }
                else if (comboBoxMaps.SelectedIndex == 6)
                {
                    PlanetName = "EuropaBiomes.png";
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(@"pack://application:,,/Images/Biomes/RSS/EuropaBiomes.png");
                    logo.EndInit();
                    image.Source = logo;
                }
                else if (comboBoxMaps.SelectedIndex == 7)
                {
                    PlanetName = "Ganymede";
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(@"pack://application:,,/Images/Biomes/RSS/GanymedeBiomes.png");
                    logo.EndInit();
                    image.Source = logo;
                }
                else if (comboBoxMaps.SelectedIndex == 8)
                {
                    PlanetName = "Iapetus";
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(@"pack://application:,,/Images/Biomes/RSS/IapetusBiomes.png");
                    logo.EndInit();
                    image.Source = logo;
                }
                else if (comboBoxMaps.SelectedIndex == 9)
                {
                    PlanetName = "Io";
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(@"pack://application:,,/Images/Biomes/RSS/IoBiomes.png");
                    logo.EndInit();
                    image.Source = logo;
                }
                else if (comboBoxMaps.SelectedIndex == 10)
                {
                    PlanetName = "Jupiter";
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(@"pack://application:,,/Images/Biomes/RSS/JupiterBiomes.png");
                    logo.EndInit();
                    image.Source = logo;
                }
                else if (comboBoxMaps.SelectedIndex == 11)
                {
                    PlanetName = "Mars";
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(@"pack://application:,,/Images/Biomes/RSS/MarsBiomes.png");
                    logo.EndInit();
                    image.Source = logo;
                }
                else if (comboBoxMaps.SelectedIndex == 12)
                {
                    PlanetName = "Mercury";
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(@"pack://application:,,/Images/Biomes/RSS/MercuryBiomes.png");
                    logo.EndInit();
                    image.Source = logo;
                }
                else if (comboBoxMaps.SelectedIndex == 13)
                {
                    PlanetName = "Mimas";
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(@"pack://application:,,/Images/Biomes/RSS/MimasBiomes.png");
                    logo.EndInit();
                    image.Source = logo;
                }
                else if (comboBoxMaps.SelectedIndex == 14)
                {
                    PlanetName = "MoonBiomes.png";
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(@"pack://application:,,/Images/Biomes/RSS/MoonBiomes.png");
                    logo.EndInit();
                    image.Source = logo;
                }
                else if (comboBoxMaps.SelectedIndex == 15)
                {
                    PlanetName = "Phobos";
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(@"pack://application:,,/Images/Biomes/RSS/PhobosBiomes.png");
                    logo.EndInit();
                    image.Source = logo;
                }
                else if (comboBoxMaps.SelectedIndex == 16)
                {
                    PlanetName = "PlutoBiomes.png";
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(@"pack://application:,,/Images/Biomes/RSS/PlutoBiomes.png");
                    logo.EndInit();
                    image.Source = logo;
                }
                else if (comboBoxMaps.SelectedIndex == 17)
                {
                    PlanetName = "RheaBiomes.png";
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(@"pack://application:,,/Images/Biomes/RSS/RheaBiomes.png");
                    logo.EndInit();
                    image.Source = logo;
                }
                else if (comboBoxMaps.SelectedIndex == 18)
                {
                    PlanetName = "Saturn";
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(@"pack://application:,,/Images/Biomes/RSS/SaturnBiomes.png");
                    logo.EndInit();
                    image.Source = logo;
                }
                else if (comboBoxMaps.SelectedIndex == 19)
                {
                    PlanetName = "Tethys";
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(@"pack://application:,,/Images/Biomes/RSS/TethysBiomes.png");
                    logo.EndInit();
                    image.Source = logo;
                }
                else if (comboBoxMaps.SelectedIndex == 20)
                {
                    PlanetName = "Titan";
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(@"pack://application:,,/Images/Biomes/RSS/TitanBiomes.png");
                    logo.EndInit();
                    image.Source = logo;
                }
                else if (comboBoxMaps.SelectedIndex == 21)
                {
                    PlanetName = "Triton";
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(@"pack://application:,,/Images/Biomes/RSS/TritonBiomes.png");
                    logo.EndInit();
                    image.Source = logo;
                }
                else if (comboBoxMaps.SelectedIndex == 22)
                {
                    PlanetName = "Uranus";
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(@"pack://application:,,/Images/Biomes/RSS/UranusBiomes.png");
                    logo.EndInit();
                    image.Source = logo;
                }
                else if (comboBoxMaps.SelectedIndex == 23)
                {
                    PlanetName = "Venus";
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(@"pack://application:,,/Images/Biomes/RSS/VenusBiomes.png");
                    logo.EndInit();
                    image.Source = logo;
                }
            }
            else if (radioButtonOuter.IsChecked == true)
            {
                if (comboBoxMaps.SelectedIndex == 0)
                {
                    PlanetName = "Hale";
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(@"pack://application:,,/Images/Biomes/OuterPlanets/Hale_Biome_Map.png");
                    logo.EndInit();
                    image.Source = logo;
                }
                else if (comboBoxMaps.SelectedIndex == 1)
                {
                    PlanetName = "Ovok";
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(@"pack://application:,,/Images/Biomes/OuterPlanets/Ovok_Biome_Map.png");
                    logo.EndInit();
                    image.Source = logo;
                }
                else if (comboBoxMaps.SelectedIndex == 2)
                {
                    PlanetName = "Polta";
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(@"pack://application:,,/Images/Biomes/OuterPlanets/Polta_Biome_Map.png");
                    logo.EndInit();
                    image.Source = logo;
                }
                else if (comboBoxMaps.SelectedIndex == 3)
                {
                    PlanetName = "Priax";
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(@"pack://application:,,/Images/Biomes/OuterPlanets/Priax_Biome_Map.png");
                    logo.EndInit();
                    image.Source = logo;
                }
                else if (comboBoxMaps.SelectedIndex == 4)
                {
                    PlanetName = "Slate";
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(@"pack://application:,,/Images/Biomes/OuterPlanets/Slate_Biome_Map.png");
                    logo.EndInit();
                    image.Source = logo;
                }
                else if (comboBoxMaps.SelectedIndex == 5)
                {
                    PlanetName = "Tal";
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(@"pack://application:,,/Images/Biomes/OuterPlanets/Tal_Biome_Map.png");
                    logo.EndInit();
                    image.Source = logo;
                }
                else if (comboBoxMaps.SelectedIndex == 6)
                {
                    PlanetName = "Tekto";
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(@"pack://application:,,/Images/Biomes/OuterPlanets/Tekto_Biome_Map.png");
                    logo.EndInit();
                    image.Source = logo;
                }
                else if (comboBoxMaps.SelectedIndex == 7)
                {
                    PlanetName = "Wal";
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(@"pack://application:,,/Images/Biomes/OuterPlanets/Wal_Biome_Map.png");
                    logo.EndInit();
                    image.Source = logo;
                }
            }
            else if (radioButtonNormal.IsChecked == true)
            {
                if (comboBoxMaps.SelectedIndex == 0)
                {
                    PlanetName = "Bop";
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(@"pack://application:,,/Images/Biomes/Bop_Biome_Map_0.90.0.png");
                    logo.EndInit();
                    image.Source = logo;
                }
                if (comboBoxMaps.SelectedIndex == 1)
                {
                    PlanetName = "Dres";
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(@"pack://application:,,/Images/Biomes/Dres_Biome_Map_0.90.0.png");
                    logo.EndInit();
                    image.Source = logo;
                }
                if (comboBoxMaps.SelectedIndex == 2)
                {
                    PlanetName = "Duna";
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(@"pack://application:,,/Images/Biomes/Duna_Biome_Map_0.90.0.png");
                    logo.EndInit();
                    image.Source = logo;
                }
                if (comboBoxMaps.SelectedIndex == 3)
                {
                    PlanetName = "Eeloo";
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(@"pack://application:,,/Images/Biomes/Eeloo_Biome_Map_0.90.0.png");
                    logo.EndInit();
                    image.Source = logo;
                }
                if (comboBoxMaps.SelectedIndex == 4)
                {
                    PlanetName = "Eve";
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(@"pack://application:,,/Images/Biomes/Eve_Biome_Map_0.90.0.png");
                    logo.EndInit();
                    image.Source = logo;
                }
                if (comboBoxMaps.SelectedIndex == 5)
                {
                    PlanetName = "Gilly";
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(@"pack://application:,,/Images/Biomes/Gilly_Biome_Map_0.90.0.png");
                    logo.EndInit();
                    image.Source = logo;
                }
                if (comboBoxMaps.SelectedIndex == 6)
                {
                    PlanetName = "Ike";
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(@"pack://application:,,/Images/Biomes/Ike_Biome_Map_0.90.0.png");
                    logo.EndInit();
                    image.Source = logo;
                }
                if (comboBoxMaps.SelectedIndex == 7)
                {
                    PlanetName = "Kerbin";
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(@"pack://application:,,/Images/Biomes/Kerbin_Biome_Map_0.90.0.png");
                    logo.EndInit();
                    image.Source = logo;
                }
                if (comboBoxMaps.SelectedIndex == 8)
                {
                    PlanetName = "Kerbin";
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(@"pack://application:,,/Images/Biomes/KerbinBiomeMap.png");
                    logo.EndInit();
                    image.Source = logo;
                }
                if (comboBoxMaps.SelectedIndex == 9)
                {
                    PlanetName = "Laythe";
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(@"pack://application:,,/Images/Biomes/Laythe_Biome_Map_0.90.0.png");
                    logo.EndInit();
                    image.Source = logo;
                }
                if (comboBoxMaps.SelectedIndex == 10)
                {
                    PlanetName = "Minmus";
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(@"pack://application:,,/Images/Biomes/Minmus_Biome_Map_0.90.0.png");
                    logo.EndInit();
                    image.Source = logo;
                }
                if (comboBoxMaps.SelectedIndex == 11)
                {
                    PlanetName = "Minmus";
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(@"pack://application:,,/Images/Biomes/Minmusbiome.png");
                    logo.EndInit();
                    image.Source = logo;
                }
                if (comboBoxMaps.SelectedIndex == 12)
                {
                    PlanetName = "Moho";
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(@"pack://application:,,/Images/Biomes/Moho_Biome_Map_0.90.0.png");
                    logo.EndInit();
                    image.Source = logo;
                }
                if (comboBoxMaps.SelectedIndex == 13)
                {
                    PlanetName = "Mun";
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(@"pack://application:,,/Images/Biomes/Mun_Biome_Map_0.90.0.png");
                    logo.EndInit();
                    image.Source = logo;
                }
                if (comboBoxMaps.SelectedIndex == 14)
                {
                    PlanetName = "Mun";
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(@"pack://application:,,/Images/Biomes/MunBiomeMap.png");
                    logo.EndInit();
                    image.Source = logo;
                }
                if (comboBoxMaps.SelectedIndex == 15)
                {
                    PlanetName = "Pol";
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(@"pack://application:,,/Images/Biomes/Pol_Biome_Map_0.90.0.png");
                    logo.EndInit();
                    image.Source = logo;
                }
                if (comboBoxMaps.SelectedIndex == 16)
                {
                    PlanetName = "Tylo";
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(@"pack://application:,,/Images/Biomes/Tylo_Biome_Map_0.90.0.png");
                    logo.EndInit();
                    image.Source = logo;
                }
                if (comboBoxMaps.SelectedIndex == 17)
                {
                    PlanetName = "Vall";
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(@"pack://application:,,/Images/Biomes/Vall_Biome_Map_0.90.0.png");
                    logo.EndInit();
                    image.Source = logo;
                }
            }
        }

        private void txtbxText()
        {
            if (PlanetName == "Moho")
            {
                txtb_Info.Text = "Moho" +
                    Environment.NewLine +
                    "The planet Moho has 12 biomes.It is one of two bodies(the other being Ike) that have their own North and South Pole biome.On the North Pole there is a big sinkhole called the Northern Sinkhole." +
                    Environment.NewLine +
                    Environment.NewLine +
                    "Moho biome list:" +
                    Environment.NewLine +
                    "North Pole, Northern Sinkhole Ridge, Northern Sinkhole," + Environment.NewLine +
                    "Highlands, Midlands, Minor Craters, Central Lowlands," + Environment.NewLine +
                    "Western Lowlands, South Western Lowlands," + Environment.NewLine +
                    "South Eastern Lowlands, Canyon, South Pole ";
            }
            if (PlanetName == "Eve")
            {
                txtb_Info.Text = "Eve" +
                    Environment.NewLine +
                    "The planet Eve has 7 biomes. It has several Explodium Seas, among which lie large continents." +
                    Environment.NewLine +
                    Environment.NewLine +
                    "Eve biome list:" +
                    Environment.NewLine +
                    "Poles, Explodium Sea, Lowlands, Midlands," + Environment.NewLine +
                    "Highlands, Peaks, Impact Ejecta";
            }
            if (PlanetName == "Gilly")
            {
                txtb_Info.Text = "Gilly" +
                    Environment.NewLine +
                    "With only 3 biomes, the moon of Eve, called Gilly, has the fewest number of biomes." +
                    Environment.NewLine +
                    Environment.NewLine +
                    "Gilly  biome list:" +
                    Environment.NewLine +
                    "Lowlands, Midlands, Highlands";
            }
            if (PlanetName == "Kerbin")
            {
                txtb_Info.Text = "Kerbin" +
                    Environment.NewLine +
                    "The planet Kerbin has 9 biomes, plus a large number of surface-only “location biomes” comprising Kerbal Space Center. Roughly 60% Kerbin's surface is Water biome. Grasslands and Highlands biomes cover most of the land area." +
                    Environment.NewLine +
                    Environment.NewLine +
                    "Kerbin is the only celestial body with location biomes: the grounds of KSC and the many structures it is composed of. Each count as a biome only with experiments performed on the surface. KSC is within a Shores biome, so experiments while flying at any altitude over the KSC count as Shores. However, it is possible to get an \"EVA Report while flying over\" the area, either while jumping, grabbing a ladder on a craft, or sitting on a EAS-1 External Command Seat." +
                    Environment.NewLine +
                    Environment.NewLine +
                    "Kerbin biome list:" +
                    Environment.NewLine +
                    "Ice Caps, Tundra, Highlands, Mountains," +
                    Environment.NewLine +
                    "Grasslands, Deserts, Badlands," +
                    Environment.NewLine +
                    "Shores, Water" +
                    Environment.NewLine +
                    Environment.NewLine +
                    "KSC location biome list:" +
                    Environment.NewLine +
                    "Administration, Astronaut Complex[Note 1], Crawlerway, Flag Pole, LaunchPad, Mission Control, " +
                    Environment.NewLine +
                    "R & D, R & D Central Building, R & D Corner Lab, R & D image Building, R & D Observatory, R & D Side Lab, R & D Small Lab,R & D Tanks, R & D Wind Tunnel," +
                    Environment.NewLine +
                    "Runway" +
                    Environment.NewLine +
                    "SPH, SPH image Building, SPH Round Tank, SPH Tanks, SPH Water Tower," +
                    Environment.NewLine +
                    "Tracking Station, Tracking Station Dish East, Tracking Station Dish North, Tracking Station Dish South, Tracking Station Hub, " +
                    Environment.NewLine +
                    "VAB, VAB image Building, VAB Pod Memorial, VAB Round Tank, VAB South Complex[Note 2], VAB Tanks" +

                    Environment.NewLine +
                    Environment.NewLine +
                    "Notes: While the per-area biomes are available from the start of a new career game, the per-structure biomes will not be found until each facility is upgraded once or twice. To access each per-structure biome, your craft needs to be touching each new building." +
                    Environment.NewLine +
                    "1.↑ In career mode, the Astronaut Complex location biome is available with a first - level building if in contact with the structure, or anywhere on its hex after the first upgrade." +
                    Environment.NewLine +
                    "2. ↑ The South Complex location biome is only accessible with a second-level VAB, making the science available there missable content.";
            }
            if (PlanetName == "Mun")
            {
                txtb_Info.Text = "Mun" +
                    Environment.NewLine +
                    "The biggest moon of Kerbin, called the Mun, has a total of 15 biomes, 7 of which are uniquely named major craters.The Mun has the largest amount of biomes in the Kerbol System.Most of the surface area belongs to the Midlands biome followed by the Highlands.Canyon biomes extend off a few major craters." +
                    Environment.NewLine +
                    Environment.NewLine +
                    "Mun  biome list:" +
                    Environment.NewLine +
                    "Poles Polar, Lowlands, Polar Crater, Highlands, Highland Craters, Midlands, Midland Craters, Northern Basin, Northwest Crater, East Farside Crater, Canyons Farside Crater, East Crater, Twin Craters, Southwest Crater";
            }
            if (PlanetName == "Minmus")
            {
                txtb_Info.Text = "Minmus" +
                    Environment.NewLine +
                    "The moon of Kerbin, called Minmus, has a total of 9 biomes. The most distinctive quality of Minmus's biomes is the variety of Flats, which in-game text describe as “lake beds”. They are almost perfectly flat and may represent salt flats. Roughly two-thirds of the surface area is irregular terrain transitioning between Highlands, Midlands, and Lowlands with Slopes in-between." +
                    Environment.NewLine +
                    Environment.NewLine +
                    "Minmus  biome list:" +
                    Environment.NewLine +
                    "Poles, Lowlands, Midlands, Highlands, Slopes, Flats, Lesser Flats, Great Flats, Greater Flats";
            }
            if (PlanetName == "Duna")
            {
                txtb_Info.Text = "Duna" +
                    Environment.NewLine +
                    "The planet Duna has 5 biomes." +
                    Environment.NewLine +
                    Environment.NewLine +
                    "Duna  biome list:" +
                    Environment.NewLine +
                    "Poles, Highlands, Midlands, Lowlands, Craters";
            }
            if (PlanetName == "Ike")
            {
                txtb_Info.Text = "Ike" +
                    Environment.NewLine +
                    "The moon of Duna, called Ike, has 8 biomes. It is one of two bodies (the other being Moho) that has their own North and South Pole biome." +
                    Environment.NewLine +
                    Environment.NewLine +
                    "Ike biome list:" +
                    Environment.NewLine +
                    "Polar Lowlands, Midlands, Lowlands, Eastern Mountain Ridge, Western Mountain Ridge, Central Mountain Range, South Eastern Mountain Range, South Pole";
            }
            if (PlanetName == "Dres")
            {
                txtb_Info.Text = "Dres" +
                    Environment.NewLine +
                    "The planet Dres has 8 biomes." +
                    Environment.NewLine +
                    Environment.NewLine +
                    "Dres biome list:" +
                    Environment.NewLine +
                    "Poles, Highlands, Midlands, Lowlands, Ridges, Impact Ejecta, Impact Craters, Canyons";
            }
            if (PlanetName == "Laythe")
            {
                txtb_Info.Text = "Laythe" +
                    Environment.NewLine +
                    "The first moon of Jool, called Laythe, has 5 biomes. Laythe exists of a huge ocean, called The Sagen Sea, with some small Dunes and Shores biomes. It also has bay, called the Cresent Bay and a Poles biome.." +
                    Environment.NewLine +
                    Environment.NewLine +
                    "Laythe biome list:" +
                    Environment.NewLine +
                    "Poles, Shores, Dunes, Cresent Bay, The Sagen Sea";
            }
            if (PlanetName == "Vall")
            {
                txtb_Info.Text = "Vall" +
                    Environment.NewLine +
                    "The second moon of Jool, called Vall, has 4 biomes." +
                    Environment.NewLine +
                    Environment.NewLine +
                    "Vall biome list:" +
                    Environment.NewLine +
                    "Poles, Highlands, Midlands, Lowlands";
            }
            if (PlanetName == "Tylo")
            {
                txtb_Info.Text = "Tylo" +
                    Environment.NewLine +
                    "The third moon of Jool, called Tylo, has 8 biomes. Three biomes have the exact same name but are seen separately in KSP. It is unknown if this is a bug, a failed reference to the novel Catch 22, or was simply overlooked by Squad." +
                    Environment.NewLine +
                    Environment.NewLine +
                    "Tylo biome list:" +
                    Environment.NewLine +
                    "Highlands, Midlands, Lowlands, Mara, Minor Craters, Major Crater, Major Crater, Major Crater";
            }
            if (PlanetName == "Bop")
            {
                txtb_Info.Text = "Bop" +
                    Environment.NewLine +
                    "The fourth moon of Jool, called Bop, has 5 biomes." +
                    Environment.NewLine +
                    Environment.NewLine +
                    "Bop biome list:" +
                    Environment.NewLine +
                    "Poles, Slopes, Peaks, Mara, Valley, Ridges";
            }
            if (PlanetName == "Pol")
            {
                txtb_Info.Text = "Pol" +
                    Environment.NewLine +
                    "The fifth moon of Jool, called Pol, has 4 biomes." +
                    Environment.NewLine +
                    Environment.NewLine +
                    "Pol biome list:" +
                    Environment.NewLine +
                    "Poles, Lowlands, Midlands, Highlands";
            }
            if (PlanetName == "Eeloo")
            {
                txtb_Info.Text = "Eeloo" +
                    Environment.NewLine +
                    "The planet Eeloo has a total of 7 biomes." +
                    Environment.NewLine +
                    Environment.NewLine +
                    "Eeloo biome list:" +
                    Environment.NewLine +
                    "Poles, Glaciers, Midlands, Lowlands, Ice Canyons, Highlands Craters";
            }
        }
        private void button_Click(object sender, RoutedEventArgs e)
        {
            
            btnchk = !btnchk;
            if (btnchk)
            {
                
                button.Content = "Less Info";
                rctMenu.Opacity = 0.9;
                txtbxText();


            }
            else
            {
                txtbxText();
                txtb_Info.Text = "";
                button.Content = "More Info";
                rctMenu.Opacity = 0.5;
            }

        }

        private void comboBoxMaps_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (btnchk)
            {
                cmbxChange();
                txtbxText();
                
            }
            else
            {
                cmbxChange();
                txtb_Info.Text = "";
            }
        }
    }
}
