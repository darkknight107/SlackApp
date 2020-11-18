using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WagesClerk.Models;

namespace WagesClerk.Data
{
    public class MockTimesheetDataRepository : ITimeSheetDataRepository
    {
        private readonly IEmployeeDataRepository _employeeData;
        public MockTimesheetDataRepository(IEmployeeDataRepository employeeData)
        {
            _employeeData = employeeData;
        }
        private IEnumerable<Shift> shifts = new List<Shift>
        {
            new Shift
            {
                Id = 1,
                ClockIn = Convert.ToDateTime("19/08/2020", System.Globalization.CultureInfo.GetCultureInfo("en-AU").DateTimeFormat),
                ClockOut = Convert.ToDateTime("20/08/2020", System.Globalization.CultureInfo.GetCultureInfo("en-AU").DateTimeFormat),
                ShiftRating = ShiftRating.Excellent
            }

        };
                
        public int GetShiftRating()
        {
            return (int)shifts.First().ShiftRating;
        }

        public string SetClockInTime(Employee employee)
        {
            DateTime clockInTime = DateTime.Now;
            Shift newShift = new Shift
            {
                ClockIn = clockInTime,
                Id = 22
            };
            employee.Shifts.Append(newShift);
            _employeeData.AddEmployeeShift(newShift, employee);
            return "Welcome " + employee.Username + ". You have successfully clocked in at " + clockInTime;
        }

        public string SetClockOutTime()
        {
            throw new NotImplementedException();
        }

        public string SetShiftRating()
        {
            throw new NotImplementedException();
        }

        public Shift GetClockedInShift(Employee employee)
        {
            return employee.Shifts.FirstOrDefault(shift =>
                shift.ClockIn.HasValue && !(shift.ClockOut.HasValue));
        }

    }
}
