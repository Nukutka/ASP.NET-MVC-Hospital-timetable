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
        private IndexParamsModel indexModel; // Для передачи в представление Index

        public HomeController()
        {
            using (var dc = new DoctorContext())
            {
                var doctors = dc.Doctors.ToList();
                var cells = dc.Cells
                    .Include(x => x.TimeTable)
                    .ToList();

                indexModel = new IndexParamsModel
                {
                    Doctors = doctors,
                    SelectedDoctors = new List<Doctor>(),
                    SelectedDate = new DateTime(0)
                };
            }
        }

        /// <summary>
        /// Генерация представления Index
        /// </summary>
        public ActionResult Index()
        {
            ViewBag.DateNow = GetDateForCalendar(DateTime.Now);
            return View(indexModel);
        }

        /// <summary>
        /// Переход на форму подтверждения записи
        /// </summary>
        /// <param name="docId">Id специалиста</param>
        /// <param name="cellId">Id выбранной ячейки</param>
        [HttpGet]
        public ActionResult Record(int docId, int cellId)
        {
            RecordParamsModel recordModel;

            using (var dc = new DoctorContext())
            {
                var doctors = dc.Doctors.ToList();
                var cells = dc.Cells
                    .Include(x => x.TimeTable)
                    .ToList();

                try
                {
                    recordModel = new RecordParamsModel
                    {
                        Doctor = doctors.First(x => x.Id == docId),
                        Cell = cells.First(x => x.Id == cellId)
                    };
                }
                catch
                {
                    ViewBag.DateNow = indexModel.SelectedDate;
                    return View("Index", indexModel);
                }
            }

            return View(recordModel);
        }

        /// <summary>
        /// Обновление ячейки в БД
        /// </summary>
        [HttpPost]
        public ActionResult Record()
        {
            using (var dc = new DoctorContext())
            {
                var doctors = dc.Doctors.ToList();
                var cells = dc.Cells
                    .Include(x => x.TimeTable)
                    .ToList();

                int cellId = int.Parse(Request.Form["CellId"]);
                int index = cells.FindIndex(x => x.Id == cellId);
                cells[index].IsEmpty = false;

                dc.SaveChanges();
            }
            ViewBag.DateNow = indexModel.SelectedDate;

            return View("Index", indexModel);
        }

        /// <summary>
        /// Обновление страницы в соответствии с выбранными специалистами
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SelectDoctor()
        {
            string selectedDate = Request.Form["Calendar"];
            IEnumerable<int> doctorsId = Request.Form["Doctor"]
                .Split(',')
                .Select(x => int.Parse(x)); // Id выбранных специалистов

            IEnumerable<Doctor> doctors = indexModel.Doctors;
            var selectedDoctors = new List<Doctor>();

            foreach(var i in doctorsId)  // Создание списка выбранных специалистов
            {
                var doc = doctors.First(x => x.Id == i);
                selectedDoctors.Add(doc);
            }

            indexModel.SelectedDoctors = selectedDoctors;
            indexModel.SelectedDate = DateTime.ParseExact(selectedDate, "yyyy-MM-dd", null);
            ViewBag.DateNow = selectedDate;

            return View("Index", indexModel);
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