using HRManager.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace HRManager.Models {

    public class Repo : IRepo {

        private readonly SqlDbContext context;

        public Repo(SqlDbContext context) {
            this.context = context;
        }

        public Employee GetAdmin(string email_id) {
            var admin = context.Employees.Where(e =>
            e.EmailAddress.ToLower().Trim() == email_id &&
            e.Role == true);
            return admin.FirstOrDefault();
        }

        public Employee GetEmployee(string email_id) {
            var employee = context.Employees.Where(e =>
            e.EmailAddress.ToLower().Trim() == email_id &&
            e.Role == false);
            return employee.FirstOrDefault();
        }

        public bool IsValid(Credentials cred) {
            var authRecord = context.Credentials.FirstOrDefault(c =>
            c.EmailAddress.ToLower().Trim() == cred.EmailAddress.ToLower().Trim() &&
            c.Password == cred.Password);
            return authRecord != null;
        }

        public void UpdateEmployee(Employee emp) {
            var employee = context.Employees.Attach(emp);
            employee.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
        }

        public void CheckInEmployee(Employee emp) {
            if (emp != null) {

                Attendance attandance = new Attendance {
                    EmployeeID = emp.ID,
                    Email = emp.EmailAddress,
                    Department = emp.Dept,
                    WorkingDate = DateTime.Now,
                    CheckType = true
                };

                context.Attendances.Add(attandance);
                context.SaveChanges();
            }
        }

        public void CheckOutEmployee(Employee emp) {
            if (emp != null) {

                Attendance attandance = new Attendance {
                    EmployeeID = emp.ID,
                    Email = emp.EmailAddress,
                    Department = emp.Dept,
                    WorkingDate = DateTime.Now,
                    CheckType = false
                };

                context.Attendances.Add(attandance);
                context.SaveChanges();
            }
        }

        public bool GetAttandanceStatus(string email_id) {
            var attandance = context.Attendances.Where(a =>
            a.Email.ToLower().Trim() == email_id && 
            a.WorkingDate.Date == DateTime.Today.Date);

            bool isPunched = false;
            foreach (Attendance a in attandance) {
                if (a.CheckType == true) isPunched = true;
                else isPunched = false;
            }

            return isPunched;
        }

        public void AddEmployee(Employee newEmp, Credentials newCred) {
            context.Employees.Add(newEmp);
            context.Credentials.Add(newCred);
            context.SaveChanges();
        }

        public IEnumerable<Employee> GetAllEmployees() {
            return context.Employees;
        }

        public IEnumerable<EmpAttendance> GetEmpAttendance() {
            List<EmpAttendance> empAttendances = new List<EmpAttendance>();
            IEnumerable<Employee> employees = GetAllEmployees();

            foreach (Employee employee in employees) {
                EmpAttendance empAttendance = new EmpAttendance {
                    Employee = employee,
                    IsActive = GetAttandanceStatus(employee.EmailAddress)
                };
                empAttendances.Add(empAttendance);
            }
            return empAttendances;
        }

        public IndividualAtten GetIndividualAttendance(string emailOfAttendance) {
            IEnumerable<Attendance> checkIns = context.Attendances.Where(a =>
            a.Email.ToLower().Trim() == emailOfAttendance && 
            a.CheckType == true).OrderBy(d =>
            d.WorkingDate);

            IEnumerable<Attendance> checkOuts = context.Attendances.Where(a =>
            a.Email.ToLower().Trim() == emailOfAttendance &&
            a.CheckType == false).OrderByDescending(d =>
            d.WorkingDate);

            List<SortedAttendance> sortedList = new List<SortedAttendance>();
            foreach (Attendance punchIn in checkIns) {
                foreach(Attendance punchOut in  checkOuts) {

                    if (punchIn.WorkingDate.Date == punchOut.WorkingDate.Date) {
                        SortedAttendance sorted = new SortedAttendance {
                            Date = punchIn.WorkingDate.ToShortDateString(),
                            CheckIn = punchIn.WorkingDate.ToShortTimeString(),
                            CheckOut = punchOut.WorkingDate.ToShortTimeString(),
                            TimeSpan = (punchOut.WorkingDate - punchIn.WorkingDate).ToString(@"h\.mm") + " hours"
                        };
                        sortedList.Add(sorted);
                        break;
                    }
                }
            }

            sortedList.Reverse();
            var indEmp = context.Employees.Where(e =>
            e.EmailAddress.ToLower().Trim() == emailOfAttendance);

            IndividualAtten ia = new IndividualAtten {
                IndividualEmp = indEmp.FirstOrDefault(),
                Sorted = sortedList
            };

            return ia;
        }
    }
}
