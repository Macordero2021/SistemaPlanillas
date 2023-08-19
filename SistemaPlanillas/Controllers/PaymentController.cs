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
    public class PaymentController : Controller
    {
        private DataBase1Config _db = new DataBase1Config();

        public ActionResult storePaymentMensual(FormCollection form)
        {

            var idUserLogin = form["idUserLogin"];
            var idUserEdit = form["idUserEdit"];
            var diasSeleccionados = form["diasSeleccionados"];
            var note = form["note"];
            var montoPago = form["montoPago"];



            Monthly_payroll storeMonthly = new Monthly_payroll
            {
                fk_iduser = int.Parse(idUserEdit),
                total_workDays = int.Parse(diasSeleccionados),
                notes = note,
                total_earnings = decimal.Parse(montoPago)
            };
            _db.Monthly_payroll.Add(storeMonthly);
            _db.SaveChanges();


            return RedirectToAction("PayrollSubModule", "Payroll", new { idUserLogin = idUserLogin });

        }

        public ActionResult storePaymentHoras(FormCollection form)
        {
            var idUserLogin = form["idUserLogin"];
            var idUserEdit = form["idUserEdit"];
            var horasTrabajadas = form["horasTrabajadas"];
            var note = form["note"];
            var montoPago = form["montoPago"];

            return View();
        }


        public ActionResult formula(int dias, decimal salary, decimal deducciones, decimal pagosExtraordinarios)
        {

            var total = (salary / 22)* dias + pagosExtraordinarios - deducciones;

            // Crear un objeto anónimo o una clase para almacenar el resultado
            var result = new { total };

            // Devolver el resultado en formato JSON
            return Json(result, JsonRequestBehavior.AllowGet);

        }

    }
}

