using HRManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRManager.ViewModels {

    public class AttendanceVM {

        public Employee CurrentUser { get; set; }

        public bool IsPunchedIn { get; set; }

        public IEnumerable<EmpAttendance> EmpAttendances { get; set; }
    }
}
