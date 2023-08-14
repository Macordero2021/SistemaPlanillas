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

        public ActionResult PayrollModule(int id)
        {
            Users user = _db.Users.Where(x => x.id == id).FirstOrDefault();
            var department = _db.Departaments.Where(x => x.id == user.Fk_Id_Deparment).FirstOrDefault();

            ViewBag.UserDept = department.name_departament;
            return View(user);
        }

        public ActionResult DeductionsModule(int id, string nameOrEmail)
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

            var usersWithDeductions = (from user in users
                                       join deduction in _db.Deductions on user.id equals deduction.fk_idUser
                                       join deductionType in _db.Deduction_type on deduction.fk_idDeductionType equals deductionType.id_Deduction_type
                                       select new UserCompositeModel
                                       {
                                           User = user,
                                           deductions = deduction,
                                           deduction_Type = deductionType
                                       }).ToList();

            Users userModel = _db.Users.Where(x => x.id == id).FirstOrDefault();
            var department = _db.Departaments.Where(x => x.id == userModel.Fk_Id_Deparment).FirstOrDefault();

            ViewBag.UserDept = department.name_departament;

            // Get the id of the logged-in user from the URL and store it in the ViewBag to be used in the view.
            ViewBag.idModel = id.ToString();

            return View(usersWithDeductions);
        }

        public ActionResult ExtraPayModule(int id, string nameOrEmail)
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

            var usersWithExtraordinaryPayment = (from user in users
                                                 join extraordinaryPayment in _db.Extraordinary_payment on user.id equals extraordinaryPayment.fk_idUser
                                                 join paymentType in _db.payment_type on extraordinaryPayment.fk_id_payment equals paymentType.id_payment
                                                 select new UserCompositeModel
                                                 {
                                                     User = user,
                                                     Extraordinary_Payment = extraordinaryPayment,
                                                     Payment_Type = paymentType
                                                 }).ToList();

            Users userModel = _db.Users.Where(x => x.id == id).FirstOrDefault();
            var department = _db.Departaments.Where(x => x.id == userModel.Fk_Id_Deparment).FirstOrDefault();

            ViewBag.UserDept = department.name_departament;

            // Get the id of the logged-in user from the URL and store it in the ViewBag to be used in the view.
            ViewBag.idModel = id.ToString();

            return View(usersWithExtraordinaryPayment);
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
            var idModel = form["idUserAdmin"]; 

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

        public ActionResult CreateDeductionForm()
        {
            string idUserLogin = Request.QueryString["idUserLogin"];

            // Retrieve lists of roles, users, user-roles-departments, and user statuses from the database.
            List<Users> users = _db.Users.ToList();
            List<Deduction_type> deductionsType = _db.Deduction_type.ToList();  


            // Create a view model containing all the retrieved lists and pass it to the view.
            UserCompositeModel viewModel = new UserCompositeModel
            {
                UsersList = users,
                deduction_TypeList = deductionsType,
            };

            int idUserLoginInt = Convert.ToInt32(Request.QueryString["idUserLogin"]);
            Users userModel = _db.Users.Where(x => x.id == idUserLoginInt).FirstOrDefault();
            var department = _db.Departaments.Where(x => x.id == userModel.Fk_Id_Deparment).FirstOrDefault();

            ViewBag.UserDept = department.name_departament;

            ViewBag.idModel = idUserLogin;

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult storeDeduction(FormCollection form)
        {
            var email = form["email"];
            var descripcion = form["descripcion"];
            var typeDeductions = form["typeDeductions"];
            var amount = form["amount"];
            var idModel = form["id"];

            if (email == "Choose a use" || descripcion == "" || typeDeductions == "Choose a Deduction type" || amount == "")
            {
                //returnar a la vista de nuevo a llevar este campo
                return RedirectToAction("CreateDeductionForm", new { idUserLogin = idModel });
            }
            else
            {
                var user = _db.Users.Where(x => x.email == email).FirstOrDefault();
                var idDeductionType = _db.Deduction_type.Where(x => x.deduction_name == typeDeductions).FirstOrDefault();

                Deductions storeDeduction = new Deductions 
                {
                    fk_idUser = user.id ,
                    notes = descripcion,
                    fk_idDeductionType = idDeductionType.id_Deduction_type,
                    deduction_value = decimal.Parse(amount)
                };
                _db.Deductions.Add(storeDeduction);
                _db.SaveChanges();

                return RedirectToAction("DeductionsModule", new {id = idModel});
            }
        }

        public ActionResult deleteDeduction()
        {
            string idUserLogin = Request.QueryString["idUserLogin"];
            string idUserDelete = Request.QueryString["idUserDelete"];
            int idToDelete = int.Parse(idUserDelete);
            Deductions deleteDeduction = _db.Deductions.Where(x => x.id_deduction == idToDelete).FirstOrDefault();
            _db.Deductions.Remove(deleteDeduction);
            _db.SaveChanges();
            return RedirectToAction("DeductionsModule", new { id = idUserLogin });
        }

        public ActionResult EditDeductionForm()
        {
            string idUserLogin = Request.QueryString["idUserLogin"];
            string idUserEdit = Request.QueryString["idUserEdit"];
            int idToEdit = int.Parse(idUserEdit);

            //el id de la tabla de deductions del que se va editar
            var editDeduction = _db.Deductions.Where(x => x.id_deduction == idToEdit).FirstOrDefault();

            //traer el email
            Users user = _db.Users.Where(x => x.id == editDeduction.fk_idUser).FirstOrDefault();

            //pasar el nota
            Deductions deductions = _db.Deductions.Where(x => x.id_deduction == editDeduction.id_deduction).FirstOrDefault ();    

            // Retrieve lists of roles, users, user-roles-departments, and user statuses from the database.
            List<Deduction_type> deductionsType = _db.Deduction_type.ToList();

            Deduction_type deductionTypeActual = _db.Deduction_type.Where(x => x.id_Deduction_type == editDeduction.fk_idDeductionType).FirstOrDefault();

            // Create a view model containing all the retrieved lists and pass it to the view.
            UserCompositeModel viewModel = new UserCompositeModel
            {
                deduction_TypeList = deductionsType,
                User = user,
                deductions = deductions,
                deduction_Type = deductionTypeActual
            };

            int idUserLoginInt = Convert.ToInt32(Request.QueryString["idUserLogin"]);
            Users userModel = _db.Users.Where(x => x.id == idUserLoginInt).FirstOrDefault();
            var department = _db.Departaments.Where(x => x.id == userModel.Fk_Id_Deparment).FirstOrDefault();

            ViewBag.UserDept = department.name_departament;

            ViewBag.idModel = idUserLogin;

            return View(viewModel);
        }

    }
}

