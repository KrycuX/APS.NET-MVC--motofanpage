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
        public ActionResult Create(string tresc)
        {
            Profil profile = db.BProfil.Single(p => p.Email == User.Identity.Name);
            Post posty = new Post();
          
            //Uzupelnij Profil
            posty.ProfilID = profile.ID;
            posty.Date = DateTime.Now;
            posty.Tresc = tresc;

            HttpPostedFileBase file = Request.Files["ObrazFile"];

            if (file!=null && file.ContentLength > 0)
            {
                posty.Obraz = file.FileName;
                file.SaveAs(HttpContext.Server.MapPath("~/Obrazki/") + posty.Obraz);
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
        public ActionResult Delete(int? id)
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

            db.BPost.Remove(post);
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
