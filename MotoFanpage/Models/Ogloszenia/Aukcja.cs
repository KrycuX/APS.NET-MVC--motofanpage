using MotoFanpage.Models.Ogólne;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MotoFanpage.Models.Ogloszenia
{
    public class Aukcja
    {
        public int ID { get; set; }

        public string Tytul { get; set; }
        public string Opis { get; set; }

        public string Obrazek { get; set; }

        public int PojazdID { get; set; }
        public virtual Pojazd Pojazd { get; set; }
       
        public int ProfilID { get; set; }
        public virtual Profil Profil { get; set; }



        public virtual List<KomentarzAukcja> LKomAukcja { get; set; }
    }
}