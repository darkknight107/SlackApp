using System;
using System.Collections.Generic;

namespace WageClerk.Data.Models
{
    public partial class Employee
    {
        public Employee()
        {
            Shifts = new HashSet<Shift>();
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public virtual ICollection<Shift> Shifts { get; set; }
    }
}
