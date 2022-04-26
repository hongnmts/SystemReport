using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SystemReport.WebAPI.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public dynamic appUser;
        
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            appUser = context.HttpContext.Items["User"];
        }
    }
}
