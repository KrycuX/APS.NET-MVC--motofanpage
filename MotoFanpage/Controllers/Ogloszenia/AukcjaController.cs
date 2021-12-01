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
using PagedList;

namespace MotoFanpage.Controllers.Ogloszenia
{
    public class AukcjaController : Controller
    {
        private FanpageContext db = new FanpageContext();

        // GET: Aukcja
        public ActionResult Index(string sortOrder, string currentFilter, string currentVehFilter, string searchString, string vehicleTypes, int? page)
        {
            ViewBag.CurrentSort = sortOrder;

            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.VehSortParm = String.IsNullOrEmpty(sortOrder) ? "veh_desc" : "veh_asc";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            if (vehicleTypes != null)
            {
                page = 1;
            }
            else
            {
                vehicleTypes = currentVehFilter;
            }

            ViewBag.CurrentFilter = searchString;
            ViewBag.CurrentVehFilter = vehicleTypes;

            var auctions = db.BAukcja.Include(a => a.Profil).Include(a => a.Pojazd);
            if (!String.IsNullOrEmpty(searchString))
            {
                auctions = auctions.Where(a => a.Tytul.Contains(searchString));
            }

            if (!String.IsNullOrEmpty(vehicleTypes))
            {
                auctions = auctions.Where(a => a.Pojazd.TypeP.ToString() == vehicleTypes);
            }

            switch (sortOrder)
            {
                case "name_desc":
                    auctions = auctions.OrderByDescending(s => s.Tytul);
                    break;
                case "veh_desc":
                    auctions = auctions.OrderByDescending(s => s.Pojazd.Marka);
                    break;
                case "veh_asc":
                    auctions = auctions.OrderBy(s => s.Pojazd.Marka);
                    break;
                default:
                    auctions = auctions.OrderBy(s => s.Tytul);
                    break;


            }
            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem { Text = "Wszystko", Value = "" });
            items.Add(new SelectListItem { Text = "Samochód", Value = "Samochód" });
            items.Add(new SelectListItem { Text = "Motocykl", Value = "Motocykl" });



            ViewBag.VehicleTypes = items;
            
           Profil profil = db.BProfil.FirstOrDefault(p => p.Email == User.Identity.Name);

            if (profil != null)
            {
                ViewBag.Profil = profil;

            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);

            return View(auctions.ToPagedList(pageNumber, pageSize));
        }
    
        // GET: Aukcja/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Aukcja auction = db.BAukcja.Find(id);

            Profil profil = db.BProfil.Find(auction.ProfilID);

            List<KomentarzAukcja> comments = db.BKomentarzAukcja.Where(c => c.AukcjaID == auction.ID).ToList();

            if (auction == null)
            {
                return HttpNotFound();
            }


            return View(auction);
        }

        // GET: Aukcja/Create
        public ActionResult Create()
        {
            Profil profil = db.BProfil.Single(p => p.Email == User.Identity.Name);
            if (profil.Login == null)
            {

                return RedirectToAction("Edit", "Profiles", new { id = profil.ID });
            }

            if(profil.LPojazdy.Count==0)
            {
                ViewBag.Info = "Nie masz przypisanego pojazdu do profilu! Dodaj go";
                return RedirectToAction("Create", "Pojazds");
            }

            
            var lista = new List<Profil>();
            lista.Add(profil);

            var pojazdy = db.BPojazd.Where(p => p.AukcjaID == 0 && p.Profil.Email == User.Identity.Name);
            ViewBag.ProfileID = new SelectList(lista, "ID", "Login");

            ViewBag.PojazdID = new SelectList(pojazdy, "ID", "Marka");
            return View();
        }

        // POST: Aukcja/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Tytul,Opis,Obrazek,PojazdID,ProfilID")] Aukcja aukcja)
        {
            var auctionM = db.BAukcja.FirstOrDefault(p => p.Tytul == aukcja.Tytul);
            Profil profil = db.BProfil.Single(p => p.Email == User.Identity.Name);
            if (!ModelState.IsValid)
            {
                return Error(aukcja);
            }

            if (auctionM != null)
            {
                ViewBag.Error = "Auction Tittle already in use ";
                return Error(aukcja);
            }
            HttpPostedFileBase file = Request.Files["plikZObrazkiem"];
            
            if (file != null && file.ContentLength > 0)
            {
                aukcja.Obrazek = file.FileName;
                file.SaveAs(HttpContext.Server.MapPath("~/Obrazki/") + aukcja.Obrazek);
            }

            aukcja.ProfilID = profil.ID;
            db.BAukcja.Add(aukcja);
            db.SaveChanges();
           
            db.BPojazd.Find(aukcja.PojazdID).AukcjaID = aukcja.ID;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        private ActionResult Error(Aukcja aukcja)
        {
            ViewBag.ProfileId = new SelectList(db.BProfil, "ID", "Login", aukcja.ProfilID);
            ViewBag.VehicleId = new SelectList(db.BPojazd, "ID", "Marka", aukcja.PojazdID);

            return View(aukcja);
        }
        // GET: Aukcja/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Aukcja aukcja = db.BAukcja.Find(id);
            if (aukcja == null)
            {
                return HttpNotFound();
            }
            ViewBag.PojazdID = new SelectList(db.BPojazd, "ID", "Marka", aukcja.PojazdID);
            ViewBag.ProfilID = new SelectList(db.BProfil, "ID", "Login", aukcja.ProfilID);
            return View(aukcja);
        }

        // POST: Aukcja/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Tytul,Opis,Obrazek,PojazdID,ProfilID")] Aukcja aukcja)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aukcja).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PojazdID = new SelectList(db.BPojazd, "ID", "Marka", aukcja.PojazdID);
            ViewBag.ProfilID = new SelectList(db.BProfil, "ID", "Login", aukcja.ProfilID);
            return View(aukcja);
        }

        // GET: Aukcja/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Aukcja aukcja = db.BAukcja.Find(id);
            if (aukcja == null)
            {
                return HttpNotFound();
            }
            return View(aukcja);
        }

        // POST: Aukcja/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Aukcja aukcja = db.BAukcja.Find(id);
            db.BAukcja.Remove(aukcja);
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
