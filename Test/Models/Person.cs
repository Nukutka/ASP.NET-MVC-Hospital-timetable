using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test.Models
{
    /// <summary>
    /// Модель человека
    /// </summary>
    public abstract class Person
    {
        public int Id { get; set; } // ID для БД
        public string FirstName { get; set; } // Имя
        public string LastName { get; set; }  // Фамилия
        public string Patronymic { get; set; }// Отчество

        public Person() { }

        public Person(string firstName, string lastName, string patronymic)
        {
            FirstName = firstName;
            LastName = lastName;
            Patronymic = patronymic;
        }
    }
}