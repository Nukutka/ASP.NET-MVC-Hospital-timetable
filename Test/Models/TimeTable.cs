using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Test.Models
{
    /// <summary>
    /// Модель расписания специалиста на один день
    /// </summary>
    public class TimeTable
    {
        public int Id { get; set; }             // ID для БД
        public DateTime Date { get; set; }      // Дата
        public ICollection<Cell> Cells { get; set; }   // Список ячеек

        [ForeignKey("Doctor")]
        public int DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; }

        public string ShortDate => Date.ToShortDateString();

        public TimeTable() { }
        public TimeTable(DateTime date, ICollection<Cell> cells)
        {
            Date = date;
            Cells = cells;
        }

        public override string ToString()
        {
            return Id.ToString();
        }
    }
}