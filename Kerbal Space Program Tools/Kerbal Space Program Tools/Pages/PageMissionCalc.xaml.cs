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
    /// Interaction logic for PageMissionCalc.xaml
    /// </summary>
    public partial class PageMissionCalc : Page
    {
        ///I use these numbers to create a table in the last function
        //Number of Stock KSP Planets
        public const int PlanetsNumber = 18;

        //Number of Moded with Outer Planets Mod KSP Planets
        public const int OuterPlanetsNumber = 32;

        //The same with Real Solar System
        public const int RssPlanetNumber = 17;

        //Second per  Day in Kerbin
        public const int KerbinTime = 21600;
        //Second per  Day in Earth
        public const int EarthTime = 86400;

        //Start new arrays from class Planet.All
        Planet.All[] ksp = new Planet.All[35];
        Planet.All[] kspNormal = new Planet.All[PlanetsNumber];
        Planet.All[] kspOuter = new Planet.All[OuterPlanetsNumber];
        Planet.All[] ksprss = new Planet.All[RssPlanetNumber];

        public double TotalTime = 0;

        public PageMissionCalc()
        {
            InitializeComponent();

            // Initialize the radio buttons

          

        }

        private void buttonMissonDVCalc_Click(object sender, RoutedEventArgs e)
        {
            MissionCalculator window = new MissionCalculator();
            window.Show();
        }

        private void Main_Loaded(object sender, RoutedEventArgs e)
        {
            radioButtonLanding.IsChecked = true;
            radioButtonLanding1.IsChecked = true;
            radioButtonLanding2.IsChecked = true;
            radioButtonNormal.IsChecked = true;
            radioButtonKerbinTime.IsChecked = true;
        }
        //Main Event most of the calculations take part here

        private void button_Click(object sender, RoutedEventArgs e)
        {
            PlanetInit();
            // Initialize the textBlocks
            this.textBlockOrigin.Text = "Origin";
            this.textBlockStop1.Text = "Stop 1";
            this.textBlockStop2.Text = "Stop 2";
            this.textBlockStop3.Text = "Stop 3";
            this.textBlockOriginTravel.Text = "Round Trip";
            this.textBlockStop1Travel.Text = "Stop 1";
            this.textBlockStop2Travel.Text = "Stop 2";
            this.textBlockStop3Travel.Text = "Stop 3";

            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.UriSource = new Uri(@"pack://application:,,/Images/Planets/None.png");
            img.EndInit();
            imageOrigin.Source = img;
            imageStop1.Source = img;
            imageStop2.Source = img;
            imageStop3.Source = img;



            //Initialize comboBoxes  
            if (comboBoxOrigin.SelectedIndex == -1)
            {
                comboBoxOrigin.SelectedIndex = 0;
            }
            if (comboBoxStop1.SelectedIndex == -1)
            {
                comboBoxStop1.SelectedIndex = 0;
            }
            if (comboBoxStop2.SelectedIndex == -1)
            {
                comboBoxStop2.SelectedIndex = 0;
            }
            if (comboBoxStop3.SelectedIndex == -1)
            {
                comboBoxStop3.SelectedIndex = 0;
            }


            /// Counter1 stores the Origin Counter stores the Previous Planet

            int PreviousPlanet = 0, Origin = 0;
            int TotalDV = 0;

            ///CurrentTime Stores the the seconds per day sellected by the user

            int CurrentTime = 0;

            ///TotalTime Stores the total time of the travel

            TotalTime = 0;

            ///DVPrevious Stores the Previous total Δv value

            int DVPrevious = 0;



            ///Give to CurrentTime a Value in seconds

            if (radioButtonKerbinTime.IsChecked == true)
            {
                CurrentTime = KerbinTime;
            }
            else
            {
                CurrentTime = EarthTime;
            }



            /// Origin ComboBox 

            if (expanderOrigin.IsExpanded == true && comboBoxOrigin.SelectedIndex != 0 && comboBoxOrigin.SelectedIndex < number())
            {

                ///Previous Planet and Origin is the Origin comboBox

                PreviousPlanet = Origin = comboBoxOrigin.SelectedIndex;

                ///Write text to Origin textBlock

                this.textBlockOrigin.Text = Texts(comboBoxOrigin.SelectedIndex);


                imageOrigin.Source = new BitmapImage(new Uri(ksp[comboBoxOrigin.SelectedIndex].ImgUri));

            }

            ///  Stop 1 Combobox 

            if (expanderStop1.IsExpanded == true && comboBoxStop1.SelectedIndex != 0 && comboBoxStop1.SelectedIndex < number())
            {

                ///Write text to Stop 1 textBlock

                this.textBlockStop1.Text = Texts(comboBoxStop1.SelectedIndex);

                /// Calculate the Total DV for the mission

                TotalDV += DeltaVCount(PreviousPlanet, comboBoxStop1.SelectedIndex);

                /// if start from low oribit is checked dont calculate DV for Surface to low orbit from origin planet  

                if (checkBoxLowOrbit.IsChecked == true)
                {
                    TotalDV -= ksp[Origin].SurfaceToLowOrbit;
                }

                /// if Stop 1 Planet has atmosphere dont calculate DV for landing

                if (ksp[comboBoxStop1.SelectedIndex].Atmosphere)
                {
                    TotalDV -= ksp[comboBoxStop1.SelectedIndex].SurfaceToLowOrbit;
                }

                ///If  low oribit is checked dont calculate DV for landing 

                else if (radioButtonLowOrbit1.IsChecked == true)
                {
                    TotalDV -= ksp[comboBoxStop1.SelectedIndex].SurfaceToLowOrbit;
                }

                ///Write text to stop 1 travel textBlock

                this.textBlockStop1Travel.Text = TravelInfo(PreviousPlanet, comboBoxStop1.SelectedIndex, CurrentTime) +

                TransferDV(TotalDV, DVPrevious);

                imageStop1.Source = new BitmapImage(new Uri(ksp[comboBoxStop1.SelectedIndex].ImgUri));

                /// Set new value for previous DV for the next combobox calculation

                DVPrevious = TotalDV;

                /// Same for the planet

                PreviousPlanet = comboBoxStop1.SelectedIndex;

                /// If Round Trip is checked and the next comboBoxes are inactive Calculate the DV for the return

                if ((checkBoxReturn.IsChecked == true) && (expanderStop3.IsExpanded == false || comboBoxStop3.SelectedIndex == 0) && (expanderStop2.IsExpanded == false || comboBoxStop2.SelectedIndex == 0))
                {
                    TotalDV += DeltaVCount(comboBoxStop1.SelectedIndex, Origin);

                    if (ksp[Origin].Atmosphere)
                    {
                        TotalDV -= ksp[Origin].SurfaceToLowOrbit;
                    }
                    if (radioButtonLowOrbit1.IsChecked == true)
                    {
                        TotalDV -= ksp[comboBoxStop1.SelectedIndex].SurfaceToLowOrbit;
                    }

                    ///write text to Origin Travel textBlock

                    this.textBlockOriginTravel.Text = TravelInfo(comboBoxStop1.SelectedIndex, Origin, CurrentTime) +
                    TransferDV(TotalDV, DVPrevious);

                }
            }


            // Stop 2 Combobox

            if (expanderStop2.IsExpanded == true && comboBoxStop2.SelectedIndex != 0 && comboBoxStop2.SelectedIndex < number())
            {
                this.textBlockStop2.Text = Texts(comboBoxStop2.SelectedIndex);
                TotalDV += DeltaVCount(PreviousPlanet, comboBoxStop2.SelectedIndex);

                /// if start from low oribit is checked dont calculate DV for Surface to low orbit from origin planet 

                if (checkBoxLowOrbit.IsChecked == true && (expanderStop1.IsExpanded == false || comboBoxStop1.SelectedIndex == 0))
                {
                    TotalDV -= ksp[Origin].SurfaceToLowOrbit;
                }


                if (radioButtonLowOrbit1.IsChecked == true && (expanderStop1.IsExpanded == true && comboBoxStop1.SelectedIndex != 0))
                {
                    TotalDV -= ksp[comboBoxStop1.SelectedIndex].SurfaceToLowOrbit;
                }

                if (ksp[comboBoxStop2.SelectedIndex].Atmosphere)
                {
                    TotalDV -= ksp[comboBoxStop2.SelectedIndex].SurfaceToLowOrbit;
                }

                else if ((radioButtonLowOrbit2.IsChecked == true))
                {
                    TotalDV -= ksp[comboBoxStop2.SelectedIndex].SurfaceToLowOrbit;
                }

                this.textBlockStop2Travel.Text = TravelInfo(PreviousPlanet, comboBoxStop2.SelectedIndex, CurrentTime) +
                TransferDV(TotalDV, DVPrevious);
                DVPrevious = TotalDV;
                PreviousPlanet = comboBoxStop2.SelectedIndex;


                imageStop2.Source = new BitmapImage(new Uri(ksp[comboBoxStop2.SelectedIndex].ImgUri));


                if ((checkBoxReturn.IsChecked == true) && (expanderStop3.IsExpanded == false || comboBoxStop3.SelectedIndex == 0))
                {
                    TotalDV += DeltaVCount(comboBoxStop2.SelectedIndex, Origin);

                    if (ksp[Origin].Atmosphere)
                    {
                        TotalDV -= ksp[Origin].SurfaceToLowOrbit;
                    }
                    if (radioButtonLowOrbit2.IsChecked == true)
                    {
                        TotalDV -= ksp[comboBoxStop2.SelectedIndex].SurfaceToLowOrbit;
                    }

                    this.textBlockOriginTravel.Text = TravelInfo(comboBoxStop2.SelectedIndex, Origin, CurrentTime) +
                    TransferDV(TotalDV, DVPrevious);
                    DVPrevious = TotalDV;

                }
            }



            if (expanderStop3.IsExpanded == true && comboBoxStop3.SelectedIndex != 0 && comboBoxStop3.SelectedIndex < number())
            {
                this.textBlockStop3.Text = Texts(comboBoxStop3.SelectedIndex);
                TotalDV += DeltaVCount(PreviousPlanet, comboBoxStop3.SelectedIndex);

                if (checkBoxLowOrbit.IsChecked == true && (expanderStop1.IsExpanded == false || comboBoxStop1.SelectedIndex == 0) && (expanderStop2.IsExpanded == false || comboBoxStop2.SelectedIndex == 0))
                {
                    TotalDV -= ksp[Origin].SurfaceToLowOrbit;
                }

                if (radioButtonLowOrbit2.IsChecked == true && (expanderStop2.IsExpanded == true && comboBoxStop2.SelectedIndex != 0))
                {
                    TotalDV -= ksp[comboBoxStop2.SelectedIndex].SurfaceToLowOrbit;
                }
                else if ((expanderStop2.IsExpanded == false || comboBoxStop2.SelectedIndex == 0) && expanderStop1.IsExpanded == true && comboBoxStop1.SelectedIndex != 0 && radioButtonLowOrbit1.IsChecked == true)
                {
                    TotalDV -= ksp[comboBoxStop1.SelectedIndex].SurfaceToLowOrbit;
                }

                if (ksp[comboBoxStop3.SelectedIndex].Atmosphere)
                {
                    TotalDV -= ksp[comboBoxStop3.SelectedIndex].SurfaceToLowOrbit;
                }

                else if ((radioButtonLowOrbit.IsChecked == true))
                {
                    TotalDV -= ksp[comboBoxStop3.SelectedIndex].SurfaceToLowOrbit;
                }

                this.textBlockStop3Travel.Text = TravelInfo(PreviousPlanet, comboBoxStop3.SelectedIndex, CurrentTime) +
                TransferDV(TotalDV, DVPrevious);
                DVPrevious = TotalDV;
                PreviousPlanet = comboBoxStop3.SelectedIndex;

                imageStop3.Source = new BitmapImage(new Uri(ksp[comboBoxStop3.SelectedIndex].ImgUri));

                if ((checkBoxReturn.IsChecked == true))
                {
                    TotalDV += DeltaVCount(comboBoxStop3.SelectedIndex, Origin);

                    if (ksp[Origin].Atmosphere)
                    {
                        TotalDV -= ksp[Origin].SurfaceToLowOrbit;
                    }
                    if (radioButtonLowOrbit.IsChecked == true)
                    {
                        TotalDV -= ksp[comboBoxStop3.SelectedIndex].SurfaceToLowOrbit;
                    }

                    this.textBlockOriginTravel.Text = TravelInfo(comboBoxStop3.SelectedIndex, Origin, CurrentTime) +
                    TransferDV(TotalDV, DVPrevious);
                    DVPrevious = TotalDV;

                }
            }
            this.textBlockTotalDV.Text = string.Format("{0:n0}", TotalDV) + "m/s";
            this.textBlockTotaTime.Text = string.Format("{0:n0}", TotalTime / CurrentTime) + " Days";

        }

        private int number()
        {
            int number;
            if (radioButtonNormal.IsChecked == true)
            {
                number = PlanetsNumber;

            }
            else if (radioButtonOuter.IsChecked == true)
            {
                number = OuterPlanetsNumber;
            }
            else
            {
                number = RssPlanetNumber;
            }
            return number;
        }

        private string TransferDV(int TotalDV, int DVPrevius)
        {
            string txt =
            Environment.NewLine + "Δv : " + string.Format("{0:n0}", (TotalDV - DVPrevius)) + " m/s";
            return txt;
        }

        private string TravelInfo(int Counter, int i, int CurrentTime)
        {
            PlanetInit();
            double HohmannTransferTime = 0, Angle = 0, Pow = 0, IntervalTime = 0;
            double OrbitalPeriod1 = 0, OrbitalPeriod2 = 0;
            const double DioTrita = 0.66666666666666666666666666666667;
            if (ksp[Counter].Orbits == ksp[i].Orbits)
            {
                OrbitalPeriod1 = ksp[Counter].OrbitalPeriod;
                OrbitalPeriod2 = ksp[i].OrbitalPeriod;

            }
            else if (ksp[i].System == ksp[Counter].System && ksp[i].Orbits != ksp[Counter].Orbits)
            {
                OrbitalPeriod1 = 0;
                OrbitalPeriod2 = 0;
            }
            else
            {
                i = ksp[i].ParentIndex;
                Counter = ksp[Counter].ParentIndex;
                OrbitalPeriod1 = ksp[Counter].OrbitalPeriod;
                OrbitalPeriod2 = ksp[i].OrbitalPeriod;
            }

            Pow = Math.Pow(OrbitalPeriod2, DioTrita) + Math.Pow(OrbitalPeriod1, DioTrita);
            HohmannTransferTime = Math.Pow(Pow, 1.5) / Math.Sqrt(32);
            Angle = 180 - (360 * (HohmannTransferTime / OrbitalPeriod2));
            if (Angle < -360)
            {
                double flush = 0;
                flush = Math.Truncate(Angle / 360);
                Angle += Math.Abs(flush * 360);

            }
            IntervalTime = Math.Abs(1 / ((1 / OrbitalPeriod1) - (1 / OrbitalPeriod2)));
            TotalTime += HohmannTransferTime;
            string txt = "";
            txt = ksp[Counter].Name + " To " + ksp[i].Name +
                                Environment.NewLine + "Trasfer Time : " + string.Format("{0:#,#.00}", HohmannTransferTime / CurrentTime) + " Days" +
                                Environment.NewLine + "Phase Angle : " + string.Format("{0:#,#.00}", Angle) + "°" +
                                Environment.NewLine + "Interval between launch Windows : " + string.Format("{0:#,#.00}", IntervalTime / CurrentTime) + " Days";

            return txt;

        }
        
        private int DeltaVCount(int i, int j)
        {
            bool flag = true;
            int TotalDV = 0;
            PlanetInit();
            if ((ksp[i].Index == 25 && ksp[j].Index == 24) || (ksp[j].Index == 25 && ksp[i].Index == 24))
            {
                TotalDV = ksp[24].SurfaceToLowOrbit + ksp[25].MoonInterceptToElipticalOrbitMPC +
                    ksp[25].MoonInterceptToElipticalOrbit + ksp[25].LowOrbitToMoonIntercept + ksp[25].SurfaceToLowOrbit;
                flag = false;
            }
            else if ((ksp[i].Index == 25 && ksp[j].Index != 24) || (ksp[j].Index == 25 && ksp[i].Index != 24))
            {
                TotalDV = (ksp[i].SurfaceToLowOrbit + ksp[i].LowOrbitToMoonIntercept +
                ksp[i].MoonInterceptToElipticalOrbit + ksp[i].MoonInterceptToElipticalOrbitMPC +
                ksp[i].ElipticalOrbitToPlanetIntercet + ksp[i].PlanetInterceptToStarElipticalOrbit +
                ksp[i].MaxPlaneChange + ksp[i].LowOrbitToElipticalOrbit +
                ksp[j].SurfaceToLowOrbit + ksp[j].LowOrbitToMoonIntercept +
                ksp[j].MoonInterceptToElipticalOrbit + ksp[j].MoonInterceptToElipticalOrbitMPC +
                ksp[j].ElipticalOrbitToPlanetIntercet + ksp[j].PlanetInterceptToStarElipticalOrbit +
                ksp[j].MaxPlaneChange) + ksp[j].LowOrbitToElipticalOrbit + 1785;
            }
            else if ((ksp[i].Index == 27 && ksp[j].Index == 26) || (ksp[j].Index == 27 && ksp[i].Index == 26))
            {
                TotalDV = (ksp[i].SurfaceToLowOrbit + ksp[i].LowOrbitToMoonIntercept +
                    ksp[j].SurfaceToLowOrbit + ksp[j].LowOrbitToMoonIntercept);
                flag = false;
            }
            else
            {
                TotalDV = (ksp[i].SurfaceToLowOrbit + ksp[i].LowOrbitToMoonIntercept +
                ksp[i].MoonInterceptToElipticalOrbit + ksp[i].MoonInterceptToElipticalOrbitMPC +
                ksp[i].ElipticalOrbitToPlanetIntercet + ksp[i].PlanetInterceptToStarElipticalOrbit +
                ksp[i].MaxPlaneChange + ksp[i].LowOrbitToElipticalOrbit +
                ksp[j].SurfaceToLowOrbit + ksp[j].LowOrbitToMoonIntercept +
                ksp[j].MoonInterceptToElipticalOrbit + ksp[j].MoonInterceptToElipticalOrbitMPC +
                ksp[j].ElipticalOrbitToPlanetIntercet + ksp[j].PlanetInterceptToStarElipticalOrbit +
                ksp[j].MaxPlaneChange + ksp[j].LowOrbitToElipticalOrbit);
            }

            if ((ksp[i].System == ksp[j].System) && flag)
            {
                TotalDV -= (ksp[i].MaxPlaneChange + ksp[j].MaxPlaneChange + ksp[i].LowOrbitToElipticalOrbit + ksp[j].LowOrbitToElipticalOrbit +
                    ksp[j].ElipticalOrbitToPlanetIntercet + ksp[j].PlanetInterceptToStarElipticalOrbit +
                    ksp[i].ElipticalOrbitToPlanetIntercet + ksp[i].PlanetInterceptToStarElipticalOrbit);
            }
            return TotalDV;
        }

        private string Texts(int i)
        {
            string TextBlockTXT = "";

            PlanetInit();
            if (ksp[i].Type == Types.Moon)
            {
                TextBlockTXT =
                        "Celestial object : " + ksp[i].Name + "    Type : " + ksp[i].Type + "    Moon of : " + ksp[i].System +
                        Environment.NewLine +
                        "Surface Gravity : " + ksp[i].SurfaceGravity + " m/s^2" + "    Low Orbit : " + ksp[i].LowOrbit + Environment.NewLine +
                        "Escape Velocity : " + ksp[i].EscapeVelocity + "    Sphere of influence : " + ksp[i].SphereOfInfluence +
                        Environment.NewLine +
                        "Atmosphere Present: " + ksp[i].Atmosphere;
            }
            else if (ksp[i].Type != Types.Moon)
            {
                TextBlockTXT =
                        "Celestial object : " + ksp[i].Name + "    Type : " + ksp[i].Type +
                        Environment.NewLine +
                        "Surface Gravity : " + ksp[i].SurfaceGravity + " m/s^2" + "    Low Orbit : " + ksp[i].LowOrbit + Environment.NewLine +
                        "Escape Velocity : " + ksp[i].EscapeVelocity + "    Sphere of influence : " + ksp[i].SphereOfInfluence +
                        Environment.NewLine +
                        "Atmosphere  Present : " + ksp[i].Atmosphere;
            }
            if (ksp[i].Atmosphere)
            {
                TextBlockTXT +=
                    "   Oxigen Present : " + ksp[i].oxigen + "    Pressure : " + ksp[i].Pressure + " atm";
            }
            TextBlockTXT += Environment.NewLine +
            "Biomes : " + ksp[i].Biomes + "   Scientific Multiplier: " + "   Surface : " + ksp[i].Surface + "    Lower atmo : " + ksp[i].LowerAtmosphere + Environment.NewLine +
            "Upper Atmo : " + ksp[i].UpperAtmosphere  + "   Near Space : " + ksp[i].NearSpace + "   Outer Space : " + ksp[i].OuterSpace + Environment.NewLine;

            TextBlockTXT += " Δv info : " + "   Surface to Low Orbit : " + string.Format("{0:n0}", ksp[i].SurfaceToLowOrbit) + "m/s" + "    Low Orbit to SOI Edge : " +
                            string.Format("{0:n0}", (ksp[i].LowOrbitToMoonIntercept + ksp[i].MoonInterceptToElipticalOrbit + ksp[i].ElipticalOrbitToPlanetIntercet + ksp[i].LowOrbitToElipticalOrbit +
                                                     ksp[i].PlanetInterceptToStarElipticalOrbit)) + "m/s";
            return TextBlockTXT;
        }


        private void button1_Click(object sender, RoutedEventArgs e)
        {

            if (radioButtonNormal.IsChecked == true)
            {
                Phase_Angle_Map window = new Phase_Angle_Map();
                window.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,/Images/Backgrounds/Planets.png")));
                window.Show();

            }
            else if (radioButtonOuter.IsChecked == true)
            {
                Phase_Angle_Map window = new Phase_Angle_Map();
                window.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,/Images/Backgrounds/Outerplanets.png")));
                window.Show();
            }
            else
            {
                Phase_Angle_Map window = new Phase_Angle_Map();
                window.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,/Images/Backgrounds/rss.jpg")));
                window.Show();
            }

            /* string[] url = new string[20];
                 url[1] = @"pack://application:,,/PhaseAngleMaps/Moho.jpg";
                 url[2] = @"pack://application:,,/PhaseAngleMaps/Eve.jpg";
                 url[4] = @"pack://application:,,/PhaseAngleMaps/Kerbin.jpg";
                 url[5] = @"pack://application:,,/PhaseAngleMaps/Mun.jpg";
                 url[6] = @"pack://application:,,/PhaseAngleMaps/Minmus.jpg";
                 url[7] = @"pack://application:,,/PhaseAngleMaps/Duna.jpg";
                 url[9] = @"pack://application:,,/PhaseAngleMaps/Dres.jpg";
                 url[10] = @"pack://application:,,/PhaseAngleMaps/Jool.jpg";
                 url[11] = @"pack://application:,,/PhaseAngleMaps/Laythe.jpg";
                 url[12] = @"pack://application:,,/PhaseAngleMaps/Val.jpg";
                 url[13] = @"pack://application:,,/PhaseAngleMaps/Tylo.jpg";
                 url[14] = @"pack://application:,,/PhaseAngleMaps/Bop.jpg";
                 url[15] = @"pack://application:,,/PhaseAngleMaps/Pol.jpg";
                 url[16] = @"pack://application:,,/PhaseAngleMaps/Moho.jpg";

             if (radioButtonOr.IsChecked == true &&(comboBoxOrigin.SelectedIndex != 3) && (comboBoxOrigin.SelectedIndex != 8) && (comboBoxOrigin.SelectedIndex < 15) && (comboBoxOrigin.SelectedIndex != 0))
             {
                 Phase_Angle_Map window = new Phase_Angle_Map();
                 window.Background = new ImageBrush(new BitmapImage(new Uri(url[comboBoxOrigin.SelectedIndex])));
                 window.Show();     
             }
             if (radioButtonStop3.IsChecked == true && (comboBoxStop3.SelectedIndex != 3) && (comboBoxStop3.SelectedIndex != 8) && comboBoxStop3.SelectedIndex < 15 && (comboBoxStop3.SelectedIndex != 0))
             {
                 Phase_Angle_Map window = new Phase_Angle_Map();
                 window.Background = new ImageBrush(new BitmapImage(new Uri(url[comboBoxStop3.SelectedIndex])));
                 window.Show();
             }
             if (radioButtonStop1.IsChecked == true && (comboBoxStop1.SelectedIndex != 3) && (comboBoxStop1.SelectedIndex != 8) && comboBoxStop1.SelectedIndex < 15 && (comboBoxStop1.SelectedIndex != 0))
             {
                 Phase_Angle_Map window = new Phase_Angle_Map();
                 window.Background = new ImageBrush(new BitmapImage(new Uri(url[comboBoxStop1.SelectedIndex])));
                 window.Show();    
             }
             if (radioButtonStop2.IsChecked == true && (comboBoxStop2.SelectedIndex != 3) && (comboBoxStop2.SelectedIndex != 8) && comboBoxStop2.SelectedIndex < 15 && (comboBoxStop2.SelectedIndex != 0))
             {
                 Phase_Angle_Map window = new Phase_Angle_Map();
                 window.Background = new ImageBrush(new BitmapImage(new Uri(url[comboBoxStop2.SelectedIndex])));
                 window.Show();
             }*/
        }

        private void radioButtonNormal_Checked(object sender, RoutedEventArgs e)
        {
            PlanetInit();
            string[] ComboTxt = new string[number()];
            if (radioButtonNormal.IsChecked == true)
            {
                this.Main.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,/Images/Backgrounds/Planets2.png")));
                for (int i = 0; i < number(); i++)
                {
                    ksp[i] = new Planet.All();
                    ksp[i].Index = kspNormal[i].Index;
                    ksp[i].Name = kspNormal[i].Name;
                    ksp[i].Type = kspNormal[i].Type;
                    ksp[i].System = kspNormal[i].System;
                    ksp[i].LowOrbit = kspNormal[i].LowOrbit;
                    ksp[i].SurfaceGravity = kspNormal[i].SurfaceGravity;
                    ksp[i].Atmosphere = kspNormal[i].Atmosphere;
                    ksp[i].SurfaceToLowOrbit = kspNormal[i].SurfaceToLowOrbit;
                    ksp[i].LowOrbitToMoonIntercept = kspNormal[i].LowOrbitToMoonIntercept;
                    ksp[i].MoonInterceptToElipticalOrbit = kspNormal[i].MoonInterceptToElipticalOrbit;
                    ksp[i].MoonInterceptToElipticalOrbitMPC = kspNormal[i].MoonInterceptToElipticalOrbitMPC;
                    ksp[i].LowOrbitToElipticalOrbit = kspNormal[i].LowOrbitToElipticalOrbit;
                    ksp[i].ElipticalOrbitToPlanetIntercet = kspNormal[i].ElipticalOrbitToPlanetIntercet;
                    ksp[i].PlanetInterceptToStarElipticalOrbit = kspNormal[i].PlanetInterceptToStarElipticalOrbit;
                    ksp[i].MaxPlaneChange = kspNormal[i].MaxPlaneChange;
                    ksp[i].EscapeVelocity = kspNormal[i].EscapeVelocity;
                    ksp[i].SphereOfInfluence = kspNormal[i].SphereOfInfluence;
                    ksp[i].oxigen = kspNormal[i].oxigen;
                    ksp[i].Height = kspNormal[i].Height;
                    ksp[i].Pressure = kspNormal[i].Pressure;
                    ksp[i].Surface = kspNormal[i].Surface;
                    ksp[i].LowerAtmosphere = kspNormal[i].LowerAtmosphere;
                    ksp[i].UpperAtmosphere = kspNormal[i].UpperAtmosphere;
                    ksp[i].NearSpace = kspNormal[i].NearSpace;
                    ksp[i].OuterSpace = kspNormal[i].OuterSpace;
                    ksp[i].OrbitalPeriod = kspNormal[i].OrbitalPeriod;
                    ksp[i].Orbits = kspNormal[i].Orbits;
                    ksp[i].ParentIndex = kspNormal[i].ParentIndex;
                    ksp[i].Biomes = kspNormal[i].Biomes;
                    ksp[i].ImgUri = kspNormal[i].ImgUri;
                }
            }
            else if (radioButtonOuter.IsChecked == true)
            {
                this.Main.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,/Images/Backgrounds/20160503002320_1.jpg")));
                for (int i = 0; i < number(); i++)
                {
                    ksp[i] = new Planet.All();
                    ksp[i].Index = kspOuter[i].Index;
                    ksp[i].Name = kspOuter[i].Name;
                    ksp[i].Type = kspOuter[i].Type;
                    ksp[i].System = kspOuter[i].System;
                    ksp[i].LowOrbit = kspOuter[i].LowOrbit;
                    ksp[i].SurfaceGravity = kspOuter[i].SurfaceGravity;
                    ksp[i].Atmosphere = kspOuter[i].Atmosphere;
                    ksp[i].SurfaceToLowOrbit = kspOuter[i].SurfaceToLowOrbit;
                    ksp[i].LowOrbitToMoonIntercept = kspOuter[i].LowOrbitToMoonIntercept;
                    ksp[i].MoonInterceptToElipticalOrbit = kspOuter[i].MoonInterceptToElipticalOrbit;
                    ksp[i].MoonInterceptToElipticalOrbitMPC = kspOuter[i].MoonInterceptToElipticalOrbitMPC;
                    ksp[i].LowOrbitToElipticalOrbit = kspOuter[i].LowOrbitToElipticalOrbit;
                    ksp[i].ElipticalOrbitToPlanetIntercet = kspOuter[i].ElipticalOrbitToPlanetIntercet;
                    ksp[i].PlanetInterceptToStarElipticalOrbit = kspOuter[i].PlanetInterceptToStarElipticalOrbit;
                    ksp[i].MaxPlaneChange = kspOuter[i].MaxPlaneChange;
                    ksp[i].EscapeVelocity = kspOuter[i].EscapeVelocity;
                    ksp[i].SphereOfInfluence = kspOuter[i].SphereOfInfluence;
                    ksp[i].oxigen = kspOuter[i].oxigen;
                    ksp[i].Height = kspOuter[i].Height;
                    ksp[i].Pressure = kspOuter[i].Pressure;
                    ksp[i].Surface = kspOuter[i].Surface;
                    ksp[i].LowerAtmosphere = kspOuter[i].LowerAtmosphere;
                    ksp[i].UpperAtmosphere = kspOuter[i].UpperAtmosphere;
                    ksp[i].NearSpace = kspOuter[i].NearSpace;
                    ksp[i].OuterSpace = kspOuter[i].OuterSpace;
                    ksp[i].OrbitalPeriod = kspOuter[i].OrbitalPeriod;
                    ksp[i].Orbits = kspOuter[i].Orbits;
                    ksp[i].ParentIndex = kspOuter[i].ParentIndex;
                    ksp[i].Biomes = kspOuter[i].Biomes;
                    ksp[i].ImgUri = kspOuter[i].ImgUri;
                }
            }
            else
            {
                this.Main.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,/Images/Backgrounds/rss.png")));
                for (int i = 0; i < number(); i++)
                {
                    ksp[i] = new Planet.All();
                    ksp[i].Index = ksprss[i].Index;
                    ksp[i].Name = ksprss[i].Name;
                    ksp[i].Type = ksprss[i].Type;
                    ksp[i].System = ksprss[i].System;
                    ksp[i].LowOrbit = ksprss[i].LowOrbit;
                    ksp[i].SurfaceGravity = ksprss[i].SurfaceGravity;
                    ksp[i].Atmosphere = ksprss[i].Atmosphere;
                    ksp[i].SurfaceToLowOrbit = ksprss[i].SurfaceToLowOrbit;
                    ksp[i].LowOrbitToMoonIntercept = ksprss[i].LowOrbitToMoonIntercept;
                    ksp[i].MoonInterceptToElipticalOrbit = ksprss[i].MoonInterceptToElipticalOrbit;
                    ksp[i].MoonInterceptToElipticalOrbitMPC = ksprss[i].MoonInterceptToElipticalOrbitMPC;
                    ksp[i].LowOrbitToElipticalOrbit = ksprss[i].LowOrbitToElipticalOrbit;
                    ksp[i].ElipticalOrbitToPlanetIntercet = ksprss[i].ElipticalOrbitToPlanetIntercet;
                    ksp[i].PlanetInterceptToStarElipticalOrbit = ksprss[i].PlanetInterceptToStarElipticalOrbit;
                    ksp[i].MaxPlaneChange = ksprss[i].MaxPlaneChange;
                    ksp[i].EscapeVelocity = ksprss[i].EscapeVelocity;
                    ksp[i].SphereOfInfluence = ksprss[i].SphereOfInfluence;
                    ksp[i].oxigen = ksprss[i].oxigen;
                    ksp[i].Height = ksprss[i].Height;
                    ksp[i].Pressure = ksprss[i].Pressure;
                    ksp[i].Surface = ksprss[i].Surface;
                    ksp[i].LowerAtmosphere = ksprss[i].LowerAtmosphere;
                    ksp[i].UpperAtmosphere = ksprss[i].UpperAtmosphere;
                    ksp[i].NearSpace = ksprss[i].NearSpace;
                    ksp[i].OuterSpace = ksprss[i].OuterSpace;
                    ksp[i].OrbitalPeriod = ksprss[i].OrbitalPeriod;
                    ksp[i].Orbits = ksprss[i].Orbits;
                    ksp[i].ParentIndex = ksprss[i].ParentIndex;
                    ksp[i].Biomes = ksprss[i].Biomes;
                    ksp[i].ImgUri = ksprss[i].ImgUri;
                }
            }
            for (int i = 0; i < number(); i++)
            {
                if (ksp[i].Type == Types.Moon)
                {
                    ComboTxt[i] = "    " + ksp[i].Name;

                }
                else
                {
                    ComboTxt[i] = ksp[i].Name;
                }
            }
            comboBoxOrigin.ItemsSource = ComboTxt;
            comboBoxStop3.ItemsSource = ComboTxt;
            comboBoxStop1.ItemsSource = ComboTxt;
            comboBoxStop2.ItemsSource = ComboTxt;
        }

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            if (checkBox.IsChecked == true)
            {
                expanderOrigin.IsExpanded = true;
                expanderStop1.IsExpanded = true;
                expanderStop2.IsExpanded = true;
                expanderStop3.IsExpanded = true;
            }
            else
            {
                expanderOrigin.IsExpanded = false;
                expanderStop1.IsExpanded = false;
                expanderStop2.IsExpanded = false;
                expanderStop3.IsExpanded = false;
            }
        }
        private void PlanetInit()
        {

            // Normal Planets

            kspNormal[0] = new Planet.All();
            kspNormal[0].Index = 0;
            kspNormal[0].Name = "None";
            kspNormal[0].Type = Types.Star;
            kspNormal[0].System = Systems.None;
            kspNormal[0].LowOrbit = "";
            kspNormal[0].SurfaceGravity = 0;
            kspNormal[0].Atmosphere = false;
            kspNormal[0].SurfaceToLowOrbit = 0;
            kspNormal[0].LowOrbitToMoonIntercept = 0;
            kspNormal[0].MoonInterceptToElipticalOrbit = 0;
            kspNormal[0].MoonInterceptToElipticalOrbitMPC = 0;
            kspNormal[0].LowOrbitToElipticalOrbit = 0;
            kspNormal[0].ElipticalOrbitToPlanetIntercet = 0;
            kspNormal[0].PlanetInterceptToStarElipticalOrbit = 0;
            kspNormal[0].MaxPlaneChange = 0;
            kspNormal[0].EscapeVelocity = "0 m/s";
            kspNormal[0].SphereOfInfluence = "";
            kspNormal[0].oxigen = false;
            kspNormal[0].Height = 00;
            kspNormal[0].Pressure = 0;
            kspNormal[0].Surface = 0;
            kspNormal[0].LowerAtmosphere = 0;
            kspNormal[0].UpperAtmosphere = 0;
            kspNormal[0].NearSpace = 0;
            kspNormal[0].OuterSpace = 0;
            kspNormal[0].OrbitalPeriod = 0;
            kspNormal[0].Orbits = Orbits.None;
            kspNormal[0].ParentIndex = 0;
            kspNormal[0].Biomes = 0;
            kspNormal[0].ImgUri = @"pack://application:,,/Images/Planets/None.png";

            kspNormal[1] = new Planet.All();
            kspNormal[1].Index = 1;
            kspNormal[1].Name = "Moho";
            kspNormal[1].Type = Types.Planet;
            kspNormal[1].System = Systems.Moho;
            kspNormal[1].LowOrbit = "50 km";
            kspNormal[1].SurfaceGravity = 2.7;
            kspNormal[1].Atmosphere = false;
            kspNormal[1].SurfaceToLowOrbit = 870;
            kspNormal[1].LowOrbitToMoonIntercept = 0;
            kspNormal[1].MoonInterceptToElipticalOrbit = 0;
            kspNormal[1].MoonInterceptToElipticalOrbitMPC = 0;
            kspNormal[1].LowOrbitToElipticalOrbit = 0;
            kspNormal[1].ElipticalOrbitToPlanetIntercet = 2410;
            kspNormal[1].PlanetInterceptToStarElipticalOrbit = 760;
            kspNormal[1].MaxPlaneChange = 2520;
            kspNormal[1].EscapeVelocity = "1,161.41 m/s";
            kspNormal[1].SphereOfInfluence = "9,646,663 m";
            kspNormal[1].oxigen = false;
            kspNormal[1].Height = 0;
            kspNormal[1].Pressure = 0;
            kspNormal[1].Surface = 6;
            kspNormal[1].LowerAtmosphere = 0;
            kspNormal[1].UpperAtmosphere = 0;
            kspNormal[1].NearSpace = 5;
            kspNormal[1].OuterSpace = 4.5;
            kspNormal[1].OrbitalPeriod = 2215754.2;
            kspNormal[1].Orbits = Orbits.Kerbol;
            kspNormal[1].ParentIndex = 1;
            kspNormal[1].Biomes = 12;
            kspNormal[1].ImgUri = @"pack://application:,,/Images/Planets/Moho.png";

            kspNormal[2] = new Planet.All();
            kspNormal[2].Index = 2;
            kspNormal[2].Name = "Eve";
            kspNormal[2].Type = Types.Planet;
            kspNormal[2].System = Systems.Eve;
            kspNormal[2].LowOrbit = "100 km";
            kspNormal[2].SurfaceGravity = 16.4;
            kspNormal[2].Atmosphere = true;
            kspNormal[2].SurfaceToLowOrbit = 8000;
            kspNormal[2].LowOrbitToMoonIntercept = 0;
            kspNormal[2].MoonInterceptToElipticalOrbit = 1330;
            kspNormal[2].MoonInterceptToElipticalOrbitMPC = 0;
            kspNormal[2].LowOrbitToElipticalOrbit = 0;
            kspNormal[2].ElipticalOrbitToPlanetIntercet = 80;
            kspNormal[2].PlanetInterceptToStarElipticalOrbit = 90;
            kspNormal[2].MaxPlaneChange = 430;
            kspNormal[2].EscapeVelocity = "4,831.96 m/s";
            kspNormal[2].SphereOfInfluence = "85,109,365 m";
            kspNormal[2].oxigen = false;
            kspNormal[2].Height = 90000;
            kspNormal[2].Pressure = 5;
            kspNormal[2].Surface = 5;
            kspNormal[2].LowerAtmosphere = 6;
            kspNormal[2].UpperAtmosphere = 5.5;
            kspNormal[2].NearSpace = 4;
            kspNormal[2].OuterSpace = 3.5;
            kspNormal[2].OrbitalPeriod = 5657995.1;
            kspNormal[2].Orbits = Orbits.Kerbol;
            kspNormal[2].ParentIndex = 2;
            kspNormal[2].Biomes = 7;
            kspNormal[2].ImgUri = @"pack://application:,,/Images/Planets/Eve.png";

            kspNormal[3] = new Planet.All();
            kspNormal[3].Index = 3;
            kspNormal[3].Name = "Gilly";
            kspNormal[3].Type = Types.Moon;
            kspNormal[3].System = Systems.Eve;
            kspNormal[3].LowOrbit = "10 km";
            kspNormal[3].SurfaceGravity = 0.049;
            kspNormal[3].Atmosphere = false;
            kspNormal[3].SurfaceToLowOrbit = 30;
            kspNormal[3].LowOrbitToMoonIntercept = 410;
            kspNormal[3].MoonInterceptToElipticalOrbit = 60;
            kspNormal[3].MoonInterceptToElipticalOrbitMPC = 0;
            kspNormal[3].LowOrbitToElipticalOrbit = 0;
            kspNormal[3].ElipticalOrbitToPlanetIntercet = 80;
            kspNormal[3].PlanetInterceptToStarElipticalOrbit = 90;
            kspNormal[3].MaxPlaneChange = 430;
            kspNormal[3].EscapeVelocity = "35.71 m/s";
            kspNormal[3].SphereOfInfluence = "126,123.27 m";
            kspNormal[3].oxigen = false;
            kspNormal[3].Height = 0;
            kspNormal[3].Pressure = 0;
            kspNormal[3].Surface = 4;
            kspNormal[3].LowerAtmosphere = 0;
            kspNormal[3].UpperAtmosphere = 0;
            kspNormal[3].NearSpace = 3;
            kspNormal[3].OuterSpace = 2.5;
            kspNormal[3].OrbitalPeriod = 388587.4;
            kspNormal[3].Orbits = Orbits.Eve;
            kspNormal[3].ParentIndex = 2;
            kspNormal[3].Biomes = 3;
            kspNormal[3].ImgUri = @"pack://application:,,/Images/Planets/Gilly.png";

            kspNormal[4] = new Planet.All();
            kspNormal[4].Index = 4;
            kspNormal[4].Name = "Kerbin";
            kspNormal[4].Type = Types.Planet;
            kspNormal[4].System = Systems.Kerbin;
            kspNormal[4].LowOrbit = "80 km";
            kspNormal[4].SurfaceGravity = 9.81;
            kspNormal[4].Atmosphere = true;
            kspNormal[4].SurfaceToLowOrbit = 3400;
            kspNormal[4].LowOrbitToMoonIntercept = 0;
            kspNormal[4].MoonInterceptToElipticalOrbit = 0;
            kspNormal[4].MoonInterceptToElipticalOrbitMPC = 0;
            kspNormal[4].LowOrbitToElipticalOrbit = 0;
            kspNormal[4].ElipticalOrbitToPlanetIntercet = 0;
            kspNormal[4].PlanetInterceptToStarElipticalOrbit = 950;
            kspNormal[4].MaxPlaneChange = 0;
            kspNormal[4].EscapeVelocity = "3,431.03 m/s";
            kspNormal[4].SphereOfInfluence = "84,159,286 m";
            kspNormal[4].oxigen = true;
            kspNormal[4].Height = 70000;
            kspNormal[4].Pressure = 1;
            kspNormal[4].Surface = 0.3;
            kspNormal[4].LowerAtmosphere = 0.7;
            kspNormal[4].UpperAtmosphere = 0.9;
            kspNormal[4].NearSpace = 1;
            kspNormal[4].OuterSpace = 1.5;
            kspNormal[4].OrbitalPeriod = 9203544.6;
            kspNormal[4].Orbits = Orbits.Kerbol;
            kspNormal[4].ParentIndex = 4;
            kspNormal[4].Biomes = 42;
            kspNormal[4].ImgUri = @"pack://application:,,/Images/Planets/Kerbin.png";

            kspNormal[5] = new Planet.All();
            kspNormal[5].Index = 5;
            kspNormal[5].Name = "Mun";
            kspNormal[5].Type = Types.Moon;
            kspNormal[5].System = Systems.Kerbin;
            kspNormal[5].LowOrbit = "14 km";
            kspNormal[5].SurfaceGravity = 1.63;
            kspNormal[5].Atmosphere = false;
            kspNormal[5].SurfaceToLowOrbit = 580;
            kspNormal[5].LowOrbitToMoonIntercept = 310;
            kspNormal[5].MoonInterceptToElipticalOrbit = 860;
            kspNormal[5].MoonInterceptToElipticalOrbitMPC = 0;
            kspNormal[5].LowOrbitToElipticalOrbit = 0;
            kspNormal[5].ElipticalOrbitToPlanetIntercet = 0;
            kspNormal[5].PlanetInterceptToStarElipticalOrbit = 950;
            kspNormal[5].MaxPlaneChange = 0;
            kspNormal[5].EscapeVelocity = "807.08 m/s";
            kspNormal[5].SphereOfInfluence = "2,429,559.1 m";
            kspNormal[5].oxigen = false;
            kspNormal[5].Height = 0;
            kspNormal[5].Pressure = 0;
            kspNormal[5].Surface = 3.5;
            kspNormal[5].LowerAtmosphere = 0;
            kspNormal[5].UpperAtmosphere = 0;
            kspNormal[5].NearSpace = 2.5;
            kspNormal[5].OuterSpace = 2;
            kspNormal[5].OrbitalPeriod = 138984.4;
            kspNormal[5].Orbits = Orbits.Kerbin;
            kspNormal[5].ParentIndex = 4;
            kspNormal[5].Biomes = 15;
            kspNormal[5].ImgUri = @"pack://application:,,/Images/Planets/Mun.png";

            kspNormal[6] = new Planet.All();
            kspNormal[6].Index = 6;
            kspNormal[6].Name = "Minmus";
            kspNormal[6].Type = Types.Moon;
            kspNormal[6].System = Systems.Kerbin;
            kspNormal[6].LowOrbit = "10 km";
            kspNormal[6].SurfaceGravity = 0.491;
            kspNormal[6].Atmosphere = false;
            kspNormal[6].SurfaceToLowOrbit = 180;
            kspNormal[6].LowOrbitToMoonIntercept = 160;
            kspNormal[6].MoonInterceptToElipticalOrbit = 930;
            kspNormal[6].MoonInterceptToElipticalOrbitMPC = 340;
            kspNormal[6].LowOrbitToElipticalOrbit = 0;
            kspNormal[6].ElipticalOrbitToPlanetIntercet = 0;
            kspNormal[6].PlanetInterceptToStarElipticalOrbit = 950;
            kspNormal[6].MaxPlaneChange = 0;
            kspNormal[6].EscapeVelocity = "242.61 m/s";
            kspNormal[6].SphereOfInfluence = "2,247,428.4 m";
            kspNormal[6].oxigen = false;
            kspNormal[6].Height = 0;
            kspNormal[6].Pressure = 0;
            kspNormal[6].Surface = 3;
            kspNormal[6].LowerAtmosphere = 0;
            kspNormal[6].UpperAtmosphere = 0;
            kspNormal[6].NearSpace = 2;
            kspNormal[6].OuterSpace = 1.5;
            kspNormal[6].OrbitalPeriod = 1077310.5;
            kspNormal[6].Orbits = Orbits.Kerbin;
            kspNormal[6].ParentIndex = 4;
            kspNormal[6].Biomes = 9;
            kspNormal[6].ImgUri = @"pack://application:,,/Images/Planets/Minmus.png";

            kspNormal[7] = new Planet.All();
            kspNormal[7].Index = 7;
            kspNormal[7].Name = "Duna";
            kspNormal[7].Type = Types.Planet;
            kspNormal[7].System = Systems.Duna;
            kspNormal[7].LowOrbit = "60 km";
            kspNormal[7].SurfaceGravity = 2.94;
            kspNormal[7].Atmosphere = true;
            kspNormal[7].SurfaceToLowOrbit = 1450;
            kspNormal[7].LowOrbitToMoonIntercept = 0;
            kspNormal[7].MoonInterceptToElipticalOrbit = 360;
            kspNormal[7].MoonInterceptToElipticalOrbitMPC = 0;
            kspNormal[7].LowOrbitToElipticalOrbit = 0;
            kspNormal[7].ElipticalOrbitToPlanetIntercet = 250;
            kspNormal[7].PlanetInterceptToStarElipticalOrbit = 130;
            kspNormal[7].MaxPlaneChange = 10;
            kspNormal[7].EscapeVelocity = "1,372.41 m/s";
            kspNormal[7].SphereOfInfluence = "47,921,949 m";
            kspNormal[7].oxigen = false;
            kspNormal[7].Height = 50000;
            kspNormal[7].Pressure = 0.0666;
            kspNormal[7].Surface = 4;
            kspNormal[7].LowerAtmosphere = 5;
            kspNormal[7].UpperAtmosphere = 4.5;
            kspNormal[7].NearSpace = 3;
            kspNormal[7].OuterSpace = 2.5;
            kspNormal[7].OrbitalPeriod = 17315400.1;
            kspNormal[7].Orbits = Orbits.Kerbol;
            kspNormal[7].ParentIndex = 7;
            kspNormal[7].Biomes = 5;
            kspNormal[7].ImgUri = @"pack://application:,,/Images/Planets/Duna.png";

            kspNormal[8] = new Planet.All();
            kspNormal[8].Index = 8;
            kspNormal[8].Name = "Ike";
            kspNormal[8].Type = Types.Moon;
            kspNormal[8].System = Systems.Duna;
            kspNormal[8].LowOrbit = "10 km";
            kspNormal[8].SurfaceGravity = 1.1;
            kspNormal[8].Atmosphere = false;
            kspNormal[8].SurfaceToLowOrbit = 390;
            kspNormal[8].LowOrbitToMoonIntercept = 180;
            kspNormal[8].MoonInterceptToElipticalOrbit = 30;
            kspNormal[8].MoonInterceptToElipticalOrbitMPC = 0;
            kspNormal[8].LowOrbitToElipticalOrbit = 0;
            kspNormal[8].ElipticalOrbitToPlanetIntercet = 250;
            kspNormal[8].PlanetInterceptToStarElipticalOrbit = 130;
            kspNormal[8].MaxPlaneChange = 10;
            kspNormal[8].EscapeVelocity = "534.48 m/s";
            kspNormal[8].SphereOfInfluence = "1,049,598.9 m";
            kspNormal[8].oxigen = false;
            kspNormal[8].Height = 0;
            kspNormal[8].Pressure = 0;
            kspNormal[8].Surface = 5;
            kspNormal[8].LowerAtmosphere = 0;
            kspNormal[8].UpperAtmosphere = 0;
            kspNormal[8].NearSpace = 4;
            kspNormal[8].OuterSpace = 3.5;
            kspNormal[8].OrbitalPeriod = 65517.9;
            kspNormal[8].Orbits = Orbits.Duna;
            kspNormal[8].ParentIndex = 7;
            kspNormal[8].Biomes = 8;
            kspNormal[8].ImgUri = @"pack://application:,,/Images/Planets/Ike.png";

            kspNormal[9] = new Planet.All();
            kspNormal[9].Index = 9;
            kspNormal[9].Name = "Dres";
            kspNormal[9].Type = Types.Planet;
            kspNormal[9].System = Systems.Dres;
            kspNormal[9].LowOrbit = "12 km";
            kspNormal[9].SurfaceGravity = 1.13;
            kspNormal[9].Atmosphere = false;
            kspNormal[9].SurfaceToLowOrbit = 430;
            kspNormal[9].LowOrbitToMoonIntercept = 0;
            kspNormal[9].MoonInterceptToElipticalOrbit = 0;
            kspNormal[9].MoonInterceptToElipticalOrbitMPC = 0;
            kspNormal[9].LowOrbitToElipticalOrbit = 0;
            kspNormal[9].ElipticalOrbitToPlanetIntercet = 1290;
            kspNormal[9].PlanetInterceptToStarElipticalOrbit = 610;
            kspNormal[9].MaxPlaneChange = 1010;
            kspNormal[9].EscapeVelocity = "558 m/s";
            kspNormal[9].SphereOfInfluence = "32,832,840 m";
            kspNormal[9].oxigen = false;
            kspNormal[9].Height = 0;
            kspNormal[9].Pressure = 0;
            kspNormal[9].Surface = 6;
            kspNormal[9].LowerAtmosphere = 0;
            kspNormal[9].UpperAtmosphere = 0;
            kspNormal[9].NearSpace = 5;
            kspNormal[9].OuterSpace = 4.5;
            kspNormal[9].OrbitalPeriod = 47893063.1;
            kspNormal[9].Orbits = Orbits.Kerbol;
            kspNormal[9].ParentIndex = 9;
            kspNormal[9].Biomes = 8;
            kspNormal[9].ImgUri = @"pack://application:,,/Images/Planets/Dres.png";

            kspNormal[10] = new Planet.All();
            kspNormal[10].Index = 10;
            kspNormal[10].Name = "Jool";
            kspNormal[10].Type = Types.Gasgiant;
            kspNormal[10].System = Systems.Jool;
            kspNormal[10].LowOrbit = "210 km";
            kspNormal[10].SurfaceGravity = 7.85;
            kspNormal[10].Atmosphere = true;
            kspNormal[10].SurfaceToLowOrbit = 14000;
            kspNormal[10].LowOrbitToMoonIntercept = 0;
            kspNormal[10].MoonInterceptToElipticalOrbit = 2810;
            kspNormal[10].MoonInterceptToElipticalOrbitMPC = 0;
            kspNormal[10].LowOrbitToElipticalOrbit = 0;
            kspNormal[10].ElipticalOrbitToPlanetIntercet = 160;
            kspNormal[10].PlanetInterceptToStarElipticalOrbit = 980;
            kspNormal[10].MaxPlaneChange = 270;
            kspNormal[10].EscapeVelocity = "9.704.43 m/s";
            kspNormal[10].SphereOfInfluence = "2.4 * 10^9 m";
            kspNormal[10].oxigen = false;
            kspNormal[10].Height = 200000;
            kspNormal[10].Pressure = 15;
            kspNormal[10].Surface = 0;
            kspNormal[10].LowerAtmosphere = 8;
            kspNormal[10].UpperAtmosphere = 7.5;
            kspNormal[10].NearSpace = 7;
            kspNormal[10].OuterSpace = 6.5;
            kspNormal[10].OrbitalPeriod = 104661432.1;
            kspNormal[10].Orbits = Orbits.Kerbol;
            kspNormal[10].ParentIndex = 10;
            kspNormal[10].Biomes = 0;
            kspNormal[10].ImgUri = @"pack://application:,,/Images/Planets/Jool.png";

            kspNormal[11] = new Planet.All();
            kspNormal[11].Index = 11;
            kspNormal[11].Name = "Laythe";
            kspNormal[11].Type = Types.Moon;
            kspNormal[11].System = Systems.Jool;
            kspNormal[11].LowOrbit = "60 km";
            kspNormal[11].SurfaceGravity = 7.85;
            kspNormal[11].Atmosphere = true;
            kspNormal[11].SurfaceToLowOrbit = 2900;
            kspNormal[11].LowOrbitToMoonIntercept = 1070;
            kspNormal[11].MoonInterceptToElipticalOrbit = 930;
            kspNormal[11].MoonInterceptToElipticalOrbitMPC = 0;
            kspNormal[11].LowOrbitToElipticalOrbit = 0;
            kspNormal[11].ElipticalOrbitToPlanetIntercet = 160;
            kspNormal[11].PlanetInterceptToStarElipticalOrbit = 980;
            kspNormal[11].MaxPlaneChange = 270;
            kspNormal[11].EscapeVelocity = "2,801.43 m/s";
            kspNormal[11].SphereOfInfluence = "3,723,645.8 m";
            kspNormal[11].oxigen = true;
            kspNormal[11].Height = 50000;
            kspNormal[11].Pressure = 0.6;
            kspNormal[11].Surface = 9;
            kspNormal[11].LowerAtmosphere = 10;
            kspNormal[11].UpperAtmosphere = 9.5;
            kspNormal[11].NearSpace = 8;
            kspNormal[11].OuterSpace = 7.5;
            kspNormal[11].OrbitalPeriod = 52980.9;
            kspNormal[11].Orbits = Orbits.Jool;
            kspNormal[11].ParentIndex = 10;
            kspNormal[11].Biomes = 5;
            kspNormal[11].ImgUri = @"pack://application:,,/Images/Planets/Laythe.png";

            kspNormal[12] = new Planet.All();
            kspNormal[12].Index = 12;
            kspNormal[12].Name = "Vall";
            kspNormal[12].Type = Types.Moon;
            kspNormal[12].System = Systems.Jool;
            kspNormal[12].LowOrbit = "15 km";
            kspNormal[12].SurfaceGravity = 2.31;
            kspNormal[12].Atmosphere = false;
            kspNormal[12].SurfaceToLowOrbit = 860;
            kspNormal[12].LowOrbitToMoonIntercept = 910;
            kspNormal[12].MoonInterceptToElipticalOrbit = 620;
            kspNormal[12].MoonInterceptToElipticalOrbitMPC = 0;
            kspNormal[12].LowOrbitToElipticalOrbit = 0;
            kspNormal[12].ElipticalOrbitToPlanetIntercet = 160;
            kspNormal[12].PlanetInterceptToStarElipticalOrbit = 980;
            kspNormal[12].MaxPlaneChange = 270;
            kspNormal[12].EscapeVelocity = "1,176.10 m/s";
            kspNormal[12].SphereOfInfluence = "2,406,401.4 m";
            kspNormal[12].oxigen = false;
            kspNormal[12].Height = 0;
            kspNormal[12].Pressure = 0;
            kspNormal[12].Surface = 9;
            kspNormal[12].LowerAtmosphere = 0;
            kspNormal[12].UpperAtmosphere = 0;
            kspNormal[12].NearSpace = 8;
            kspNormal[12].OuterSpace = 7.5;
            kspNormal[12].OrbitalPeriod = 105962.1;
            kspNormal[12].Orbits = Orbits.Jool;
            kspNormal[12].ParentIndex = 10;
            kspNormal[12].Biomes = 4;
            kspNormal[12].ImgUri = @"pack://application:,,/Images/Planets/Vall.png";

            kspNormal[13] = new Planet.All();
            kspNormal[13].Index = 13;
            kspNormal[13].Name = "Tylo";
            kspNormal[13].Type = Types.Moon;
            kspNormal[13].System = Systems.Jool;
            kspNormal[13].LowOrbit = "10 km";
            kspNormal[13].SurfaceGravity = 7.85;
            kspNormal[13].Atmosphere = false;
            kspNormal[13].SurfaceToLowOrbit = 2270;
            kspNormal[13].LowOrbitToMoonIntercept = 1100;
            kspNormal[13].MoonInterceptToElipticalOrbit = 400;
            kspNormal[13].MoonInterceptToElipticalOrbitMPC = 0;
            kspNormal[13].LowOrbitToElipticalOrbit = 0;
            kspNormal[13].ElipticalOrbitToPlanetIntercet = 160;
            kspNormal[13].PlanetInterceptToStarElipticalOrbit = 980;
            kspNormal[13].MaxPlaneChange = 270;
            kspNormal[13].EscapeVelocity = "3,068.81 m/s";
            kspNormal[13].SphereOfInfluence = "10,856,518 m";
            kspNormal[13].oxigen = false;
            kspNormal[13].Height = 0;
            kspNormal[13].Pressure = 0;
            kspNormal[13].Surface = 9.5;
            kspNormal[13].LowerAtmosphere = 0;
            kspNormal[13].UpperAtmosphere = 0;
            kspNormal[13].NearSpace = 8.5;
            kspNormal[13].OuterSpace = 8;
            kspNormal[13].OrbitalPeriod = 211926.4;
            kspNormal[13].Orbits = Orbits.Jool;
            kspNormal[13].ParentIndex = 10;
            kspNormal[13].Biomes = 8;
            kspNormal[13].ImgUri = @"pack://application:,,/Images/Planets/Tylo.png";

            kspNormal[14] = new Planet.All();
            kspNormal[14].Index = 14;
            kspNormal[14].Name = "Bop";
            kspNormal[14].Type = Types.Moon;
            kspNormal[14].System = Systems.Jool;
            kspNormal[14].LowOrbit = "10 km";
            kspNormal[14].SurfaceGravity = 0.589;
            kspNormal[14].Atmosphere = false;
            kspNormal[14].SurfaceToLowOrbit = 220;
            kspNormal[14].LowOrbitToMoonIntercept = 900;
            kspNormal[14].MoonInterceptToElipticalOrbit = 220;
            kspNormal[14].MoonInterceptToElipticalOrbitMPC = 2440;
            kspNormal[14].LowOrbitToElipticalOrbit = 0;
            kspNormal[14].ElipticalOrbitToPlanetIntercet = 160;
            kspNormal[14].PlanetInterceptToStarElipticalOrbit = 980;
            kspNormal[14].MaxPlaneChange = 270;
            kspNormal[14].EscapeVelocity = "276.62 m/s";
            kspNormal[14].SphereOfInfluence = "1,221,060.9 m";
            kspNormal[14].oxigen = false;
            kspNormal[14].Height = 0;
            kspNormal[14].Pressure = 0;
            kspNormal[14].Surface = 9;
            kspNormal[14].LowerAtmosphere = 0;
            kspNormal[14].UpperAtmosphere = 0;
            kspNormal[14].NearSpace = 8;
            kspNormal[14].OuterSpace = 7.5;
            kspNormal[14].OrbitalPeriod = 544507.4;
            kspNormal[14].Orbits = Orbits.Jool;
            kspNormal[14].ParentIndex = 10;
            kspNormal[14].Biomes = 5;
            kspNormal[14].ImgUri = @"pack://application:,,/Images/Planets/Bop.png";

            kspNormal[15] = new Planet.All();
            kspNormal[15].Index = 15;
            kspNormal[15].Name = "Pol";
            kspNormal[15].Type = Types.Moon;
            kspNormal[15].System = Systems.Jool;
            kspNormal[15].LowOrbit = "10 km";
            kspNormal[15].SurfaceGravity = 0.373;
            kspNormal[15].Atmosphere = false;
            kspNormal[15].SurfaceToLowOrbit = 130;
            kspNormal[15].LowOrbitToMoonIntercept = 820;
            kspNormal[15].MoonInterceptToElipticalOrbit = 160;
            kspNormal[15].MoonInterceptToElipticalOrbitMPC = 700;
            kspNormal[15].LowOrbitToElipticalOrbit = 0;
            kspNormal[15].ElipticalOrbitToPlanetIntercet = 160;
            kspNormal[15].PlanetInterceptToStarElipticalOrbit = 980;
            kspNormal[15].MaxPlaneChange = 270;
            kspNormal[15].EscapeVelocity = "181.12 m/s";
            kspNormal[15].SphereOfInfluence = "1,042,135.9 m";
            kspNormal[15].oxigen = false;
            kspNormal[15].Height = 0;
            kspNormal[15].Pressure = 0;
            kspNormal[15].Surface = 9;
            kspNormal[15].LowerAtmosphere = 0;
            kspNormal[15].UpperAtmosphere = 0;
            kspNormal[15].NearSpace = 8;
            kspNormal[15].OuterSpace = 7.5;
            kspNormal[15].OrbitalPeriod = 901902.6;
            kspNormal[15].Orbits = Orbits.Jool;
            kspNormal[15].ParentIndex = 10;
            kspNormal[15].Biomes = 4;
            kspNormal[15].ImgUri = @"pack://application:,,/Images/Planets/Pol.png";

            kspNormal[16] = new Planet.All();
            kspNormal[16].Index = 16;
            kspNormal[16].Name = "Eeloo";
            kspNormal[16].Type = Types.Planet;
            kspNormal[16].System = Systems.Eeloo;
            kspNormal[16].LowOrbit = "10 km";
            kspNormal[16].SurfaceGravity = 1.69;
            kspNormal[16].Atmosphere = false;
            kspNormal[16].SurfaceToLowOrbit = 620;
            kspNormal[16].LowOrbitToMoonIntercept = 0;
            kspNormal[16].MoonInterceptToElipticalOrbit = 0;
            kspNormal[16].MoonInterceptToElipticalOrbitMPC = 0;
            kspNormal[16].LowOrbitToElipticalOrbit = 0;
            kspNormal[16].ElipticalOrbitToPlanetIntercet = 1370;
            kspNormal[16].PlanetInterceptToStarElipticalOrbit = 1140;
            kspNormal[16].MaxPlaneChange = 1330;
            kspNormal[16].EscapeVelocity = "841.83 m/s";
            kspNormal[16].SphereOfInfluence = "1.2 * 10^8 m";
            kspNormal[16].oxigen = false;
            kspNormal[16].Height = 0;
            kspNormal[16].Pressure = 0;
            kspNormal[16].Surface = 15;
            kspNormal[16].LowerAtmosphere = 0;
            kspNormal[16].UpperAtmosphere = 0;
            kspNormal[16].NearSpace = 12;
            kspNormal[16].OuterSpace = 10;
            kspNormal[16].OrbitalPeriod = 156992048.4;
            kspNormal[16].Orbits = Orbits.Kerbol;
            kspNormal[16].ParentIndex = 16;
            kspNormal[16].Biomes = 7;
            kspNormal[16].ImgUri = @"pack://application:,,/Images/Planets/Eeloo.png";


            kspNormal[17] = new Planet.All();
            kspNormal[17].Index = 17;
            kspNormal[17].Name = "Kerbol";
            kspNormal[17].Type = Types.Star;
            kspNormal[17].System = Systems.Kerbol;
            kspNormal[17].LowOrbit = "610 km";
            kspNormal[17].SurfaceGravity = 17.1;
            kspNormal[17].Atmosphere = true;
            kspNormal[17].SurfaceToLowOrbit = 67000;
            kspNormal[17].LowOrbitToMoonIntercept = 0;
            kspNormal[17].MoonInterceptToElipticalOrbit = 0;
            kspNormal[17].MoonInterceptToElipticalOrbitMPC = 0;
            kspNormal[17].LowOrbitToElipticalOrbit = 0;
            kspNormal[17].ElipticalOrbitToPlanetIntercet = 13700;
            kspNormal[17].PlanetInterceptToStarElipticalOrbit = 6000;
            kspNormal[17].MaxPlaneChange = 0;
            kspNormal[17].EscapeVelocity = "94,672.01 m/s";
            kspNormal[17].SphereOfInfluence = "∞";
            kspNormal[17].oxigen = true;
            kspNormal[17].Height = 600000;
            kspNormal[17].Pressure = 0.158;
            kspNormal[17].Surface = 0;
            kspNormal[17].LowerAtmosphere = 1;
            kspNormal[17].UpperAtmosphere = 1;
            kspNormal[17].NearSpace = 7;
            kspNormal[17].OuterSpace = 2;
            kspNormal[17].OrbitalPeriod = 0;
            kspNormal[17].Orbits = Orbits.None;
            kspNormal[17].ParentIndex = 17;
            kspNormal[17].Biomes = 0;
            kspNormal[17].ImgUri = @"pack://application:,,/Images/Planets/Kerbol.png";

            // Outer Planet Planets


            kspOuter[0] = new Planet.All();
            kspOuter[0].Index = 0;
            kspOuter[0].Name = "None";
            kspOuter[0].Type = Types.Star;
            kspOuter[0].System = Systems.None;
            kspOuter[0].LowOrbit = "";
            kspOuter[0].SurfaceGravity = 0;
            kspOuter[0].Atmosphere = false;
            kspOuter[0].SurfaceToLowOrbit = 0;
            kspOuter[0].LowOrbitToMoonIntercept = 0;
            kspOuter[0].MoonInterceptToElipticalOrbit = 0;
            kspOuter[0].MoonInterceptToElipticalOrbitMPC = 0;
            kspOuter[0].LowOrbitToElipticalOrbit = 0;
            kspOuter[0].ElipticalOrbitToPlanetIntercet = 0;
            kspOuter[0].PlanetInterceptToStarElipticalOrbit = 0;
            kspOuter[0].MaxPlaneChange = 0;
            kspOuter[0].EscapeVelocity = "0 m/s";
            kspOuter[0].SphereOfInfluence = "";
            kspOuter[0].oxigen = false;
            kspOuter[0].Height = 00;
            kspOuter[0].Pressure = 0;
            kspOuter[0].Surface = 0;
            kspOuter[0].LowerAtmosphere = 0;
            kspOuter[0].UpperAtmosphere = 0;
            kspOuter[0].NearSpace = 0;
            kspOuter[0].OuterSpace = 0;
            kspOuter[0].OrbitalPeriod = 0;
            kspOuter[0].Orbits = Orbits.None;
            kspOuter[0].ParentIndex = 0;
            kspOuter[0].Biomes = 0;
            kspOuter[0].ImgUri = @"pack://application:,,/Images/Planets/None.png";

            kspOuter[1] = new Planet.All();
            kspOuter[1].Index = 1;
            kspOuter[1].Name = "Moho";
            kspOuter[1].Type = Types.Planet;
            kspOuter[1].System = Systems.Moho;
            kspOuter[1].LowOrbit = "50 km";
            kspOuter[1].SurfaceGravity = 2.7;
            kspOuter[1].Atmosphere = false;
            kspOuter[1].SurfaceToLowOrbit = 870;
            kspOuter[1].LowOrbitToMoonIntercept = 0;
            kspOuter[1].MoonInterceptToElipticalOrbit = 0;
            kspOuter[1].MoonInterceptToElipticalOrbitMPC = 0;
            kspOuter[1].LowOrbitToElipticalOrbit = 0;
            kspOuter[1].ElipticalOrbitToPlanetIntercet = 2410;
            kspOuter[1].PlanetInterceptToStarElipticalOrbit = 760;
            kspOuter[1].MaxPlaneChange = 2520;
            kspOuter[1].EscapeVelocity = "1,161.41 m/s";
            kspOuter[1].SphereOfInfluence = "9,646,663 m";
            kspOuter[1].oxigen = false;
            kspOuter[1].Height = 0;
            kspOuter[1].Pressure = 0;
            kspOuter[1].Surface = 6;
            kspOuter[1].LowerAtmosphere = 0;
            kspOuter[1].UpperAtmosphere = 0;
            kspOuter[1].NearSpace = 5;
            kspOuter[1].OuterSpace = 4.5;
            kspOuter[1].OrbitalPeriod = 2215754.2;
            kspOuter[1].Orbits = Orbits.Kerbol;
            kspOuter[1].ParentIndex = 1;
            kspOuter[1].Biomes = 12;
            kspOuter[1].ImgUri = @"pack://application:,,/Images/Planets/Moho.png";

            kspOuter[2] = new Planet.All();
            kspOuter[2].Index = 2;
            kspOuter[2].Name = "Eve";
            kspOuter[2].Type = Types.Planet;
            kspOuter[2].System = Systems.Eve;
            kspOuter[2].LowOrbit = "100 km";
            kspOuter[2].SurfaceGravity = 16.4;
            kspOuter[2].Atmosphere = true;
            kspOuter[2].SurfaceToLowOrbit = 8000;
            kspOuter[2].LowOrbitToMoonIntercept = 0;
            kspOuter[2].MoonInterceptToElipticalOrbit = 1330;
            kspOuter[2].MoonInterceptToElipticalOrbitMPC = 0;
            kspOuter[2].LowOrbitToElipticalOrbit = 0;
            kspOuter[2].ElipticalOrbitToPlanetIntercet = 80;
            kspOuter[2].PlanetInterceptToStarElipticalOrbit = 90;
            kspOuter[2].MaxPlaneChange = 430;
            kspOuter[2].EscapeVelocity = "4,831.96 m/s";
            kspOuter[2].SphereOfInfluence = "85,109,365 m";
            kspOuter[2].oxigen = false;
            kspOuter[2].Height = 90000;
            kspOuter[2].Pressure = 5;
            kspOuter[2].Surface = 5;
            kspOuter[2].LowerAtmosphere = 6;
            kspOuter[2].UpperAtmosphere = 5.5;
            kspOuter[2].NearSpace = 4;
            kspOuter[2].OuterSpace = 3.5;
            kspOuter[2].OrbitalPeriod = 5657995.1;
            kspOuter[2].Orbits = Orbits.Kerbol;
            kspOuter[2].ParentIndex = 2;
            kspOuter[2].Biomes = 7;
            kspOuter[2].ImgUri = @"pack://application:,,/Images/Planets/Eve.png";

            kspOuter[3] = new Planet.All();
            kspOuter[3].Index = 3;
            kspOuter[3].Name = "Gilly";
            kspOuter[3].Type = Types.Moon;
            kspOuter[3].System = Systems.Eve;
            kspOuter[3].LowOrbit = "10 km";
            kspOuter[3].SurfaceGravity = 0.049;
            kspOuter[3].Atmosphere = false;
            kspOuter[3].SurfaceToLowOrbit = 30;
            kspOuter[3].LowOrbitToMoonIntercept = 410;
            kspOuter[3].MoonInterceptToElipticalOrbit = 60;
            kspOuter[3].MoonInterceptToElipticalOrbitMPC = 0;
            kspOuter[3].LowOrbitToElipticalOrbit = 0;
            kspOuter[3].ElipticalOrbitToPlanetIntercet = 80;
            kspOuter[3].PlanetInterceptToStarElipticalOrbit = 90;
            kspOuter[3].MaxPlaneChange = 430;
            kspOuter[3].EscapeVelocity = "35.71 m/s";
            kspOuter[3].SphereOfInfluence = "126,123.27 m";
            kspOuter[3].oxigen = false;
            kspOuter[3].Height = 0;
            kspOuter[3].Pressure = 0;
            kspOuter[3].Surface = 4;
            kspOuter[3].LowerAtmosphere = 0;
            kspOuter[3].UpperAtmosphere = 0;
            kspOuter[3].NearSpace = 3;
            kspOuter[3].OuterSpace = 2.5;
            kspOuter[3].OrbitalPeriod = 388587.4;
            kspOuter[3].Orbits = Orbits.Eve;
            kspOuter[3].ParentIndex = 2;
            kspOuter[3].Biomes = 3;
            kspOuter[3].ImgUri = @"pack://application:,,/Images/Planets/Gilly.png";

            kspOuter[4] = new Planet.All();
            kspOuter[4].Index = 4;
            kspOuter[4].Name = "Kerbin";
            kspOuter[4].Type = Types.Planet;
            kspOuter[4].System = Systems.Kerbin;
            kspOuter[4].LowOrbit = "80 km";
            kspOuter[4].SurfaceGravity = 9.81;
            kspOuter[4].Atmosphere = true;
            kspOuter[4].SurfaceToLowOrbit = 3400;
            kspOuter[4].LowOrbitToMoonIntercept = 0;
            kspOuter[4].MoonInterceptToElipticalOrbit = 0;
            kspOuter[4].MoonInterceptToElipticalOrbitMPC = 0;
            kspOuter[4].LowOrbitToElipticalOrbit = 0;
            kspOuter[4].ElipticalOrbitToPlanetIntercet = 0;
            kspOuter[4].PlanetInterceptToStarElipticalOrbit = 950;
            kspOuter[4].MaxPlaneChange = 0;
            kspOuter[4].EscapeVelocity = "3,431.03 m/s";
            kspOuter[4].SphereOfInfluence = "84,159,286 m";
            kspOuter[4].oxigen = true;
            kspOuter[4].Height = 70000;
            kspOuter[4].Pressure = 1;
            kspOuter[4].Surface = 0.3;
            kspOuter[4].LowerAtmosphere = 0.7;
            kspOuter[4].UpperAtmosphere = 0.9;
            kspOuter[4].NearSpace = 1;
            kspOuter[4].OuterSpace = 1.5;
            kspOuter[4].OrbitalPeriod = 9203544.6;
            kspOuter[4].Orbits = Orbits.Kerbol;
            kspOuter[4].ParentIndex = 4;
            kspOuter[4].Biomes = 42;
            kspOuter[4].ImgUri = @"pack://application:,,/Images/Planets/Kerbin.png";

            kspOuter[5] = new Planet.All();
            kspOuter[5].Index = 5;
            kspOuter[5].Name = "Mun";
            kspOuter[5].Type = Types.Moon;
            kspOuter[5].System = Systems.Kerbin;
            kspOuter[5].LowOrbit = "14 km";
            kspOuter[5].SurfaceGravity = 1.63;
            kspOuter[5].Atmosphere = false;
            kspOuter[5].SurfaceToLowOrbit = 580;
            kspOuter[5].LowOrbitToMoonIntercept = 310;
            kspOuter[5].MoonInterceptToElipticalOrbit = 860;
            kspOuter[5].MoonInterceptToElipticalOrbitMPC = 0;
            kspOuter[5].LowOrbitToElipticalOrbit = 0;
            kspOuter[5].ElipticalOrbitToPlanetIntercet = 0;
            kspOuter[5].PlanetInterceptToStarElipticalOrbit = 950;
            kspOuter[5].MaxPlaneChange = 0;
            kspOuter[5].EscapeVelocity = "807.08 m/s";
            kspOuter[5].SphereOfInfluence = "2,429,559.1 m";
            kspOuter[5].oxigen = false;
            kspOuter[5].Height = 0;
            kspOuter[5].Pressure = 0;
            kspOuter[5].Surface = 3.5;
            kspOuter[5].LowerAtmosphere = 0;
            kspOuter[5].UpperAtmosphere = 0;
            kspOuter[5].NearSpace = 2.5;
            kspOuter[5].OuterSpace = 2;
            kspOuter[5].OrbitalPeriod = 138984.4;
            kspOuter[5].Orbits = Orbits.Kerbin;
            kspOuter[5].ParentIndex = 4;
            kspOuter[5].Biomes = 15;
            kspOuter[5].ImgUri = @"pack://application:,,/Images/Planets/Mun.png";

            kspOuter[6] = new Planet.All();
            kspOuter[6].Index = 6;
            kspOuter[6].Name = "Minmus";
            kspOuter[6].Type = Types.Moon;
            kspOuter[6].System = Systems.Kerbin;
            kspOuter[6].LowOrbit = "10 km";
            kspOuter[6].SurfaceGravity = 0.491;
            kspOuter[6].Atmosphere = false;
            kspOuter[6].SurfaceToLowOrbit = 180;
            kspOuter[6].LowOrbitToMoonIntercept = 160;
            kspOuter[6].MoonInterceptToElipticalOrbit = 930;
            kspOuter[6].MoonInterceptToElipticalOrbitMPC = 340;
            kspOuter[6].LowOrbitToElipticalOrbit = 0;
            kspOuter[6].ElipticalOrbitToPlanetIntercet = 0;
            kspOuter[6].PlanetInterceptToStarElipticalOrbit = 950;
            kspOuter[6].MaxPlaneChange = 0;
            kspOuter[6].EscapeVelocity = "242.61 m/s";
            kspOuter[6].SphereOfInfluence = "2,247,428.4 m";
            kspOuter[6].oxigen = false;
            kspOuter[6].Height = 0;
            kspOuter[6].Pressure = 0;
            kspOuter[6].Surface = 3;
            kspOuter[6].LowerAtmosphere = 0;
            kspOuter[6].UpperAtmosphere = 0;
            kspOuter[6].NearSpace = 2;
            kspOuter[6].OuterSpace = 1.5;
            kspOuter[6].OrbitalPeriod = 1077310.5;
            kspOuter[6].Orbits = Orbits.Kerbin;
            kspOuter[6].ParentIndex = 4;
            kspOuter[6].Biomes = 9;
            kspOuter[6].ImgUri = @"pack://application:,,/Images/Planets/Minmus.png";

            kspOuter[7] = new Planet.All();
            kspOuter[7].Index = 7;
            kspOuter[7].Name = "Duna";
            kspOuter[7].Type = Types.Planet;
            kspOuter[7].System = Systems.Duna;
            kspOuter[7].LowOrbit = "60 km";
            kspOuter[7].SurfaceGravity = 2.94;
            kspOuter[7].Atmosphere = true;
            kspOuter[7].SurfaceToLowOrbit = 1450;
            kspOuter[7].LowOrbitToMoonIntercept = 0;
            kspOuter[7].MoonInterceptToElipticalOrbit = 360;
            kspOuter[7].MoonInterceptToElipticalOrbitMPC = 0;
            kspOuter[7].LowOrbitToElipticalOrbit = 0;
            kspOuter[7].ElipticalOrbitToPlanetIntercet = 250;
            kspOuter[7].PlanetInterceptToStarElipticalOrbit = 130;
            kspOuter[7].MaxPlaneChange = 10;
            kspOuter[7].EscapeVelocity = "1,372.41 m/s";
            kspOuter[7].SphereOfInfluence = "47,921,949 m";
            kspOuter[7].oxigen = false;
            kspOuter[7].Height = 50000;
            kspOuter[7].Pressure = 0.0666;
            kspOuter[7].Surface = 4;
            kspOuter[7].LowerAtmosphere = 5;
            kspOuter[7].UpperAtmosphere = 4.5;
            kspOuter[7].NearSpace = 3;
            kspOuter[7].OuterSpace = 2.5;
            kspOuter[7].OrbitalPeriod = 17315400.1;
            kspOuter[7].Orbits = Orbits.Kerbol;
            kspOuter[7].ParentIndex = 7;
            kspOuter[7].Biomes = 5;
            kspOuter[7].ImgUri = @"pack://application:,,/Images/Planets/Duna.png";

            kspOuter[8] = new Planet.All();
            kspOuter[8].Index = 8;
            kspOuter[8].Name = "Ike";
            kspOuter[8].Type = Types.Moon;
            kspOuter[8].System = Systems.Duna;
            kspOuter[8].LowOrbit = "10 km";
            kspOuter[8].SurfaceGravity = 1.1;
            kspOuter[8].Atmosphere = false;
            kspOuter[8].SurfaceToLowOrbit = 390;
            kspOuter[8].LowOrbitToMoonIntercept = 180;
            kspOuter[8].MoonInterceptToElipticalOrbit = 30;
            kspOuter[8].MoonInterceptToElipticalOrbitMPC = 0;
            kspOuter[8].LowOrbitToElipticalOrbit = 0;
            kspOuter[8].ElipticalOrbitToPlanetIntercet = 250;
            kspOuter[8].PlanetInterceptToStarElipticalOrbit = 130;
            kspOuter[8].MaxPlaneChange = 10;
            kspOuter[8].EscapeVelocity = "534.48 m/s";
            kspOuter[8].SphereOfInfluence = "1,049,598.9 m";
            kspOuter[8].oxigen = false;
            kspOuter[8].Height = 0;
            kspOuter[8].Pressure = 0;
            kspOuter[8].Surface = 5;
            kspOuter[8].LowerAtmosphere = 0;
            kspOuter[8].UpperAtmosphere = 0;
            kspOuter[8].NearSpace = 4;
            kspOuter[8].OuterSpace = 3.5;
            kspOuter[8].OrbitalPeriod = 65517.9;
            kspOuter[8].Orbits = Orbits.Duna;
            kspOuter[8].ParentIndex = 7;
            kspOuter[8].Biomes = 8;
            kspOuter[8].ImgUri = @"pack://application:,,/Images/Planets/Ike.png";

            kspOuter[9] = new Planet.All();
            kspOuter[9].Index = 9;
            kspOuter[9].Name = "Dres";
            kspOuter[9].Type = Types.Planet;
            kspOuter[9].System = Systems.Dres;
            kspOuter[9].LowOrbit = "12 km";
            kspOuter[9].SurfaceGravity = 1.13;
            kspOuter[9].Atmosphere = false;
            kspOuter[9].SurfaceToLowOrbit = 430;
            kspOuter[9].LowOrbitToMoonIntercept = 0;
            kspOuter[9].MoonInterceptToElipticalOrbit = 0;
            kspOuter[9].MoonInterceptToElipticalOrbitMPC = 0;
            kspOuter[9].LowOrbitToElipticalOrbit = 0;
            kspOuter[9].ElipticalOrbitToPlanetIntercet = 1290;
            kspOuter[9].PlanetInterceptToStarElipticalOrbit = 610;
            kspOuter[9].MaxPlaneChange = 1010;
            kspOuter[9].EscapeVelocity = "558 m/s";
            kspOuter[9].SphereOfInfluence = "32,832,840 m";
            kspOuter[9].oxigen = false;
            kspOuter[9].Height = 0;
            kspOuter[9].Pressure = 0;
            kspOuter[9].Surface = 6;
            kspOuter[9].LowerAtmosphere = 0;
            kspOuter[9].UpperAtmosphere = 0;
            kspOuter[9].NearSpace = 5;
            kspOuter[9].OuterSpace = 4.5;
            kspOuter[9].OrbitalPeriod = 47893063.1;
            kspOuter[9].Orbits = Orbits.Kerbol;
            kspOuter[9].ParentIndex = 9;
            kspOuter[9].Biomes = 8;
            kspOuter[9].ImgUri = @"pack://application:,,/Images/Planets/Dres.png";

            kspOuter[10] = new Planet.All();
            kspOuter[10].Index = 10;
            kspOuter[10].Name = "Jool";
            kspOuter[10].Type = Types.Gasgiant;
            kspOuter[10].System = Systems.Jool;
            kspOuter[10].LowOrbit = "210 km";
            kspOuter[10].SurfaceGravity = 7.85;
            kspOuter[10].Atmosphere = true;
            kspOuter[10].SurfaceToLowOrbit = 14000;
            kspOuter[10].LowOrbitToMoonIntercept = 0;
            kspOuter[10].MoonInterceptToElipticalOrbit = 2810;
            kspOuter[10].MoonInterceptToElipticalOrbitMPC = 0;
            kspOuter[10].LowOrbitToElipticalOrbit = 0;
            kspOuter[10].ElipticalOrbitToPlanetIntercet = 160;
            kspOuter[10].PlanetInterceptToStarElipticalOrbit = 980;
            kspOuter[10].MaxPlaneChange = 270;
            kspOuter[10].EscapeVelocity = "9.704.43 m/s";
            kspOuter[10].SphereOfInfluence = "2.4 * 10^9 m";
            kspOuter[10].oxigen = false;
            kspOuter[10].Height = 200000;
            kspOuter[10].Pressure = 15;
            kspOuter[10].Surface = 0;
            kspOuter[10].LowerAtmosphere = 8;
            kspOuter[10].UpperAtmosphere = 7.5;
            kspOuter[10].NearSpace = 7;
            kspOuter[10].OuterSpace = 6.5;
            kspOuter[10].OrbitalPeriod = 104661432.1;
            kspOuter[10].Orbits = Orbits.Kerbol;
            kspOuter[10].ParentIndex = 10;
            kspOuter[10].Biomes = 0;
            kspOuter[10].ImgUri = @"pack://application:,,/Images/Planets/Jool.png";

            kspOuter[11] = new Planet.All();
            kspOuter[11].Index = 11;
            kspOuter[11].Name = "Laythe";
            kspOuter[11].Type = Types.Moon;
            kspOuter[11].System = Systems.Jool;
            kspOuter[11].LowOrbit = "60 km";
            kspOuter[11].SurfaceGravity = 7.85;
            kspOuter[11].Atmosphere = true;
            kspOuter[11].SurfaceToLowOrbit = 2900;
            kspOuter[11].LowOrbitToMoonIntercept = 1070;
            kspOuter[11].MoonInterceptToElipticalOrbit = 930;
            kspOuter[11].MoonInterceptToElipticalOrbitMPC = 0;
            kspOuter[11].LowOrbitToElipticalOrbit = 0;
            kspOuter[11].ElipticalOrbitToPlanetIntercet = 160;
            kspOuter[11].PlanetInterceptToStarElipticalOrbit = 980;
            kspOuter[11].MaxPlaneChange = 270;
            kspOuter[11].EscapeVelocity = "2,801.43 m/s";
            kspOuter[11].SphereOfInfluence = "3,723,645.8 m";
            kspOuter[11].oxigen = true;
            kspOuter[11].Height = 50000;
            kspOuter[11].Pressure = 0.6;
            kspOuter[11].Surface = 9;
            kspOuter[11].LowerAtmosphere = 10;
            kspOuter[11].UpperAtmosphere = 9.5;
            kspOuter[11].NearSpace = 8;
            kspOuter[11].OuterSpace = 7.5;
            kspOuter[11].OrbitalPeriod = 52980.9;
            kspOuter[11].Orbits = Orbits.Jool;
            kspOuter[11].ParentIndex = 10;
            kspOuter[11].Biomes = 5;
            kspOuter[11].ImgUri = @"pack://application:,,/Images/Planets/Laythe.png";

            kspOuter[12] = new Planet.All();
            kspOuter[12].Index = 12;
            kspOuter[12].Name = "Vall";
            kspOuter[12].Type = Types.Moon;
            kspOuter[12].System = Systems.Jool;
            kspOuter[12].LowOrbit = "15 km";
            kspOuter[12].SurfaceGravity = 2.31;
            kspOuter[12].Atmosphere = false;
            kspOuter[12].SurfaceToLowOrbit = 860;
            kspOuter[12].LowOrbitToMoonIntercept = 910;
            kspOuter[12].MoonInterceptToElipticalOrbit = 620;
            kspOuter[12].MoonInterceptToElipticalOrbitMPC = 0;
            kspOuter[12].LowOrbitToElipticalOrbit = 0;
            kspOuter[12].ElipticalOrbitToPlanetIntercet = 160;
            kspOuter[12].PlanetInterceptToStarElipticalOrbit = 980;
            kspOuter[12].MaxPlaneChange = 270;
            kspOuter[12].EscapeVelocity = "1,176.10 m/s";
            kspOuter[12].SphereOfInfluence = "2,406,401.4 m";
            kspOuter[12].oxigen = false;
            kspOuter[12].Height = 0;
            kspOuter[12].Pressure = 0;
            kspOuter[12].Surface = 9;
            kspOuter[12].LowerAtmosphere = 0;
            kspOuter[12].UpperAtmosphere = 0;
            kspOuter[12].NearSpace = 8;
            kspOuter[12].OuterSpace = 7.5;
            kspOuter[12].OrbitalPeriod = 105962.1;
            kspOuter[12].Orbits = Orbits.Jool;
            kspOuter[12].ParentIndex = 10;
            kspOuter[12].Biomes = 4;
            kspOuter[12].ImgUri = @"pack://application:,,/Images/Planets/Vall.png";

            kspOuter[13] = new Planet.All();
            kspOuter[13].Index = 13;
            kspOuter[13].Name = "Tylo";
            kspOuter[13].Type = Types.Moon;
            kspOuter[13].System = Systems.Jool;
            kspOuter[13].LowOrbit = "10 km";
            kspOuter[13].SurfaceGravity = 7.85;
            kspOuter[13].Atmosphere = false;
            kspOuter[13].SurfaceToLowOrbit = 2270;
            kspOuter[13].LowOrbitToMoonIntercept = 1100;
            kspOuter[13].MoonInterceptToElipticalOrbit = 400;
            kspOuter[13].MoonInterceptToElipticalOrbitMPC = 0;
            kspOuter[13].LowOrbitToElipticalOrbit = 0;
            kspOuter[13].ElipticalOrbitToPlanetIntercet = 160;
            kspOuter[13].PlanetInterceptToStarElipticalOrbit = 980;
            kspOuter[13].MaxPlaneChange = 270;
            kspOuter[13].EscapeVelocity = "3,068.81 m/s";
            kspOuter[13].SphereOfInfluence = "10,856,518 m";
            kspOuter[13].oxigen = false;
            kspOuter[13].Height = 0;
            kspOuter[13].Pressure = 0;
            kspOuter[13].Surface = 9.5;
            kspOuter[13].LowerAtmosphere = 0;
            kspOuter[13].UpperAtmosphere = 0;
            kspOuter[13].NearSpace = 8.5;
            kspOuter[13].OuterSpace = 8;
            kspOuter[13].OrbitalPeriod = 211926.4;
            kspOuter[13].Orbits = Orbits.Jool;
            kspOuter[13].ParentIndex = 10;
            kspOuter[13].Biomes = 8;
            kspOuter[13].ImgUri = @"pack://application:,,/Images/Planets/Tylo.png";

            kspOuter[14] = new Planet.All();
            kspOuter[14].Index = 14;
            kspOuter[14].Name = "Bop";
            kspOuter[14].Type = Types.Moon;
            kspOuter[14].System = Systems.Jool;
            kspOuter[14].LowOrbit = "10 km";
            kspOuter[14].SurfaceGravity = 0.589;
            kspOuter[14].Atmosphere = false;
            kspOuter[14].SurfaceToLowOrbit = 220;
            kspOuter[14].LowOrbitToMoonIntercept = 900;
            kspOuter[14].MoonInterceptToElipticalOrbit = 220;
            kspOuter[14].MoonInterceptToElipticalOrbitMPC = 2440;
            kspOuter[14].LowOrbitToElipticalOrbit = 0;
            kspOuter[14].ElipticalOrbitToPlanetIntercet = 160;
            kspOuter[14].PlanetInterceptToStarElipticalOrbit = 980;
            kspOuter[14].MaxPlaneChange = 270;
            kspOuter[14].EscapeVelocity = "276.62 m/s";
            kspOuter[14].SphereOfInfluence = "1,221,060.9 m";
            kspOuter[14].oxigen = false;
            kspOuter[14].Height = 0;
            kspOuter[14].Pressure = 0;
            kspOuter[14].Surface = 9;
            kspOuter[14].LowerAtmosphere = 0;
            kspOuter[14].UpperAtmosphere = 0;
            kspOuter[14].NearSpace = 8;
            kspOuter[14].OuterSpace = 7.5;
            kspOuter[14].OrbitalPeriod = 544507.4;
            kspOuter[14].Orbits = Orbits.Jool;
            kspOuter[14].ParentIndex = 10;
            kspOuter[14].Biomes = 5;
            kspOuter[14].ImgUri = @"pack://application:,,/Images/Planets/Bop.png";

            kspOuter[15] = new Planet.All();
            kspOuter[15].Index = 15;
            kspOuter[15].Name = "Pol";
            kspOuter[15].Type = Types.Moon;
            kspOuter[15].System = Systems.Jool;
            kspOuter[15].LowOrbit = "10 km";
            kspOuter[15].SurfaceGravity = 0.373;
            kspOuter[15].Atmosphere = false;
            kspOuter[15].SurfaceToLowOrbit = 130;
            kspOuter[15].LowOrbitToMoonIntercept = 820;
            kspOuter[15].MoonInterceptToElipticalOrbit = 160;
            kspOuter[15].MoonInterceptToElipticalOrbitMPC = 700;
            kspOuter[15].LowOrbitToElipticalOrbit = 0;
            kspOuter[15].ElipticalOrbitToPlanetIntercet = 160;
            kspOuter[15].PlanetInterceptToStarElipticalOrbit = 980;
            kspOuter[15].MaxPlaneChange = 270;
            kspOuter[15].EscapeVelocity = "181.12 m/s";
            kspOuter[15].SphereOfInfluence = "1,042,135.9 m";
            kspOuter[15].oxigen = false;
            kspOuter[15].Height = 0;
            kspOuter[15].Pressure = 0;
            kspOuter[15].Surface = 9;
            kspOuter[15].LowerAtmosphere = 0;
            kspOuter[15].UpperAtmosphere = 0;
            kspOuter[15].NearSpace = 8;
            kspOuter[15].OuterSpace = 7.5;
            kspOuter[15].OrbitalPeriod = 901902.6;
            kspOuter[15].Orbits = Orbits.Jool;
            kspOuter[15].ParentIndex = 10;
            kspOuter[15].Biomes = 4;
            kspOuter[15].ImgUri = @"pack://application:,,/Images/Planets/Pol.png";

            kspOuter[16] = new Planet.All();
            kspOuter[16].Index = 16;
            kspOuter[16].Name = "Kerbol";
            kspOuter[16].Type = Types.Star;
            kspOuter[16].System = Systems.Kerbol;
            kspOuter[16].LowOrbit = "610 km";
            kspOuter[16].SurfaceGravity = 17.1;
            kspOuter[16].Atmosphere = true;
            kspOuter[16].SurfaceToLowOrbit = 67000;
            kspOuter[16].LowOrbitToMoonIntercept = 0;
            kspOuter[16].MoonInterceptToElipticalOrbit = 0;
            kspOuter[16].MoonInterceptToElipticalOrbitMPC = 0;
            kspOuter[16].LowOrbitToElipticalOrbit = 0;
            kspOuter[16].ElipticalOrbitToPlanetIntercet = 13700;
            kspOuter[16].PlanetInterceptToStarElipticalOrbit = 6000;
            kspOuter[16].MaxPlaneChange = 0;
            kspOuter[16].EscapeVelocity = "94,672.01 m/s";
            kspOuter[16].SphereOfInfluence = "∞";
            kspOuter[16].oxigen = true;
            kspOuter[16].Height = 600000;
            kspOuter[16].Pressure = 0.158;
            kspOuter[16].Surface = 0;
            kspOuter[16].LowerAtmosphere = 1;
            kspOuter[16].UpperAtmosphere = 1;
            kspOuter[16].NearSpace = 7;
            kspOuter[16].OuterSpace = 2;
            kspOuter[16].OrbitalPeriod = 0;
            kspOuter[16].Orbits = Orbits.None;
            kspOuter[16].ParentIndex = 16;
            kspOuter[16].Biomes = 0;
            kspOuter[16].ImgUri = @"pack://application:,,/Images/Planets/Kerbol.png";


            kspOuter[17] = new Planet.All();
            kspOuter[17].Index = 17;
            kspOuter[17].Name = "Sarnus";
            kspOuter[17].Type = Types.Gasgiant;
            kspOuter[17].System = Systems.Sarnus;
            kspOuter[17].LowOrbit = "314 km";
            kspOuter[17].SurfaceGravity = 2.92;
            kspOuter[17].Atmosphere = true;
            kspOuter[17].SurfaceToLowOrbit = 20000;
            kspOuter[17].LowOrbitToMoonIntercept = 0;
            kspOuter[17].MoonInterceptToElipticalOrbit = 0;
            kspOuter[17].MoonInterceptToElipticalOrbitMPC = 0;
            kspOuter[17].LowOrbitToElipticalOrbit = 1580;
            kspOuter[17].ElipticalOrbitToPlanetIntercet = 1660;
            kspOuter[17].PlanetInterceptToStarElipticalOrbit = 1390;
            kspOuter[17].MaxPlaneChange = 350;
            kspOuter[17].EscapeVelocity = "5,566.67 m/s";
            kspOuter[17].SphereOfInfluence = "2.7 * 10^10 m";
            kspOuter[17].oxigen = false;
            kspOuter[17].Height = 304000;
            kspOuter[17].Pressure = 14;
            kspOuter[17].Surface = 0;
            kspOuter[17].LowerAtmosphere = 10;
            kspOuter[17].UpperAtmosphere = 9.5;
            kspOuter[17].NearSpace = 9;
            kspOuter[17].OuterSpace = 8.5;
            kspOuter[17].OrbitalPeriod = 258921266;
            kspOuter[17].Orbits = Orbits.Kerbol;
            kspOuter[17].ParentIndex = 17;
            kspOuter[17].Biomes = 0;
            kspOuter[17].ImgUri = @"pack://application:,,/Images/Planets/Sarnus.png";

            kspOuter[18] = new Planet.All();
            kspOuter[18].Index = 18;
            kspOuter[18].Name = "Tekto";
            kspOuter[18].Type = Types.Moon;
            kspOuter[18].System = Systems.Sarnus;
            kspOuter[18].LowOrbit = "120 km";
            kspOuter[18].SurfaceGravity = 2.46;
            kspOuter[18].Atmosphere = true;
            kspOuter[18].SurfaceToLowOrbit = 2600;
            kspOuter[18].LowOrbitToMoonIntercept = 350;
            kspOuter[18].MoonInterceptToElipticalOrbit = 1540;
            kspOuter[18].MoonInterceptToElipticalOrbitMPC = 630;
            kspOuter[18].LowOrbitToElipticalOrbit = 1580;
            kspOuter[18].ElipticalOrbitToPlanetIntercet = 1660;
            kspOuter[18].PlanetInterceptToStarElipticalOrbit = 1390;
            kspOuter[18].MaxPlaneChange = 350;
            kspOuter[18].EscapeVelocity = "1,172.62 m/s";
            kspOuter[18].SphereOfInfluence = "8,637,005.2 m";
            kspOuter[18].oxigen = false;
            kspOuter[18].Height = 100500;
            kspOuter[18].Pressure = 1.23;
            kspOuter[18].Surface = 11;
            kspOuter[18].LowerAtmosphere = 11;
            kspOuter[18].UpperAtmosphere = 10.5;
            kspOuter[18].NearSpace = 10;
            kspOuter[18].OuterSpace = 9.5;
            kspOuter[18].OrbitalPeriod = 666041;
            kspOuter[18].Orbits = Orbits.Sarnus;
            kspOuter[18].ParentIndex = 17;
            kspOuter[18].Biomes = 9;
            kspOuter[18].ImgUri = @"pack://application:,,/Images/Planets/Tekto.png";

            kspOuter[19] = new Planet.All();
            kspOuter[19].Index = 19;
            kspOuter[19].Name = "Slate";
            kspOuter[19].Type = Types.Moon;
            kspOuter[19].System = Systems.Sarnus;
            kspOuter[19].LowOrbit = "25 km";
            kspOuter[19].SurfaceGravity = 8.34;
            kspOuter[19].Atmosphere = false;
            kspOuter[19].SurfaceToLowOrbit = 2150;
            kspOuter[19].LowOrbitToMoonIntercept = 890;
            kspOuter[19].MoonInterceptToElipticalOrbit = 1460;
            kspOuter[19].MoonInterceptToElipticalOrbitMPC = 155;
            kspOuter[19].LowOrbitToElipticalOrbit = 1580;
            kspOuter[19].ElipticalOrbitToPlanetIntercet = 1660;
            kspOuter[19].PlanetInterceptToStarElipticalOrbit = 1390;
            kspOuter[19].MaxPlaneChange = 350;
            kspOuter[19].EscapeVelocity = "3,000.93 m/s";
            kspOuter[19].SphereOfInfluence = "10,421,088 m";
            kspOuter[19].oxigen = false;
            kspOuter[19].Height = 0;
            kspOuter[19].Pressure = 0;
            kspOuter[19].Surface = 11.5;
            kspOuter[19].LowerAtmosphere = 0;
            kspOuter[19].UpperAtmosphere = 0;
            kspOuter[19].NearSpace = 10.5;
            kspOuter[19].OuterSpace = 10;
            kspOuter[19].OrbitalPeriod = 192738;
            kspOuter[19].Orbits = Orbits.Sarnus;
            kspOuter[19].ParentIndex = 17;
            kspOuter[19].Biomes = 10;
            kspOuter[19].ImgUri = @"pack://application:,,/Images/Planets/Slate.png";

            kspOuter[20] = new Planet.All();
            kspOuter[20].Index = 20;
            kspOuter[20].Name = "Eeloo";
            kspOuter[20].Type = Types.Moon;
            kspOuter[20].System = Systems.Sarnus;
            kspOuter[20].LowOrbit = "10 km";
            kspOuter[20].SurfaceGravity = 1.69;
            kspOuter[20].Atmosphere = false;
            kspOuter[20].SurfaceToLowOrbit = 540;
            kspOuter[20].LowOrbitToMoonIntercept = 490;
            kspOuter[20].MoonInterceptToElipticalOrbit = 1120;
            kspOuter[20].MoonInterceptToElipticalOrbitMPC = 160;
            kspOuter[20].LowOrbitToElipticalOrbit = 1580;
            kspOuter[20].ElipticalOrbitToPlanetIntercet = 1660;
            kspOuter[20].PlanetInterceptToStarElipticalOrbit = 1390;
            kspOuter[20].MaxPlaneChange = 350;
            kspOuter[20].EscapeVelocity = "841.83 m/s";
            kspOuter[20].SphereOfInfluence = "1,158,907.8 m";
            kspOuter[20].oxigen = false;
            kspOuter[20].Height = 0;
            kspOuter[20].Pressure = 0;
            kspOuter[20].Surface = 11;
            kspOuter[20].LowerAtmosphere = 0;
            kspOuter[20].UpperAtmosphere = 0;
            kspOuter[20].NearSpace = 10;
            kspOuter[20].OuterSpace = 9.5;
            kspOuter[20].OrbitalPeriod = 57905;
            kspOuter[20].Orbits = Orbits.Sarnus;
            kspOuter[20].ParentIndex = 17;
            kspOuter[20].Biomes = 7;
            kspOuter[20].ImgUri = @"pack://application:,,/Images/Planets/Eeloo.png";

            kspOuter[21] = new Planet.All();
            kspOuter[21].Index = 21;
            kspOuter[21].Name = "Ovok";
            kspOuter[21].Type = Types.Moon;
            kspOuter[21].System = Systems.Sarnus;
            kspOuter[21].LowOrbit = "10 km";
            kspOuter[21].SurfaceGravity = 0.020;
            kspOuter[21].Atmosphere = false;
            kspOuter[21].SurfaceToLowOrbit = 70;
            kspOuter[21].LowOrbitToMoonIntercept = 400;
            kspOuter[21].MoonInterceptToElipticalOrbit = 650;
            kspOuter[21].MoonInterceptToElipticalOrbitMPC = 100;
            kspOuter[21].LowOrbitToElipticalOrbit = 1580;
            kspOuter[21].ElipticalOrbitToPlanetIntercet = 1660;
            kspOuter[21].PlanetInterceptToStarElipticalOrbit = 1390;
            kspOuter[21].MaxPlaneChange = 350;
            kspOuter[21].EscapeVelocity = "31.94 m/s";
            kspOuter[21].SphereOfInfluence = "23,364.32 m";
            kspOuter[21].oxigen = false;
            kspOuter[21].Height = 0;
            kspOuter[21].Pressure = 0;
            kspOuter[21].Surface = 11;
            kspOuter[21].LowerAtmosphere = 0;
            kspOuter[21].UpperAtmosphere = 0;
            kspOuter[21].NearSpace = 10;
            kspOuter[21].OuterSpace = 9.5;
            kspOuter[21].OrbitalPeriod = 29435;
            kspOuter[21].Orbits = Orbits.Sarnus;
            kspOuter[21].ParentIndex = 17;
            kspOuter[21].Biomes = 3;
            kspOuter[21].ImgUri = @"pack://application:,,/Images/Planets/Ovok.png";

            kspOuter[22] = new Planet.All();
            kspOuter[22].Index = 22;
            kspOuter[22].Name = "Hale";
            kspOuter[22].Type = Types.Moon;
            kspOuter[22].System = Systems.Sarnus;
            kspOuter[22].LowOrbit = "8 km";
            kspOuter[22].SurfaceGravity = 0.023;
            kspOuter[22].Atmosphere = false;
            kspOuter[22].SurfaceToLowOrbit = 40;
            kspOuter[22].LowOrbitToMoonIntercept = 450;
            kspOuter[22].MoonInterceptToElipticalOrbit = 540;
            kspOuter[22].MoonInterceptToElipticalOrbitMPC = 70;
            kspOuter[22].LowOrbitToElipticalOrbit = 1580;
            kspOuter[22].ElipticalOrbitToPlanetIntercet = 1660;
            kspOuter[22].PlanetInterceptToStarElipticalOrbit = 1390;
            kspOuter[22].MaxPlaneChange = 350;
            kspOuter[22].EscapeVelocity = "16.45 m/s";
            kspOuter[22].SphereOfInfluence = "6,588.8 m";
            kspOuter[22].oxigen = false;
            kspOuter[22].Height = 0;
            kspOuter[22].Pressure = 0;
            kspOuter[22].Surface = 11;
            kspOuter[22].LowerAtmosphere = 0;
            kspOuter[22].UpperAtmosphere = 0;
            kspOuter[22].NearSpace = 10;
            kspOuter[22].OuterSpace = 9.5;
            kspOuter[22].OrbitalPeriod = 23551;
            kspOuter[22].Orbits = Orbits.Sarnus;
            kspOuter[22].ParentIndex = 17;
            kspOuter[22].Biomes = 2;
            kspOuter[22].ImgUri = @"pack://application:,,/Images/Planets/Hale.png";

            kspOuter[23] = new Planet.All();
            kspOuter[23].Index = 23;
            kspOuter[23].Name = "Urlum";
            kspOuter[23].Type = Types.Gasgiant;
            kspOuter[23].System = Systems.Urlum;
            kspOuter[23].LowOrbit = "134 km";
            kspOuter[23].SurfaceGravity = 2.52;
            kspOuter[23].Atmosphere = true;
            kspOuter[23].SurfaceToLowOrbit = 9000;
            kspOuter[23].LowOrbitToMoonIntercept = 0;
            kspOuter[23].MoonInterceptToElipticalOrbit = 0;
            kspOuter[23].MoonInterceptToElipticalOrbitMPC = 0;
            kspOuter[23].LowOrbitToElipticalOrbit = 940;
            kspOuter[23].ElipticalOrbitToPlanetIntercet = 1490;
            kspOuter[23].PlanetInterceptToStarElipticalOrbit = 1600;
            kspOuter[23].MaxPlaneChange = 100;
            kspOuter[23].EscapeVelocity = "3,313.18 m/s";
            kspOuter[23].SphereOfInfluence = "2.5626107 *10 m";
            kspOuter[23].oxigen = false;
            kspOuter[23].Height = 125000;
            kspOuter[23].Pressure = 7;
            kspOuter[23].Surface = 0;
            kspOuter[23].LowerAtmosphere = 12;
            kspOuter[23].UpperAtmosphere = 11.5;
            kspOuter[23].NearSpace = 11;
            kspOuter[23].OuterSpace = 10.5;
            kspOuter[23].OrbitalPeriod = 744247753;
            kspOuter[23].Orbits = Orbits.Kerbol;
            kspOuter[23].ParentIndex = 23;
            kspOuter[23].Biomes = 0;
            kspOuter[23].ImgUri = @"pack://application:,,/Images/Planets/Urlum.png";

            kspOuter[24] = new Planet.All();
            kspOuter[24].Index = 24;
            kspOuter[24].Name = "Wal";
            kspOuter[24].Type = Types.Moon;
            kspOuter[24].System = Systems.Urlum;
            kspOuter[24].LowOrbit = "35 km";
            kspOuter[24].SurfaceGravity = 3.63;
            kspOuter[24].Atmosphere = false;
            kspOuter[24].SurfaceToLowOrbit = 1200;
            kspOuter[24].LowOrbitToMoonIntercept = 810;
            kspOuter[24].MoonInterceptToElipticalOrbit = 900;
            kspOuter[24].MoonInterceptToElipticalOrbitMPC = 75;
            kspOuter[24].LowOrbitToElipticalOrbit = 940;
            kspOuter[24].ElipticalOrbitToPlanetIntercet = 1490;
            kspOuter[24].PlanetInterceptToStarElipticalOrbit = 1600;
            kspOuter[24].MaxPlaneChange = 100;
            kspOuter[24].EscapeVelocity = "1,638.9 m/s";
            kspOuter[24].SphereOfInfluence = "18.933.505 m";
            kspOuter[24].oxigen = false;
            kspOuter[24].Height = 00;
            kspOuter[24].Pressure = 0;
            kspOuter[24].Surface = 13;
            kspOuter[24].LowerAtmosphere = 0;
            kspOuter[24].UpperAtmosphere = 0;
            kspOuter[24].NearSpace = 12;
            kspOuter[24].OuterSpace = 11.5;
            kspOuter[24].OrbitalPeriod = 1009238;
            kspOuter[24].Orbits = Orbits.Urlum;
            kspOuter[24].ParentIndex = 23;
            kspOuter[24].Biomes = 5;
            kspOuter[24].ImgUri = @"pack://application:,,/Images/Planets/Wal.png";

            kspOuter[25] = new Planet.All();
            kspOuter[25].Index = 25;
            kspOuter[25].Name = "Tal";
            kspOuter[25].Type = Types.Moon;
            kspOuter[25].System = Systems.Urlum;
            kspOuter[25].LowOrbit = "20 km";
            kspOuter[25].SurfaceGravity = 0.441;
            kspOuter[25].Atmosphere = false;
            kspOuter[25].SurfaceToLowOrbit = 100;
            kspOuter[25].LowOrbitToMoonIntercept = 150;
            kspOuter[25].MoonInterceptToElipticalOrbit = 380;
            kspOuter[25].MoonInterceptToElipticalOrbitMPC = 37;
            kspOuter[25].LowOrbitToElipticalOrbit = 940;
            kspOuter[25].ElipticalOrbitToPlanetIntercet = 1490;
            kspOuter[25].PlanetInterceptToStarElipticalOrbit = 1600;
            kspOuter[25].MaxPlaneChange = 100;
            kspOuter[25].EscapeVelocity = "139.37 m/s";
            kspOuter[25].SphereOfInfluence = "139,966.65.9 m";
            kspOuter[25].oxigen = false;
            kspOuter[25].Height = 0;
            kspOuter[25].Pressure = 0;
            kspOuter[25].Surface = 13;
            kspOuter[25].LowerAtmosphere = 0;
            kspOuter[25].UpperAtmosphere = 0;
            kspOuter[25].NearSpace = 12;
            kspOuter[25].OuterSpace = 11.5;
            kspOuter[25].OrbitalPeriod = 48866;
            kspOuter[25].Orbits = Orbits.Urlum;
            kspOuter[25].ParentIndex = 23;
            kspOuter[25].Biomes = 2;
            kspOuter[25].ImgUri = @"pack://application:,,/Images/Planets/Tal.png";

            kspOuter[26] = new Planet.All();
            kspOuter[26].Index = 26;
            kspOuter[26].Name = "Priax";
            kspOuter[26].Type = Types.Moon;
            kspOuter[26].System = Systems.Urlum;
            kspOuter[26].LowOrbit = "45 km";
            kspOuter[26].SurfaceGravity = 0.618;
            kspOuter[26].Atmosphere = false;
            kspOuter[26].SurfaceToLowOrbit = 260;
            kspOuter[26].LowOrbitToMoonIntercept = 460;
            kspOuter[26].MoonInterceptToElipticalOrbit = 620;
            kspOuter[26].MoonInterceptToElipticalOrbitMPC = 100;
            kspOuter[26].LowOrbitToElipticalOrbit = 940;
            kspOuter[26].ElipticalOrbitToPlanetIntercet = 1490;
            kspOuter[26].PlanetInterceptToStarElipticalOrbit = 1600;
            kspOuter[26].MaxPlaneChange = 100;
            kspOuter[26].EscapeVelocity = "302.44 m/s";
            kspOuter[26].SphereOfInfluence = "446,767.60 m";
            kspOuter[26].oxigen = false;
            kspOuter[26].Height = 0;
            kspOuter[26].Pressure = 0;
            kspOuter[26].Surface = 13;
            kspOuter[26].LowerAtmosphere = 0;
            kspOuter[26].UpperAtmosphere = 0;
            kspOuter[26].NearSpace = 12;
            kspOuter[26].OuterSpace = 11.5;
            kspOuter[26].OrbitalPeriod = 73005;
            kspOuter[26].Orbits = Orbits.Urlum;
            kspOuter[26].ParentIndex = 23;
            kspOuter[26].Biomes = 4;
            kspOuter[26].ImgUri = @"pack://application:,,/Images/Planets/Priax.png";

            kspOuter[27] = new Planet.All();
            kspOuter[27].Index = 27;
            kspOuter[27].Name = "Polta";
            kspOuter[27].Type = Types.Moon;
            kspOuter[27].System = Systems.Urlum;
            kspOuter[27].LowOrbit = "15 km";
            kspOuter[27].SurfaceGravity = 1.86;
            kspOuter[27].Atmosphere = false;
            kspOuter[27].SurfaceToLowOrbit = 640;
            kspOuter[27].LowOrbitToMoonIntercept = 440;
            kspOuter[27].MoonInterceptToElipticalOrbit = 620;
            kspOuter[27].MoonInterceptToElipticalOrbitMPC = 100;
            kspOuter[27].LowOrbitToElipticalOrbit = 940;
            kspOuter[27].ElipticalOrbitToPlanetIntercet = 1490;
            kspOuter[27].PlanetInterceptToStarElipticalOrbit = 1600;
            kspOuter[27].MaxPlaneChange = 100;
            kspOuter[27].EscapeVelocity = "905.60 m/s";
            kspOuter[27].SphereOfInfluence = "1,661,114.9 m";
            kspOuter[27].oxigen = false;
            kspOuter[27].Height = 0;
            kspOuter[27].Pressure = 0;
            kspOuter[27].Surface = 13;
            kspOuter[27].LowerAtmosphere = 0;
            kspOuter[27].UpperAtmosphere = 0;
            kspOuter[27].NearSpace = 12;
            kspOuter[27].OuterSpace = 11.5;
            kspOuter[27].OrbitalPeriod = 73005;
            kspOuter[27].Orbits = Orbits.Urlum;
            kspOuter[27].ParentIndex = 23;
            kspOuter[27].Biomes = 4;
            kspOuter[27].ImgUri = @"pack://application:,,/Images/Planets/Polta.png";

            kspOuter[28] = new Planet.All();
            kspOuter[28].Index = 28;
            kspOuter[28].Name = "Neidon";
            kspOuter[28].Type = Types.Gasgiant;
            kspOuter[28].System = Systems.Neidon;
            kspOuter[28].LowOrbit = "120 km";
            kspOuter[28].SurfaceGravity = 3.08;
            kspOuter[28].Atmosphere = true;
            kspOuter[28].SurfaceToLowOrbit = 9000;
            kspOuter[28].LowOrbitToMoonIntercept = 0;
            kspOuter[28].MoonInterceptToElipticalOrbit = 0;
            kspOuter[28].MoonInterceptToElipticalOrbitMPC = 0;
            kspOuter[28].LowOrbitToElipticalOrbit = 1040;
            kspOuter[28].ElipticalOrbitToPlanetIntercet = 1340;
            kspOuter[28].PlanetInterceptToStarElipticalOrbit = 1700;
            kspOuter[28].MaxPlaneChange = 110;
            kspOuter[28].EscapeVelocity = "3,635.2 m/s";
            kspOuter[28].SphereOfInfluence = "4.4163271 *10 m";
            kspOuter[28].oxigen = false;
            kspOuter[28].Height = 1110000;
            kspOuter[28].Pressure = 6;
            kspOuter[28].Surface = 0;
            kspOuter[28].LowerAtmosphere = 14;
            kspOuter[28].UpperAtmosphere = 13.5;
            kspOuter[28].NearSpace = 13;
            kspOuter[28].OuterSpace = 12.5;
            kspOuter[28].OrbitalPeriod = 1519864480;
            kspOuter[28].Orbits = Orbits.Kerbol;
            kspOuter[28].ParentIndex = 28;
            kspOuter[28].Biomes = 0;
            kspOuter[28].ImgUri = @"pack://application:,,/Images/Planets/Neidon.png";


            kspOuter[29] = new Planet.All();
            kspOuter[29].Index = 29;
            kspOuter[29].Name = "Thatmo";
            kspOuter[29].Type = Types.Moon;
            kspOuter[29].System = Systems.Neidon;
            kspOuter[29].LowOrbit = "30 km";
            kspOuter[29].SurfaceGravity = 2.28;
            kspOuter[29].Atmosphere = true;
            kspOuter[29].SurfaceToLowOrbit = 950;
            kspOuter[29].LowOrbitToMoonIntercept = 450;
            kspOuter[29].MoonInterceptToElipticalOrbit = 930;
            kspOuter[29].MoonInterceptToElipticalOrbitMPC = 0;
            kspOuter[29].LowOrbitToElipticalOrbit = 1040;
            kspOuter[29].ElipticalOrbitToPlanetIntercet = 1340;
            kspOuter[29].PlanetInterceptToStarElipticalOrbit = 1700;
            kspOuter[29].MaxPlaneChange = 110;
            kspOuter[29].EscapeVelocity = "1,140.98 m/s";
            kspOuter[29].SphereOfInfluence = "5,709,379.1 m";
            kspOuter[29].oxigen = false;
            kspOuter[29].Height = 20000;
            kspOuter[29].Pressure = 0.01;
            kspOuter[29].Surface = 13;
            kspOuter[29].LowerAtmosphere = 1;
            kspOuter[29].UpperAtmosphere = 1;
            kspOuter[29].NearSpace = 12;
            kspOuter[29].OuterSpace = 11.5;
            kspOuter[29].OrbitalPeriod = 306390;
            kspOuter[29].Orbits = Orbits.Neidon;
            kspOuter[29].ParentIndex = 28;
            kspOuter[29].Biomes = 0;
            kspOuter[29].ImgUri = @"pack://application:,,/Images/Planets/Thatmo.png";

            kspOuter[30] = new Planet.All();
            kspOuter[30].Index = 30;
            kspOuter[30].Name = "Nissee";
            kspOuter[30].Type = Types.Moon;
            kspOuter[30].System = Systems.Neidon;
            kspOuter[30].LowOrbit = "10 km";
            kspOuter[30].SurfaceGravity = 0.441;
            kspOuter[30].Atmosphere = false;
            kspOuter[30].SurfaceToLowOrbit = 850;
            kspOuter[30].LowOrbitToMoonIntercept = 3600;
            kspOuter[30].MoonInterceptToElipticalOrbit = 1000;
            kspOuter[30].MoonInterceptToElipticalOrbitMPC = 0;
            kspOuter[30].LowOrbitToElipticalOrbit = 1040;
            kspOuter[30].ElipticalOrbitToPlanetIntercet = 1340;
            kspOuter[30].PlanetInterceptToStarElipticalOrbit = 1700;
            kspOuter[30].MaxPlaneChange = 110;
            kspOuter[30].EscapeVelocity = "162.75 m/s";
            kspOuter[30].SphereOfInfluence = "7,366,476.6 m";
            kspOuter[30].oxigen = false;
            kspOuter[30].Height = 0;
            kspOuter[30].Pressure = 0;
            kspOuter[30].Surface = 13;
            kspOuter[30].LowerAtmosphere = 0;
            kspOuter[30].UpperAtmosphere = 0;
            kspOuter[30].NearSpace = 12;
            kspOuter[30].OuterSpace = 11.5;
            kspOuter[30].OrbitalPeriod = 17977964;
            kspOuter[30].Orbits = Orbits.Neidon;
            kspOuter[30].ParentIndex = 28;
            kspOuter[30].Biomes = 0;
            kspOuter[30].ImgUri = @"pack://application:,,/Images/Planets/Nissee.png";

            kspOuter[31] = new Planet.All();
            kspOuter[31].Index = 31;
            kspOuter[31].Name = "Plock";
            kspOuter[31].Type = Types.Planet;
            kspOuter[31].System = Systems.Plock;
            kspOuter[31].LowOrbit = "10 km";
            kspOuter[31].SurfaceGravity = 1.45;
            kspOuter[31].Atmosphere = false;
            kspOuter[31].SurfaceToLowOrbit = 480;
            kspOuter[31].LowOrbitToMoonIntercept = 0;
            kspOuter[31].MoonInterceptToElipticalOrbit = 0;
            kspOuter[31].MoonInterceptToElipticalOrbitMPC = 0;
            kspOuter[31].LowOrbitToElipticalOrbit = 210;
            kspOuter[31].ElipticalOrbitToPlanetIntercet = 900;
            kspOuter[31].PlanetInterceptToStarElipticalOrbit = 23000;
            kspOuter[31].MaxPlaneChange = 500;
            kspOuter[31].EscapeVelocity = "740.82 m/s";
            kspOuter[31].SphereOfInfluence = "6.1284606×10 m";
            kspOuter[31].oxigen = false;
            kspOuter[31].Height = 0;
            kspOuter[31].Pressure = 0;
            kspOuter[31].Surface = 16;
            kspOuter[31].LowerAtmosphere = 0;
            kspOuter[31].UpperAtmosphere = 0;
            kspOuter[31].NearSpace = 15;
            kspOuter[31].OuterSpace = 14.5;
            kspOuter[31].OrbitalPeriod = 2276142536;
            kspOuter[31].Orbits = Orbits.Kerbol;
            kspOuter[31].ParentIndex = 31;
            kspOuter[31].Biomes = 0;
            kspOuter[31].ImgUri = @"pack://application:,,/Images/Planets/Plock.png";


            //Real Solar System Planets

            ksprss[0] = new Planet.All();
            ksprss[0].Index = 0;
            ksprss[0].Name = "None";

            ksprss[1] = new Planet.All();
            ksprss[1].Index = 1;
            ksprss[1].Name = "Mercury";
            ksprss[1].Type = Types.Planet;
            ksprss[1].System = Systems.Mercury;
            ksprss[1].LowOrbit = "100 km";
            ksprss[1].SurfaceGravity = 3.7;
            ksprss[1].Atmosphere = false;
            ksprss[1].SurfaceToLowOrbit = 3060;
            ksprss[1].LowOrbitToMoonIntercept = 0;
            ksprss[1].MoonInterceptToElipticalOrbit = 0;
            ksprss[1].MoonInterceptToElipticalOrbitMPC = 0;
            ksprss[1].LowOrbitToElipticalOrbit = 0;
            ksprss[1].ElipticalOrbitToPlanetIntercet = 1220;
            ksprss[1].PlanetInterceptToStarElipticalOrbit = 8650;
            ksprss[1].MaxPlaneChange = 0;
            ksprss[1].EscapeVelocity = "4,200 m/s";
            ksprss[1].SphereOfInfluence = "112,424 km";
            ksprss[1].oxigen = false;
            ksprss[1].Height = 0;
            ksprss[1].Pressure = 0;
            ksprss[1].Surface = 0;
            ksprss[1].LowerAtmosphere = 0;
            ksprss[1].UpperAtmosphere = 0;
            ksprss[1].NearSpace = 0;
            ksprss[1].OuterSpace = 0;
            ksprss[1].OrbitalPeriod = 7600530.24;
            ksprss[1].Orbits = Orbits.Sun;
            ksprss[1].ParentIndex = 1;
            ksprss[1].Biomes = 0;
            ksprss[1].ImgUri = @"pack://application:,,/Images/Planets/RSS/Mercury.png";


            ksprss[2] = new Planet.All();
            ksprss[2].Index = 2;
            ksprss[2].Name = "Venus";
            ksprss[2].Type = Types.Planet;
            ksprss[2].System = Systems.Venus;
            ksprss[2].LowOrbit = "400 km";
            ksprss[2].SurfaceGravity = 8.87;
            ksprss[2].Atmosphere = true;
            ksprss[2].SurfaceToLowOrbit = 27000;
            ksprss[2].LowOrbitToMoonIntercept = 2940;
            ksprss[2].MoonInterceptToElipticalOrbit = 0;
            ksprss[2].MoonInterceptToElipticalOrbitMPC = 0;
            ksprss[2].LowOrbitToElipticalOrbit = 0;
            ksprss[2].ElipticalOrbitToPlanetIntercet = 2940;
            ksprss[2].PlanetInterceptToStarElipticalOrbit = 6400;
            ksprss[2].MaxPlaneChange = 0;
            ksprss[2].EscapeVelocity = "10,361.4 m/s";
            ksprss[2].SphereOfInfluence = "616,282 km";
            ksprss[2].oxigen = false;
            ksprss[2].Height = 140000;
            ksprss[2].Pressure = 90;
            ksprss[2].Surface = 0;
            ksprss[2].LowerAtmosphere = 0;
            ksprss[2].UpperAtmosphere = 0;
            ksprss[2].NearSpace = 0;
            ksprss[2].OuterSpace = 0;
            ksprss[2].OrbitalPeriod = 19414166.4;
            ksprss[2].Orbits = Orbits.Sun;
            ksprss[2].ParentIndex = 2;
            ksprss[2].Biomes = 0;
            ksprss[2].ImgUri = @"pack://application:,,/Images/Planets/RSS/Venus.png";

            ksprss[3] = new Planet.All();
            ksprss[3].Index = 5;
            ksprss[3].Name = "Earth";
            ksprss[3].Type = Types.Planet;
            ksprss[3].System = Systems.Earth;
            ksprss[3].LowOrbit = "250 km";
            ksprss[3].SurfaceGravity = 9.81;
            ksprss[3].Atmosphere = true;
            ksprss[3].SurfaceToLowOrbit = 9400;
            ksprss[3].LowOrbitToMoonIntercept = 0;
            ksprss[3].MoonInterceptToElipticalOrbit = 0;
            ksprss[3].MoonInterceptToElipticalOrbitMPC = 0;
            ksprss[3].LowOrbitToElipticalOrbit = 0;
            ksprss[3].ElipticalOrbitToPlanetIntercet = 0;
            ksprss[3].PlanetInterceptToStarElipticalOrbit = 3210;
            ksprss[3].MaxPlaneChange = 0;
            ksprss[3].EscapeVelocity = "11,186 m/s";
            ksprss[3].SphereOfInfluence = "924,008 km";
            ksprss[3].oxigen = true;
            ksprss[3].Height = 130000;
            ksprss[3].Pressure = 1;
            ksprss[3].Surface = 0;
            ksprss[3].LowerAtmosphere = 0;
            ksprss[3].UpperAtmosphere = 0;
            ksprss[3].NearSpace = 0;
            ksprss[3].OuterSpace = 0;
            ksprss[3].OrbitalPeriod = 31558118.4;
            ksprss[3].Orbits = Orbits.Sun;
            ksprss[3].ParentIndex = 3;
            ksprss[3].Biomes = 0;
            ksprss[3].ImgUri = @"pack://application:,,/Images/Planets/RSS/Earth.png";

            ksprss[4] = new Planet.All();
            ksprss[4].Index = 4;
            ksprss[4].Name = "Moon";
            ksprss[4].Type = Types.Moon;
            ksprss[4].System = Systems.Earth;
            ksprss[4].LowOrbit = "100 km";
            ksprss[4].SurfaceGravity = 1.62;
            ksprss[4].Atmosphere = false;
            ksprss[4].SurfaceToLowOrbit = 1730;
            ksprss[4].LowOrbitToMoonIntercept = 680;
            ksprss[4].MoonInterceptToElipticalOrbit = 3260;
            ksprss[4].MoonInterceptToElipticalOrbitMPC = 0;
            ksprss[4].LowOrbitToElipticalOrbit = 0;
            ksprss[4].ElipticalOrbitToPlanetIntercet = 3210;
            ksprss[4].PlanetInterceptToStarElipticalOrbit = 0;
            ksprss[4].MaxPlaneChange = 0;
            ksprss[4].EscapeVelocity = "2,376.1 m/s";
            ksprss[4].SphereOfInfluence = "66,173 km";
            ksprss[4].oxigen = false;
            ksprss[4].Height = 0;
            ksprss[4].Pressure = 0;
            ksprss[4].Surface = 0;
            ksprss[4].LowerAtmosphere = 0;
            ksprss[4].UpperAtmosphere = 0;
            ksprss[4].NearSpace = 0;
            ksprss[4].OuterSpace = 0;
            ksprss[4].OrbitalPeriod = 2360534.4;
            ksprss[4].Orbits = Orbits.Earth;
            ksprss[4].ParentIndex = 3;
            ksprss[4].Biomes = 0;
            ksprss[4].ImgUri = @"pack://application:,,/Images/Planets/RSS/Moon.png";

            ksprss[5] = new Planet.All();
            ksprss[5].Index = 5;
            ksprss[5].Name = "Mars";
            ksprss[5].Type = Types.Planet;
            ksprss[5].System = Systems.Mars;
            ksprss[5].LowOrbit = "250 km";
            ksprss[5].SurfaceGravity = 3.711;
            ksprss[5].Atmosphere = true;
            ksprss[5].SurfaceToLowOrbit = 3800;
            ksprss[5].LowOrbitToMoonIntercept = 0;
            ksprss[5].MoonInterceptToElipticalOrbit = 1440;
            ksprss[5].MoonInterceptToElipticalOrbitMPC = 0;
            ksprss[5].LowOrbitToElipticalOrbit = 0;
            ksprss[5].ElipticalOrbitToPlanetIntercet = 0;
            ksprss[5].PlanetInterceptToStarElipticalOrbit = 1060;
            ksprss[5].MaxPlaneChange = 0;
            ksprss[5].EscapeVelocity = "5,034 m/s";
            ksprss[5].SphereOfInfluence = "577,254 km";
            ksprss[5].oxigen = false;
            ksprss[5].Height = 130000;
            ksprss[5].Pressure = 0.0129;
            ksprss[5].Surface = 0;
            ksprss[5].LowerAtmosphere = 0;
            ksprss[5].UpperAtmosphere = 0;
            ksprss[5].NearSpace = 0;
            ksprss[5].OuterSpace = 0;
            ksprss[5].OrbitalPeriod = 59354294.4;
            ksprss[5].Orbits = Orbits.Sun;
            ksprss[5].ParentIndex = 5;
            ksprss[5].Biomes = 0;
            ksprss[5].ImgUri = @"pack://application:,,/Images/Planets/RSS/Mars.png";

            ksprss[6] = new Planet.All();
            ksprss[6].Index = 6;
            ksprss[6].Name = "Phobos";
            ksprss[6].Type = Types.Moon;
            ksprss[6].System = Systems.Mars;
            ksprss[6].LowOrbit = "1 km";
            ksprss[6].SurfaceGravity = 0.0057;
            ksprss[6].Atmosphere = false;
            ksprss[6].SurfaceToLowOrbit = 8;
            ksprss[6].LowOrbitToMoonIntercept = 3;
            ksprss[6].MoonInterceptToElipticalOrbit = 1280;
            ksprss[6].MoonInterceptToElipticalOrbitMPC = 0;
            ksprss[6].LowOrbitToElipticalOrbit = 0;
            ksprss[6].ElipticalOrbitToPlanetIntercet = 0;
            ksprss[6].PlanetInterceptToStarElipticalOrbit = 1060;
            ksprss[6].MaxPlaneChange = 0;
            ksprss[6].EscapeVelocity = "14 m/s";
            ksprss[6].SphereOfInfluence = "47 km";
            ksprss[6].oxigen = false;
            ksprss[6].Height = 0;
            ksprss[6].Pressure = 0;
            ksprss[6].Surface = 0;
            ksprss[6].LowerAtmosphere = 0;
            ksprss[6].UpperAtmosphere = 0;
            ksprss[6].NearSpace = 0;
            ksprss[6].OuterSpace = 0;
            ksprss[6].OrbitalPeriod = 27553.843872;
            ksprss[6].Orbits = Orbits.Mars;
            ksprss[6].ParentIndex = 5;
            ksprss[6].Biomes = 0;
            ksprss[6].ImgUri = @"pack://application:,,/Images/Planets/RSS/Phobos.png";

            ksprss[7] = new Planet.All();
            ksprss[7].Index = 7;
            ksprss[7].Name = "Deimos";
            ksprss[7].Type = Types.Moon;
            ksprss[7].System = Systems.Mars;
            ksprss[7].LowOrbit = "1 km";
            ksprss[7].SurfaceGravity = 0.003;
            ksprss[7].Atmosphere = false;
            ksprss[7].SurfaceToLowOrbit = 4;
            ksprss[7].LowOrbitToMoonIntercept = 2;
            ksprss[7].MoonInterceptToElipticalOrbit = 990;
            ksprss[7].MoonInterceptToElipticalOrbitMPC = 0;
            ksprss[7].LowOrbitToElipticalOrbit = 0;
            ksprss[7].ElipticalOrbitToPlanetIntercet = 0;
            ksprss[7].PlanetInterceptToStarElipticalOrbit = 1060;
            ksprss[7].MaxPlaneChange = 0;
            ksprss[7].EscapeVelocity = "5 m/s";
            ksprss[7].SphereOfInfluence = "45 km";
            ksprss[7].oxigen = false;
            ksprss[7].Height = 0;
            ksprss[7].Pressure = 0;
            ksprss[7].Surface = 0;
            ksprss[7].LowerAtmosphere = 0;
            ksprss[7].UpperAtmosphere = 0;
            ksprss[7].NearSpace = 0;
            ksprss[7].OuterSpace = 0;
            ksprss[7].OrbitalPeriod = 109123.2;
            ksprss[7].Orbits = Orbits.Mars;
            ksprss[7].ParentIndex = 5;
            ksprss[7].Biomes = 0;
            ksprss[7].ImgUri = @"pack://application:,,/Images/Planets/RSS/Deimos.png";

            ksprss[8] = new Planet.All();
            ksprss[8].Index = 8;
            ksprss[8].Name = "Jupiter";
            ksprss[8].Type = Types.Gasgiant;
            ksprss[8].System = Systems.Jupiter;
            ksprss[8].LowOrbit = "2000 km";
            ksprss[8].SurfaceGravity = 24.79;
            ksprss[8].Atmosphere = true;
            ksprss[8].SurfaceToLowOrbit = 45000;
            ksprss[8].LowOrbitToMoonIntercept = 0;
            ksprss[8].MoonInterceptToElipticalOrbit = 17200;
            ksprss[8].MoonInterceptToElipticalOrbitMPC = 0;
            ksprss[8].LowOrbitToElipticalOrbit = 0;
            ksprss[8].ElipticalOrbitToPlanetIntercet = 0;
            ksprss[8].PlanetInterceptToStarElipticalOrbit = 3360;
            ksprss[8].MaxPlaneChange = 0;
            ksprss[8].EscapeVelocity = "60201.5 m/s";
            ksprss[8].SphereOfInfluence = "48,196,240 km";
            ksprss[8].oxigen = false;
            ksprss[8].Height = 800000;
            ksprss[8].Pressure = 0;
            ksprss[8].Surface = 0;
            ksprss[8].LowerAtmosphere = 0;
            ksprss[8].UpperAtmosphere = 0;
            ksprss[8].NearSpace = 0;
            ksprss[8].OuterSpace = 0;
            ksprss[8].OrbitalPeriod = 374335776;
            ksprss[8].Orbits = Orbits.Sun;
            ksprss[8].ParentIndex = 8;
            ksprss[8].Biomes = 0;
            ksprss[8].ImgUri = @"pack://application:,,/Images/Planets/RSS/Jupiter.png";

            ksprss[9] = new Planet.All();
            ksprss[9].Index = 9;
            ksprss[9].Name = "Io";
            ksprss[9].Type = Types.Moon;
            ksprss[9].System = Systems.Jupiter;
            ksprss[9].LowOrbit = "100 km";
            ksprss[9].SurfaceGravity = 1.796;
            ksprss[9].Atmosphere = false;
            ksprss[9].SurfaceToLowOrbit = 1850;
            ksprss[9].LowOrbitToMoonIntercept = 730;
            ksprss[9].MoonInterceptToElipticalOrbit = 10320;
            ksprss[9].MoonInterceptToElipticalOrbitMPC = 0;
            ksprss[9].LowOrbitToElipticalOrbit = 0;
            ksprss[9].ElipticalOrbitToPlanetIntercet = 0;
            ksprss[9].PlanetInterceptToStarElipticalOrbit = 3360;
            ksprss[9].MaxPlaneChange = 0;
            ksprss[9].EscapeVelocity = "2,565.6 m/s";
            ksprss[9].SphereOfInfluence = "7,841 km";
            ksprss[9].oxigen = false;
            ksprss[9].Height = 0;
            ksprss[9].Pressure = 0;
            ksprss[9].Surface = 0;
            ksprss[9].LowerAtmosphere = 0;
            ksprss[9].UpperAtmosphere = 0;
            ksprss[9].NearSpace = 0;
            ksprss[9].OuterSpace = 0;
            ksprss[9].OrbitalPeriod = 152841.6;
            ksprss[9].Orbits = Orbits.Jupiter;
            ksprss[9].ParentIndex = 8;
            ksprss[9].Biomes = 0;
            ksprss[9].ImgUri = @"pack://application:,,/Images/Planets/RSS/Io.png";

            ksprss[10] = new Planet.All();
            ksprss[10].Index = 10;
            ksprss[10].Name = "Europa";
            ksprss[10].Type = Types.Moon;
            ksprss[10].System = Systems.Jupiter;
            ksprss[10].LowOrbit = "100 km";
            ksprss[10].SurfaceGravity = 1.314;
            ksprss[10].Atmosphere = false;
            ksprss[10].SurfaceToLowOrbit = 1480;
            ksprss[10].LowOrbitToMoonIntercept = 580;
            ksprss[10].MoonInterceptToElipticalOrbit = 8890;
            ksprss[10].MoonInterceptToElipticalOrbitMPC = 0;
            ksprss[10].LowOrbitToElipticalOrbit = 0;
            ksprss[10].ElipticalOrbitToPlanetIntercet = 0;
            ksprss[10].PlanetInterceptToStarElipticalOrbit = 3360;
            ksprss[10].MaxPlaneChange = 0;
            ksprss[10].EscapeVelocity = "2,032.6 m/s";
            ksprss[10].SphereOfInfluence = "9,728 km";
            ksprss[10].oxigen = false;
            ksprss[10].Height = 0;
            ksprss[10].Pressure = 0;
            ksprss[10].Surface = 0;
            ksprss[10].LowerAtmosphere = 0;
            ksprss[10].UpperAtmosphere = 0;
            ksprss[10].NearSpace = 0;
            ksprss[10].OuterSpace = 0;
            ksprss[10].OrbitalPeriod = 306822.0384;
            ksprss[10].Orbits = Orbits.Jupiter;
            ksprss[10].ParentIndex = 8;
            ksprss[10].Biomes = 0;
            ksprss[10].ImgUri = @"pack://application:,,/Images/Planets/RSS/Europa.png";

            ksprss[11] = new Planet.All();
            ksprss[11].Index = 11;
            ksprss[11].Name = "Ganymede";
            ksprss[11].Type = Types.Moon;
            ksprss[11].System = Systems.Jupiter;
            ksprss[11].LowOrbit = "100 km";
            ksprss[11].SurfaceGravity = 1.428;
            ksprss[11].Atmosphere = false;
            ksprss[11].SurfaceToLowOrbit = 1970;
            ksprss[11].LowOrbitToMoonIntercept = 790;
            ksprss[11].MoonInterceptToElipticalOrbit = 6700;
            ksprss[11].MoonInterceptToElipticalOrbitMPC = 0;
            ksprss[11].LowOrbitToElipticalOrbit = 0;
            ksprss[11].ElipticalOrbitToPlanetIntercet = 0;
            ksprss[11].PlanetInterceptToStarElipticalOrbit = 3360;
            ksprss[11].MaxPlaneChange = 0;
            ksprss[11].EscapeVelocity = "2,745.5 m/s";
            ksprss[11].SphereOfInfluence = "24,362 km";
            ksprss[11].oxigen = false;
            ksprss[11].Height = 0;
            ksprss[11].Pressure = 0;
            ksprss[11].Surface = 0;
            ksprss[11].LowerAtmosphere = 0;
            ksprss[11].UpperAtmosphere = 0;
            ksprss[11].NearSpace = 0;
            ksprss[11].OuterSpace = 0;
            ksprss[11].OrbitalPeriod = 618153.375744;
            ksprss[11].Orbits = Orbits.Jupiter;
            ksprss[11].ParentIndex = 8;
            ksprss[11].Biomes = 0;
            ksprss[11].ImgUri = @"pack://application:,,/Images/Planets/RSS/Ganymede.png";

            ksprss[12] = new Planet.All();
            ksprss[12].Index = 12;
            ksprss[12].Name = "Calisto";
            ksprss[12].Type = Types.Moon;
            ksprss[12].System = Systems.Jupiter;
            ksprss[12].LowOrbit = "100 km";
            ksprss[12].SurfaceGravity = 1.235;
            ksprss[12].Atmosphere = false;
            ksprss[12].SurfaceToLowOrbit = 1760;
            ksprss[12].LowOrbitToMoonIntercept = 70;
            ksprss[12].MoonInterceptToElipticalOrbit = 5140;
            ksprss[12].MoonInterceptToElipticalOrbitMPC = 0;
            ksprss[12].LowOrbitToElipticalOrbit = 0;
            ksprss[12].ElipticalOrbitToPlanetIntercet = 0;
            ksprss[12].PlanetInterceptToStarElipticalOrbit = 3360;
            ksprss[12].MaxPlaneChange = 0;
            ksprss[12].EscapeVelocity = "2,441.5 m/s";
            ksprss[12].SphereOfInfluence = "37,706 km";
            ksprss[12].oxigen = false;
            ksprss[12].Height = 0;
            ksprss[12].Pressure = 0;
            ksprss[12].Surface = 0;
            ksprss[12].LowerAtmosphere = 0;
            ksprss[12].UpperAtmosphere = 0;
            ksprss[12].NearSpace = 0;
            ksprss[12].OuterSpace = 0;
            ksprss[12].OrbitalPeriod = 1441931.18976;
            ksprss[12].Orbits = Orbits.Jupiter;
            ksprss[12].ParentIndex = 8;
            ksprss[12].Biomes = 0;
            ksprss[12].ImgUri = @"pack://application:,,/Images/Planets/RSS/Calisto.png";

            ksprss[13] = new Planet.All();
            ksprss[13].Index = 13;
            ksprss[13].Name = "Saturn";
            ksprss[13].Type = Types.Gasgiant;
            ksprss[13].System = Systems.Saturn;
            ksprss[13].LowOrbit = "2000 km";
            ksprss[13].SurfaceGravity = 10.44;
            ksprss[13].Atmosphere = true;
            ksprss[13].SurfaceToLowOrbit = 30000;
            ksprss[13].LowOrbitToMoonIntercept = 0;
            ksprss[13].MoonInterceptToElipticalOrbit = 10230;
            ksprss[13].MoonInterceptToElipticalOrbitMPC = 0;
            ksprss[13].LowOrbitToElipticalOrbit = 0;
            ksprss[13].ElipticalOrbitToPlanetIntercet = 0;
            ksprss[13].PlanetInterceptToStarElipticalOrbit = 4500;
            ksprss[13].MaxPlaneChange = 0;
            ksprss[13].EscapeVelocity = "35,482.5 m/s";
            ksprss[13].SphereOfInfluence = "54,479,873 km";
            ksprss[13].oxigen = false;
            ksprss[13].Height = 800000;
            ksprss[13].Pressure = 0;
            ksprss[13].Surface = 0;
            ksprss[13].LowerAtmosphere = 0;
            ksprss[13].UpperAtmosphere = 0;
            ksprss[13].NearSpace = 0;
            ksprss[13].OuterSpace = 0;
            ksprss[13].OrbitalPeriod = 929596608;
            ksprss[13].Orbits = Orbits.Sun;
            ksprss[13].ParentIndex = 13;
            ksprss[13].Biomes = 0;
            ksprss[13].ImgUri = @"pack://application:,,/Images/Planets/RSS/Saturn.png";

            ksprss[14] = new Planet.All();
            ksprss[14].Index = 14;
            ksprss[14].Name = "Titan";
            ksprss[14].Type = Types.Moon;
            ksprss[14].System = Systems.Saturn;
            ksprss[14].LowOrbit = "1000 km";
            ksprss[14].SurfaceGravity = 1.352;
            ksprss[14].Atmosphere = true;
            ksprss[14].SurfaceToLowOrbit = 7600;
            ksprss[14].LowOrbitToMoonIntercept = 660;
            ksprss[14].MoonInterceptToElipticalOrbit = 3060;
            ksprss[14].MoonInterceptToElipticalOrbitMPC = 0;
            ksprss[14].LowOrbitToElipticalOrbit = 0;
            ksprss[14].ElipticalOrbitToPlanetIntercet = 0;
            ksprss[14].PlanetInterceptToStarElipticalOrbit = 4500;
            ksprss[14].MaxPlaneChange = 0;
            ksprss[14].EscapeVelocity = "2,645.3 m/s";
            ksprss[14].SphereOfInfluence = "43,321 km";
            ksprss[14].oxigen = false;
            ksprss[14].Height = 600000;
            ksprss[14].Pressure = 1.470;
            ksprss[14].Surface = 0;
            ksprss[14].LowerAtmosphere = 0;
            ksprss[14].UpperAtmosphere = 0;
            ksprss[14].NearSpace = 0;
            ksprss[14].OuterSpace = 0;
            ksprss[14].OrbitalPeriod = 1377648000;
            ksprss[14].Orbits = Orbits.Saturn;
            ksprss[14].ParentIndex = 13;
            ksprss[14].Biomes = 0;
            ksprss[14].ImgUri = @"pack://application:,,/Images/Planets/RSS/Titan.png";

            ksprss[15] = new Planet.All();
            ksprss[15].Index = 15;
            ksprss[15].Name = "Uranus";
            ksprss[15].Type = Types.Planet;
            ksprss[15].System = Systems.Uranus;
            ksprss[15].LowOrbit = "1000 km";
            ksprss[15].SurfaceGravity = 8.69;
            ksprss[15].Atmosphere = true;
            ksprss[15].SurfaceToLowOrbit = 18000;
            ksprss[15].LowOrbitToMoonIntercept = 0;
            ksprss[15].MoonInterceptToElipticalOrbit = 0;
            ksprss[15].MoonInterceptToElipticalOrbitMPC = 0;
            ksprss[15].LowOrbitToElipticalOrbit = 6120;
            ksprss[15].ElipticalOrbitToPlanetIntercet = 0;
            ksprss[15].PlanetInterceptToStarElipticalOrbit = 5280;
            ksprss[15].MaxPlaneChange = 0;
            ksprss[15].EscapeVelocity = "21,565.2 m/s";
            ksprss[15].SphereOfInfluence = "51,691,684 km";
            ksprss[15].oxigen = false;
            ksprss[15].Height = 700000;
            ksprss[15].Pressure = 3485.12;
            ksprss[15].Surface = 0;
            ksprss[15].LowerAtmosphere = 0;
            ksprss[15].UpperAtmosphere = 0;
            ksprss[15].NearSpace = 0;
            ksprss[15].OuterSpace = 0;
            ksprss[15].OrbitalPeriod = 2651486400;
            ksprss[15].Orbits = Orbits.Sun;
            ksprss[15].ParentIndex = 15;
            ksprss[15].Biomes = 0;
            ksprss[15].ImgUri = @"pack://application:,,/Images/Planets/RSS/Uranus.png";

            ksprss[16] = new Planet.All();
            ksprss[16].Index = 16;
            ksprss[16].Name = "Neptune";
            ksprss[16].Type = Types.Planet;
            ksprss[16].System = Systems.Neptune;
            ksprss[16].LowOrbit = "1000 km";
            ksprss[16].SurfaceGravity = 11.15;
            ksprss[16].Atmosphere = true;
            ksprss[16].SurfaceToLowOrbit = 19000;
            ksprss[16].LowOrbitToMoonIntercept = 0;
            ksprss[16].MoonInterceptToElipticalOrbit = 0;
            ksprss[16].MoonInterceptToElipticalOrbitMPC = 0;
            ksprss[16].LowOrbitToElipticalOrbit = 6750;
            ksprss[16].ElipticalOrbitToPlanetIntercet = 0;
            ksprss[16].PlanetInterceptToStarElipticalOrbit = 5390;
            ksprss[16].MaxPlaneChange = 0;
            ksprss[16].EscapeVelocity = "23,733.9 m/s";
            ksprss[16].SphereOfInfluence = "86,641,942 km";
            ksprss[16].oxigen = false;
            ksprss[16].Height = 700000;
            ksprss[16].Pressure = 3685.12;
            ksprss[16].Surface = 0;
            ksprss[16].LowerAtmosphere = 0;
            ksprss[16].UpperAtmosphere = 0;
            ksprss[16].NearSpace = 0;
            ksprss[16].OuterSpace = 0;
            ksprss[16].OrbitalPeriod = 5199724800;
            ksprss[16].Orbits = Orbits.Sun;
            ksprss[16].ParentIndex = 15;
            ksprss[16].Biomes = 0;
            ksprss[16].ImgUri = @"pack://application:,,/Images/Planets/RSS/Neptune.png";



            
        }

        private void button_Click_1(object sender, RoutedEventArgs e)
        {

            
        }

      
    }

}



