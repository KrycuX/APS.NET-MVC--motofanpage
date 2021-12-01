using MotoFanpage.Models.Ogólne;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MotoFanpage.Models.Fanpage
{
    public class WiadomoscPriv
    {
        public int ID { get; set; }
        public string Tresc { get; set; }

        public int IdOdbierajacego { get; set; }

        public int ProfilID { get; set; }
        public virtual Profil Profil { get; set; }

      
    }
}