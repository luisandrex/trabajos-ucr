using Microsoft.AspNetCore.Authorization;
using PRIME_UCR.Application.DTOs.UserAdministration;
using System;
using System.Collections.Generic;
using System.Text;
using PRIME_UCR.Domain.Constants;

namespace PRIME_UCR.Application.Implementations.UserAdministration
{
    /*
     * Class used to handle the authorization of pages.
     */
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class HasPermissionAttribute : AuthorizeAttribute
    {
        public HasPermissionAttribute(AuthorizationPermissions permission) : base (permission.ToString())
        {

        }
    }
}
