using MotoFanpage.Models.Ogólne;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MotoFanpage.Models.Ogloszenia
{
    public class KomentarzAukcja
    {
        public int ID { get; set; }

        public string Tresc { get; set; }

        public int AukcjaID { get; set; }

        public int ProfilId { get; set; }
        public DateTime Date { get; set; }
        public virtual Aukcja Aukcja { get; set; }
        public virtual Profil Profil{ get; set; }
    }
}