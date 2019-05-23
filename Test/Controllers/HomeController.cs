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
        private readonly CultureInfo culture;           // для парсинга дат
        private readonly DateTimeStyles dateTimeStyles;

        public HomeController()
        {
            culture = CultureInfo.CurrentCulture;
            dateTimeStyles = DateTimeStyles.NoCurrentDateDefault;
        }

        /// <summary>
        /// Генерация данных для представления Index
        /// </summary>
        public ActionResult Index()
        {
            IndexParamsModel indexModel;

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
                ViewBag.CalendarDate = GetDateForCalendar(DateTime.Now);
            }
            return View(indexModel);
        }

        /// <summary>
        /// Обновление страницы в соответствии с выбранными специалистами
        /// </summary>
        [HttpPost]
        public ActionResult SelectDoctor()
        {
            IndexParamsModel indexModel;

            using (var dc = new DoctorContext())
            {
                var doctors = dc.Doctors.ToList();
                var cells = dc.Cells
                    .Include(x => x.TimeTable)
                    .ToList();

                string tmpDate = Request.Form["Calendar"]; 
                string tmpIds = Request.Form["Doctor"];

                // Если выбрана дата и специлисты, готовим данные для представления
                if (DateTime.TryParseExact(tmpDate, "yyyy-MM-dd", culture, dateTimeStyles, out var selectedDate) && tmpIds != null)
                {
                    IEnumerable<int> doctorsId = tmpIds.Split(',')
                        .Select(x => int.Parse(x));

                    var selectedDoctors = new List<Doctor>();

                    foreach (var i in doctorsId)  // Создание списка выбранных специалистов
                    {
                        Doctor doc = doctors.First(x => x.Id == i);
                        selectedDoctors.Add(doc);
                    }

                    indexModel = new IndexParamsModel
                    {
                        Doctors = doctors,
                        SelectedDoctors = selectedDoctors,
                        SelectedDate = selectedDate
                    };
                    ViewBag.CalendarDate = tmpDate;
                }
                else // Иначе ничего не меняем
                {
                    indexModel = new IndexParamsModel
                    {
                        Doctors = doctors,
                        SelectedDoctors = new List<Doctor>(),
                        SelectedDate = new DateTime(0)
                    };
                    ViewBag.CalendarDate = tmpDate ?? GetDateForCalendar(DateTime.Now);
                }
            }

            return View("Index", indexModel);
        }

        /// <summary>
        /// Переход на форму подтверждения записи
        /// </summary>
        /// <param name="docId">Id специалиста</param>
        /// <param name="cellId">Id выбранной ячейки</param>
        [HttpGet]
        public ActionResult Record(int? docId, int? cellId, string selDate)
        {
            RecordParamsModel recordModel;
            IndexParamsModel indexModel;

            using (var dc = new DoctorContext())
            {
                var doctors = dc.Doctors.ToList();
                var cells = dc.Cells
                    .Include(x => x.TimeTable)
                    .ToList();

                if (docId != null && cellId != null && selDate != null) // Если все параметры введены, переходим на Record
                {
                    recordModel = new RecordParamsModel
                    {
                        Doctor = doctors.First(x => x.Id == docId),
                        Cell = cells.First(x => x.Id == cellId)
                    };
                    return View("Record", recordModel);
                }
                else // Иначе остаемся в Index
                {
                    indexModel = new IndexParamsModel
                    {
                        Doctors = doctors,
                        SelectedDoctors = new List<Doctor>(),
                        SelectedDate = DateTime.ParseExact(selDate, "yyyy-MM-dd", culture, dateTimeStyles)
                    };
                    ViewBag.CalendarDate = selDate;
                    return View("Index", indexModel);
                }
            }
        }

        /// <summary>
        /// Обновление ячейки в БД
        /// </summary>
        [HttpPost]
        public ActionResult Record()
        {
            IndexParamsModel indexModel;

            using (var dc = new DoctorContext())
            {
                var doctors = dc.Doctors.ToList();
                var cells = dc.Cells
                    .Include(x => x.TimeTable)
                    .ToList();

                string selDate = Request.Form["SelDate"];

                int cellId = int.Parse(Request.Form["CellId"]);
                int index = cells.FindIndex(x => x.Id == cellId);
                cells[index].IsEmpty = false;       // Занимаем ячейку

                dc.SaveChanges();

                indexModel = new IndexParamsModel
                {
                    Doctors = doctors,
                    SelectedDoctors = new List<Doctor>(),
                    SelectedDate = DateTime.ParseExact(selDate, "dd.MM.yyyy", culture, dateTimeStyles)
                };

            }
            ViewBag.CalendarDate = GetDateForCalendar(indexModel.SelectedDate);

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