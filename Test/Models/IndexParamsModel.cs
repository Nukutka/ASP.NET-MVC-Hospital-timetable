using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test.Models
{
    /// <summary>
    /// Данные для передачи из HomeController в Index
    /// </summary>
    public class IndexParamsModel
    {
        public IEnumerable<Doctor> Doctors { get; set; }
        public IEnumerable<Doctor> SelectedDoctors { get; set; }
        public DateTime SelectedDate { get; set; } // Для расписаний выбранных специалистов
    }
}