using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MotoFanpage.Controllers.Fanpage;
using MotoFanpage.Controllers.Ogólne;
using MotoFanpage.DAL;
using MotoFanpage.Models.Fanpage;
using MotoFanpage.Models.Ogólne;
using System;
using System.Data.Entity;
using System.Web.Mvc;

namespace TestyJednostkowe
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public  void TestHomeError()
        {

            var controller = new HomeController();
            var result = controller.Index(1) as ViewResult;

            var bag = result.ViewData.Keys;

            Assert.AreEqual(1, bag.Count);
        }

        
        [TestMethod]
        public void TestDetails()
        {
            

          
            var controller = new PostController();
            var result = controller.GetIp();

            var pojazd = result;

            Assert.AreEqual("Volvo", pojazd);
        }
        

    }
}
