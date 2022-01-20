using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Application.DTOs.UserAdministration
{
    public class UserValidationInfoModel
    {
        public UserValidationInfoModel()
        {
            PasswordModel = new NewPasswordModel();
        }

        public string EmailEncoded { get; set; }

        public string Code1Encoded { get; set; }

        public string Code2Encoded { get; set; }

        public NewPasswordModel PasswordModel { get; set; }

        public string Email { get
        {
            return Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(EmailEncoded));
        } }

        public string EmailValidationToken { get
        {
            return Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(Code1Encoded + Code2Encoded));
        } }
    
    }
}
