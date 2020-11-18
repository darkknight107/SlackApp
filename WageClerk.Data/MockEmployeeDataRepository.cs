using System;
using System.Collections.Generic;
using System.Linq;
using WageClerk.Data;
using WageClerk.Data.Models;

namespace WagesClerk.Data
{
    public class MockEmployeeDataRepository:IEmployeeDataRepository
    {
        private IEnumerable<Employee> _employees;
        private Employee _employee;

        public MockEmployeeDataRepository()
        {
            _employees = new List<Employee>
            {
                new Employee
                {
                    Id = 1,
                    Username = "shirish",
                    Shifts = new List<Shift>
                    {
                        new Shift
                        {
                            Id = 55555,
                            ClockIn = DateTime.Now,
                            ClockOut = DateTime.Now
                        }
                    }
                },
                new Employee
                {
                    Id = 2,
                    Username = "notsocoolbro"
                }
            };
        }

        public Employee GetEmployee(string username)
        {
           
            return _employees.FirstOrDefault(uName => uName.Username == username);
        }

        public void SetEmployee(Employee employee)
        {
            Employee newEmployee = new Employee
            {
                Id = employee.Id,
                Username = employee.Username
            };
            _employees.Append(newEmployee);
        }

        public void AddEmployeeShift(Shift shift, Employee employee)
        {
            if (_employees.Contains(employee))
            {
                employee.Shifts.Append(shift);
            }
        }

   

    }
}
