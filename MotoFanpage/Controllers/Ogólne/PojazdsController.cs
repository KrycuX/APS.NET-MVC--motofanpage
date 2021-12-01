using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MotoFanpage.DAL;
using MotoFanpage.Models.Ogólne;

namespace MotoFanpage.Controllers.Ogólne
{
    public class PojazdsController : Controller
    {
        private FanpageContext db = new FanpageContext();

        // GET: Pojazds
        public ActionResult Index()
        {
            var bPojazd = db.BPojazd.Include(p => p.Profil);
            return View(bPojazd.ToList());
        }

        // GET: Pojazds/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pojazd pojazd = db.BPojazd.Find(id);
            if (pojazd == null)
            {
                return HttpNotFound();
            }
            return View(pojazd);
        }

        // GET: Pojazds/Create
        public ActionResult Create()
        {
            ViewBag.ProfilID = new SelectList(db.BProfil, "ID", "Login");
            return View();
        }

        // POST: Pojazds/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Marka,Model,Przebieg,RokProdukcji,MOC,TypeP,AuctionId,ProfilID")] Pojazd pojazd)
        {
            if (ModelState.IsValid)
            {
                var profil = db.BProfil.FirstOrDefault(p => p.Email == User.Identity.Name);
                pojazd.ProfilID = profil.ID;

                db.BPojazd.Add(pojazd);
                db.SaveChanges();
                return RedirectToAction("Details","Profil");
            }

            ViewBag.ProfilID = new SelectList(db.BProfil, "ID", "Login", pojazd.ProfilID);
            return View(pojazd);
        }

        // GET: Pojazds/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pojazd pojazd = db.BPojazd.Find(id);
            if (pojazd == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProfilID = new SelectList(db.BProfil, "ID", "Login", pojazd.ProfilID);
            return View(pojazd);
        }

        // POST: Pojazds/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Marka,Model,Przebieg,RokProdukcji,MOC,TypeP,AuctionId,ProfilID")] Pojazd pojazd)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pojazd).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProfilID = new SelectList(db.BProfil, "ID", "Login", pojazd.ProfilID);
            return View(pojazd);
        }

        // GET: Pojazds/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pojazd pojazd = db.BPojazd.Find(id);
            if (pojazd == null)
            {
                return HttpNotFound();
            }
            return View(pojazd);
        }

        // POST: Pojazds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pojazd pojazd = db.BPojazd.Find(id);
            db.BPojazd.Remove(pojazd);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult UserPojazd(int? id)
        {
            Profil profil = db.BProfil.Single(p => p.Email == User.Identity.Name);
            var vehicles = db.BPojazd.Where(p => p.ProfilID == profil.ID);

            return View(vehicles.ToList());
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
