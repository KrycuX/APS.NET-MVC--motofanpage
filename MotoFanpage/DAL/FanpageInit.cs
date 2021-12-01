using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MotoFanpage.Models;
using MotoFanpage.Models.Fanpage;
using MotoFanpage.Models.Ogólne;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MotoFanpage.DAL
{
    public class FanpageInit : System.Data.Entity.DropCreateDatabaseIfModelChanges<FanpageContext>
    {
        protected override void Seed(FanpageContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            roleManager.Create(new IdentityRole("Admin"));
            roleManager.Create(new IdentityRole("User"));

            var user = new ApplicationUser { UserName = "myAdmin@gmail.com" };
            var user2 = new ApplicationUser { UserName = "myUser@gmail.com" };
            string pass = "Nike!2345";


            var profil = new List<Profil>
            {
            new Profil{Email=user.UserName, Name="Admin",Password=pass,ConfirmPassword=pass,Surname="Admin",Login="Admin",LZnajomi=new List<Znajomy>(),Zdjecie="noImage.jpg",LMsg=new List<WiadomoscPriv>()},
            new Profil { Email = user2.UserName, Name = "User",Password=pass,ConfirmPassword=pass, Surname = "User", Login = "User",LZnajomi=new List<Znajomy>(),Zdjecie="noImage.jpg",LMsg=new List<WiadomoscPriv>() }
            };

            
            userManager.Create(user, pass);
            userManager.AddToRole(user.Id, "Admin");
            userManager.Create(user2, pass);
            userManager.AddToRole(user2.Id, "User");


            profil.ForEach(t => context.BProfil.Add(t));
            context.SaveChanges();
            var post = new List<Post>
            {
                new Post{ProfilID=profil[1].ID,Date=DateTime.Now,Tresc="Testowy pościk",Likes=new List<Profil>()},
                new Post{ProfilID=profil[1].ID,Date=DateTime.Now,Tresc="Testowy pościk2",Likes=new List<Profil>()}
            };

            post.ForEach(t => context.BPost.Add(t));
            context.SaveChanges();

        }
    }
}