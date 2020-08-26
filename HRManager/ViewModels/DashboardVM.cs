using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using HRManager.Models;
using Microsoft.AspNetCore.Http;

namespace HRManager.ViewModels {

    public class DashboardVM {

        public Employee Employee { get; set; }

        public bool IsPunchedIn { get; set; }

        public int ID { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
        public string LastName { get; set; }

        [MaxLength(50, ErrorMessage = "Middle name cannot exceed 50 characters")]
        public string MiddleName { get; set; }

        [Required]
        public Dept? Dept { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string DOB { get; set; }

        [Required]
        public string HomeAddress { get; set; }

        public string TellNo { get; set; }

        [Required]
        public string MobileNo { get; set; }

        [Required]
        [Display(Name = "Office Email")]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
            ErrorMessage = "Invalid Email Format")]
        public string EmailAddress { get; set; }

        public IFormFile ImagePath { get; set; }
    }
}
