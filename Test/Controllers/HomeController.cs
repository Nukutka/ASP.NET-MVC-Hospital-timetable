using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test.Models;

namespace Test.Controllers
{
    public class HomeController : Controller
    {
        private IndexParamsModel model; // Для передачи в представление
        private DoctorContext dc;       // Для получения данных из БД

        public HomeController()
        {
            dc = new DoctorContext();

            var doctors = dc.Doctors.ToList();
            var timeTables = dc.TimeTables.Include(x => x.Doctor).ToList(); // Костыль для Linq lazy loading
            var cells = dc.Cells.Include(x => x.TimeTable).ToList();

            model = new IndexParamsModel
            {
                Doctors = doctors,
                SelectedDoctors = new List<Doctor>(),
                SelectedDate = new DateTime(0)
            };
        }

        /// <summary>
        /// Генерация представления Index
        /// </summary>
        public ActionResult Index()
        {
            ViewBag.DateNow = GetDateForCalendar(DateTime.Now);
            return View(model);
        }

        /// <summary>
        /// Обновление страницы в соответствии с выбранными специалистами
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SelectDoctor()
        {
            IEnumerable<int> doctorsId = Request.Form["Doctor"]
                .Split(',')
                .Select(x => int.Parse(x)); // Id выбранных специалистов

            IEnumerable<Doctor> doctors = model.Doctors;
            var selectedDoctors = new List<Doctor>();

            foreach(var i in doctorsId)  // Создание списка выбранных специалистов
            {
                var doc = doctors.First(x => x.Id == i);
                selectedDoctors.Add(doc);
            }

            model.SelectedDoctors = selectedDoctors;
            model.SelectedDate = DateTime.Now;  // Заглушка на выбор даты
            ViewBag.DateNow = GetDateForCalendar(DateTime.Now);

            return View("Index", model);
        }

        /// <summary>
        /// Получение даты в виде гггг-мм-дд
        /// </summary>
        /// <param name="dateTime">Дата</param>
        private string GetDateForCalendar(DateTime dateTime)
        {
            return $"{dateTime.Year}-{dateTime.Month.ToString("D2")}-{dateTime.Day.ToString("D2")}";
        }
    }
}