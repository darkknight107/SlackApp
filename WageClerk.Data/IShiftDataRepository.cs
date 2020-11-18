using System;
using System.Collections.Generic;
using System.Text;
using WagesClerk.Models;

namespace WageClerk.Data.Models
{
    public interface IShiftDataRepository
    {
        public Shift GetShiftById(int id);
        public void Add(Shift shift);
        public string SetClockInTime(Employee employee);
        public Shift GetClockedInShift(Employee employee);
        public string SetClockOutTime(Shift shift, Employee employee);
        public string SetShiftRating(Employee employee, Rating rating);
        public string GetEmployeeMorale(Employee employee);
    }
}
