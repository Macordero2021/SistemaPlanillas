using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SistemaPlanillas.Models;


namespace SistemaPlanillas.Controllers
{
    public class UserController : Controller
    {
        private DataBaseConfig _db = new DataBaseConfig();

        // GET: User
        public ActionResult FormUserCreate()
        {
            var departments = _db.Departments.ToList();
            return View(departments);
        }

        [HttpPost]
        public ActionResult storeUser(FormCollection form)
        {
            string name = form["name"];
            string lastName = form["lastName"];
            string email = form["email"];
            string phone = form["phone"];
            string pass1 = form["pass1"];
            string pass2 = form["pass2"];
            string departments = form["departments"];

            if (pass1 == pass2)
            {
                //las contrasenas coinciden
                var usersData = _db.Users.ToList();
                var existeCorreo = 0;

                foreach (Users item in usersData)
                {
                    if (item.email == email)
                    {
                        existeCorreo = 1;
                        break;
                    }
                }

                if (existeCorreo == 1)
                {
                    //el correo existe en otro usuario
                    TempData["repeatEmail"] = "El correo ya existe en otro usuario";
                    var departments3 = _db.Departments.ToList();
                    return View("FormUserCreate", departments3);
                }
                else
                {

                    DateTime fechaActual = DateTime.Now;


                    //RolDepartmentUser storeUserDepartment = new RolDepartmentUser();
                    //debemos obtener el id
                    //Guardar usuario
                    Users storeUser = new Users();
                    storeUser.name = name;
                    storeUser.lastName = lastName;
                    storeUser.email = email;
                    storeUser.phone = phone;
                    storeUser.password = pass1;
                    storeUser.fkStatus = 1;
                    storeUser.dateCreate =  fechaActual;
                    storeUser.dateUpdate = fechaActual;
                    storeUser.fkPaymentMethod = 1;

                    _db.Users.Add(storeUser);
                    _db.SaveChanges();

                }

            }
            TempData["contraseñaIncorrecta"] = "Las contraseñas no coinciden";

            var departments2 = _db.Departments.ToList();
            return View("FormUserCreate", departments2);

        }
    }
}