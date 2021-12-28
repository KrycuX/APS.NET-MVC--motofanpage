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
    public class WiadomoscPrivController : Controller
    {
        private FanpageContext db = new FanpageContext();

        // GET: WiadomoscPriv
        public ActionResult Index()
        {
            Profil profil = db.BProfil.FirstOrDefault(p => p.Email == User.Identity.Name);

            List<WiadomoscPriv> lista = db.BWiadomoscPriv.Where(p=>p.IdOdbierajacego==profil.ID || p.ProfilID==profil.ID).ToList();
            List<Profil> listaP = new List<Profil>();
            foreach(var item in lista)
            {
                ViewBag.Tresc = item.Tresc;
                ViewBag.Autor = item.Profil;

                if (item.ProfilID==profil.ID)
                { 
                       Profil profil2 = db.BProfil.FirstOrDefault(p => p.ID == item.IdOdbierajacego );
                   

                if(!listaP.Contains(profil2))
                { 
                       listaP.Add(profil2);
                }

                }else if(item.IdOdbierajacego==profil.ID)
                {
                    Profil profil2 = db.BProfil.FirstOrDefault(p => p.ID == item.ProfilID);


                    if (!listaP.Contains(profil2))
                    {
                        listaP.Add(profil2);
                    }
                }

               



            }
           

            return View(listaP);
        }
        public ActionResult KonwersacjaPW(int id)
        {
            Profil profilOdb = db.BProfil.FirstOrDefault(p => p.ID == id);
            Profil profilWys = db.BProfil.FirstOrDefault(p => p.Email==User.Identity.Name);


            List<WiadomoscPriv> lista = db.BWiadomoscPriv.Where(p => p.IdOdbierajacego == profilOdb.ID && p.ProfilID == profilWys.ID 
            || p.IdOdbierajacego==profilWys.ID && p.ProfilID==profilOdb.ID).ToList();

            ViewBag.ProfilOdb = profilOdb;
            ViewBag.ProfilWys = profilWys;

            return View(lista);
        }

        // GET: WiadomoscPriv/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WiadomoscPriv wiadomoscPriv = db.BWiadomoscPriv.Find(id);
            if (wiadomoscPriv == null)
            {
                return HttpNotFound();
            }
            return View(wiadomoscPriv);
        }

        // GET: WiadomoscPriv/Create
        public ActionResult Create()
        {
            ViewBag.ProfilID = new SelectList(db.BProfil, "ID", "Login");
            return View();
        }

        // POST: WiadomoscPriv/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int idOdbierajacego,string tresc)
        {
            Profil profil = db.BProfil.FirstOrDefault(p => p.Email == User.Identity.Name);
           

            WiadomoscPriv msg = new WiadomoscPriv();

            msg.IdOdbierajacego = idOdbierajacego;
            msg.Tresc = tresc;
            msg.ProfilID = profil.ID;


            if (ModelState.IsValid)
            {
                db.BWiadomoscPriv.Add(msg);

                db.BProfil.FirstOrDefault(p => p.Email == User.Identity.Name).LMsg.Add(msg);
              
                db.SaveChanges();
                return RedirectToAction("KonwersacjaPW",new {id=idOdbierajacego } );
            }

           
            return RedirectToAction("KonwersacjaPW",idOdbierajacego);
        }

        // GET: WiadomoscPriv/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WiadomoscPriv wiadomoscPriv = db.BWiadomoscPriv.Find(id);
            if (wiadomoscPriv == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProfilID = new SelectList(db.BProfil, "ID", "Login", wiadomoscPriv.ProfilID);
            return View(wiadomoscPriv);
        }

        // POST: WiadomoscPriv/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Tresc,IdOdbierajacego,ProfilID")] WiadomoscPriv wiadomoscPriv)
        {
            if (ModelState.IsValid)
            {
                db.Entry(wiadomoscPriv).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProfilID = new SelectList(db.BProfil, "ID", "Login", wiadomoscPriv.ProfilID);
            return View(wiadomoscPriv);
        }

        // GET: WiadomoscPriv/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WiadomoscPriv wiadomoscPriv = db.BWiadomoscPriv.Find(id);
            if (wiadomoscPriv == null)
            {
                return HttpNotFound();
            }
            return View(wiadomoscPriv);
        }

        // POST: WiadomoscPriv/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            WiadomoscPriv wiadomoscPriv = db.BWiadomoscPriv.Find(id);
            db.BWiadomoscPriv.Remove(wiadomoscPriv);
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
