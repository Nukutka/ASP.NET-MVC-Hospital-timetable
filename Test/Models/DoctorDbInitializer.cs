using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Test.Models
{
    /// <summary>
    /// Инициализатор таблиц БД
    /// </summary>
    public class DoctorDbInitializer : DropCreateDatabaseAlways<DoctorContext>
    {
        // Потом надо убрать
        private string[] firstNames =  { "Александр", "Никита", "Носок", "Олеся", "Мария" };
        private string[] lastNames =   { "Анкудинов", "Бертов", "Кусок", "Клеин", "Отова" };
        private string[] patronymics = { "Антонович", "Санвич", "Засок", "Слеся", "Арваи" };

        /// <summary>
        /// Перезаписывает данные таблиц в БД
        /// </summary>
        /// <param name="dc"></param>
        protected override void Seed(DoctorContext dc)
        {
            for (int i = 0; i < 5; i++)
            {
                ICollection<TimeTable> timeTables = GetRandomTimeTables(14, 10);
                var doctor = new Doctor
                {
                    FirstName = firstNames[i],
                    LastName = lastNames[i],
                    Patronymic = patronymics[i],
                    TimeTables = timeTables
                };
                dc.Doctors.Add(doctor);
            }

            base.Seed(dc);
        }

        /// <summary>
        /// Возвращает случайное расписание
        /// </summary>
        /// <param name="days">Количество дней в расписании</param>
        /// <param name="length">Длина в минутах приема одного пациента</param>
        private List<TimeTable> GetRandomTimeTables(int days, int length)
        {
            var r = new Random();
            var timeTables = new List<TimeTable>(days);
            for (int i = 0; i < days; i++)
            {
                var date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(i);
                DateTime time = date;
                var cells = new List<Cell>(r.Next(25, 36)); // случайное количество записей в день
                time = time.AddHours(r.Next(8, 13));       // случайное начало дня 8,9,10,11,12
                for (int j = 0; j < cells.Capacity; j++)
                {
                    cells.Add(new Cell(time, r.Next(0, 2) == 0)); // случайное заполнение занятых ячеек 
                    time += new TimeSpan(0, length, 0);
                }

                timeTables.Add(new TimeTable(date, cells));
            }
            return timeTables;
        }
    }
}