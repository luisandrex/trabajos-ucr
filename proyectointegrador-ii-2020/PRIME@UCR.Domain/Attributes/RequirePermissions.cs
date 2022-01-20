using System;
using System.Collections.Generic;
using PRIME_UCR.Domain.Constants;

namespace PRIME_UCR.Domain.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RequirePermissions : Attribute
    {
        public AuthorizationPermissions[] Permissions { get; private set; }

        public RequirePermissions(AuthorizationPermissions[] permissions)
        {
            Permissions = permissions;
        }
    }
}