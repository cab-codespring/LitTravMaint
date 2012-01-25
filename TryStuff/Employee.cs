using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TryStuff
{
    public class Employee
    {
        public Employee(string fname, string lname, int id)
        {
            FirstName = fname;
            LastName = lname;
            EmployeeID = id;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int EmployeeID { get; set; }

        public override string ToString()
        {
            return FirstName + " " + LastName;
        }
    }
}
