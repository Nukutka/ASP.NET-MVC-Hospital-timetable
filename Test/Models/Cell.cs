using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test.Models
{
    /// <summary>
    /// Модель ячейки записи к специалисту
    /// </summary>
    public class Cell
    {
        public int Id { get; set; }
        public DateTime Time { get; set; } // Время записи
        public bool IsEmpty { get; set; }  // Свободна ли ячейка

        public Cell(DateTime time, bool isEmpty)
        {
            Time = time;
            IsEmpty = isEmpty;
        }
    }
}