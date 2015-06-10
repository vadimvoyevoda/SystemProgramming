using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Person_Library
{
    public enum PersonMaritalStatus
    {
        Married,
        Single
    }

    public class Person
    {
        string              _name;
        string              _surname;
        int                 _age;
        PersonMaritalStatus _status = PersonMaritalStatus.Single;

        public Person(
            string name,
            string surname,
            int    age
            )
        {
            _name    = name;
            _surname = surname;
            _age     = age;
        }

        public void Print()
        {
            Console.WriteLine(
                "Person:\nName:{0}\nSurname:{1}\nAge:{2}\nMaritalStatus:{3}\n",
                _name, _surname, _age, _status
                );
        }
    }
}
