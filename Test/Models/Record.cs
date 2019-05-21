using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test.Models
{
    /// <summary>
    /// Модель записи пациента к специалисту, пока подумаю, нужна ли С:
    /// </summary>
    public class Record
    {
        public int Id { get; set; }            // ID для БД
        public Patient Patient { get; set; }   // Пациент
        public Doctor Doctor { get; set; }     // Специалист
        public DateTime DateTime { get; set; } // Время записи
    }
}