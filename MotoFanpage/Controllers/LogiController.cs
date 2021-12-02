using MotoFanpage.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MotoFanpage.Controllers
{
    public class LogiController : Controller
    {
        FanpageContext db = new FanpageContext();

        // GET: Logi
        public ActionResult Index()
        {
            var list = db.BLogi.ToList();

            return View(list);
        }
    }
}