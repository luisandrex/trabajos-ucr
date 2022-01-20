using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Application.Exceptions.UserAdministration
{
    public class NotAuthorizedException : Exception
    {
        public NotAuthorizedException(string message) : base(message)
        {
        }
    }
}
