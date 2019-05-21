using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test.Models
{
    /// <summary>
    /// Модель специалиста
    /// </summary>
    public class Doctor : Person
    {
        public ICollection<TimeTable> TimeTables { get; set; } // Расписания на разные даты

        public Doctor() { }

        public Doctor(string firstName, string lastName, string patronymic, ICollection<TimeTable> timeTables) 
            : base(firstName,lastName,patronymic)
        {
            TimeTables = timeTables;
        }

        /// <summary>
        /// Запись в виде Фамилия И.О.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{LastName} {FirstName[0]}.{Patronymic[0]}.";
        }
    }
}