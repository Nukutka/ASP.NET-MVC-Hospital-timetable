using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test.Models
{
    /// <summary>
    /// Модель расписания специалиста на один день
    /// </summary>
    public class TimeTable
    {
        public int Id { get; set; }             // Id для БД
        public DateTime Date { get; set; }      // Дата
        public List<Cell> Cells { get; set; }   // Список ячеек

        public TimeTable(DateTime date, List<Cell> cells)
        {
            Date = date;
            Cells = cells;
        }
    }
}