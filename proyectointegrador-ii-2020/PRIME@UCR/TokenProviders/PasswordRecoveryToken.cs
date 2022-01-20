using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Application.TokenProviders
{
    public class PasswordRecoveryTokenProvider<TUser> : DataProtectorTokenProvider<TUser> where TUser : class
    {
        public PasswordRecoveryTokenProvider(IDataProtectionProvider dataProtectionProvider,
                IOptions<PasswordRecoveryTokenProviderOptions> options,
                ILogger<DataProtectorTokenProvider<TUser>> logger)
            : base(dataProtectionProvider, options, logger)
        {
        }
    }

    public class PasswordRecoveryTokenProviderOptions : DataProtectionTokenProviderOptions { }
}
