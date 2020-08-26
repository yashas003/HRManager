using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HRManager.Models {
    
    public class Attendance {
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        public string Email { get; set; }
        public Dept? Department { get; set; }
        public DateTime WorkingDate { get; set; }
        public bool CheckType { get; set; }
    }
}
