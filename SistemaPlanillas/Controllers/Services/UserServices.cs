using SistemaPlanillas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaPlanillas.Controllers.Services
{
    /// <summary>
    /// Represents the possible results of the UserService's operations.
    /// </summary>
    public enum UserServiceResult
    {
        Success,                // The operation was successful.
        PasswordMismatch,       // The provided passwords do not match.
        EmailAlreadyExists,     // The email address already exists for another user.
        InvalidDepartment       // An invalid department was selected during user creation.
    }

    public class UserService
    {
        private DataBase1Config _db;

        /// <summary>
        /// Initializes a new instance of the UserService class with the provided database configuration.
        /// </summary>
        /// <param name="db">The database configuration used to access user-related data.</param>
        public UserService(DataBase1Config db)
        {
            _db = db;
        }

        /// <summary>
        /// Creates a new user in the database.
        /// </summary>
        /// <param name="name">User's first name.</param>
        /// <param name="lastName">User's last name.</param>
        /// <param name="email">User's email address.</param>
        /// <param name="phone">User's phone number.</param>
        /// <param name="pass1">User's password.</param>
        /// <param name="pass2">Confirmation of user's password.</param>
        /// <param name="department">Department selected by the user.</param>
        /// <returns>True if the user is created successfully, otherwise, returns false.</returns>
        public UserServiceResult CreateNewUser(string name, string lastName, string email, string phone, string pass1, string pass2, string department)
        {
            // Validate passwords
            if (pass1 != pass2)
            {
                return UserServiceResult.PasswordMismatch;
            }

            // Check if the email already exists for another user
            var existingUser = _db.Users.Any(u => u.email == email);
            if (existingUser)
            {
                return UserServiceResult.EmailAlreadyExists;
            }

            // Check if a valid department has been selected
            if (department == "Choose a department")
            {
                return UserServiceResult.InvalidDepartment;
            }

            // Get the current date and time
            DateTime fechaActual = DateTime.Now;

            //Get the id of the department
            int idDepartment = _db.Departaments.FirstOrDefault(d => d.name_departament == department).id;

            // Save the user
            Users newUser = new Users
            {
                name = name,
                lastname = lastName,
                email = email,
                phone = phone,
                password = pass1,
                fk_id_status = 1,
                fk_salary = 1,
                fk_id_paymentmethod = 1,
                Fk_Id_Deparment = idDepartment
            };

            _db.Users.Add(newUser);
            _db.SaveChanges();

            // Code to save data in the User_Updates table
            var lastUser = _db.Users.OrderByDescending(u => u.id).FirstOrDefault();
            var idLastUser = lastUser.id;
            User_Updates storeDates = new User_Updates
            {
                fk_user_create = idLastUser,
                id_updateuser = idLastUser,
                date_create = fechaActual,
                date_update = fechaActual
            };

            _db.User_Updates.Add(storeDates);
            _db.SaveChanges();

            // Code to save the department and role of the created user
            var departamentFind = _db.Departaments.FirstOrDefault(d => d.name_departament == department);
            if (departamentFind != null)
            {
                User_RolAndDepartment storeRolesDepartament = new User_RolAndDepartment
                {
                    fk_id_user = idLastUser,
                    fk_id_departament = departamentFind.id,
                    fk_id_rol = 1
                };

                _db.User_RolAndDepartment.Add(storeRolesDepartament);
                _db.SaveChanges();
            }

            return UserServiceResult.Success;
        }

    }
}