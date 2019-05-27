using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test.Models;

namespace Test.Controllers
{
    public class HomeController : Controller
    {
        public HomeController() { }

        /// <summary>
        /// Генерация данных для представления Index
        /// </summary>
        public ActionResult Index()
        {
            Hospital hospital;

            using (var dc = new DataContext())
            {
                hospital = dc.Hospitals
                    .Include(x => x.Doctors)
                    .First();
            }

            ViewBag.CalendarDate = GetDateForCalendar(DateTime.Now);

            return View(hospital);
        }

        /// <summary>
        /// Передача в Index выбранных id специалистов
        /// </summary>
        [HttpGet]
        public ActionResult InputIds()
        {
            Hospital hospital;

            using (var dc = new DataContext())
            {
                hospital = dc.Hospitals
                    .Include(x => x.Doctors)
                    .First();
            }

            ViewBag.CalendarDate = Request.Params["date"] ?? GetDateForCalendar(DateTime.Now);
            ViewBag.Indexes = new int[] { };

            if (Request.Params["doctor"] != null)
            {
                ViewBag.Indexes = Request.Params["doctor"]
                    .Split(',')
                    .Select(x => int.Parse(x))
                    .ToArray();
            }

            return View("Index", hospital);
        }

        /// <summary>
        /// Выводит расписания специалистов через частичное представление
        /// </summary>
        /// <param name="doctorIds">Идентификаторы специалистов</param>
        /// <param name="date">Дата</param>
        public ActionResult ShowTimeTables(int[] doctorIds, DateTime? date)
        {
            int[] docIds = doctorIds ?? (int[])Session["doctor-ids"]; // После перехода с Record возвращаем выбранных специалистов

            List<TimeTable> timeTables = new List<TimeTable>();

            if (docIds != null && date.HasValue)
            {
                Session["doctor-ids"] = docIds;

                foreach (var id in docIds)
                {
                    timeTables.Add(LoadTimeTable(id, date.Value));
                }
            }

            return PartialView("TTBlock", timeTables);
        }

        /// <summary>
        /// Загружает из БД расписание конкретного специалиста на определенную дату
        /// </summary>
        /// <param name="doctorId">Идентификатор специалиста</param>
        /// <param name="date">Дата</param>
        private TimeTable LoadTimeTable(int doctorId, DateTime date)
        {
            TimeTable timeTable;

            using (var dc = new DataContext())
            {
                timeTable = dc.TimeTables
                   .Include(x => x.Cells)
                   .Include(x => x.Doctor)
                   .Where(x => x.DoctorId == doctorId)
                   .Where(x => x.Date == date)
                   .ToList()
                   .FirstOrDefault();
            }

            return timeTable;
        }

        /// <summary>
        /// Переход на форму подтверждения записи
        /// </summary>
        /// <param name="doctorId">Id специалиста</param>
        /// <param name="cellId">Id выбранной ячейки</param>
        /// <param name="date">Выбранная дата</param>
        [HttpGet]
        public ActionResult Record(int? doctorId, int? cellId, string date)
        {
            Record record;
            Hospital hospital;

            if (doctorId != null && cellId != null && date != null)  // Если все параметры введены, переходим на Record
            {
                using (var dc = new DataContext())
                {
                    var doctor = dc.Doctors
                        .FirstOrDefault(x => x.Id == doctorId);
                    var cell = dc.Cells
                        .Include(x => x.TimeTable)
                        .FirstOrDefault(x => x.Id == cellId);

                    record = new Record
                    {
                        Doctor = doctor,
                        Cell = cell
                    };
                }

                return View(record);
            }

            else // Иначе остаемся в Index
            {
                using (var dc = new DataContext())
                {
                    hospital = dc.Hospitals
                        .Include(x => x.Doctors)
                        .First();
                }
                ViewBag.CalendarDate = date ?? GetDateForCalendar(DateTime.Now);

                return View("Index", hospital);
            }

        }

        /// <summary>
        /// Обновление ячейки в БД
        /// </summary>
        [HttpPost]
        public ActionResult Record()
        {
            Hospital hospital;

            using (var dc = new DataContext())
            {
                var cells = dc.Cells
                    .Include(x => x.TimeTable)
                    .ToList();

                int cellId = int.Parse(Request.Form["cell-id"]);
                int index = cells.FindIndex(x => x.Id == cellId);
                cells[index].IsEmpty = false;       // Занимаем ячейку

                dc.SaveChanges();

                hospital = dc.Hospitals
                    .Include(x => x.Doctors)
                    .FirstOrDefault();
            }

            ViewBag.CalendarDate = GetDateForCalendar(DateTime.Parse(Request.Form["sel-date"]));
            ViewBag.Indexes = (int[])Session["doctor-ids"];

            return View("Index", hospital);
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