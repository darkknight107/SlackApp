using System;
using Arch.EntityFrameworkCore.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WageClerk.Data.Models;
using WagesClerk.Data;

namespace WageClerk.Data
{
    public class EmployeeShiftDataRepository : IEmployeeShiftDataRepository
    {
        private IRepository<Employee> _employeeRepo;
        //private IRepository<EmployeeShift> _employeeShiftRepo;
        private IRepository<Shift> _shift;
        private IUnitOfWork<InnovationDayContext> _unitOfWork;

        public EmployeeShiftDataRepository(IUnitOfWork<InnovationDayContext> unitOfWork)
        {
            _employeeRepo = unitOfWork.GetRepository<Employee>();
            _shift = unitOfWork.GetRepository<Shift>();
            _unitOfWork = unitOfWork;
        }

        public Shift GetClockedInShift(Employee employee)
        {
            return new Shift();
        }

        public int GetShiftRating()
        {
            throw new System.NotImplementedException();
        }

        public string SetClockInTime(Employee employee)
        {
            throw new System.NotImplementedException();
        }

        public string SetClockOutTime()
        {
            throw new System.NotImplementedException();
        }

        public string SetShiftRating()
        {
            throw new System.NotImplementedException();
        }
    }
}
