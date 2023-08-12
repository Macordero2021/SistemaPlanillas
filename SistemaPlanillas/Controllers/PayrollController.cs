using SistemaPlanillas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaPlanillas.Controllers
{
    public class PayrollController : Controller
    {
        private DataBase1Config _db = new DataBase1Config();

        public ActionResult HumanResources(int userId, string nameOrEmail)
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
                                 join dept in _db.Departaments on roleDept.fk_id_departament equals dept.id
                                 join status in _db.User_Status on user.fk_id_status equals status.id
                                 select new UserCompositeModel
                                 {
                                     User = user,
                                     Department = dept,
                                     Status = status
                                 }).ToList();

            // Get the id of the logged-in user from the URL and store it in the ViewBag to be used in the view.
            ViewBag.userId = userId;

            return View(usersWithInfo);
        }



        public ActionResult PayrollModule(int userId)
        {
            Users user = _db.Users.Where(x => x.id == userId).FirstOrDefault();
            return View(user);
        }
    }
}