using MotoFanpage.Models;
using MotoFanpage.Models.Fanpage;
using MotoFanpage.Models.Ogloszenia;
using MotoFanpage.Models.Ogólne;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace MotoFanpage.DAL
{
    public class FanpageContext : DbContext

    {
        public FanpageContext() : base("DefaultConnection")
        { }

      
        public DbSet<KomentarzFanpage> BKomentarzFanpage { get; set; }
        public DbSet<Post> BPost { get; set; }
       
        public DbSet<WiadomoscPriv> BWiadomoscPriv { get; set; }
        public DbSet<Aukcja> BAukcja { get; set; }
        public DbSet<KomentarzAukcja> BKomentarzAukcja { get; set; }
        public DbSet<Pojazd> BPojazd { get; set; }
        public DbSet<Profil> BProfil { get; set; }
        public DbSet<FriendReq> BFriendReq { get; set; }
        public DbSet<Logi> BLogi { get; set; }
        public DbSet<Obraz> BObraz { get; set; }
        

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<KomentarzFanpage>().HasRequired<Profil>(k => k.Profil).WithMany(p => p.LKomFanpage).HasForeignKey(k => k.ProfilId).WillCascadeOnDelete(false);
            modelBuilder.Entity<KomentarzFanpage>().HasRequired<Post>(k => k.Post).WithMany(p => p.LKomPostProf).HasForeignKey(k => k.PostId).WillCascadeOnDelete(false);

            modelBuilder.Entity<KomentarzAukcja>().HasRequired<Profil>(k => k.Profil).WithMany(p => p.LKomAukcja).HasForeignKey(k => k.ProfilId).WillCascadeOnDelete(false);
            modelBuilder.Entity<KomentarzAukcja>().HasRequired<Aukcja>(k => k.Aukcja).WithMany(p => p.LKomAukcja).HasForeignKey(k => k.AukcjaID).WillCascadeOnDelete(false);

            modelBuilder.Entity<Pojazd>().HasRequired<Profil>(a =>a.Profil ).WithMany(p => p.LPojazdy).HasForeignKey(a=>a.ProfilID).WillCascadeOnDelete(false);
            modelBuilder.Entity<Post>().HasRequired<Profil>(k => k.Profil).WithMany(p => p.LPost).HasForeignKey(k => k.ProfilID).WillCascadeOnDelete(false);

         

            modelBuilder.Entity<WiadomoscPriv>().HasRequired<Profil>(k => k.Profil).WithMany(p => p.LMsg).HasForeignKey(k => k.ProfilID).WillCascadeOnDelete(false);
          
          //  modelBuilder.Entity<Znajomy>().HasRequired<Profil>(k => k.Profil).WithMany(p => p.LZnajomi).HasForeignKey(k => k.IdZnajomego).WillCascadeOnDelete(false);



         
           

             
        }




    }
}