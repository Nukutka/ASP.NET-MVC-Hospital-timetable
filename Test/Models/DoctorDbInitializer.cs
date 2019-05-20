using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Test.Models
{
    /// <summary>
    /// Инициализатор таблиц
    /// </summary>
    public class DoctorDbInitializer : DropCreateDatabaseAlways<DoctorContext>
    {
        private string[] firstNames =  { "Александр", "Никита", "Носок", "Олеся", "Мария" };
        private string[] lastNames =   { "Анкудинов", "Бертов", "Кусок", "Клеин", "Отова" };
        private string[] patronymics = { "Антонович", "Санвич", "Засок", "Слеся", "Арваи" };
        protected override void Seed(DoctorContext dc)
        {
            for (int i = 0; i < 5; i++)
            {
                var doctor = new Doctor($"{firstNames[i]}", $"{lastNames[i]}", $"{patronymics[i]}", GetRandomTimeTables(14));
                dc.Doctors.Add(doctor);
            }

            base.Seed(dc);
        }

        /// <summary>
        /// Возвращает случайное расписание
        /// </summary>
        /// <param name="days">Количество дней в расписании</param>
        private List<TimeTable> GetRandomTimeTables(int days)
        {
            var r = new Random();
            var timeTables = new List<TimeTable>(days);
            for (int i = 0; i < days; i++)
            {
                var date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(i);
                DateTime time = date;    
                var cells = new List<Cell>(r.Next(5, 26)); // случайное количество записей в день
                time = time.AddHours(r.Next(8, 13));       // случайное начало дня 8,9,10,11,12
                for (int j = 0; j < cells.Capacity; j++)
                {
                    var length = new TimeSpan(0, 10, 0); // длина приема одного пациента
                    cells.Add(new Cell(time, r.Next(0, 2) == 0)); // случайное заполнение занятых ячеек 
                    time += length;
                }

                timeTables.Add(new TimeTable(date, cells));
            }
            return timeTables;
        }
    }
}