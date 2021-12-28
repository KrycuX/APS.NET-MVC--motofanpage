using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MotoFanpage.DAL;
using MotoFanpage.Models.Fanpage;
using MotoFanpage.Models.Ogólne;

namespace MotoFanpage.Controllers.Fanpage
{
    public class FriendReqController : Controller
    {
        private FanpageContext db = new FanpageContext();

        // GET: FriendReq
        public ActionResult Index()
        {
            Profil profil = db.BProfil.FirstOrDefault(p => p.Email == User.Identity.Name);
            ViewBag.ProfilID = profil.ID;
            List<Profil> lista = db.BProfil.ToList();

            ViewBag.Profil = lista;
        
            return View(db.BFriendReq.ToList());
        }

        // GET: FriendReq/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FriendReq friendReq = db.BFriendReq.Find(id);
            if (friendReq == null)
            {
                return HttpNotFound();
            }
            return View(friendReq);
        }

        [HttpPost]
       
        public ActionResult Akcept(int idOdbierajacego,int idWysylajacego)
        {
            db.BFriendReq.FirstOrDefault(p => p.IdOdbierajacego == idOdbierajacego && p.IdWysylajacego == idWysylajacego).Status=2;

            db.BProfil.FirstOrDefault(p => p.ID == idOdbierajacego);
            db.BProfil.FirstOrDefault(p => p.ID == idWysylajacego);

            Znajomy znajomy = new Znajomy { 
                IdZnajomego = idOdbierajacego,
                Imie = db.BProfil.FirstOrDefault(p => p.ID == idOdbierajacego).Name,
                Nazwisko = db.BProfil.FirstOrDefault(p => p.ID == idOdbierajacego).Surname
        };
            Znajomy znajomy2 = new Znajomy { IdZnajomego = idWysylajacego,
                Imie = db.BProfil.FirstOrDefault(p => p.ID == idWysylajacego).Name,
                Nazwisko = db.BProfil.FirstOrDefault(p => p.ID == idWysylajacego).Surname
            };



               db.BProfil.FirstOrDefault(p => p.ID == idOdbierajacego).LZnajomi.Add(znajomy2);
               db.BProfil.FirstOrDefault(p => p.ID == idWysylajacego).LZnajomi.Add(znajomy);

            db.BFriendReq.FirstOrDefault(p => p.IdOdbierajacego == idOdbierajacego && p.IdWysylajacego == idWysylajacego).Status=2;
            db.SaveChanges();

            return RedirectToAction("Index","Home");
        }
        [HttpPost]

        public ActionResult Odrzuc(int idOdbierajacego, int idWysylajacego)
        {
            db.BFriendReq.FirstOrDefault(p => p.IdOdbierajacego == idOdbierajacego && p.IdWysylajacego == idWysylajacego).Status = 2;

          



      

           var request=db.BFriendReq.FirstOrDefault(p => p.IdOdbierajacego == idOdbierajacego && p.IdWysylajacego == idWysylajacego);
            db.BFriendReq.Remove(request);
            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }



        // GET: FriendReq/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FriendReq/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string email,int idOdbierajacego)
        {
            Profil profil = db.BProfil.FirstOrDefault(p => p.Email == User.Identity.Name);

            FriendReq request = new FriendReq();

           

          
                request.IdWysylajacego = profil.ID;
                request.IdOdbierajacego = idOdbierajacego;
                request.Status = 1;
                // 1 = wysłany 2= Akceptowany 3= Odrzucone

                db.BFriendReq.Add(request);
                db.SaveChanges();
                return RedirectToAction("DetailsOther","Profil",new {id=idOdbierajacego });
          

           
        }

        // GET: FriendReq/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FriendReq friendReq = db.BFriendReq.Find(id);
            if (friendReq == null)
            {
                return HttpNotFound();
            }
            return View(friendReq);
        }

        // POST: FriendReq/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,idWysylajacego,idOdbierajacego,Status")] FriendReq friendReq)
        {
            if (ModelState.IsValid)
            {
                db.Entry(friendReq).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(friendReq);
        }

        // GET: FriendReq/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FriendReq friendReq = db.BFriendReq.Find(id);
            if (friendReq == null)
            {
                return HttpNotFound();
            }
            return View(friendReq);
        }

        // POST: FriendReq/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FriendReq friendReq = db.BFriendReq.Find(id);
            db.BFriendReq.Remove(friendReq);
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
