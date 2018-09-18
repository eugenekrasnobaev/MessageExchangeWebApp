using System;
using System.Linq;
using System.Web.Security;
using MessageExchangeWebApp.Models;

namespace MessageExchangeWebApp.Providers
{

    public class CustomRoleProvider : RoleProvider
    {
        public override string[] GetRolesForUser(string username)
        {
            var roles = new string[] { };
            using (var db = new MessageExchangeContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Login == username);
                if (user?.Role != null)
                {
                    roles = new string[] { user.Role };
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
            using (var db = new MessageExchangeContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Login == username);

                if (user?.Role != null && user.Role == roleName)
                    return true;
                return false;
            }
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get => throw new NotImplementedException(); 
            set => throw new NotImplementedException(); 
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