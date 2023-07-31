using SistemaPlanillas.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;


namespace SistemaPlanillas.Controllers
{
    public class RoleController : Controller
    {

        private DataBase1Config _db = new DataBase1Config();
        public ActionResult RolConfig(int id)
        {

            var idModel = id.ToString();
            ViewBag.idModel = idModel;


            var Roles = _db.Roles.ToList();
            return View("RolConfig",Roles);
        }


        [HttpPost]
        public ActionResult StoreRol(FormCollection form,int id)
        {
            var idModel = id;


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

                TempData["successRol"] = "El rol se guardo correctamente";

                return RedirectToAction("RolConfig", new { id = idModel });
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

                    TempData["successRol"] = "El rol se guardo correctamente";

                    return RedirectToAction("RolConfig", new { id = idModel });
                }
                else
                {
                    //debo returnar a la vista de .con un temp diciendo que ya se repite ese nombre del rol
                    TempData["repeatNameRol"] = "El nombre del rol ya existe";

                    return RedirectToAction("RolConfig", new { id = idModel });
                }

            }

        }

        [HttpPost]
        public ActionResult DeleteRol(FormCollection form)
        {

            var idModel = form["idUser"];

            // Retrieve idRol input from the form.
            string idRol = form["id"];
            var idInt = int.Parse(idRol);

            Roles deleteRol = _db.Roles.Where(x => x.id == idInt).FirstOrDefault(); 
            _db.Roles.Remove(deleteRol);
            _db.SaveChanges();

            TempData["deleteSuccess"] = "Rol eliminado correctamente";

            return RedirectToAction("RolConfig", new { id = idModel });
        }

        public ActionResult FormEditRol()
        {
            // Obtener los valores enviados mediante GET desde el formulario
            var idModel = Request.QueryString["idUser"];
            ViewBag.idModel = idModel;

            var idRol = Request.QueryString["id"];
            var idInt = int.Parse(idRol);

            Roles editRol = _db.Roles.Where(x => x.id == idInt).FirstOrDefault();
           
            return View("EditRolConfig",editRol);

        }


        [HttpPost]
        public ActionResult StoreEditRol(FormCollection form, int id)
        {
            //es el de la url del usuario que esta logeado
            var idModel = id;


            // Retrieve nameRol input from the form.
            string newNameRol = form["rol"];
            string oldNameRol = form["oldRol"];


            if (newNameRol == oldNameRol) {

                return RedirectToAction("RolConfig", new { id = idModel });
            }
            else
            {
                //primero traer la lista de los roles 
                var Roles = _db.Roles.ToList();

                //verificar si el nombre del rol ya existe
                var existe = 0;

                foreach (var item in Roles)
                {
                    if (item.name_rol == newNameRol)
                    {
                        existe = 1;
                        break;
                    }
                }

                if (existe == 1) {

                    return RedirectToAction("RolConfig", new { id = idModel });
                }
                else
                {
                    Roles editRol = _db.Roles.Where(x => x.name_rol == oldNameRol).FirstOrDefault();
                    editRol.name_rol = newNameRol;  
                    _db.SaveChanges();

                    return RedirectToAction("RolConfig", new { id = idModel });

                }
            }   

        }

        public ActionResult AsingRol(int id)
        {
            List<Roles> roles = _db.Roles.ToList();
            List<Users> users = _db.Users.ToList();
            List<User_RolAndDepartment> RolesDeparmentsUser1 = _db.User_RolAndDepartment.ToList();
            List<User_Status> Status1 = _db.User_Status.ToList();


            modelCompuesto viewModel = new modelCompuesto
            {
                Role = roles,
                User = users,
                RoleDeparmentUser = RolesDeparmentsUser1,
                Status = Status1    
            };

            //es el de la url del usuario que esta logeado
            var idModel = id.ToString();
            ViewBag.idModel = idModel;


            return View("AsingRol", viewModel);
        }
    }      
}