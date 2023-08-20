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


using System;
using System.Globalization;
using System.Linq;

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
        public ActionResult StoreHourlyPayment(int idUserLogin, int idUserEdit, string dayWorked, int hoursWorked, string note, string hourlyCalculatedAmount)
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
                total_earnings = decimal.Parse(hourlyCalculatedAmount),
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

        public ActionResult declineMonthyPayment(int idUserEdit, int idUserLogin, int idMonthly, string SalaryType)
        {
            // Buscar el registro a eliminar
            Monthly_payroll monthlyPaymentToDelete = _db.Monthly_payroll.Find(idMonthly);
            _db.Monthly_payroll.Remove(monthlyPaymentToDelete);
            _db.SaveChanges();

            return RedirectToAction("PayReportView", "Payroll", new { idUserLogin = idUserLogin, idUserEdit = idUserEdit, salaryType = SalaryType });
        }

        public ActionResult approveHourlyPayment(int idUserEdit, int idUserLogin, string SalaryType)
        {
            var total_earnings = _db.hourly_payroll.Where(x => x.fk_iduser == idUserEdit).Select(x => x.total_earnings).Sum();

            int selectedMonth = Convert.ToInt32(Request["selectedMonth"]);

            // Obtener el mes de la fecha como entero
            int currentMonth = selectedMonth;

            Payroll_history approveMonthly = new Payroll_history
            {
                fk_idUser = idUserEdit,
                payment_day = DateTime.Now.ToString("dd/MM/yyyy"),
                payment_amount = total_earnings,
                fk_id_payment = currentMonth,
                payment_type = SalaryType
            };
            _db.Payroll_history.Add(approveMonthly);
            _db.SaveChanges();

            var timeList = _db.hourly_payroll.Where(x => x.fk_iduser == idUserEdit && x.Payment_Status == "Pending").ToList();

            foreach (var hourlyPayroll in timeList)
            {
                // Convertir el string a un objeto DateTime utilizando ParseExact
                DateTime date2 = DateTime.ParseExact(hourlyPayroll.work_day, "dd/MM/yyyy", null);
                // Obtener el mes de la fecha como entero
                int currentMonth2 = date2.Month;
                if (currentMonth2 == currentMonth)
                {
                    hourlyPayroll.Payment_Status = "Approved";
                    _db.SaveChanges();
                }
            }

            return RedirectToAction("PayReportView", "Payroll", new { idUserLogin = idUserLogin, idUserEdit = idUserEdit, salaryType = SalaryType });
        }

        public ActionResult declineHourlyPayment(int idUserEdit, int idUserLogin, int idHourly, string SalaryType)
        {
            // Buscar el registro a eliminar
            hourly_payroll hourlyPaymentToDelete = _db.hourly_payroll.Find(idHourly);
            _db.hourly_payroll.Remove(hourlyPaymentToDelete);
            _db.SaveChanges();
            return RedirectToAction("PayReportView", "Payroll", new { idUserLogin = idUserLogin, idUserEdit = idUserEdit, salaryType = SalaryType });
        }

    }
}

