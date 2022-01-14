using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MotoFanpage.DAL;
using MotoFanpage.Models;
using MotoFanpage.Models.Fanpage;
using MotoFanpage.Models.Ogólne;

namespace MotoFanpage.Controllers.Ogólne
{
    public class ProfilController : Controller
    {
        private FanpageContext db = new FanpageContext();
        

        // GET: Profil
        public ActionResult Index(string currentFilter, string searchString)
        {
            var profil = db.BProfil.ToList();
            if (searchString != null)
            {
                 profil = db.BProfil.ToList();

            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
           
           
            if (!String.IsNullOrEmpty(searchString))
            {
                profil = profil.Where(a => a.Name.Contains(searchString) || a.Surname.Contains(searchString) ||(a.Name+" "+a.Surname).Contains(searchString) ).ToList();
            }

            return View(profil);
        }

        public ActionResult UserFollow()
        {
            Profil profile = db.BProfil.Single(p => p.Email == User.Identity.Name);

            return View(db.BProfil.Single(p => p.Email == User.Identity.Name).FavAuct);
        }


        // GET: Profil/Details/5
        public ActionResult Details()
        {
            
            Profil profil = db.BProfil.Single(p=>p.Email==User.Identity.Name);
         

            if (profil == null)
            {
                return HttpNotFound();
            }
            return View(profil);
        }

        public ActionResult DetailsOther(int id)
        {

            Profil profil = db.BProfil.FirstOrDefault(p=>p.ID==id);
           Profil profil2 = db.BProfil.FirstOrDefault(p=>p.Email==User.Identity.Name);

          
            List<Profil> list2 = db.BProfil.ToList();

           

           
           

            List<FriendReq> reqs = new List<FriendReq>();
            reqs = null;
            if (db.BFriendReq!=null)
            {  
           reqs = db.BFriendReq.Where(p=>p.IdOdbierajacego==profil.ID && p.IdWysylajacego==profil2.ID ||
           p.IdOdbierajacego == profil2.ID && p.IdWysylajacego == profil.ID
           ).ToList();
            }

            if (profil == null)
            {
                return HttpNotFound();
            }

            ViewBag.Request = reqs;
            ViewBag.Profil = profil2;
          
            return View("Details",profil);
        }

        // GET: Profil/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Profil/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Login,Email,Name,Surname,MyProperty")] Profil profil)
        {
            if (ModelState.IsValid)
            {
                db.BProfil.Add(profil);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(profil);
        }

        // GET: Profil/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profil profil = db.BProfil.Find(id);
            if (profil == null)
            {
                return HttpNotFound();
            }
            return View(profil);
        }


        // POST: Profil/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Profil profil)
        {
            HttpPostedFileBase file = Request.Files["ObrazFile"];
          
           


                if (file != null && file.ContentLength > 0)
            {
            
                profil.Zdjecie = file.FileName;
                db.BProfil.First(p => p.ID == profil.ID).Zdjecie = file.FileName;
                file.SaveAs(HttpContext.Server.MapPath("~/Obrazki/") + profil.Zdjecie);
            }

            if (ModelState.IsValid)
            {
                db.BProfil.First(p => p.ID == profil.ID).Name = profil.Name;
                db.BProfil.First(p => p.ID == profil.ID).Surname = profil.Surname;
                db.BProfil.First(p => p.ID == profil.ID).Login = profil.Login;
                db.BProfil.First(p => p.ID == profil.ID).Password = profil.Password;
                db.BProfil.First(p => p.ID == profil.ID).ConfirmPassword = profil.ConfirmPassword;
              
               
                db.SaveChanges();
                return RedirectToAction("Details");
            }
            return View(profil);
        }

        // GET: Profil/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profil profil = db.BProfil.Find(id);
            if (profil == null)
            {
                return HttpNotFound();
            }
            return View(profil);
        }

        // POST: Profil/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Profil profil = db.BProfil.Find(id);
            db.BProfil.Remove(profil);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
