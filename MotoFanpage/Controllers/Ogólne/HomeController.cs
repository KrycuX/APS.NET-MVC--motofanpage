using MotoFanpage.DAL;
using MotoFanpage.Models.Fanpage;
using MotoFanpage.Models.Ogólne;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MotoFanpage.Controllers.Ogólne
{
    public class HomeController : Controller
    {
        FanpageContext db = new FanpageContext();
        public ActionResult Index(int? error)
        {
            Profil profile = db.BProfil.FirstOrDefault(p => p.Email == User.Identity.Name);
            List<Post> post = db.BPost.OrderByDescending(p=>p.Date).ToList();
         
          
                ViewBag.Profil = profile;
            if(error==1)
            { 
                 ViewBag.Error = "Nie można dodać pustego komentarza!";
            }

            return View(post);
        }

    

        public ActionResult DajLike(int? id)
        {
            Post post = db.BPost.Find(id);
            if (db.BProfil.Single(p => p.Email == User.Identity.Name).Likes == null)
            {
                var listLikesPost = new List<Profil>();
                listLikesPost.Add( db.BProfil.Single(p => p.Email == User.Identity.Name));
                db.BPost.Find(id).Likes=listLikesPost;
                var lista = new List<Post>();
                lista.Add(post);
                db.BProfil.Single(p => p.Email == User.Identity.Name).Likes = lista;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                var listLikesPost = db.BProfil.Single(p => p.Email == User.Identity.Name);
                db.BPost.Find(id).Likes.Add(listLikesPost);
                db.BProfil.Single(p => p.Email == User.Identity.Name).Likes.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

        
        }

        public ActionResult UnLike(int? id)
        {
            Post post = db.BPost.Find(id);
            Profil profil = db.BProfil.FirstOrDefault(p => p.Email == User.Identity.Name);
            db.BProfil.Single(p => p.Email == User.Identity.Name).Likes.Remove(post);

           db.BPost.Find(id).Likes.Remove(profil);
           
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}