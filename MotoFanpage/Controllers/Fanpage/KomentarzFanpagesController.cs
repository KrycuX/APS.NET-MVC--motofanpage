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
using MotoFanpage.Models.Fanpage;
using MotoFanpage.Models.Ogólne;

namespace MotoFanpage.Controllers.Fanpage
{
    public class KomentarzFanpagesController : Controller
    {
        private FanpageContext db = new FanpageContext();

        // GET: KomentarzFanpages
        public ActionResult Index()
        {
            var bKomentarzFanpage = db.BKomentarzFanpage.Include(k => k.Post).Include(k => k.Profil);
            return View(bKomentarzFanpage.ToList());
        }

        // GET: KomentarzFanpages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KomentarzFanpage komentarzFanpage = db.BKomentarzFanpage.Find(id);
            if (komentarzFanpage == null)
            {
                return HttpNotFound();
            }
            return View(komentarzFanpage);
        }

        // GET: KomentarzFanpages/Create
        public ActionResult Create()
        {
            ViewBag.PostId = new SelectList(db.BPost, "ID", "Tresc");
            ViewBag.ProfilId = new SelectList(db.BProfil, "ID", "Login");
            return View();
        }

        // POST: KomentarzFanpages/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int id, string content)
        {
            Profil profile = db.BProfil.Single(p => p.Email == User.Identity.Name);
            KomentarzFanpage comments = new KomentarzFanpage();
            comments.PostId = id;
            //Uzupelnij Profil
            comments.ProfilId = profile.ID;
            comments.Date = DateTime.Now;
            comments.Tresc = content;
            db.BKomentarzFanpage.Add(comments);

            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        // GET: KomentarzFanpages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KomentarzFanpage komentarzFanpage = db.BKomentarzFanpage.Find(id);
            if (komentarzFanpage == null)
            {
                return HttpNotFound();
            }
            ViewBag.PostId = new SelectList(db.BPost, "ID", "Tresc", komentarzFanpage.PostId);
            ViewBag.ProfilId = new SelectList(db.BProfil, "ID", "Login", komentarzFanpage.ProfilId);
            return View(komentarzFanpage);
        }

        // POST: KomentarzFanpages/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Tresc,Date,PostId,ProfilId")] KomentarzFanpage komentarzFanpage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(komentarzFanpage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PostId = new SelectList(db.BPost, "ID", "Tresc", komentarzFanpage.PostId);
            ViewBag.ProfilId = new SelectList(db.BProfil, "ID", "Login", komentarzFanpage.ProfilId);
            return View(komentarzFanpage);
        }

        // GET: KomentarzFanpages/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KomentarzFanpage komentarzFanpage = db.BKomentarzFanpage.Find(id);
            if (komentarzFanpage == null)
            {
                return HttpNotFound();
            }
            Profil profil = db.BProfil.FirstOrDefault(p => p.Email == User.Identity.Name);

            Logi logs = new Logi { Date = DateTime.Now, IpAdress = GetIp(), IdElement = id, WhatElement = "KomentarzPost", Instruction = "Delete", IdUser = profil.ID };

            db.BKomentarzFanpage.Remove(komentarzFanpage);
            db.BLogi.Add(logs);
            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        // POST: KomentarzFanpages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            KomentarzFanpage komentarzFanpage = db.BKomentarzFanpage.Find(id);
            db.BKomentarzFanpage.Remove(komentarzFanpage);
            db.SaveChanges();
            return RedirectToAction("Index");
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
