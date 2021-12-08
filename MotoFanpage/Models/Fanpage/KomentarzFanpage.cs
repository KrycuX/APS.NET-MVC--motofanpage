using MotoFanpage.Models.Ogloszenia;
using MotoFanpage.Models.Ogólne;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MotoFanpage.Models.Fanpage
{
    public class KomentarzFanpage
    {
        public int ID { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "{0} musi zawierać co najmniej następującą liczbę znaków: {2}.", MinimumLength = 1)]
        public string Tresc { get; set; }

        public DateTime Date { get; set; }
        public int PostId { get; set; }
 

        public int ProfilId { get; set; }

        
        public virtual Post Post { get; set; }
        public virtual Profil Profil { get; set; }
    }
}