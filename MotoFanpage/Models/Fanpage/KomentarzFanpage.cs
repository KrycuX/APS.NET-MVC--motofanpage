using MotoFanpage.Models.Ogloszenia;
using MotoFanpage.Models.Ogólne;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MotoFanpage.Models.Fanpage
{
    public class KomentarzFanpage
    {
        public int ID { get; set; }

        public string Tresc { get; set; }

        public DateTime Date { get; set; }
        public int PostId { get; set; }
 

        public int ProfilId { get; set; }

        
        public virtual Post Post { get; set; }
        public virtual Profil Profil { get; set; }
    }
}