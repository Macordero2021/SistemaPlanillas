using SistemaPlanillas.Models;
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
            Users user = _db.Users.Where(x => x.id == userId).FirstOrDefault();
            return View(user);
        }
    }
}