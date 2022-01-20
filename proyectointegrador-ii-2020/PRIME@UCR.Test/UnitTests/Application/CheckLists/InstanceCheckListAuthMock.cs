using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using PRIME_UCR.Application.Services.CheckLists;
using PRIME_UCR.Application.Services.UserAdministration;

namespace PRIME_UCR.Test.UnitTests.Application.CheckLists
{
    public class InstanceCheckListAuthMock : Mock<IPrimeSecurityService>
    {
    }
}
