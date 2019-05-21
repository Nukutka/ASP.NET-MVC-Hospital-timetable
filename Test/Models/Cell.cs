using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Test.Models
{
    /// <summary>
    /// Модель ячейки записи к специалисту
    /// </summary>
    public class Cell
    {
        public int Id { get; set; }        // ID для БД
        public DateTime Time { get; set; } // Время записи
        public bool IsEmpty { get; set; }  // Свободна ли ячейка

        [ForeignKey("TimeTable")]
        public int TimeTableId { get; set; } 
        public virtual TimeTable TimeTable { get; set; }

        public Cell() { }

        public Cell(DateTime time, bool isEmpty)
        {
            Time = time;
            IsEmpty = isEmpty;
        }
    }
}