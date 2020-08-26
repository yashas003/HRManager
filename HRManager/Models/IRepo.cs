using HRManager.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace HRManager.Models {

    public interface IRepo {
        Employee GetEmployee(string email_id);
        void UpdateEmployee(Employee emp);
        Employee GetAdmin(string email_id);
        bool IsValid(Credentials credentials);
        void CheckInEmployee(Employee emp);
        void CheckOutEmployee(Employee emp);
        bool GetAttandanceStatus(string email_id);
        IEnumerable<Employee> GetAllEmployees();
        IEnumerable<EmpAttendance> GetEmpAttendance();
        void AddEmployee(Employee newEmp, Credentials newCred);
        IndividualAtten GetIndividualAttendance(string emailOfAttendance);
    }
}
