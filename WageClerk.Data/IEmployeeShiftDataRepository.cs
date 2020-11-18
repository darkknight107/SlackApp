using WageClerk.Data.Models;

namespace WagesClerk.Data
{
    public interface IEmployeeShiftDataRepository
    {
        public string SetClockInTime(Employee employee);
        public Shift GetClockedInShift(Employee employee);
        public string SetClockOutTime();
        public string SetShiftRating();
        public int GetShiftRating();
    }
}
