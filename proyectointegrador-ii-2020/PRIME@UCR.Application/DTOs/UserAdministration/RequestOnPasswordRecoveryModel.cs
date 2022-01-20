using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Application.DTOs.UserAdministration
{
    public class RequestOnPasswordRecoveryModel
    {
        public RequestOnPasswordRecoveryModel()
        {
            Email = String.Empty;
            Message = String.Empty;
        }

        public string Email { get; set; }

        public string Message { get; set; }

        public string StatusMessage { get; set; }
    }
}
