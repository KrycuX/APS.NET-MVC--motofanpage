﻿using System;
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
    public class PostController : Controller
    {
        private FanpageContext db = new FanpageContext();

        // GET: Post
        public ActionResult Index()
        {
            var bPost = db.BPost.Include(p => p.Profil);
            return View(bPost.ToList());
        }

        // GET: Post/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.BPost.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Post/Create
        public ActionResult Create()
        {
            ViewBag.ProfilID = new SelectList(db.BProfil, "ID", "Login");
            return View();
        }

        // POST: Post/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string tresc,IEnumerable<HttpPostedFileBase> myFiles)
        {
            Profil profile = db.BProfil.Single(p => p.Email == User.Identity.Name);
            Post posty = new Post();
          
            
            posty.ProfilID = profile.ID;
            posty.Date = DateTime.Now;
            posty.Tresc = tresc;

            posty.Obraz = new List<Obraz>();






                foreach (var file in myFiles)
                {

                    if (file != null && file.ContentLength > 0)
                    {
                        Obraz obraz = new Obraz { Name = file.FileName };
                        db.BObraz.Add(obraz);
                        posty.Obraz.Add(obraz);
                        file.SaveAs(HttpContext.Server.MapPath("~/Obrazki/") + file.FileName);
                    }

                }

           

            db.BPost.Add(posty);
            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        // GET: Post/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.BPost.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProfilID = new SelectList(db.BProfil, "ID", "Login", post.ProfilID);
            return View(post);
        }

        // POST: Post/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ProfilID,Date,Tresc")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProfilID = new SelectList(db.BProfil, "ID", "Login", post.ProfilID);
            return View(post);
        }

        // GET: Post/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Post post = db.BPost.Find(id);

            if (post == null)
            {
                return HttpNotFound();
            }

            Profil profil = db.BProfil.FirstOrDefault(p=>p.Email==User.Identity.Name);

            db.BObraz.RemoveRange(post.Obraz);

            Logi logs = new Logi {Date=DateTime.Now,IpAdress=GetIp(),IdElement=id,WhatElement="Post",Instruction="Delete",IdUser=profil.ID};


            db.BPost.Remove(post);

            db.BLogi.Add(logs);

            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        // POST: Post/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.BPost.Find(id);
            db.BPost.Remove(post);
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
