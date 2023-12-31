﻿using SistemaPlanillas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaPlanillas.Controllers.Services
{
    public class RoleService
    {
        private DataBase1Config _db;

        public RoleService(DataBase1Config db)
        {
            _db = db;
        }

        public enum UpdateResult
        {
            Success,
            UserNotFound,
            EmailExists,
            NoChangesMade,
            InvalidField,
            ErrorUpdatingUser
        }

        /// <summary>
        /// Gets the name of the role for the user with the specified ID.
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>User's role name or null if no role is assigned.</returns>
        public string GetUserRoleName(int userId)
        {
            var roleUser = _db.User_RolAndDepartment.FirstOrDefault(x => x.fk_id_user == userId);
            if (roleUser != null)
            {
                var role = _db.Roles.FirstOrDefault(x => x.id == roleUser.fk_id_rol);
                return role?.name_rol;
            }

            return null; // User has no assigned role
        }

        public UpdateResult UpdateProfileField(int userId, string fieldToUpdate, string fieldValue)
        {
            Users user = _db.Users.FirstOrDefault(u => u.id == userId);
            if (user == null)
            {
                // If the user with the provided ID is not found, return UserNotFound to indicate that the update failed
                return UpdateResult.UserNotFound;
            }

            // Validate if the field to update is the email and if the new value already exists in the database
            if (fieldToUpdate == "email")
            {
                var existingUser = _db.Users.FirstOrDefault(u => u.email == fieldValue);
                if (existingUser != null && existingUser.id != userId)
                {
                    // If the email already exists and does not belong to the same user, return EmailExists and show an error message
                    return UpdateResult.EmailExists;
                }
            }

            // Verify if the new value is the same as the current value in the database
            if (fieldToUpdate == "name" && user.name == fieldValue
                || fieldToUpdate == "lastname" && user.lastname == fieldValue
                || fieldToUpdate == "phone" && user.phone == fieldValue
                || fieldToUpdate == "email" && user.email == fieldValue)
            {
                // If the new value is the same as the current value, return NoChangesMade and show an error message
                return UpdateResult.NoChangesMade;
            }

            // Update the corresponding field in the User object
            switch (fieldToUpdate)
            {
                case "name":
                    user.name = fieldValue;
                    break;
                case "lastname":
                    user.lastname = fieldValue;
                    break;
                case "phone":
                    user.phone = fieldValue;
                    break;
                case "email":
                    user.email = fieldValue;
                    break;
                default:
                    return UpdateResult.InvalidField;
            }

            // Save the changes to the database
            try
            {
                _db.SaveChanges();
                // Return Success to indicate that the update was successful
                return UpdateResult.Success;
            }
            catch (Exception)
            {
                // If there is an error while saving the changes to the database, return ErrorUpdatingUser to indicate that the update failed
                return UpdateResult.ErrorUpdatingUser;
            }
        }
    }
}