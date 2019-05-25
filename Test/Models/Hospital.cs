using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test.Models
{
    /// <summary>
    /// Модель поликлиники
    /// </summary>
    public class Hospital
    {
        public int Id { get; set; }         // ID для БД
        public string Name { get; set; }    // Название поликлиники
        public ICollection<Doctor> Doctors { get; set; } // Список специалистов, работающих в поликлинике

        public Hospital() { }

        public Hospital(string name, ICollection<Doctor> doctors)
        {
            Name = name;
            Doctors = doctors;
        }
    }
}