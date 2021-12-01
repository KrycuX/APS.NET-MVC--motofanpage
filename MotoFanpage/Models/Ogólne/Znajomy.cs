using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MotoFanpage.Models.Ogólne
{
    public class Znajomy
    {
        public int ID { get; set; }
        public int IdZnajomego { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }

        public virtual Profil Profil { get; set; }
    }
}