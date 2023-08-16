﻿using SistemaPlanillas.Controllers.Services;
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

            if (NewPaymentMethod != "Choose a new PaymentMethod")
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

        public ActionResult PayrollModule(int userId, string nameOrEmail)
        {
            // Verificar si nameOrEmail no es nulo y luego eliminar los espacios en blanco del inicio y final
            if (nameOrEmail != null)
            {
                nameOrEmail = nameOrEmail.Trim();
            }

            List<Users> userList;
            if (string.IsNullOrEmpty(nameOrEmail))
            {
                // Retrieve all users if no nameOrEmail provided.
                userList = _db.Users.ToList();
            }
            else
            {
                // Retrieve users matching the provided nameOrEmail.
                userList = _db.Users.Where(x => x.name.Contains(nameOrEmail) || x.email.Contains(nameOrEmail)).ToList();
            }

            // Get the department of the logged user
            Users userDept = _db.Users.Where(x => x.id == userId).FirstOrDefault();
            var department = _db.Departaments.Where(x => x.id == userDept.Fk_Id_Deparment).FirstOrDefault();
            ViewBag.UserDept = department.name_departament;

            // Get the id of the logged-in user from the URL and store it in the ViewBag to be used in the view.
            ViewBag.userId = userId;
            return View(userList);
        }

        public ActionResult DeductionsModule(int idUserLogin, int idUserEdit)
        {
            // Retrieve the user matching the idUserEdit received
            List<Users> users = _db.Users.Where(x => x.id == idUserEdit).ToList();

            var usersWithDeductions = (from user in users
                                       join deduction in _db.Deductions on user.id equals deduction.fk_idUser
                                       join deductionType in _db.Deduction_type on deduction.fk_idDeductionType equals deductionType.id_Deduction_type
                                       select new UserCompositeModel
                                       {
                                           User = user,
                                           deductions = deduction,
                                           deduction_Type = deductionType
                                       }).ToList();

            // Get the department of the logged user
            Users userModel = _db.Users.Where(x => x.id == idUserLogin).FirstOrDefault();
            var department = _db.Departaments.Where(x => x.id == userModel.Fk_Id_Deparment).FirstOrDefault();

            ViewBag.UserDept = department.name_departament;

            // Get the id of the logged-in user and the user to edit
            ViewBag.idUserLogin = idUserLogin;
            ViewBag.idUserEdit = idUserEdit;

            return View(usersWithDeductions);
        }

        public ActionResult CreateDeductionForm(int idUserLogin, int idUserEdit)
        {
            // Retrieve lists of roles, users, user-roles-departments, and user statuses from the database.
            Users user = _db.Users.Where(x => x.id == idUserEdit).FirstOrDefault();
            List<Deduction_type> deductionsType = _db.Deduction_type.ToList();  

            // Create a view model containing all the retrieved lists and pass it to the view.
            UserCompositeModel viewModel = new UserCompositeModel
            {
                User = user,
                deduction_TypeList = deductionsType,
            };

            // Get the department of the logged user
            Users userModel = _db.Users.Where(x => x.id == idUserLogin).FirstOrDefault();
            var department = _db.Departaments.Where(x => x.id == userModel.Fk_Id_Deparment).FirstOrDefault();
            ViewBag.UserDept = department.name_departament;

            // Get the id of the logged-in user
            ViewBag.idUserLogin = idUserLogin;

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult storeDeduction(string description, string typeDeductions, string amount, int idUserLogin, int idUserEdit)
        {
            if (description == "" || typeDeductions == "Choose a Deduction type" || amount == "")
            {
                //returnar a la vista de nuevo a llevar este campo
                //mandan un temp diciendo que debe llenar los campos
                return RedirectToAction("CreateDeductionForm", new { idUserLogin = idUserLogin, idUserEdit = idUserEdit });
            }
            else
            {
                var user = _db.Users.Where(x => x.id == idUserEdit).FirstOrDefault();
                var idDeductionType = _db.Deduction_type.Where(x => x.deduction_name == typeDeductions).FirstOrDefault();

                Deductions storeDeduction = new Deductions
                {
                    fk_idUser = user.id,
                    notes = description,
                    fk_idDeductionType = idDeductionType.id_Deduction_type,
                    deduction_value = decimal.Parse(amount)
                };
                _db.Deductions.Add(storeDeduction);
                _db.SaveChanges();

                TempData["CreateStatus"] = "success";
                return RedirectToAction("DeductionsModule", new { idUserLogin = idUserLogin, idUserEdit = idUserEdit });
            }
        }

        public ActionResult EditDeductionForm(int idUserLogin, int idDeductEdit)
        {
            //el id de la tabla de deductions del que se va editar
            var editDeduction = _db.Deductions.Where(x => x.id_deduction == idDeductEdit).FirstOrDefault();

            //traer el email
            Users user = _db.Users.Where(x => x.id == editDeduction.fk_idUser).FirstOrDefault();
            //pasar el deductions para la descripcion
            Deductions deductions = _db.Deductions.Where(x => x.id_deduction == editDeduction.id_deduction).FirstOrDefault();
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

            Users userModel = _db.Users.Where(x => x.id == idUserLogin).FirstOrDefault();
            var department = _db.Departaments.Where(x => x.id == userModel.Fk_Id_Deparment).FirstOrDefault();
            ViewBag.UserDept = department.name_departament;

            ViewBag.idUserLogin = idUserLogin;

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult StoreEditDeduction(string description, string typeDeductions, string amount, int idUserLogin, int idDeductionEdit, int idUserEdit)
        {
            if (description == "" || typeDeductions == "Choose a Deduction type" || amount == "")
            {
                //returnar a la vista de nuevo a llevar este campo

                //mandan un temp diciendo que debe llenar los campos
                return RedirectToAction("EditDeductionForm", new { idUserLogin = idUserLogin, idUserEdit = idUserEdit });
            }
            else
            {
                //traer el id nuevo tipo de deduccion
                var deductionTypeId = _db.Deduction_type.Where(x => x.deduction_name == typeDeductions).FirstOrDefault();

                Deductions deductionEditStore = _db.Deductions.Where(x => x.id_deduction == idDeductionEdit).FirstOrDefault();
                deductionEditStore.notes = description;
                deductionEditStore.fk_idDeductionType = deductionTypeId.id_Deduction_type;
                deductionEditStore.deduction_value = decimal.Parse(amount);

                _db.SaveChanges();

                TempData["UpdateStatus"] = "success";
                return RedirectToAction("DeductionsModule", new { idUserLogin = idUserLogin, idUserEdit = idUserEdit });
            }
        }

        public ActionResult deleteDeduction(int idUserLogin, int idUserEdit, int idDeductDelete)
        {
            Deductions deleteDeduction = _db.Deductions.Where(x => x.id_deduction == idDeductDelete).FirstOrDefault();
            _db.Deductions.Remove(deleteDeduction);
            _db.SaveChanges();

            TempData["DeleteStatus"] = "success";
            return RedirectToAction("DeductionsModule", new { idUserLogin = idUserLogin, idUserEdit = idUserEdit });
        }

        public ActionResult ExtraPayModule(int idUserLogin, int idUserEdit)
        {
            // Retrieve the user matching the idUserEdit received
            List<Users> users = _db.Users.Where(x => x.id == idUserEdit).ToList();

            var usersWithExtraordinaryPayment = (from user in users
                                                 join extraordinaryPayment in _db.Extraordinary_payment on user.id equals extraordinaryPayment.fk_idUser
                                                 join paymentType in _db.payment_type on extraordinaryPayment.fk_id_payment equals paymentType.id_payment
                                                 select new UserCompositeModel
                                                 {
                                                     User = user,
                                                     Extraordinary_Payment = extraordinaryPayment,
                                                     Payment_Type = paymentType
                                                 }).ToList();

            //get the department of the logged user
            Users userModel = _db.Users.Where(x => x.id == idUserLogin).FirstOrDefault();
            var department = _db.Departaments.Where(x => x.id == userModel.Fk_Id_Deparment).FirstOrDefault();
            ViewBag.UserDept = department.name_departament;

            // Get the id of the logged-in user and the user to edit
            ViewBag.idUserLogin = idUserLogin;
            ViewBag.idUserEdit = idUserEdit;

            return View(usersWithExtraordinaryPayment);
        }

        public ActionResult CreateExtraPaymentForm(int idUserLogin, int idUserEdit)
        {
            // Retrieve lists of roles, users, user-roles-departments, and user statuses from the database.
            Users user = _db.Users.Where(x => x.id == idUserEdit).FirstOrDefault();
            List<payment_type> extraPaymentType = _db.payment_type.ToList();


            // Create a view model containing all the retrieved lists and pass it to the view.
            UserCompositeModel viewModel = new UserCompositeModel
            {
                User = user,
                Payment_TypeList = extraPaymentType,
            };

            // Get the department of the logged user
            Users userModel = _db.Users.Where(x => x.id == idUserLogin).FirstOrDefault();
            var department = _db.Departaments.Where(x => x.id == userModel.Fk_Id_Deparment).FirstOrDefault();
            ViewBag.UserDept = department.name_departament;

            // Get the id of the logged-in user
            ViewBag.idUserLogin = idUserLogin;

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult storeExtraPayment(string description, string typePayment, string amount, int idUserLogin, int idUserEdit)
        {
            if (description == "" || typePayment == "Choose a extraordinary payment type" || amount == "")
            {
                //returnar a la vista de nuevo a llevar este campo
                //mandan un temp diciendo que debe llenar los campos
                return RedirectToAction("CreateExtraPaymentForm", new { idUserLogin = idUserLogin, idUserEdit = idUserEdit });
            }
            else
            {
                var user = _db.Users.Where(x => x.id == idUserEdit).FirstOrDefault();
                //buscar el id del tipo de extraordinario pago
                var idExtraPaymentType = _db.payment_type.Where(x => x.payment_name == typePayment).FirstOrDefault();

                Extraordinary_payment storeExtraOrdinaryPayment = new Extraordinary_payment
                {
                    fk_idUser = user.id,
                    notes = description,
                    fk_id_payment = idExtraPaymentType.id_payment,
                    payment_value = decimal.Parse(amount)
                };
                _db.Extraordinary_payment.Add(storeExtraOrdinaryPayment);
                _db.SaveChanges();

                TempData["CreateStatus"] = "success";
                return RedirectToAction("ExtraPayModule", new { idUserLogin = idUserLogin, idUserEdit = idUserEdit });
            }
        }

        public ActionResult EditExtraPaymentForm(int idUserLogin, int idExtraEdit)
        {
            //el id de la tabla de extraPayment del que se va editar
            var editExtraPayment = _db.Extraordinary_payment.Where(x => x.id_extraordinary_payment == idExtraEdit).FirstOrDefault();

            //traer el "Users" para usar el email
            Users user = _db.Users.Where(x => x.id == editExtraPayment.fk_idUser).FirstOrDefault();
            //traer el "Extraordinary_payment" para usar el email pasar el nota
            Extraordinary_payment extraPaymentNote = _db.Extraordinary_payment.Where(x => x.id_extraordinary_payment == editExtraPayment.id_extraordinary_payment).FirstOrDefault();
            // Retrieve lists of roles, users, user-roles-departments, and user statuses from the database.
            List<payment_type> extraPaymentsType = _db.payment_type.ToList();//aqui paso los tipos de payment
            payment_type extraPaymentTypeActual = _db.payment_type.Where(x => x.id_payment == editExtraPayment.fk_id_payment).FirstOrDefault();

            // Create a view model containing all the retrieved lists and pass it to the view.
            UserCompositeModel viewModel = new UserCompositeModel
            {
                Payment_TypeList = extraPaymentsType,
                User = user,
                Extraordinary_Payment = extraPaymentNote,
                Payment_Type = extraPaymentTypeActual 
            };

            Users userModel = _db.Users.Where(x => x.id == idUserLogin).FirstOrDefault();
            var department = _db.Departaments.Where(x => x.id == userModel.Fk_Id_Deparment).FirstOrDefault();
            ViewBag.UserDept = department.name_departament;

            ViewBag.idUserLogin = idUserLogin;

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult storeEditExtraPayment(string description, string typePayment, string amount, int idUserLogin, int idExtraEdit, int idUserEdit)
        {
            if (description == "" || amount == "")
            {
                //returnar a la vista de nuevo a llevar este campo

                //mandan un temp diciendo que debe llenar los campos
                return RedirectToAction("EditExtraPaymentForm", new { idUserLogin = idUserLogin, idUserEdit = idUserEdit });
            }
            else
            {
                //traer el id nuevo tipo de extra payment
                var extraPaymentTypeId = _db.payment_type.Where(x => x.payment_name == typePayment).FirstOrDefault();

                Extraordinary_payment extraPaymentEditStore = _db.Extraordinary_payment.Where(x => x.id_extraordinary_payment == idExtraEdit).FirstOrDefault();
                extraPaymentEditStore.notes = description;
                extraPaymentEditStore.fk_id_payment = extraPaymentTypeId.id_payment;
                extraPaymentEditStore.payment_value = decimal.Parse(amount);

                _db.SaveChanges();

                TempData["UpdateStatus"] = "success";
                return RedirectToAction("ExtraPayModule", new { idUserLogin = idUserLogin, idUserEdit = idUserEdit });
            }
        }
        public ActionResult deleteExtraPayment(int idUserLogin, int idUserEdit, int idExtraDelete)
        {
            Extraordinary_payment deleteExtraPayment = _db.Extraordinary_payment.Where(x => x.id_extraordinary_payment == idExtraDelete).FirstOrDefault();
            _db.Extraordinary_payment.Remove(deleteExtraPayment);
            _db.SaveChanges();

            TempData["DeleteStatus"] = "success";
            return RedirectToAction("ExtraPayModule", new { idUserLogin = idUserLogin, idUserEdit = idUserEdit });
        }

        public ActionResult PayrollSubModule(int idUserLogin, int idUserEdit)
        {
            // Retrieve the user matching the idUserEdit received
            List<Users> users = _db.Users.Where(x => x.id == idUserEdit).ToList();

            //get the department of the logged user
            Users userModel = _db.Users.Where(x => x.id == idUserLogin).FirstOrDefault();
            var department = _db.Departaments.Where(x => x.id == userModel.Fk_Id_Deparment).FirstOrDefault();
            ViewBag.UserDept = department.name_departament;

            // Get the id of the logged-in user and the user to edit
            ViewBag.idUserLogin = idUserLogin;
            ViewBag.idUserEdit = idUserEdit;

            return View();
        }

    }
}

