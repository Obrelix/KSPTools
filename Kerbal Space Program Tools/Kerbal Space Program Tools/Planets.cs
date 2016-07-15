using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kerbal_Space_Progam_Tools
{
    public enum Types
    {
        Star,
        Planet,
        Moon,
        Gasgiant

    }
    public enum Systems
    {
        Kerbol,
        Moho,
        Eve,
        Kerbin,
        Duna,
        Dres,
        Jool,
        Eeloo,
        Sarnus,
        Urlum,
        Neidon,
        Plock,
        Earth,
        Mercury,
        Venus,
        Mars,
        Jupiter,
        Saturn,
        Uranus,
        Neptune,
        None
    }
    public enum Orbits
    {
        Sun,
        Kerbol,
        Eve,
        Kerbin,
        Duna,
        Jool,
        Sarnus,
        Urlum,
        Neidon,
        Plock,
        Earth,
        Mercury,
        Venus,
        Mars,
        Jupiter,
        Saturn,
        Uranus,
        Neptune,
        None
    }

    public class Planet
    {
        public string Name { get; set; }
        public int Index { get; set; }
        public int ParentIndex { get; set; }
        public string ImgUri { get; set; }


        public Planet()
        {

        }
        public class Info : Planet
        {

            public Types Type { get; set; }
            public Systems System { get; set; }
            public string LowOrbit { get; set; }
            public string EscapeVelocity { get; set; }
            public double SurfaceGravity { get; set; }
            public string SphereOfInfluence { get; set; }

            public Info()
            {

            }
        }

        public class DV : Info
        {

            public int SurfaceToLowOrbit { get; set; }
            public int LowOrbitToMoonIntercept { get; set; }
            public int MoonInterceptToElipticalOrbit { get; set; }
            public int MoonInterceptToElipticalOrbitMPC { get; set; }
            public int LowOrbitToElipticalOrbit { get; set; }
            public int ElipticalOrbitToPlanetIntercet { get; set; }
            public int PlanetInterceptToStarElipticalOrbit { get; set; }
            public int MaxPlaneChange { get; set; }

            public DV()
            {

            }
        }
        public class ScientificMultiplier : DV
        {
            public int Biomes { get; set; }
            public double Surface { get; set; }
            public double LowerAtmosphere { get; set; }
            public double UpperAtmosphere { get; set; }
            public double NearSpace { get; set; }
            public double OuterSpace { get; set; }

            public ScientificMultiplier()
            {

            }
        }
        public class Atmo : ScientificMultiplier
        {
            public bool Atmosphere { get; set; }
            public bool oxigen { get; set; }
            public double Pressure { get; set; }
            public double Height { get; set; }

            public Atmo()
            {

            }

        }
        public class Orbit : Atmo
        {
            public double OrbitalPeriod { get; set; }
            public Orbits Orbits { get; set; }

            public Orbit()
            {

            }

        }
        public class All : Orbit
        {
            public All()
            {

            }
        }
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
        public Planet.All[] ksp = new Planet.All[35];
        public Planet.All[] kspNormal = new Planet.All[PlanetsNumber];
        public Planet.All[] kspOuter = new Planet.All[OuterPlanetsNumber];
        public Planet.All[] ksprss = new Planet.All[RssPlanetNumber];

        public double TotalTime = 0;



    }
}