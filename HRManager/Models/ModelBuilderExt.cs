using HRManager.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HRManager.Models {

    public static class ModelBuilderExt {

        public static void Seed(this ModelBuilder modelBuilder) {
            modelBuilder.Entity<Employee>().HasData(
                    new Employee {
                        ID = 1,
                        Role = true,
                        FirstName = "Yashas",
                        LastName = "Gowda",
                        MiddleName = null,
                        Dept = Dept.HR,
                        Gender = "Male",
                        DOB = "1997-11-03",
                        HomeAddress = "Bangalore, Karnataka, India",
                        TellNo = null,
                        MobileNo = "9108831063",
                        EmailAddress = "yashas348@gmail.com"
                    });
            modelBuilder.Entity<Credentials>().HasData(
                    new Credentials {
                        ID = 1,
                        EmailAddress = "yashas348@gmail.com",
                        Password = GetEncryptedPassword("Yashas@003")
                    });;
            modelBuilder.Entity<Attendance>();
        }

        private static string GetEncryptedPassword(string password) {
            using var alg = SHA256.Create();
            return string.Join(null, alg.ComputeHash(Encoding.UTF8.GetBytes(password)).Select(x => x.ToString("x2")));
        }
    }
}
