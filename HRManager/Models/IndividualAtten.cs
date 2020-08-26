using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRManager.Models {

    public class IndividualAtten {

        public Employee IndividualEmp { get; set; }

        public IEnumerable<SortedAttendance> Sorted { get; set; }
    }
}
