using SistemaPlanillas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;
using System.Web.WebPages;

namespace SistemaPlanillas.Controllers
{
    public class LicenceController : Controller
    {

        private DataBase1Config _db = new DataBase1Config();

        public ActionResult HolidaysModule(int userId)
        {

            ViewBag.idUserLogin = userId;

            List<Users> users = _db.Users.ToList();

            var usersWithInfo = (from user in users
                                 join userHolidays in _db.UserHolidays on user.id equals userHolidays.fk_id_user
                                 select new UserCompositeModel
                                 {
                                     User = user,
                                     UserHolidays = userHolidays,
                                 }).ToList();


            //get the department of the logged user
            Users userModel = _db.Users.Where(x => x.id == userId).FirstOrDefault();
            var department = _db.Departaments.Where(x => x.id == userModel.Fk_Id_Deparment).FirstOrDefault();
            ViewBag.UserDept = department.name_departament;


            return View(usersWithInfo);
        }

        public ActionResult EditUserHoliday(int idUserLogin, int idUserEdit)
        {
            UserHolidays idEditUserHoliday = _db.UserHolidays.Where(x => x.fk_id_user == idUserEdit).FirstOrDefault();

            //traer el email
            Users user = _db.Users.Where(x => x.id == idUserEdit).FirstOrDefault();
            UserHolidays Holidays = _db.UserHolidays.Where(x => x.fk_id_user == idUserEdit).FirstOrDefault();

            // Create a view model containing all the retrieved lists and pass it to the view.
            UserCompositeModel viewModel = new UserCompositeModel
            {
                User = user,
                UserHolidays = Holidays
            };

            Users userModel = _db.Users.Where(x => x.id == idUserLogin).FirstOrDefault();
            var department = _db.Departaments.Where(x => x.id == userModel.Fk_Id_Deparment).FirstOrDefault();

            ViewBag.UserDept = department.name_departament;
            ViewBag.idUserLogin = idUserLogin;

            return View(viewModel);
        }

        public ActionResult storeEditUserHolidays(int idUserLogin, int idUserEdit, string Holidays)
        {

            UserHolidays NewHolidays = _db.UserHolidays.Where(x => x.fk_id_user == idUserEdit).FirstOrDefault();

            NewHolidays.Holidays = Holidays;

            _db.SaveChanges();
            return RedirectToAction("HolidaysModule", new { userId = idUserLogin });
        }

        public ActionResult LicencesViewUser(int id)
        {
            //aqui debo mostrar una vista con las licencias del usuario que esta logueado

            Users idUserLogin = _db.Users.Where(x => x.id == id).FirstOrDefault();
            var department = _db.Departaments.Where(x => x.id == idUserLogin.Fk_Id_Deparment).FirstOrDefault();

            var licensesUser = _db.License_Application.Where(x=>x.fk_id_user == idUserLogin.id).ToList(); //licencias del usuario
            var License_List_Type = _db.License_Type.ToList();//tipos de loicencia

            var userRole = Session["role"];

            // Create a view model containing all the retrieved lists and pass it to the view.
            UserCompositeModel viewModel = new UserCompositeModel
            {
               License_ApplicationList = licensesUser,
               License_List_Type = License_List_Type
            };

            ViewBag.idUserLogin = idUserLogin.id;
            ViewBag.UserRole = userRole;
            ViewBag.UserDept = department.name_departament;
            return View(viewModel);
        }




        public ActionResult Licences(int id)
        {
            Users user = _db.Users.Where(x => x.id == id).FirstOrDefault();
            var department = _db.Departaments.Where(x => x.id == user.Fk_Id_Deparment).FirstOrDefault();
            var License_List_Type = _db.License_Type.ToList();
            var userRole = Session["role"];

            // Create a view model containing all the retrieved lists and pass it to the view.
            UserCompositeModel viewModel = new UserCompositeModel
            {
                User = user,
                Department = department,
                License_List_Type = License_List_Type,
            };

            ViewBag.UserRole = userRole;
            ViewBag.UserDept = department.name_departament;
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult storeLicenseUser(int idUserLogin, int idUserEdit, string typeLicense, DateTime startDay, DateTime finallyDay, string selectedDays, string notes)
        {
            UserHolidays daysHolidays = _db.UserHolidays.Where(x => x.fk_id_user == idUserLogin).FirstOrDefault();
            int daysAvailable = int.Parse(daysHolidays.Holidays);

            if(int.Parse(selectedDays) >= daysAvailable)
            {
                TempData["CreateStatus"] = "failed";
                return RedirectToAction("Licences", new { id = idUserLogin });
            }
            else 
            { 
                License_Application idUser = _db.License_Application.Where(x => x.fk_id_user == idUserEdit).FirstOrDefault();
                int licenceType = _db.License_Type.Where(x => x.name_license == typeLicense).Select(x => x.id_license_type).FirstOrDefault();
                License_Application license_Application = new License_Application
                {
                    fk_id_user = idUserLogin,
                    fk_id_license_type = licenceType,
                    startDate = startDay,
                    endDay = finallyDay,
                    daysLicense = selectedDays,
                    notes = notes,
                    status_license = "Process"
                };

                _db.License_Application.Add(license_Application);
                _db.SaveChanges();
                return RedirectToAction("LicencesViewUser", new { id = idUserLogin });
            }
        }
    }
}