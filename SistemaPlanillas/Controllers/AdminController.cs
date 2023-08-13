using SistemaPlanillas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaPlanillas.Controllers
{
    public class AdminController : Controller
    {
        private DataBase1Config _db = new DataBase1Config();

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
            return View("RoleModule", Roles);
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

                return RedirectToAction("RoleModule", "Admin", new { id = idModel });
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

                    return RedirectToAction("RoleModule", "Admin", new { id = idModel });
                }
                else
                {
                    // If the role name already exists, show an error message and return to the view.
                    TempData["repeatNameRol"] = "El nombre del rol ya existe";

                    return RedirectToAction("RoleModule", "Admin", new { id = idModel });
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

            return RedirectToAction("RoleModule", "Admin", new { id = idModel });
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

            return View(editRole);
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

                return RedirectToAction("RoleModule", "Admin", new { id = idModel });
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
                    return RedirectToAction("RoleModule", "Admin", new { id = idModel });
                }
                else
                {
                    // Update the role's name and save changes to the database.
                    Roles editRole = _db.Roles.Where(x => x.name_rol == oldNameRole).FirstOrDefault();
                    editRole.name_rol = newNameRole;
                    _db.SaveChanges();

                    return RedirectToAction("RoleModule", "Admin", new { id = idModel });
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
            // Verificar si nameOrEmail no es nulo y luego eliminar los espacios en blanco del inicio y final
            if (nameOrEmail != null)
            {
                nameOrEmail = nameOrEmail.Trim();
            }

            List<Users> users;

            if (string.IsNullOrEmpty(nameOrEmail))
            {
                // Retrieve all users if no nameOrEmail provided.
                users = _db.Users.ToList();
            }
            else
            {
                // Retrieve users matching the provided nameOrEmail.
                users = _db.Users.Where(x => x.name.Contains(nameOrEmail) || x.email.Contains(nameOrEmail)).ToList();
            }

            var usersWithInfo = (from user in users
                                 join roleDept in _db.User_RolAndDepartment on user.id equals roleDept.fk_id_user
                                 join role in _db.Roles on roleDept.fk_id_rol equals role.id
                                 join status in _db.User_Status on user.fk_id_status equals status.id
                                 select new UserCompositeModel
                                 {
                                     User = user,
                                     Role = role,
                                     Status = status
                                 }).ToList();

            // Get the id of the logged-in user from the URL and store it in the ViewBag to be used in the view.
            ViewBag.idModel = id.ToString();

            return View(usersWithInfo);
        }

        public ActionResult AssignRoleForm(int id)
        {

            string idUserLogin = Request.QueryString["idUserLogin"];

            // Retrieve lists of roles, users, user-roles-departments, and user statuses from the database.
            Users users = _db.Users.Where(x => x.id == id).FirstOrDefault();
            User_RolAndDepartment RoleDeparment = _db.User_RolAndDepartment.Where(x => x.fk_id_user == id).FirstOrDefault();
            User_Status Status1 = _db.User_Status.Where(x => x.id == users.fk_id_status).FirstOrDefault();
            Roles roles = _db.Roles.Where(x => x.id == RoleDeparment.fk_id_rol).FirstOrDefault();
            List<User_Status> status2 = _db.User_Status.ToList();
            List<Roles> roles2 = _db.Roles.ToList();

            // Create a view model containing all the retrieved lists and pass it to the view.
            UserToEdit viewModel = new UserToEdit
            {
                userToEdit = users,
                statusToEdit = Status1,
                roleToEdit = roles,
                StatusList = status2,
                RolesList = roles2
            };

            // Get the id of the logged-in user from the URL and store it in the ViewBag to be used in the view.
            var idModel = idUserLogin.ToString(); //aqui tiene que ir el usuario que esta logueado
            ViewBag.idModel = idModel; //aqui tiene que ir el usuario que esta logueado

            return View(viewModel);
        }

        public ActionResult StoreEditAssignRoleForm(FormCollection form)
        {
            var idModel = form["idUserAdmin"]; ;

            // Retrieve the new and old names of the role from the form data.
            string newStatus = form["newStatus"];
            string newRole = form["newRol"];
            string newEmail = form["newEmail"];
            string idEdit = form["id"];
            int idEditInt = int.Parse(idEdit.ToString());

            // Retrieve the user's info to update
            Users UserId2 = _db.Users.Where(x => x.id == idEditInt).FirstOrDefault();

            if (newEmail != "")
            {
                UserId2.email = newEmail;
            }

            if (newStatus != "Choose a new Status")
            {
                var idstatus = _db.User_Status.Where(x => x.name_status == newStatus).FirstOrDefault();
                UserId2.fk_id_status = idstatus.id;
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
    }
}