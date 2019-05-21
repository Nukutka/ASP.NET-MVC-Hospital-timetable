using System;
using System.Collections.Generic;
using System.Web;
using System.Data.Entity;

namespace Test.Models
{
    /// <summary>
    /// Контекст данных для БД 
    /// </summary>
    public class DoctorContext : DbContext
    {
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<TimeTable> TimeTables { get; set; }
        public DbSet<Cell> Cells { get; set; }
    }
}