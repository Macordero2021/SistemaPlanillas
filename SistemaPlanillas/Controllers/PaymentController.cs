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
            var horasTrabajadas = form["horasTrabajadas"];
            var note = form["note"];
            var montoPago = form["montoPago"];



            return View();
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


    }
}

