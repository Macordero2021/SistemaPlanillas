using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SistemaPlanillas.Models;

namespace SistemaPlanillas.Controllers
{
    public class UserController : Controller
    {
        private DataBase1Config _db = new DataBase1Config();

        // GET: User
        public ActionResult SignupForm()
        {
            var departments = _db.departaments.ToList();
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
                    var departments3 = _db.departaments.ToList();
                    return View("SignupForm", departments3);
                }
                else
                {

                    if (departments == "Seleccione un departamento")
                    {
                        //El departamento no se selecciono
                        //pasar los oldValues para los inputs en caso de que las contrasenas no coincidan
                        ViewBag.name = name;
                        ViewBag.lastName = lastName;
                        ViewBag.email = email;
                        ViewBag.phone = phone;
                        ViewBag.pass = pass1;


                        TempData["seleccionarDepartamento"] = "Debe seleccionar un departamento";
                        var departments2 = _db.departaments.ToList();

                        return View("SignupForm", departments2);

                    }
                    else
                    {
                        //obtener fecha actual
                        DateTime fechaActual = DateTime.Now;

                        //Guardar usuario
                        Users storeUser = new Users();
                        storeUser.name = name;
                        storeUser.lastname = lastName;
                        storeUser.email = email;
                        storeUser.phone = phone;
                        storeUser.password = pass1;
                        storeUser.fk_id_status = 1;
                        storeUser.salary = "1";
                        storeUser.fk_id_paymentmethod = 1;

                        _db.Users.Add(storeUser);
                        _db.SaveChanges();
                        TempData["UsuarioCreadoCorrectamente"] = "Usuario Creado Exitosamente";
                        var departments2 = _db.departaments.ToList();
                        return View("SignupForm", departments2);
                    }
                }
                
            }
            else
            {

                //pasar los oldValues para los inputs en caso de que las contrasenas no coincidan
                ViewBag.name = name;
                ViewBag.lastName = lastName;
                ViewBag.email = email;
                ViewBag.phone = phone;

                TempData["contraseñaIncorrecta"] = "Las contraseñas no coinciden";
                var departments2 = _db.departaments.ToList();

                return View("SignupForm", departments2);
            }

        }

        public ActionResult LoginForm()
        {
            return View();
        }
    }
}