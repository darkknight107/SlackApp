using System;
using System.Collections.Generic;

namespace WageClerk.Data.Models
{
    public partial class Shift
    {
        public int Id { get; set; }
        public DateTime? ClockIn { get; set; }
        public DateTime? ClockOut { get; set; }
        public string ShiftRating { get; set; }
        public int EmployeeId { get; set; }
    }
}
