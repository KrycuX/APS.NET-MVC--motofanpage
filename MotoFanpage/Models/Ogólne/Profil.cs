using MotoFanpage.Models.Ogloszenia;

using MotoFanpage.Models.Fanpage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MotoFanpage.Models.Ogólne
{
    public class Profil
    {
        public int ID { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "{0} musi zawierać co najmniej następującą liczbę znaków: {2}.", MinimumLength = 3)]
        [Display(Name = "Nazwa Użytkownika")]
        public string Login { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} musi zawierać co najmniej następującą liczbę znaków: {2}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }
      

        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź hasło")]
        [Compare("Password", ErrorMessage = "Hasło i jego potwierdzenie są niezgodne.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Adres e-mail")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Imię")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Nazwisko")]
        public string Surname { get; set; }

        public string Zdjecie { get; set; }


        public int MyProperty { get; set; }

        public virtual List<Znajomy> LZnajomi { get; set; }
        public virtual List<Pojazd> LPojazdy{ get; set;}
        public virtual List<Post> LPost{ get; set; }
        public virtual List<Post> Likes{ get; set; }
        public virtual List<WiadomoscPriv> LMsg { get; set; }
        public virtual List<Aukcja> FavAuct { get; set; }
        public virtual List<Aukcja> LAukcja { get; set; }
        public virtual List<KomentarzAukcja> LKomAukcja { get; set; }
        public virtual List<KomentarzFanpage> LKomFanpage { get; set; }
    }
}