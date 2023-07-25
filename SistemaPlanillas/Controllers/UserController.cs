using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI;
using SistemaPlanillas.Controllers.Services;
using SistemaPlanillas.Models;

namespace SistemaPlanillas.Controllers
{
    public class UserController : Controller
    {
        private DataBase1Config _db = new DataBase1Config();
        private UserService _userService;
        private RoleService _roleService;

        public UserController()
        {
            _userService = new UserService(_db);
            _roleService = new RoleService(_db);
        }

        /// <summary>
        /// Displays the signup form view with a list of departments.
        /// </summary>
        /// <returns>The view containing the signup form and a list of departments.</returns>
        public ActionResult SignupForm()
        {
            var departments = _db.Departaments.ToList();
            return View(departments);
        }

        /// <summary>
        /// Handles the user signup form submission.
        /// </summary>
        /// <param name="form">The form collection containing user input data.</param>
        /// <returns>If successful, redirects to the SignupForm view with a success message. If there are errors, redisplay the SignupForm view with appropriate error messages.</returns>
        [HttpPost]
        public ActionResult StoreUser(FormCollection form)
        {
            // Retrieve user input from the form.
            string name = form["name"];
            string lastName = form["lastName"];
            string email = form["email"];
            string phone = form["phone"];
            string pass1 = form["pass1"];
            string pass2 = form["pass2"];
            string departments = form["departments"];

            // Create a new user based on the provided data using UserService.
            var result = _userService.CreateNewUser(name, lastName, email, phone, pass1, pass2, departments);
            
            if (result == UserServiceResult.Success)
            {
                // User created successfully, redirect to the SignupForm view with a success message.
                TempData["UsuarioCreadoCorrectamente"] = "User created successfully";
                var departmentsList = _db.Departaments.ToList();
                Console.WriteLine("New user created: " + name + " " + lastName + " - " + email); // Agregar esta línea para imprimir los datos del usuario recién creado
                return View("SignupForm", departmentsList);
            }
            else
            {
                // Some criteria are incorrect, store the data in TempData for displaying errors and redisplay the SignupForm view.
                TempData["name"] = name;
                TempData["lastName"] = lastName;
                TempData["email"] = email;
                TempData["phone"] = phone;
                TempData["departments"] = departments;

                switch (result)
                {
                    case UserServiceResult.PasswordMismatch:
                        TempData["contraseñaIncorrecta"] = "The passwords do not match";
                        break;
                    case UserServiceResult.EmailAlreadyExists:
                        TempData["repeatEmail"] = "The email already exists for another user";
                        break;
                    case UserServiceResult.InvalidDepartment:
                        TempData["seleccionarDepartamento"] = "You must select a department";
                        break;
                }

                var departmentsList = _db.Departaments.ToList();
                return View("SignupForm", departmentsList);
            }
          
        }

        /// <summary>
        /// Displays the login form view.
        /// </summary>
        /// <returns>The view containing the login form.</returns>
        public ActionResult LoginForm()
        {
            return View();
        }

        /// <summary>
        /// Handles the user login form submission.
        /// </summary>
        /// <param name="form">The form collection containing user login credentials.</param>
        /// <returns>If successful, redirects to the Dashboard view with the user ID. If there are errors, redisplay the LoginForm view with an appropriate error message.</returns>
        [HttpPost]
        public ActionResult Login(FormCollection form)
        {
            // Retrieve email and password from the login form.
            string email = form["email"];
            string password = form["pass1"];

            // Check if the provided credentials match any user in the database.
            var user = _db.Users.FirstOrDefault(u => u.email == email && u.password == password);

            if (user != null)
            {
                // User found, continue with the login process.

                // Get the role ID from the User_RolAndDepartment table to find the user's role.
                User_RolAndDepartment idRolUser = _db.User_RolAndDepartment.Where(x => x.fk_id_user == user.id).FirstOrDefault();

                // Get the user's role using the RoleService.
                string rolName = _roleService.GetUserRoleName(idRolUser.fk_id_user);

                Console.WriteLine(rolName);

                // Store the user's role name in the session.
                Session["role"] = rolName;

                if (Session["role"] == null)
                {
                    // Failed to retrieve role, redirect to the LoginForm view.
                    return View("LoginForm");
                }
                else
                {
                    // Successful login, redirect to the Dashboard view with the user ID.
                    int userId = user.id;
                    return RedirectToAction("Dashboard", "User", new { userId });
                }
            }
            else
            {
                // Invalid credentials, store an error message and redisplay the LoginForm view.
                TempData["loginError"] = "Invalid credentials";
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