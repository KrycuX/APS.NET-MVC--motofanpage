using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MotoFanpage.DAL;
using MotoFanpage.Models.Ogloszenia;
using MotoFanpage.Models.Ogólne;

namespace MotoFanpage.Controllers.Ogloszenia
{
    public class KomentarzAukcjaController : Controller
    {
        private FanpageContext db = new FanpageContext();

        // GET: KomentarzAukcja
        public ActionResult Index()
        {
            var bKomentarzAukcja = db.BKomentarzAukcja.Include(k => k.Aukcja).Include(k => k.Profil);
            return View(bKomentarzAukcja.ToList());
        }

        // GET: KomentarzAukcja/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KomentarzAukcja komentarzAukcja = db.BKomentarzAukcja.Find(id);
            if (komentarzAukcja == null)
            {
                return HttpNotFound();
            }
            return View(komentarzAukcja);
        }

        // GET: KomentarzAukcja/Create
        public ActionResult Create(int? id)
        {
            Profil profil = db.BProfil.FirstOrDefault(p => p.Email == User.Identity.Name);
            ViewBag.AukcjaID = id;
            ViewBag.Profil = profil;
            return View();
        }

        // POST: KomentarzAukcja/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int id, string content)
        {

            Profil profile = db.BProfil.Single(p => p.Email == User.Identity.Name);
            KomentarzAukcja comments = new KomentarzAukcja();
            comments.AukcjaID = id;
            //Uzupelnij Profil
            comments.ProfilId = profile.ID;
            comments.Date = DateTime.Now;
            comments.Tresc = content;
            db.BKomentarzAukcja.Add(comments);

            db.SaveChanges();
            return RedirectToAction("Details", "Aukcja", new { id = comments.AukcjaID });
        }

        // GET: KomentarzAukcja/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KomentarzAukcja komentarzAukcja = db.BKomentarzAukcja.Find(id);
            if (komentarzAukcja == null)
            {
                return HttpNotFound();
            }
            ViewBag.AukcjaID = new SelectList(db.BAukcja, "ID", "Tytul", komentarzAukcja.AukcjaID);
            ViewBag.ProfilId = new SelectList(db.BProfil, "ID", "Login", komentarzAukcja.ProfilId);
            return View(komentarzAukcja);
        }

        // POST: KomentarzAukcja/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Tresc,AukcjaID,ProfilId,Date")] KomentarzAukcja komentarzAukcja)
        {
            if (ModelState.IsValid)
            {
                db.Entry(komentarzAukcja).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AukcjaID = new SelectList(db.BAukcja, "ID", "Tytul", komentarzAukcja.AukcjaID);
            ViewBag.ProfilId = new SelectList(db.BProfil, "ID", "Login", komentarzAukcja.ProfilId);
            return View(komentarzAukcja);
        }

        // GET: KomentarzAukcja/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KomentarzAukcja komentarzAukcja = db.BKomentarzAukcja.Find(id);
            if (komentarzAukcja == null)
            {
                return HttpNotFound();
            }
            
            db.BKomentarzAukcja.Remove(komentarzAukcja);
            db.SaveChanges();
            return RedirectToAction("Details", "Aukcja", new { id = komentarzAukcja.AukcjaID });



           
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
