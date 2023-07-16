using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Helpers;
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
                    //El departamento no se selecciono
                    //pasar los oldValues para los inputs en caso de que las contrasenas no coincidan
                    ViewBag.name = name;
                    ViewBag.lastName = lastName;
                    ViewBag.phone = phone;
                    ViewBag.pass = pass1;


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
                        //Falta encryptar contrasena

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

                        //codigo para guardar los datos en  la tabla update_user
                        var users = _db.Users.ToList(); // trae toda la lista de los usuarios
                        var Lastuser = users.LastOrDefault(); // busca el ultimo usuario
                        var idLastuser = Lastuser.id; // guarda en la variable el id del ultimo usuario
                        update_users storeDates = new update_users();
                        storeDates.fk_user_create = idLastuser;
                        storeDates.id_updateuser = idLastuser;
                        storeDates.date_create = fechaActual;
                        storeDates.date_update = fechaActual;

                        _db.update_users.Add(storeDates);
                        _db.SaveChanges();

                        // codigo para guardar el departamento y el rol del usuario creado
                        var departamentfind = _db.departaments.Where(x => x.name_departament == departments).FirstOrDefault(); // Busca el departamento seleccionado
                        var departmentId = departamentfind.id; // guarda el id del depártamento
                        Rol_Departament_User storeRolsDeparment = new Rol_Departament_User();
                        // guarda en la tabla los datos necesarios
                        storeRolsDeparment.fk_id_user = idLastuser;
                        storeRolsDeparment.fk_id_departament = departmentId;
                        storeRolsDeparment.fk_id_rol = 1;

                        _db.Rol_Departament_User.Add(storeRolsDeparment);
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

        //Codigo para validar el login con las credenciales en la base de datos 

        [HttpPost]
        public ActionResult Login(FormCollection form)
        {
            string email = form["email"];
            string password = form["pass1"];

            var user = _db.Users.FirstOrDefault(u => u.email == email && u.password == password);

            if (user != null)
            {
                //Obtengo el id del roll de la tabla  Rol_Departament_User para luego buscar el rol que tiene asignado este usuario
                Rol_Departament_User idRolUser = _db.Rol_Departament_User.Where(x => x.fk_id_user == user.id).FirstOrDefault();

                //obtengo el rol de ese usuario en la tabla de roles
                Roles rolName = _db.Roles.Where(x => x.id == idRolUser.fk_id_rol).FirstOrDefault();
             
                //get role // name
                Session["role"] = rolName.name_rol;


                if (Session["role"] == null)
                {
                    return View("LoginForm");
                }
                else
                {
                    int userId = user.id;
                    // Inicio de sesión exitoso
                    return RedirectToAction("Dashboard", "User",new { userId });
                }

               
            }
            else
            {
                // Credenciales inválidas
                TempData["loginError"] = "Credenciales inválidas";
                return View("LoginForm");
            }
        }

        public ActionResult Dashboard(int userId)
        {

            Users user = _db.Users.Where(x => x.id == userId).FirstOrDefault();
            return View("Dashboard", user);


        }

    }
}