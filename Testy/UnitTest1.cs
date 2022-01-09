using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Web.Mvc;


namespace Testy
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            
                var controller = new MotoFanpage.Controllers.Ogólne.PojazdsController();
                var result = controller.Details(1) as ViewResult;
                Assert.AreEqual("Details", result.ViewName);
            
        }
    }
}
