using HRManager.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HRManager.Models {
    
    public class Employee {
        public int ID { get; set; }

        [Required]
        public bool Role { get; set; }

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
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
            ErrorMessage = "Invalid Email Format")]
        public string EmailAddress { get; set; }

        public string ImagePath { get; set; }
    }
}
