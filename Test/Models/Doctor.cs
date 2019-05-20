using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test.Models
{
    /// <summary>
    /// Модель специалиста
    /// </summary>
    public class Doctor : People
    {
        public List<TimeTable> TimeTables { get; set; } // Расписания на разные даты

        public Doctor() { }

        public Doctor(string firstName, string lastName, string patronymic, List<TimeTable> timeTables) 
            : base(firstName,lastName,patronymic)
        {
            TimeTables = timeTables;
        }

        public override string ToString()
        {
            return $"{LastName} {FirstName[0]}. {Patronymic[0]}.";
        }
    }
}