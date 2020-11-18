using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arch.EntityFrameworkCore.UnitOfWork;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using WageClerk.Data.Models;
using WagesClerk.Models;

namespace WageClerk.Data
{
    public class ShiftDataRepository : IShiftDataRepository
    {
        private IRepository<Employee> _employeeRepo;
      //  private IRepository<EmployeeShift> _employeeShiftRepo;
        private IRepository<Shift> _shiftRepo;
        private IUnitOfWork<InnovationDayContext> _unitOfWork;

        public ShiftDataRepository(IUnitOfWork<InnovationDayContext> unitOfWork)
        {
            _employeeRepo = unitOfWork.GetRepository<Employee>();
            //_employeeShiftRepo = unitOfWork.GetRepository<EmployeeShift>();
            _shiftRepo = unitOfWork.GetRepository<Shift>();
            _unitOfWork = unitOfWork;
        }

        public Shift GetShiftById(int id)
        {
            return new Shift();
        }

        public void Add(Shift shift)
        {
            _shiftRepo.Insert(shift);
            _unitOfWork.SaveChanges();
        }

        public string SetClockInTime(Employee employee)
        {
            DateTime clockInDateTime = DateTime.Now;
            employee.Shifts.Add(new Shift
            {
                ClockIn = clockInDateTime,
                EmployeeId = employee.Id
            });
            _shiftRepo.Insert(employee.Shifts);
            _unitOfWork.SaveChanges();
            return "Welcome " + employee.UserName + " :wave:! You have clocked in at: *" + clockInDateTime + "* :clock1: ";
        }

        public string SetClockOutTime(Shift shift, Employee employee)
        {
            DateTime clockOutDateTime = DateTime.Now;
            shift.ClockOut = clockOutDateTime;
            _shiftRepo.Update(shift);
            _unitOfWork.SaveChanges();
            return "Hey " + employee.UserName + " :wave:! You have clocked out at: *" + clockOutDateTime + "* :clock1: \r\n \r\n Rate your shift out of 4 using the */ratemyshift <1-4>* command! :spiral_note_pad: ";
        }

        public Shift GetClockedInShift(Employee employee)
        {
            IEnumerable<Shift> employeeShifts = _shiftRepo.GetAll().Where(t => t.EmployeeId == employee.Id);
            return employeeShifts.FirstOrDefault(t => t.ClockIn.HasValue & !t.ClockOut.HasValue);
        }

        public string SetShiftRating(Employee employee, Rating rating)
        {
            //get all employees and get the latest shift
            IEnumerable<Shift> employeeShifts = _shiftRepo.GetAll().Where(t => t.EmployeeId == employee.Id);
            var latestShift = employeeShifts.Last();

            //add the rating for the shift
            latestShift.ShiftRating = rating.ToString();
            _shiftRepo.Update(employeeShifts.Last());
            _unitOfWork.SaveChanges();
            return "Thank you for providing your feedback " + employee.UserName + "! :pray:";
        }

        public string GetEmployeeMorale(Employee employee)
        {
            IEnumerable<Shift> employeeShifts = _shiftRepo.GetAll().Where(t => t.EmployeeId == employee.Id);
            var ratings = new List<int>();

            foreach (var shift in employeeShifts)
            {
                Rating rating;
                if (!String.IsNullOrEmpty(shift.ShiftRating))
                {
                    rating = (Rating) Enum.Parse(typeof(Rating), shift.ShiftRating);
                }
                else
                {
                    rating = Rating.Okay;
                }
                ratings.Add((int)rating);
            }

            var averageRating = ratings.Sum() / ratings.Count;

            //determine employee morale
            if (averageRating <= 1)
            {
                return "Employee morale is Poor :white_frowning_face: ";
            }
            else if (averageRating <= 2)
            {
                return "Employee morale is Okay :neutral_face: ";
            }
            else if (averageRating <= 3)
            {
                return "Employee morale is Good :slightly_smiling_face: ";
            }
            else if (averageRating > 3)
            {
                return "Employee morale is Excellent :grin: ";
            }

            return "Oops that doesn't seem right. Please try again. :worried: ";

        }




    }
}
