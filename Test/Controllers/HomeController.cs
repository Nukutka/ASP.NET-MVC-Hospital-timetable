using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test.Models;

namespace Test.Controllers
{
    public class HomeController : Controller
    {
        private DoctorContext dc;

        public HomeController()
        {
            dc = new DoctorContext();
        }

        public ActionResult Index()
        {
            IEnumerable<Doctor> doctors = dc.Doctors; // все специалисты из бд
            ViewBag.Doctors = doctors; // передача в представление

            DateTime tmp = DateTime.Now;
            string dateNow = $"{tmp.Year}-{tmp.Month.ToString("D2")}-{tmp.Day.ToString("D2")}";
            tmp += new TimeSpan(14, 0, 0, 0);
            string dateMax = $"{tmp.Year}-{tmp.Month.ToString("D2")}-{tmp.Day.ToString("D2")}";
            ViewBag.DateNow = dateNow;
            ViewBag.DateMax = dateMax;

            return View();
        }

        [HttpGet]
        public ActionResult SelectDoctor(int id)
        {
            ViewBag.DoctorId = id;
            return View();
        }

        [HttpPost]
        public string SelectDoctor()
        {
            //purchase.Date = DateTime.Now;
            //db.Purchases.Add(purchase);
            //db.SaveChanges();
            var x = Request.Form["doctor"];
            return "Успешно";
        }
    }
}