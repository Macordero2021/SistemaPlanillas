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
    public class PayrollController : Controller
    {
        private DataBase1Config _db = new DataBase1Config();

        public ActionResult HumanResources(int id, string nameOrEmail)
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
                                 join salary in _db.Salary on user.id equals salary.fk_user
                                 join salaryType in _db.Salary_Type on salary.fk_salary_type equals salaryType.id
                                 select new UserCompositeModel
                                 {
                                     User = user,
                                     Department = dept,
                                     Status = status,
                                     Salary = salary,
                                     Salary_Type = salaryType
                                 }).ToList();

            // Get the id of the logged-in user from the URL and store it in the ViewBag to be used in the view.
            ViewBag.idModel = id.ToString();

            return View(usersWithInfo);
        }


        public ActionResult DDoPX(int id, string nameOrEmail)
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
                                 join salary in _db.Salary on user.id equals salary.fk_user
                                 join salaryType in _db.Salary_Type on salary.fk_salary_type equals salaryType.id
                                 select new UserCompositeModel
                                 {
                                     User = user,
                                     Department = dept,
                                     Status = status,
                                     Salary = salary,
                                     Salary_Type = salaryType
                                 }).ToList();

            // Get the id of the logged-in user from the URL and store it in the ViewBag to be used in the view.
            ViewBag.idModel = id.ToString();

            return View(usersWithInfo);
        }



        public ActionResult PayrollModule(int id)
        {
            Users user = _db.Users.Where(x => x.id == id).FirstOrDefault();
            return View(user);
        }

        public ActionResult EditHumanResources(int id)
        {
            string idUserLogin = Request.QueryString["idUserLogin"];

            // Retrieve lists of roles, users, user-roles-departments, and user statuses from the database.
            Users users = _db.Users.Where(x => x.id == id).FirstOrDefault();
            User_RolAndDepartment RoleDeparment = _db.User_RolAndDepartment.Where(x => x.fk_id_user == id).FirstOrDefault();
            User_Status Status1 = _db.User_Status.Where(x => x.id == users.fk_id_status).FirstOrDefault();
            List<User_Status> status2 = _db.User_Status.ToList();
            List<Departaments> departaments = _db.Departaments.ToList();
            List<Salary_Type> salary_Types = _db.Salary_Type.ToList();

            // Create a view model containing all the retrieved lists and pass it to the view.
            UserToEdit viewModel = new UserToEdit
            {
                userToEdit = users,
                statusToEdit = Status1,
                StatusList = status2,
                DepartamentsList = departaments,
                salary_Types = salary_Types
            };

            ViewBag.idModel = id.ToString();
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult StoreEditHumanForm(FormCollection form)
        {
            var idModel = form["idUserAdmin"]; ;

            // Retrieve the new and old names of the role from the form data.
            string NewDepartment = form["NewDepartments"];
            string NewStatus = form["NewStatus"];
            string NewPaymentMethod = form["NewPaymentMethod"];
            string NewSalary = form["NewSalary"];
            string idEdit = form["id"];
            int idEditInt = int.Parse(idEdit.ToString());

            // Retrieve the user's info to update
            Users UserId2 = _db.Users.Where(x => x.id == idEditInt).FirstOrDefault();

            if (NewDepartment != "Choose a new department")
            {
                var idDepartament = _db.Departaments.Where(x => x.name_departament == NewDepartment).FirstOrDefault();
                UserId2.Fk_Id_Deparment = idDepartament.id;

                var idDeparment2 = _db.User_RolAndDepartment.Where(x => x.fk_id_user == UserId2.id).FirstOrDefault();  
                idDeparment2.fk_id_departament = idDepartament.id;      

            }
            
            if (NewStatus != "Choose a new Status")
            {
                var idstatus = _db.User_Status.Where(x => x.name_status == NewStatus).FirstOrDefault();
                UserId2.fk_id_status = idstatus.id;
            }

            if(NewPaymentMethod != "Choose a new PaymentMethod")
            {
                //eliminar de la tabla de usuarios el campo fk_id_paymentmethod y luego eliminar la tabla que se llama Payment_Method
                //UserId2.fk_id_paymentmethod = salaryType.id;

                var salaryType = _db.Salary_Type.Where(x => x.SalaryType == NewPaymentMethod).FirstOrDefault();
                var typeSalaryTableSalary = _db.Salary.Where(x => x.fk_user == UserId2.id).FirstOrDefault();
                typeSalaryTableSalary.fk_salary_type = salaryType.id;
            }

            if (NewSalary != "")
            {
                var userTableSalary = _db.Salary.Where(x => x.fk_user == UserId2.id).FirstOrDefault();
                decimal salaryFloat = decimal.Parse(NewSalary);
                userTableSalary.SalaryAmount = salaryFloat;
            }

            _db.SaveChanges();
            return RedirectToAction("HumanResources", new { id = idModel, nameOrEmail = "" });
        }
    }
}

