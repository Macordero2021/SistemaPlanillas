using SistemaPlanillas.Controllers.Services;
using SistemaPlanillas.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using static SistemaPlanillas.Controllers.Services.RoleService;

namespace SistemaPlanillas.Controllers
{
    public class PaymentController : Controller
    {
        private DataBase1Config _db = new DataBase1Config();

        [HttpPost]
        public ActionResult storeMonthlyPayment(int idUserLogin, int idUserEdit, string selectedDays, string note, string calculatedAmount)
        {
            Monthly_payroll storeMonthly = new Monthly_payroll
            {
                fk_iduser = idUserEdit,
                total_workDays = int.Parse(selectedDays),
                notes = note,
                total_earnings = decimal.Parse(calculatedAmount),
                Payment_Status = "Pending"
            };
            _db.Monthly_payroll.Add(storeMonthly);
            _db.SaveChanges();

            return RedirectToAction("PayrollSubModule", "Payroll", new { idUserLogin = idUserLogin });
        }

        [HttpPost]
        public ActionResult StoreHourlyPayment(int idUserLogin, int idUserEdit, string dayWorked, int hoursWorked, string note, decimal hourlyCalculatedAmount)
        {
            // Convertir la fecha de yyyy-MM-dd a dd/MM/yyyy
            DateTime parsedDate = DateTime.ParseExact(dayWorked, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            string formattedDate = parsedDate.ToString("dd/MM/yyyy");

            hourly_payroll hourlyPayment = new hourly_payroll
            {
                fk_iduser = idUserEdit,
                work_day = formattedDate,
                worked_hours = hoursWorked,
                notes = note,
                total_earnings = hourlyCalculatedAmount,
                Payment_Status = "Pending"
            };
            _db.hourly_payroll.Add(hourlyPayment);
            _db.SaveChanges();

            return RedirectToAction("PayrollSubModule", "Payroll", new { idUserLogin = idUserLogin });
        }

        public ActionResult approveMonthyPayment(int idUserEdit, int idUserLogin, int idMonthly, string SalaryType)
        {
            var amount = _db.Monthly_payroll.Where(x => x.id_monthly_payroll == idMonthly).Select(x => x.total_earnings).SingleOrDefault();

            
            Payroll_history approveMonthly = new Payroll_history
            {
                fk_idUser = idUserEdit,
                payment_day = DateTime.Now.ToString("dd/MM/yyyy"),
                payment_amount = amount,
                fk_id_payment = idMonthly,
                payment_type = SalaryType
            };
           _db.Payroll_history.Add(approveMonthly);
           _db.SaveChanges();

            Monthly_payroll editStatusPayment = _db.Monthly_payroll.Where(x => x.id_monthly_payroll == idMonthly).FirstOrDefault();
            editStatusPayment.Payment_Status = "Approved";
            _db.SaveChanges();


            return RedirectToAction("PayReportView", "Payroll", new { idUserLogin = idUserLogin, idUserEdit = idUserEdit, salaryType = SalaryType });
        }

    }
}

