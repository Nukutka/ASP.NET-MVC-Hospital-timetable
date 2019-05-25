using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test.Models
{
    /// <summary>
    /// Модель записи пациента к специалисту
    /// </summary>
    public class Record
    {
        public int Id { get; set; }            // ID 
        public Doctor Doctor { get; set; }     // Специалист
        public Cell Cell { get; set; }         // Выбранная ячейка
    }
}