using HRManager.Models;
using HRManager.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HRManager.Controllers {

    public class EmployeeController : Controller {

        private readonly IRepo employeeRepo;
        private readonly IHostingEnvironment hostingEnv;

        public EmployeeController(IRepo employeeRepo, IHostingEnvironment hostingEnv) {
            this.hostingEnv = hostingEnv;
            this.employeeRepo = employeeRepo;
        }

        [HttpGet]
        public ViewResult Login() {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginVM credFromVM)  {
            if (ModelState.IsValid) {
                Credentials credFromUser = new Credentials() {
                    EmailAddress = credFromVM.EmailAddress,
                    Password = GetEncryptedPassword(credFromVM.Password)
                };

                if (employeeRepo.IsValid(credFromUser)) {
                    HttpContext.Session.SetInt32("id", credFromUser.ID);
                    HttpContext.Session.SetString("email_id", credFromUser.EmailAddress);
                    HttpContext.Session.SetString("password", credFromUser.Password);
                    return RedirectToAction("dashboard");
                }
            }
            return View();
        }

        public IActionResult Logout() {
            HttpContext.Session.Clear();
            return RedirectToAction("login");
        }

        [HttpGet]
        public IActionResult Dashboard() {
            string emailId = HttpContext.Session.GetString("email_id");
            Employee employee = employeeRepo.GetEmployee(emailId);

            if (employee != null) {
                DashboardVM dvm = new DashboardVM {
                    Employee = employee,
                    IsPunchedIn = employeeRepo.GetAttandanceStatus(emailId),
                    Dept = employee.Dept
                };
                return View(dvm);
            }
            return RedirectToAction("login");
        }

        [HttpPost]
        public IActionResult EditDetails(DashboardVM dvm) {
            if (ModelState.IsValid) {
                Employee newEmp = employeeRepo.GetEmployee(dvm.EmailAddress);
                newEmp.FirstName = dvm.FirstName;
                newEmp.MiddleName = dvm.MiddleName;
                newEmp.LastName = dvm.LastName;
                newEmp.Dept = dvm.Dept == null ? newEmp.Dept : dvm.Dept;
                newEmp.Gender = dvm.Gender;
                newEmp.DOB = dvm.DOB;
                newEmp.HomeAddress = dvm.HomeAddress;
                newEmp.TellNo = dvm.TellNo;
                newEmp.MobileNo = dvm.MobileNo;
                if (dvm.ImagePath != null) {

                    if (newEmp.ImagePath != null) {
                        string filePath = Path.Combine(hostingEnv.WebRootPath, "images", newEmp.ImagePath);
                        System.IO.File.Delete(filePath);
                    }
                    newEmp.ImagePath = GetImagePath(dvm.ImagePath);
                }
                employeeRepo.UpdateEmployee(newEmp);
            }
            return RedirectToAction("dashboard");
        }

        public IActionResult checkIn() {
            string emailId = HttpContext.Session.GetString("email_id");
            employeeRepo.CheckInEmployee(employeeRepo.GetEmployee(emailId));
            return RedirectToAction("dashboard");
        }

        public IActionResult checkOut() {
            string emailId = HttpContext.Session.GetString("email_id");
            employeeRepo.CheckOutEmployee(employeeRepo.GetEmployee(emailId));
            return RedirectToAction("dashboard");
        }

        private string GetImagePath(IFormFile imagePath) {
            string uniqueName = null;
            if (imagePath != null) {
                string folder = Path.Combine(hostingEnv.WebRootPath, "images");
                uniqueName = Guid.NewGuid().ToString() + imagePath.FileName;
                string filePath = Path.Combine(folder, uniqueName);
                using var fileStream = new FileStream(filePath, FileMode.Create);
                imagePath.CopyTo(fileStream);
            }
            return uniqueName;
        }

        private string GetEncryptedPassword(string password)
        {
            using var alg = SHA256.Create();
            return string.Join(null, alg.ComputeHash(Encoding.UTF8.GetBytes(password)).Select(x => x.ToString("x2")));
        }
    }
}
