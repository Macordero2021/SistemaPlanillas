using SistemaPlanillas.Models;
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

        /// <summary>
        /// Gets the name of the role for the user with the specified ID.
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>User's role name or null if no role is assigned.</returns>
        public string GetUserRoleName(int userId)
        {
            var roleUser = _db.Rol_Departament_User.FirstOrDefault(x => x.fk_id_user == userId);
            if (roleUser != null)
            {
                var role = _db.Roles.FirstOrDefault(x => x.id == roleUser.fk_id_rol);
                return role?.name_rol;
            }

            return null; // User has no assigned role
        }
    }
}