﻿using SistemaPlanillas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;

namespace SistemaPlanillas.Controllers
{
    public class LicenceController : Controller
    {

        private DataBase1Config _db = new DataBase1Config();

        public ActionResult HolidaysModule(int userId)
        {

            ViewBag.idUserLogin = userId;

            List<Users> users= _db.Users.ToList();

            var usersWithInfo = (from user in users
                                 join userHolidays in _db.UserHolidays on user.id equals userHolidays.fk_id_user
                                 select new UserCompositeModel
                                 {
                                     User = user,
                                     UserHolidays = userHolidays,
                                 }).ToList();


            //get the department of the logged user
            Users userModel = _db.Users.Where(x => x.id == userId).FirstOrDefault();
            var department = _db.Departaments.Where(x => x.id == userModel.Fk_Id_Deparment).FirstOrDefault();
            ViewBag.UserDept = department.name_departament;


            return View(usersWithInfo);
        }

        public ActionResult EditUserHoliday(int idUserLogin, int idUserEdit)
        {
            UserHolidays idEditUserHoliday = _db.UserHolidays.Where(x => x.fk_id_user == idUserEdit).FirstOrDefault();

            //traer el email
            Users user = _db.Users.Where(x => x.id == idUserEdit).FirstOrDefault();
            UserHolidays Holidays = _db.UserHolidays.Where(x => x.fk_id_user == idUserEdit).FirstOrDefault();

            // Create a view model containing all the retrieved lists and pass it to the view.
            UserCompositeModel viewModel = new UserCompositeModel
            {
                User = user,
                UserHolidays = Holidays
            };

            Users userModel = _db.Users.Where(x => x.id == idUserLogin).FirstOrDefault();
            var department = _db.Departaments.Where(x => x.id == userModel.Fk_Id_Deparment).FirstOrDefault();
            ViewBag.UserDept = department.name_departament;

            ViewBag.idUserLogin = idUserLogin;

            return View(viewModel);
        }

        public ActionResult storeEditUserHolidays(int idUserLogin, int idUserEdit, string Holidays)
        {

            UserHolidays NewHolidays = _db.UserHolidays.Where(x => x.fk_id_user == idUserEdit).FirstOrDefault();

            NewHolidays.Holidays = Holidays;

            _db.SaveChanges();
            return RedirectToAction("HolidaysModule", new { userId = idUserLogin});
        }
    }
}