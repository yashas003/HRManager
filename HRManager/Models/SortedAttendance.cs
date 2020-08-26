using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRManager.Models {

    public class SortedAttendance {
        public string Date { get; set; }

        public string CheckIn { get; set; }

        public string CheckOut { get; set; }

        public string TimeSpan { get; set; }
    }
}
