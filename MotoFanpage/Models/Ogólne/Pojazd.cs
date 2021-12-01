using MotoFanpage.Models.Ogloszenia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MotoFanpage.Models.Ogólne
{
    public class Pojazd
    {
        public int ID { get; set; }
        
        public string Marka { get; set; }
        
        public string Model { get; set; }
       
        public int Przebieg { get; set; }
       
        public int RokProdukcji{ get; set; }
       
        public int MOC { get; set; }
        public TypeP? TypeP { get; set; }

        public int AukcjaID { get; set; }
        //  public virtual Auction Auction { get; set; }
        
        public int ProfilID { get; set; }
        public virtual Profil Profil { get; set; }
    }
    public enum TypeP
    { Samochód, Motocykl,
    Skuter}
}