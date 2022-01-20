using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Application.DTOs.UserAdministration
{
    public class RecoveryPasswordInfoModel
    {
        public RecoveryPasswordInfoModel()
        {
            PasswordModel = new NewPasswordModel();
        }

        public string PasswordRecoveryToken1Encoded { get; set; }

        public string PasswordRecoveryToken2Encoded { get; set; }

        public string EmailEncoded { get; set; }

        public NewPasswordModel PasswordModel { get; set; }
        
        public string Email { get {
                return Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(EmailEncoded));
            } }

        public string PasswordRecoveryToken { get
            {
                return Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(PasswordRecoveryToken1Encoded + PasswordRecoveryToken2Encoded));
            } }
    }
}
