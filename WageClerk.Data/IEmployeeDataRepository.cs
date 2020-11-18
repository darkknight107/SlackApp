
using WageClerk.Data.Models;

namespace WageClerk.Data
{
    public interface IEmployeeDataRepository
    {
        public Employee GetByUserName(string username);
        public Employee Add(Employee employee);
        public void AddShift(Shift shift, Employee employee);
    }
}
