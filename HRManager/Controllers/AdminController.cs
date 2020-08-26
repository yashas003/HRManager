using HRManager.Models;
using HRManager.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HRManager.Controllers {

    public class AdminController : Controller {
        private readonly IRepo adminRepo;
        private readonly IHostingEnvironment hostingEnv;

        public AdminController(IRepo adminRepo, IHostingEnvironment hostingEnv) {
            this.hostingEnv = hostingEnv;
            this.adminRepo = adminRepo;
        }
        
        [HttpGet]
        public ViewResult Login() {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginVM credFromVM) {
            if (ModelState.IsValid) {
                Credentials credFromUser = new Credentials() {
                    EmailAddress = credFromVM.EmailAddress,
                    Password = GetEncryptedPassword(credFromVM.Password)
                };

                if (adminRepo.IsValid(credFromUser)) {
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
            Employee admin = adminRepo.GetAdmin(emailId);

            if (admin != null) {
                DashboardVM dvm = new DashboardVM {
                    Employee = admin,
                    IsPunchedIn = adminRepo.GetAttandanceStatus(emailId),
                    Dept = admin.Dept
                };
                return View(dvm);
            }
            return RedirectToAction("login");
        }

        [HttpPost]
        public IActionResult EditDetails(DashboardVM dvm) {

            if (ModelState.IsValid) {
                Employee newEmp = adminRepo.GetAdmin(dvm.EmailAddress);
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
                adminRepo.UpdateEmployee(newEmp);
            }
            return RedirectToAction("dashboard");
        }

        [HttpGet]
        public IActionResult EmployeeList() {
            string emailId = HttpContext.Session.GetString("email_id");

            EmployeeListVM dvm = new EmployeeListVM {
                Employee = adminRepo.GetAdmin(emailId),
                Employees = adminRepo.GetAllEmployees(),
                IsPunchedIn = adminRepo.GetAttandanceStatus(emailId),
            };
            return View(dvm);
        }

        [HttpGet]
        public IActionResult Attendances() {
            string emailId = HttpContext.Session.GetString("email_id");

            AttendanceVM avm = new AttendanceVM {
                CurrentUser = adminRepo.GetAdmin(emailId),
                IsPunchedIn = adminRepo.GetAttandanceStatus(emailId),
                EmpAttendances = adminRepo.GetEmpAttendance()
            };
            return View(avm);
        }

        [HttpGet]
        public IActionResult Policy() {
            return View();
        }

        public IActionResult Attendance(string email) {
            string emailId = HttpContext.Session.GetString("email_id");

            IndividualAttenVM iavm = new IndividualAttenVM { 
                CurrentUser = adminRepo.GetAdmin(emailId),
                IsPunchedIn = adminRepo.GetAttandanceStatus(emailId),
                IndividualAtten = adminRepo.GetIndividualAttendance(email)
            };
            return View(iavm);
        }

        public IActionResult AddEmployee(EmployeeListVM elvm) {
            if (ModelState.IsValid) {
                Employee newEmp = new Employee {
                    Role = elvm.Role == 0 ? true : false,
                    FirstName = elvm.FirstName,
                    MiddleName = elvm.MiddleName,
                    LastName = elvm.LastName,
                    Dept = elvm.Dept,
                    Gender = elvm.Gender,
                    DOB = elvm.DOB,
                    HomeAddress = elvm.HomeAddress,
                    TellNo = elvm.TellNo,
                    MobileNo = elvm.MobileNo,
                    EmailAddress = elvm.EmailAddress,
                    ImagePath = GetImagePath(elvm.ImagePath)
                };

                string encryptedPassword = GetEncryptedPassword(elvm.Password);
                Credentials newCred = new Credentials {
                    EmailAddress = elvm.EmailAddress,
                    Password = encryptedPassword
                };

                adminRepo.AddEmployee(newEmp, newCred);
            }
            return RedirectToAction("employeelist");
        }

        public IActionResult CheckInFromDashboard()
        {
            CheckIn();
            return RedirectToAction("dashboard");
        }
        public IActionResult CheckOutFromDashboard()
        {
            CheckOut();
            return RedirectToAction("dashboard");
        }

        public IActionResult CheckInFromEmployees()
        {
            CheckIn();
            return RedirectToAction("employeelist");
        }
        public IActionResult CheckOutFromEmployees()
        {
            CheckOut();
            return RedirectToAction("employeelist");
        }

        public IActionResult CheckInFromAttendances() {
            CheckIn();
            return RedirectToAction("attendances");
        }
        public IActionResult CheckOutFromAttendances() {
            CheckOut();
            return RedirectToAction("attendances");
        }

        public IActionResult CheckInFromPolicy()
        {
            CheckIn();
            return RedirectToAction("policy");
        }
        public IActionResult CheckOutFromPolicy()
        {
            CheckOut();
            return RedirectToAction("policy");
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

        private void CheckIn() {
            string email_id = HttpContext.Session.GetString("email_id");
            adminRepo.CheckInEmployee(adminRepo.GetAdmin(email_id));
        }

        private void CheckOut() {
            string email_id = HttpContext.Session.GetString("email_id");
            adminRepo.CheckOutEmployee(adminRepo.GetAdmin(email_id));
        }

        private string GetEncryptedPassword(string password) {
            using var alg = SHA256.Create();
            return string.Join(null, alg.ComputeHash(Encoding.UTF8.GetBytes(password)).Select(x => x.ToString("x2")));
        }
    }
}
