using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kompetenzcheck.Controller
{
    public class Fahrzeug
    {
        public int FahrzeugID { get; set; }
        public string Bezeichnung { get; set; }
        public int Baujahr { get; set; }
        public float Preis { get; set; }
        public string Kategorie { get; set; }
        public int HerstellerID { get; set; }
    }
}
