using SistemaPlanillas.Models;
using SistemaPlanillas.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaPlanillas.Controllers
{
    public class EmployeeController : Controller
    {
        private DataBase1Config _db = new DataBase1Config();

        public ActionResult ConsultModule(int userId)
        {
            // Retrieve the user matching the idUserEdit received
            Users user = _db.Users.Where(x => x.id == userId).FirstOrDefault();

            var userSalary = (
                from salary_ in _db.Salary
                join salaryType_ in _db.Salary_Type on salary_.fk_salary_type equals salaryType_.id
                where salary_.fk_user == userId
                select new
                {
                    SalaryAmount = salary_.SalaryAmount,
                    SalaryType = salaryType_.SalaryType
                }).FirstOrDefault();
            string salaryType = userSalary.SalaryType;
            //mandar el salario del usuario que se va editar
            var salary = _db.Salary.Where(x => x.fk_user == userId).Select(x => x.SalaryAmount).FirstOrDefault();
            //traer todas las deducciones del usuario que se esta editando
            var deductions = _db.Deductions.Where(x => x.fk_idUser == userId).Sum(x => x.deduction_value);
            //traer todos los pagos extraordinarios
            var paymentsExtraordinary = _db.Extraordinary_payment.Where(x => x.fk_idUser == userId).Sum(x => x.payment_value);

            // Create an instance of PayrollViewModel
            var usersWithInfo = new PayrollViewModel
            {
                User = user,
                SalaryType = salaryType,
                Salary = salary ?? 0,
                Deductions = deductions ?? 0,
                PaymentsExtraordinary = paymentsExtraordinary ?? 0
            };
            var monthlyPaymentCompositeModels = (
                from history in _db.Payroll_history
                where history.fk_idUser == userId
                select new MonthlyPaymentCompositeModel
                {
                    History = history,
                    MonthlyPayroll = _db.Monthly_payroll.FirstOrDefault(monthly => monthly.id_monthly_payroll.ToString() == history.id_payment)
                }).ToList();

            var hourlyPaymentCompositeModels = (
                from history in _db.Payroll_history
                where history.fk_idUser == userId
                select new HourlyPaymentCompositeModel
                {
                    History = history,
                    HourlyPayrolls = _db.hourly_payroll
                        .Where(hourly => hourly.Payment_Status == "Approved" && hourly.work_day.EndsWith(history.id_payment))
                        .OrderBy(hourly => hourly.work_day)
                        .ToList()
                }).ToList();

            var viewModel = new MultipleModels
            {
                UsersWithInfo = usersWithInfo,
                MonthlyPayments = monthlyPaymentCompositeModels,
                HourlyPayments = hourlyPaymentCompositeModels
            };

            //get the department of the logged user
            Users userModel = _db.Users.Where(x => x.id == userId).FirstOrDefault();
            var department = _db.Departaments.Where(x => x.id == userModel.Fk_Id_Deparment).FirstOrDefault();
            ViewBag.UserDept = department.name_departament;

            // Get the id of the logged-in user and the user to edit
            ViewBag.idUserLogin = userId;

            return View(viewModel);
        }
    }
}