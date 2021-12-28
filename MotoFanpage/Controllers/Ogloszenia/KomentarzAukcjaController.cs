using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MotoFanpage.DAL;
using MotoFanpage.Models;
using MotoFanpage.Models.Ogloszenia;
using MotoFanpage.Models.Ogólne;

namespace MotoFanpage.Controllers.Ogloszenia
{
    public class KomentarzAukcjaController : Controller
    {
        private FanpageContext db = new FanpageContext();

  
      

       

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
        public ActionResult Delete(int id)
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
            Profil profil = db.BProfil.FirstOrDefault(p => p.Email == User.Identity.Name);

            Logi logs = new Logi { 
                Date = DateTime.Now,
                IpAdress = GetIp(),
                IdElement = id,
                WhatElement = "KomentarzAukcja",
                Instruction = "Delete",
                IdUser = profil.ID };

            db.BKomentarzAukcja.Remove(komentarzAukcja);
            db.SaveChanges();
            return RedirectToAction("Details", "Aukcja", new { id = komentarzAukcja.AukcjaID });



           
        }

        protected string GetIp()
        {
            HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
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
