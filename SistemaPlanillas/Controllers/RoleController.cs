using SistemaPlanillas.Controllers.Services;
using SistemaPlanillas.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using static SistemaPlanillas.Controllers.Services.RoleService;

namespace SistemaPlanillas.Controllers
{
    public class RoleController : Controller
    {

        private DataBase1Config _db = new DataBase1Config();

        private RoleService _roleService;

        public RoleController()
        {
            _roleService = new RoleService(_db);
        }

        /// <summary>
        /// Action method for displaying the view when the role is "Undefined."
        /// </summary>
        /// <returns>The view displaying information related to the "Undefined" role.</returns>
        public ActionResult UndefinedView(int userId)
        {
            Users user = _db.Users.Where(x => x.id == userId).FirstOrDefault();
            var userRole = Session["role"];
            ViewBag.UserRole = userRole;
            return View(user);
        }

        /// <summary>
        /// Action method for displaying the view when the role is "ADMIN."
        /// </summary>
        /// <returns>The view displaying information related to the "ADMIN" role.</returns>
        public ActionResult AdminView(int userId)
        {
            Users user = _db.Users.Where(x => x.id == userId).FirstOrDefault();
            var userRole = Session["role"];
            ViewBag.UserRole = userRole;
            return View(user);
        }

        /// <summary>
        /// Action method for displaying the view when the role is "Employee."
        /// </summary>
        /// <returns>The view displaying information related to the "Employee" role.</returns>
        public ActionResult EmployeeView(int userId)
        {
            Users user = _db.Users.Where(x => x.id == userId).FirstOrDefault();
            var userRole = Session["role"];
            ViewBag.UserRole = userRole;
            return View(user);
        }

        /// <summary>
        /// Action method for displaying the view when the role is "Payroll."
        /// </summary>
        /// <returns>The view displaying information related to the "Payroll" role.</returns>
        public ActionResult PayrollView(int userId)
        {
            Users user = _db.Users.Where(x => x.id == userId).FirstOrDefault();
            var userRole = Session["role"];
            ViewBag.UserRole = userRole;
            return View(user);
        }

        /// <summary>
        /// Action method for displaying the view "
        /// </summary>
        /// <returns>The view displaying information related to the user role.</returns>
        public ActionResult ProfileModule(int id)
        {
            // Retrieve lists of roles, users, user-roles-departments, and user statuses from the database.

            Users user = _db.Users.Where(x => x.id == id).FirstOrDefault();
            User_Status status = _db.User_Status.Where(x => x.id == user.fk_id_status).FirstOrDefault();
            var role = Session["role"];
            User_RolAndDepartment rolAndDepart = _db.User_RolAndDepartment.Where(x => x.fk_id_user == id).FirstOrDefault();
            Departaments departament = _db.Departaments.Where(x => x.id == user.Fk_Id_Deparment).FirstOrDefault();
            var nameDepartment = departament.name_departament;


            // Create a view model containing all the retrieved lists and pass it to the view.
            modelCompuestoInd viewModel = new modelCompuestoInd
            {
                User = user,
                RoleDeparmentUser = rolAndDepart,
                Departaments = departament,
                Status = status
            };

            ViewBag.role = role;

            return View("GlobalModules/ProfileModule", viewModel);
        }

        /// <summary>
        /// Action method for updating user information.
        /// </summary>
        /// <param name="userId">The ID of the user to update.</param>
        /// <param name="fieldName">The name of the field to update.</param>
        /// <param name="value">The new value to assign to the field.</param>
        /// <returns>
        /// A JSON result indicating the outcome of the update operation.
        /// </returns>
        [HttpPost]
        public ActionResult UpdateUserInfo(int userId, string fieldName, string value)
        {
            var result = _roleService.UpdateProfileField(userId, fieldName, value);

            switch (result)
            {
                case UpdateResult.Success:
                    return Json(new { success = true, message = "User information updated successfully" });
                case UpdateResult.UserNotFound:
                    return Json(new { error = "User not found" });
                case UpdateResult.EmailExists:
                    return Json(new { error = "Email already exists" });
                case UpdateResult.NoChangesMade:
                    return Json(new { error = "No changes made" });
                case UpdateResult.InvalidField:
                    return Json(new { error = "Invalid field" });
                case UpdateResult.ErrorUpdatingUser:
                default:
                    return Json(new { error = "Error updating user" });
            }
        }
    }
}