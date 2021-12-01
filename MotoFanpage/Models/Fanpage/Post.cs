using MotoFanpage.Models.Ogólne;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MotoFanpage.Models.Fanpage
{
    public class Post 
    {
        public int ID { get; set; }
        public int ProfilID { get; set; }
        public virtual Profil Profil { get; set; }
        public DateTime Date { get; set; }
        public string Tresc { get; set; }
        public string Obraz { get; set; }

        public virtual List<Profil> Likes{ get; set; }
        public virtual List<KomentarzFanpage> LKomPostProf { get; set; }

    }
}