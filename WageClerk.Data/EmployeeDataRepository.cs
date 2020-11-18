using Arch.EntityFrameworkCore.UnitOfWork;
using WageClerk.Data.Models;

namespace WageClerk.Data
{
    public class EmployeeDataRepository : IEmployeeDataRepository
    {
        private IRepository<Employee> _employeeRepo;
        private IUnitOfWork<InnovationDayContext> _unitOfWork;

        public EmployeeDataRepository(IUnitOfWork<InnovationDayContext> unitOfWork)
        {
            _employeeRepo = unitOfWork.GetRepository<Employee>();
            _unitOfWork = unitOfWork;
        }

        public void AddShift(Shift shift, Employee employee)
        {
            throw new System.NotImplementedException();
        }

        public Employee GetByUserName(string username)
        {
            return _employeeRepo.GetFirstOrDefault(predicate: e => e.UserName == username);
        }

        public Employee Add(Employee employee)
        {
            _employeeRepo.Insert(employee);
            _unitOfWork.SaveChanges();
            return employee;
        }
    }
}
