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
using static SistemaPlanillas.Controllers.Services.ProfileService;

namespace SistemaPlanillas.Controllers
{
    public class RoleController : Controller
    {

        private DataBase1Config _db = new DataBase1Config();

        private ProfileService _profileService;

        public RoleController()
        {
            _profileService = new ProfileService(_db);
        }


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
        /// Action method for displaying the view of the "RoleModule"."
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult RoleModule(int id)
        {
            var idModel = id.ToString();
            ViewBag.idModel = idModel;

            var Roles = _db.Roles.ToList();
            return View("AdminModules/RoleModule", Roles);
        }

        /// <summary>
        /// Action method for storing a new role
        /// </summary>
        /// <param name="form">The form data submitted by the user.</param>
        /// <param name="id">The ID of the role.</param>
        /// <returns>
        ///     If successful, redirects to the "RoleModule" action with the updated role list.
        ///     If the role name already exists, redirects to the "RoleModule" action with an error message.
        /// </returns>
        [HttpPost]
        public ActionResult CreateRole(FormCollection form, int id)
        {
            var idModel = id;


            // Retrieve nameRole input from the form.
            string nameRole = form["rol"];

            // Retrieve all roles from the database.
            var Roles = _db.Roles.ToList();


            // Check if the list of roles is empty.
            if (Roles == null)
            {
                // If the list is empty, create a new role, save it to the database, and show success message.
                Roles storeRole = new Roles();
                storeRole.name_rol = nameRole;
                _db.Roles.Add(storeRole);
                _db.SaveChanges();

                TempData["successRol"] = "The new role was created succesfully";

                return RedirectToAction("RoleModule", "Role", new { id = idModel });
            }
            else
            {
                // If the list of roles is not empty, check if the role with the same name already exists.
                var existe = 0;

                foreach (var item in Roles)
                {
                    if (item.name_rol == nameRole)
                    {
                        existe = 1;
                        break;
                    }
                }

                // If the role name does not exist, create a new role, save it to the database, and show success message.
                if (existe == 0)
                {

                    Roles storeRole = new Roles();
                    storeRole.name_rol = nameRole;
                    _db.Roles.Add(storeRole);
                    _db.SaveChanges();

                    TempData["successRol"] = "The new role was created succesfully";

                    return RedirectToAction("RoleModule", "Role", new { id = idModel });
                }
                else
                {
                    // If the role name already exists, show an error message and return to the view.
                    TempData["repeatNameRol"] = "El nombre del rol ya existe";

                    return RedirectToAction("RoleModule", "Role", new { id = idModel });
                }
            }
        }

        /// <summary>
        /// Action method for deleting a role.
        /// </summary>
        /// <param name="form">The form data submitted by the user.</param>
        /// <returns>
        ///     If successful, redirects to the "RolModule" action with the updated role list.
        /// </returns>
        [HttpPost]
        public ActionResult DeleteRole(FormCollection form)
        {
            var idModel = form["idUser"];

            // Retrieve idRol input from the form.
            string idRole = form["id"];
            var idInt = int.Parse(idRole);

            // Find the role in the database, remove it, and save changes.
            Roles deleteRole = _db.Roles.Where(x => x.id == idInt).FirstOrDefault();
            _db.Roles.Remove(deleteRole);
            _db.SaveChanges();

            TempData["deleteSuccess"] = "Rol eliminado correctamente";

            return RedirectToAction("RoleModule", "Role", new { id = idModel });
        }

        /// <summary>
        /// Action method for displaying the edit role form.
        /// </summary>
        /// <returns>The view for editing a role.</returns>
        public ActionResult EditRoleForm()
        {
            // Get the values sent via GET from the form.
            var idModel = Request.QueryString["idUser"];
            ViewBag.idModel = idModel;

            var idRole = Request.QueryString["id"];
            var idInt = int.Parse(idRole);

            // Find the role in the database and pass it to the view.
            Roles editRole = _db.Roles.Where(x => x.id == idInt).FirstOrDefault();

            return View("AdminModules/EditRoleForm", editRole);
        }

        /// <summary>
        /// Action method for storing the edited role.
        /// </summary>
        /// <param name="form">The form data submitted by the user.</param>
        /// <param name="id">The ID of the role.</param>
        /// <returns>
        ///     If successful, redirects to the "RoleModule" action with the updated role list.
        ///     If the role name already exists, redirects to the "RoleModule" action with an error message.
        /// </returns>
        [HttpPost]
        public ActionResult StoreEditedRole(FormCollection form, int id)
        {
            // Get the id of the logged-in user from the URL.
            var idModel = id;

            // Retrieve the new and old names of the role from the form data.
            string newNameRole = form["rol"];
            string oldNameRole = form["oldRol"];

            // If the new name is the same as the old name, no changes are made, and the user is redirected.
            if (newNameRole == oldNameRole)
            {

                return RedirectToAction("RoleModule", "Role", new { id = idModel });
            }
            else
            {
                // Retrieve all roles from the database. 
                var Roles = _db.Roles.ToList();

                // Check if the new role name already exists.
                var existe = 0;

                foreach (var item in Roles)
                {
                    if (item.name_rol == newNameRole)
                    {
                        existe = 1;
                        break;
                    }
                }

                // If the new role name already exists, redirect with a message.
                if (existe == 1)
                {
                    //ADD MESSAJE THAT A ROLE ALREADY EXISTS WITH THAT NAME
                    return RedirectToAction("RoleModule", "Role", new { id = idModel });
                }
                else
                {
                    // Update the role's name and save changes to the database.
                    Roles editRole = _db.Roles.Where(x => x.name_rol == oldNameRole).FirstOrDefault();
                    editRole.name_rol = newNameRole;
                    _db.SaveChanges();

                    return RedirectToAction("RoleModule", "Role", new { id = idModel });
                }
            }
        }

        /// <summary>
        /// Action method for displaying the view for assigning roles to users.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <returns>The view for assigning roles to users.</returns>
        public ActionResult AssignRole(int id, string nameOrEmail)
        {
            string nameOrEmail2 = nameOrEmail;

            if (nameOrEmail2 == null || nameOrEmail2 == "")
            {
                // Retrieve lists of roles, users, user-roles-departments, and user statuses from the database.
                List<Roles> roles = _db.Roles.ToList();
                List<Users> users = _db.Users.ToList();
                List<User_RolAndDepartment> RolesDeparmentsUser1 = _db.User_RolAndDepartment.ToList();
                List<User_Status> Status1 = _db.User_Status.ToList();
                List<Departaments> Departaments1 = _db.Departaments.ToList();

                // Create a view model containing all the retrieved lists and pass it to the view.
                modelCompuesto viewModel = new modelCompuesto
                {
                    Role = roles,
                    User = users,
                    RoleDeparmentUser = RolesDeparmentsUser1,
                    Status = Status1,
                    Departaments = Departaments1

                };

                // Get the id of the logged-in user from the URL and store it in the ViewBag to be used in the view.
                var idModel = id.ToString();
                ViewBag.idModel = idModel;

                return View("AdminModules/AssignRole", viewModel);

            }
            else
            {
                //aqui ya se realizo la busqueda 
                // Retrieve lists of roles, users, user-roles-departments, and user statuses from the database.
                List<Roles> roles = _db.Roles.ToList();
                List<Users> users = _db.Users.Where(x => x.name.Contains(nameOrEmail2) || x.email.Contains(nameOrEmail2)).ToList();
                List<User_RolAndDepartment> RolesDeparmentsUser1 = _db.User_RolAndDepartment.ToList();
                List<User_Status> Status1 = _db.User_Status.ToList();
                List<Departaments> departaments = _db.Departaments.ToList();

                // Create a view model containing all the retrieved lists and pass it to the view.
                modelCompuesto viewModel = new modelCompuesto
                {
                    Role = roles,
                    User = users,
                    RoleDeparmentUser = RolesDeparmentsUser1,
                    Status = Status1,
                    Departaments = departaments
                };

                // Get the id of the logged-in user from the URL and store it in the ViewBag to be used in the view.
                var idModel = id.ToString();
                ViewBag.idModel = idModel;

                return View("AdminModules/AssignRole", viewModel);
            }
        }

        public ActionResult AssignRoleForm(int id2)
        {

            string idUserLogin = Request.QueryString["idUserLogin"];

            // Retrieve lists of roles, users, user-roles-departments, and user statuses from the database.

            Users users = _db.Users.Where(x => x.id == id2).FirstOrDefault();
            User_RolAndDepartment RoleDeparment = _db.User_RolAndDepartment.Where(x => x.fk_id_user == id2).FirstOrDefault();
            User_Status Status1 = _db.User_Status.Where(x => x.id == users.fk_id_status).FirstOrDefault();
            Roles roles = _db.Roles.Where(x => x.id == RoleDeparment.fk_id_rol).FirstOrDefault();
            List<User_Status> status2 = _db.User_Status.ToList();
            List<Roles> roles2 = _db.Roles.ToList();

           // Create a view model containing all the retrieved lists and pass it to the view.
           UserAsingEdit viewModel = new UserAsingEdit
            {
                User2 = users,
                Status = Status1,
                roles = roles,
                Status2 = status2,
                Role = roles2
           };

            // Get the id of the logged-in user from the URL and store it in the ViewBag to be used in the view.
            var idModel = idUserLogin.ToString(); //aqui tiene que ir el usuario que esta logueado
            ViewBag.idModel = idModel; //aqui tiene que ir el usuario que esta logueado

            return View("AdminModules/AssignRoleForm", viewModel);
        }

        public ActionResult StoreEditAssignRoleForm(FormCollection form)
        {
            // Get the id of the logged-in user from the URL.
                        var idModel = form["idUserAdmin"]; ; 

            // Retrieve the new and old names of the role from the form data.

                        string newStatus = form["newStatus"];
                        string newRole = form["newRol"];
                        string newEmail = form["newEmail"];
                        string idEdit = form["id"];
                        int idEditInt = int.Parse(idEdit.ToString());



            // Update the role's name and save changes to the database.


            /*
            Roles editRole = _db.Roles.Where(x => x.name_rol == oldNameRole).FirstOrDefault();
            editRole.name_rol = newNameRole;
            _db.SaveChanges();
            */
                         Users Userid2 = _db.Users.Where(x => x.id == idEditInt).FirstOrDefault();
                        if (newEmail != "")
                        {
                            Userid2.email = newEmail;
                        }
                        if (newStatus != "Choose a new Status") {
                            var idstatus = _db.User_Status.Where(x => x.name_status == newStatus).FirstOrDefault();
                            Userid2.fk_id_status = idstatus.id;    
                        }
                        if (newRole != "Choose a new Role")
                        {
                            var idRoll = _db.Roles.Where(x => x.name_rol == newRole).FirstOrDefault();
                            User_RolAndDepartment userDepartmentRol = _db.User_RolAndDepartment.Where(x => x.fk_id_user == idEditInt).FirstOrDefault();
                            userDepartmentRol.fk_id_rol = idRoll.id;    
                        }

                        _db.SaveChanges();
            return RedirectToAction("AssignRole", new { id = idModel, nameOrEmail = "" });
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
            var result = _profileService.UpdateProfileField(userId, fieldName, value);

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