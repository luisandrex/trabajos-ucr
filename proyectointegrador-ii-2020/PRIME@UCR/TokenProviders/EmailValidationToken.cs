using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Application.TokenProviders
{
    public class EmailValidationTokenProvider<TUser> : DataProtectorTokenProvider<TUser> where TUser : class
    {
        public EmailValidationTokenProvider(IDataProtectionProvider dataProtectionProvider,
                IOptions<PasswordRecoveryTokenProviderOptions> options,
                ILogger<DataProtectorTokenProvider<TUser>> logger)
            : base(dataProtectionProvider, options, logger)
        {
        }
    }

    public class EmailValidationTokenProviderOptions : DataProtectionTokenProviderOptions { }
}