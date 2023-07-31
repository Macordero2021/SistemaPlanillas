using SistemaPlanillas.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;


namespace SistemaPlanillas.Controllers
{
    public class RoleController : Controller
    {

        private DataBase1Config _db = new DataBase1Config();


        /// <summary>
        /// Action method for displaying the view when the role is "Undefined."
        /// </summary>
        /// <returns>The view displaying information related to the "Undefined" role.</returns>
        public ActionResult UndefinedView(int userId)
        {
            Users user = _db.Users.Where(x => x.id == userId).FirstOrDefault();

            return View(user);
        }

        /// <summary>
        /// Action method for displaying the view when the role is "ADMIN."
        /// </summary>
        /// <returns>The view displaying information related to the "ADMIN" role.</returns>
        public ActionResult AdminView(int userId)
        {
            Users user = _db.Users.Where(x => x.id == userId).FirstOrDefault();

            return View(user);
        }

        /// <summary>
        /// Action method for displaying the view when the role is "Employee."
        /// </summary>
        /// <returns>The view displaying information related to the "Employee" role.</returns>
        public ActionResult EmployeeView(int userId)
        {
            Users user = _db.Users.Where(x => x.id == userId).FirstOrDefault();

            return View(user);
        }

        /// <summary>
        /// Action method for displaying the view when the role is "Payroll."
        /// </summary>
        /// <returns>The view displaying information related to the "Payroll" role.</returns>
        public ActionResult PayrollView(int userId)
        {
            Users user = _db.Users.Where(x => x.id == userId).FirstOrDefault();

            return View(user);
        }

        public ActionResult RolConfig(int id)
        {
            var idModel = id.ToString();
            ViewBag.idModel = idModel;

            var Roles = _db.Roles.ToList();
            return View("~/Views/Role/AdminModules/RolConfig.cshtml", Roles);
        }


        /// <summary>
        /// Action method for storing a new role
        /// </summary>
        /// <param name="form">The form data submitted by the user.</param>
        /// <param name="id">The ID of the role.</param>
        /// <returns>
        ///     If successful, redirects to the "RolConfig" action with the updated role list.
        ///     If the role name already exists, redirects to the "RolConfig" action with an error message.
        /// </returns>
        [HttpPost]
        public ActionResult StoreRol(FormCollection form,int id)
        {
            var idModel = id;


            // Retrieve nameRol input from the form.
            string nameRol = form["rol"];

            // Retrieve all roles from the database.
            var Roles = _db.Roles.ToList();


            // Check if the list of roles is empty.
            if (Roles == null) {
                // If the list is empty, create a new role, save it to the database, and show success message.
                Roles storeRol = new Roles();
                storeRol.name_rol = nameRol;
                _db.Roles.Add(storeRol);
                _db.SaveChanges();

                TempData["successRol"] = "El rol se guardo correctamente";

                return RedirectToAction("RolConfig", "Role", new { id = idModel });
            }
            else
            {
                // If the list of roles is not empty, check if the role with the same name already exists.
                var existe = 0;

                foreach (var item in Roles)
                {
                    if (item.name_rol == nameRol)
                    {
                        existe = 1;
                        break;
                    }
                }

                // If the role name does not exist, create a new role, save it to the database, and show success message.
                if (existe == 0) {

                    Roles storeRol = new Roles();
                    storeRol.name_rol = nameRol;
                    _db.Roles.Add(storeRol);
                    _db.SaveChanges();

                    TempData["successRol"] = "El rol se guardo correctamente";

                    return RedirectToAction("RolConfig", "Role", new { id = idModel });
                }
                else
                {
                    // If the role name already exists, show an error message and return to the view.
                    TempData["repeatNameRol"] = "El nombre del rol ya existe";

                    return RedirectToAction("RolConfig", "Role", new { id = idModel });
                }
            }
        } //End of ActionResult StoreRol

        /// <summary>
        /// Action method for deleting a role.
        /// </summary>
        /// <param name="form">The form data submitted by the user.</param>
        /// <returns>
        ///     If successful, redirects to the "RolConfig" action with the updated role list.
        /// </returns>
        [HttpPost]
        public ActionResult DeleteRol(FormCollection form)
        {
            var idModel = form["idUser"];

            // Retrieve idRol input from the form.
            string idRol = form["id"];
            var idInt = int.Parse(idRol);

            // Find the role in the database, remove it, and save changes.
            Roles deleteRol = _db.Roles.Where(x => x.id == idInt).FirstOrDefault(); 
            _db.Roles.Remove(deleteRol);
            _db.SaveChanges();

            TempData["deleteSuccess"] = "Rol eliminado correctamente";

            return RedirectToAction("RolConfig", "Role", new { id = idModel });
        }

        /// <summary>
        /// Action method for displaying the edit role form.
        /// </summary>
        /// <returns>The view for editing a role.</returns>
        public ActionResult FormEditRol()
        {
            // Get the values sent via GET from the form.
            var idModel = Request.QueryString["idUser"];
            ViewBag.idModel = idModel;

            var idRol = Request.QueryString["id"];
            var idInt = int.Parse(idRol);

            // Find the role in the database and pass it to the view.
            Roles editRol = _db.Roles.Where(x => x.id == idInt).FirstOrDefault();
           
            return View("~/Views/Role/AdminModules/EditRolConfig.cshtml", editRol);
        }

        /// <summary>
        /// Action method for storing the edited role.
        /// </summary>
        /// <param name="form">The form data submitted by the user.</param>
        /// <param name="id">The ID of the role.</param>
        /// <returns>
        ///     If successful, redirects to the "RolConfig" action with the updated role list.
        ///     If the role name already exists, redirects to the "RolConfig" action with an error message.
        /// </returns>
        [HttpPost]
        public ActionResult StoreEditRol(FormCollection form, int id)
        {
            // Get the id of the logged-in user from the URL.
            var idModel = id;

            // Retrieve the new and old names of the role from the form data.
            string newNameRol = form["rol"];
            string oldNameRol = form["oldRol"];

            // If the new name is the same as the old name, no changes are made, and the user is redirected.
            if (newNameRol == oldNameRol) {

                return RedirectToAction("RolConfig", "Role", new { id = idModel });
            }
            else
            {
                // Retrieve all roles from the database. 
                var Roles = _db.Roles.ToList();

                // Check if the new role name already exists.
                var existe = 0;

                foreach (var item in Roles)
                {
                    if (item.name_rol == newNameRol)
                    {
                        existe = 1;
                        break;
                    }
                }

                // If the new role name already exists, redirect with a message.
                if (existe == 1) {
                    //ADD MESSAJE THAT A ROLE ALREADY EXISTS WITH THAT NAME
                    return RedirectToAction("RolConfig", "Role", new { id = idModel });
                }
                else
                {
                    // Update the role's name and save changes to the database.
                    Roles editRol = _db.Roles.Where(x => x.name_rol == oldNameRol).FirstOrDefault();
                    editRol.name_rol = newNameRol;  
                    _db.SaveChanges();

                    return RedirectToAction("RolConfig", "Role", new { id = idModel });
                }
            }   
        }

        /// <summary>
        /// Action method for displaying the view for assigning roles to users.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <returns>The view for assigning roles to users.</returns>
        public ActionResult AsignRol(int id)
        {
            // Retrieve lists of roles, users, user-roles-departments, and user statuses from the database.
            List<Roles> roles = _db.Roles.ToList();
            List<Users> users = _db.Users.ToList();
            List<User_RolAndDepartment> RolesDeparmentsUser1 = _db.User_RolAndDepartment.ToList();
            List<User_Status> Status1 = _db.User_Status.ToList();

            // Create a view model containing all the retrieved lists and pass it to the view.
            modelCompuesto viewModel = new modelCompuesto
            {
                Role = roles,
                User = users,
                RoleDeparmentUser = RolesDeparmentsUser1,
                Status = Status1    
            };

            // Get the id of the logged-in user from the URL and store it in the ViewBag to be used in the view.
            var idModel = id.ToString();
            ViewBag.idModel = idModel;

            return View("~/Views/Role/AdminModules/AsignRol.cshtml", viewModel);
        }
    }      
}