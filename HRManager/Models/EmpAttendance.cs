using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRManager.Models {

    public class EmpAttendance {
        public Employee Employee { get; set; }
        public bool IsActive { get; set; }
    }
}
