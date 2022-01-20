using PRIME_UCR.Application.DTOs.UserAdministration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIME_UCR.Pages.UserAdministration
{
    public partial class RequestOfPasswordRecovery
    {
        public RequestOnPasswordRecoveryModel passwordRecoveryModel;

        protected override void OnInitialized()
        {
            passwordRecoveryModel = new RequestOnPasswordRecoveryModel();
        }
    }
}
