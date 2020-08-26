using HRManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRManager.ViewModels {

    public class IndividualAttenVM {
        public Employee CurrentUser { get; set; }

        public bool IsPunchedIn { get; set; }

        public IndividualAtten IndividualAtten { get; set; }
    }
}
