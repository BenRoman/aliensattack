using System;
using System.Linq;
using System.Web.Security;
using FunnyV1.Models;
using System.Data.Entity;

namespace FunnyV1.Providers
{
    public class CustomRoleProvider : RoleProvider
    {
        public override string[] GetRolesForUser(string username)
        {
            string[] roles = new string[] { };
            using (UserContext db = new UserContext())
            {
                Human user = db.Humen.Include(u => u.Role).FirstOrDefault(u => u.Email == username);
                Alien alien = db.Aliens.Include(u => u.Role).FirstOrDefault(u => u.Email == username);
                if (user != null && user.Role != null)
                {
                    roles = new string[] { user.Role.Name };
                }
                else if (alien != null && alien.Role != null)
                {
                    roles = new string[] { alien.Role.Name };
                }
                return roles;
            }
        }
        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }
        public override bool IsUserInRole(string username, string roleName)
        {
            using (UserContext db = new UserContext())
            {
                Human user = db.Humen.Include(u => u.Role).FirstOrDefault(u => u.Email == username);
                Alien alien = db.Aliens.Include(u => u.Role).FirstOrDefault(u => u.Email == username);

                if (user != null && user.Role != null && user.Role.Name == roleName)
                {
                    return true;
                }
                else if (alien != null && alien.Role != null && alien.Role.Name == roleName)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}