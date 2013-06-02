using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace ManagerPanel
{
    public class Project
    {
        public String Name { get; set; }
        protected ObservableCollection<Employee> employees = new ObservableCollection<Employee>();

        public ObservableCollection<Employee> Employees
        {
            get { return employees; }
            set { employees = value; }

        }

        public Project(String name)
        {
            this.Name = name;
            employees.Add(new Employee("Michael Anderbergson",
        "13", name));
            employees.Add(new Employee("Chris Ashton",
                    "12", "12.12.12")); 
            employees.Add(new Employee("Cassie Hicks",
                    "66", " "));
            employees.Add(new Employee("Guido Pica",
                    "32", "07.07.12"));
        }

    }
}
