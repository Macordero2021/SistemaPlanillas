using SistemaPlanillas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace SistemaPlanillas.Controllers
{
    public class RoleController : Controller
    {
        public ActionResult RolConfig()
        {
            return View("RolConfig");
        }
    }
       
}