using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test.Models
{
    /// <summary>
    /// Модель пациента, пока подумаю, нужна ли С:
    /// </summary>
    public class Patient : People
    {
        public Patient(string firstName, string lastName, string patronymic)
            : base(firstName, lastName, patronymic)
        {
        }
    }
}