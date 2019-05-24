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
        public IEnumerable<Doctor> Doctors { get; set; }         // Все специалист для списка слева
        public IEnumerable<Doctor> SelectedDoctors { get; set; } // Выбранные специалисты для отображения их расписаний
        public DateTime SelectedDate { get; set; }               // Для расписаний выбранных специалистов
    }
}