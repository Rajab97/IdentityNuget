using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MyBankIdentity.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyBankIdentity.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class MyBankAuthAttribute : Attribute, IAuthorizationFilter
    {
        /// <summary>
        /// Permissions can be multiple comma (,) separated strings
        /// Example: read, write
        /// </summary>
        private readonly string Permissions;
        private readonly string[] PermissionArray;

        public MyBankAuthAttribute()
        {
        }

        public MyBankAuthAttribute(string permissions)
        {
            this.Permissions = permissions;
        }

        public MyBankAuthAttribute(params string[] permissions)
        {
            this.PermissionArray = permissions;
        }


        public void OnAuthorization(AuthorizationFilterContext context)
        {
            UserDTO user = null;
            if (IsUserExistInContext(context.HttpContext))
            {
                user = JsonConvert.DeserializeObject<UserDTO>(context.HttpContext.Items["User"].ToString());
            }
            if (user == null || !user.IsEnabled)
            {
                // not logged in
                context.Result = new UnauthorizedResult();
                return;
            }

            if (string.IsNullOrEmpty(Permissions) && (PermissionArray == null || PermissionArray.Length == 0))
            {
                //Validation cannot take place without any permissions so returning unauthorized
                context.Result = new UnauthorizedResult();
                return;
            }

            var module = user.Role.Modules.FirstOrDefault();
            if (module == null)
            {
                context.Result = new StatusCodeResult(403);
                return;
            }

         
            var userPermissions = new List<string>();

            var permissionList = module.Permissions;
            foreach (var perm in permissionList)
            {
                userPermissions.Add(perm.Name.Trim().ToLower());
            }

            string[] requiredPermissions;
            if (!string.IsNullOrEmpty(Permissions))
            {
                requiredPermissions = Permissions.Split(',');
            } else
            {
                requiredPermissions = PermissionArray;
            }
            
            var isAuth = false;
            foreach (var permission in requiredPermissions)
            {
                if (userPermissions.Contains(permission.Trim().ToLower()))
                {
                    isAuth = true; //User Authorized. Wihtout setting any result value and just returning is sufficent for authorizing user

                }
            }

            if (userPermissions.Contains("full"))
            {
                isAuth = true; 
            }

            if (!isAuth)
            {
                context.Result = new StatusCodeResult(403);
            }

        }
        private bool IsUserExistInContext(HttpContext httpContext)
        {
            if (httpContext.Items["User"] == null)
            {
                return false;
            }
            return true;
        }
    }

}
