using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Test.Models;

namespace Test.Utils
{
    /// <summary>
    /// Инициализатор таблиц БД
    /// </summary>
    public class DoctorDbInitializer : DropCreateDatabaseAlways<DataContext>
    {
        // Потом надо убрать
        private string[] firstNames = { "Александр", "Никита", "Носок", "Олеся", "Мария", "Оксана", "Дамир", "Евгений", "Яна", "Боромир", "Гундир", "Собак", "Никита", "Денис", "Лариса" };
        private string[] lastNames = { "Анкудинов", "Бертов", "Кусок", "Клеин", "Отова", "Копчикова", "Кленов", "Жеистов", "Черикова", "Вафлин", "Семейный", "Котов", "Крупцов", "Улеткин", "Черепанова" };
        private string[] patronymics = { "Антонович", "Санвич", "Засок", "Слеся", "Арваи", "Сергеевна", "Никитич", "Раисович", "Романовна", "Гендальфович", "Сергеевич", "Бобикович", "Витальевич", "Челиковач", "Мариновна" };
        private readonly Random random;

        public DoctorDbInitializer()
        {
            random = new Random();
        }

        /// <summary>
        /// Перезаписывает данные таблиц в БД
        /// </summary>
        protected override void Seed(DataContext dc)
        {
            var hospital = new Hospital("Городская поликлиника", new List<Doctor>());

            for (int i = 0; i < 15; i++)
            {
                ICollection<TimeTable> timeTables = GetRandomTimeTables(14, 10);
                var doctor = new Doctor
                {
                    FirstName = firstNames[i],
                    LastName = lastNames[i],
                    Patronymic = patronymics[i],
                    TimeTables = timeTables
                };

                hospital.Doctors.Add(doctor);
            }

            dc.Hospitals.Add(hospital);

            base.Seed(dc);
        }

        /// <summary>
        /// Возвращает случайное расписание
        /// </summary>
        /// <param name="days">Количество дней в расписании</param>
        /// <param name="length">Длина в минутах приема одного пациента</param>
        private List<TimeTable> GetRandomTimeTables(int days, int length)
        {
            var timeTables = new List<TimeTable>(days);

            for (int i = 0; i < days; i++)
            {
                var date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(i);
                DateTime time = date;
                var cells = new List<Cell>(random.Next(25, 36)); // случайное количество записей в день
                time = time.AddHours(random.Next(8, 13));       // случайное начало дня 8,9,10,11,12
                for (int j = 0; j < cells.Capacity; j++)
                {
                    cells.Add(new Cell(time, random.Next(0, 2) == 0)); // случайное заполнение занятых ячеек 
                    time += new TimeSpan(0, length, 0);
                }

                timeTables.Add(new TimeTable(date, cells));
            }

            return timeTables;
        }
    }
}