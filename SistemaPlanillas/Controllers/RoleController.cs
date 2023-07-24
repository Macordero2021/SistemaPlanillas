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

        private DataBase1Config _db = new DataBase1Config();
        public ActionResult RolConfig()
        {
            var Roles = _db.Roles.ToList();
            return View("RolConfig",Roles);
        }


        [HttpPost]
        public ActionResult StoreRol(FormCollection form)
        {
            // Retrieve nameRol input from the form.
            string nameRol = form["rol"];

            //primero traer la lista de los roles 
            var Roles = _db.Roles.ToList();


            //si la lista ersta vacia Guardo el nombre del rol
            if (Roles == null) {
                Roles storeRol = new Roles();
                storeRol.name_rol = nameRol;
                _db.Roles.Add(storeRol);
                _db.SaveChanges();
            }
            else
            {
                //verificar si el nombre del rol ya existe
                var existe = 0;

                foreach (var item in Roles)
                {
                    if (item.name_rol == nameRol)
                    {
                        existe = 1;
                        break;
                    }
                }

                //no se repite el nombre 
                if (existe == 0) {
                    Roles storeRol = new Roles();
                    storeRol.name_rol = nameRol;
                    _db.Roles.Add(storeRol);
                    _db.SaveChanges();
                }
                else
                {
                    //debo returnar a la vista de .con un temp diciendo que ya se repite ese nombre del rol
                    TempData["repeatNameRol"] = "El nombre del rol ya existe";

                    return RedirectToAction("RolConfig");
                }

            }

        }


        }
       
}