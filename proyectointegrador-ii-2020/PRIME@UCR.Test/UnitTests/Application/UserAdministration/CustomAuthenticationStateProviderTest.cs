using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using PRIME_UCR.Application.DTOs.UserAdministration;
using PRIME_UCR.Application.Implementations.UserAdministration;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Domain.Models.UserAdministration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PRIME_UCR.Test.UnitTests.Application.UserAdministration
{
    public class CustomAuthenticationStateProviderTest
    {
        [Fact]
        public async void GetClaimIdentityNullTest()
        {
            var localMock = new Mock<ILocalStorageService>();
            var store = new Mock<IUserStore<Usuario>>();
            var userManagerMock = new Mock<UserManager<Usuario>>(new Mock<IUserStore<Usuario>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<Usuario>>().Object,
                new IUserValidator<Usuario>[0],
                new IPasswordValidator<Usuario>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<Usuario>>>().Object);
            var signInManagerMock = new Mock<SignInManager<Usuario>>(userManagerMock.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<Usuario>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<ILogger<SignInManager<Usuario>>>().Object,
                new Mock<IAuthenticationSchemeProvider>().Object,
                new Mock<IUserConfirmation<Usuario>>().Object);
           
            var authenticationServiceMock = new Mock<PRIME_UCR.Application.Services.UserAdministration.IAuthenticationService>();
            localMock.Setup(s => s.GetItemAsync<string>("token")).Returns(new ValueTask<string>(String.Empty));
            authenticationServiceMock.Setup(a => a.GetUserByEmailAsync(String.Empty));
            authenticationServiceMock.Setup(a => a.GetAllProfilesWithDetailsAsync()).Returns(Task.FromResult(new List<Perfil>()));

            JWTKeyModel key = new JWTKeyModel()
            {
                JWT_Key = "jm.NK;}OS\"gZ^wK)u0+|\\v(ADRIAN?xG~Ln_Vd\"<=[7')YNg:Lb/94~+fQ^/CRR"
            };

            var iOptionsMock = new Mock<IOptions<JWTKeyModel>>();
            iOptionsMock.Setup(s => s.Value).Returns(key);

            var customAuthenticationStateProvider = new CustomAuthenticationStateProvider(signInManagerMock.Object, 
                userManagerMock.Object,
                localMock.Object,
                authenticationServiceMock.Object,
                iOptionsMock.Object);

            var result = await customAuthenticationStateProvider.GetAuthenticationStateAsync();
            Assert.Null(result.User.Identity.Name);
        }

       

        [Fact]
        public async void AuthenticateLoginOnValidSubmit()
        {
            var localMock = new Mock<ILocalStorageService>();
            var store = new Mock<IUserStore<Usuario>>();
            var userManagerMock = new Mock<UserManager<Usuario>>(new Mock<IUserStore<Usuario>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<Usuario>>().Object,
                new IUserValidator<Usuario>[0],
                new IPasswordValidator<Usuario>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<Usuario>>>().Object);
            var signInManagerMock = new Mock<SignInManager<Usuario>>(userManagerMock.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<Usuario>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<ILogger<SignInManager<Usuario>>>().Object,
                new Mock<IAuthenticationSchemeProvider>().Object,
                new Mock<IUserConfirmation<Usuario>>().Object);
            var authenticationServiceMock = new Mock<PRIME_UCR.Application.Services.UserAdministration.IAuthenticationService>();

            userManagerMock.Setup(u => u.FindByEmailAsync("test@test.com")).Returns(Task.FromResult(new Usuario
            {
                Id = "test-test",
                UserName = "test@test.com",
                Email = "test@test.com",
                PasswordHash = "Test.1234"
            }));

            authenticationServiceMock.Setup(a => a.GetUserWithDetailsAsync("test-test")).Returns(Task.FromResult(new Usuario
            {
                Id = "test-test",
                UserName = "test@test.com",
                Email = "test@test.com",
                PasswordHash = "Test.1234",
                UsuariosYPerfiles = new List<Pertenece>()
                {
                    new Pertenece()
                    {
                        IDPerfil = "Admin",
                        IDUsuario = "test-test",
                        Perfil = new Perfil()
                        {
                            NombrePerfil = "Admin"
                        }
                    },
                    new Pertenece()
                    {
                        IDPerfil = "Medico",
                        IDUsuario = "test-test",
                        Perfil = new Perfil()
                        {
                            NombrePerfil = "Admin"
                        }
                    }
                }
            }));

            authenticationServiceMock.Setup(a => a.GetAllProfilesWithDetailsAsync()).Returns(Task.FromResult(new List<Perfil>() {
                new Perfil()
                {
                    NombrePerfil = "Admin",
                    PerfilesYPermisos = new List<Permite>()
                    {
                        new Permite()
                        {
                            IDPerfil = "Admin",
                            IDPermiso = 1,
                            Permiso = new Permiso()
                            {
                                IDPermiso = 1
                            }
                        },
                        new Permite()
                        {
                            IDPerfil = "Admin",
                            IDPermiso = 2,
                            Permiso = new Permiso()
                            {
                                IDPermiso = 2
                            }
                        }
                    }
                },
                new Perfil()
                {
                    NombrePerfil = "Medico",
                    PerfilesYPermisos = new List<Permite>()
                    {
                        new Permite()
                        {
                            IDPerfil = "Medico",
                            IDPermiso = 3,
                            Permiso = new Permiso()
                            {
                                IDPermiso = 3
                            }
                        },
                        new Permite()
                        {
                            IDPerfil = "Medico",
                            IDPermiso = 4,
                            Permiso = new Permiso()
                            {
                                IDPermiso = 4
                            }
                        }
                    }
                },
            }));

            signInManagerMock.Setup(s => s.CheckPasswordSignInAsync(It.IsAny<Usuario>(), "Test.1234", false)).Returns(Task.FromResult(SignInResult.Success));
            JWTKeyModel key = new JWTKeyModel()
            {
                JWT_Key = "jm.NK;}OS\"gZ^wK)u0+|\\v(ADRIAN?xG~Ln_Vd\"<=[7')YNg:Lb/94~+fQ^/CRR"
            };
            var iOptionsMock = new Mock<IOptions<JWTKeyModel>>();
            iOptionsMock.Setup(s => s.Value).Returns(key);

            var customAuthenticationStateProvider = new CustomAuthenticationStateProvider(signInManagerMock.Object,
                userManagerMock.Object,
                localMock.Object,
                authenticationServiceMock.Object,
                iOptionsMock.Object);

            var result = await customAuthenticationStateProvider.AuthenticateLogin(new LogInModel
            {
                Correo = "test@test.com",
                Contraseña = "Test.1234"
            });

            Assert.True(result);
        }

        [Fact]
        public async void AuthenticateLoginOnInvalidUserSubmit()
        {
            var localMock = new Mock<ILocalStorageService>();
            var store = new Mock<IUserStore<Usuario>>();
            var userManagerMock = new Mock<UserManager<Usuario>>(new Mock<IUserStore<Usuario>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<Usuario>>().Object,
                new IUserValidator<Usuario>[0],
                new IPasswordValidator<Usuario>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<Usuario>>>().Object);
            var signInManagerMock = new Mock<SignInManager<Usuario>>(userManagerMock.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<Usuario>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<ILogger<SignInManager<Usuario>>>().Object,
                new Mock<IAuthenticationSchemeProvider>().Object,
                new Mock<IUserConfirmation<Usuario>>().Object);
            var authenticationServiceMock = new Mock<PRIME_UCR.Application.Services.UserAdministration.IAuthenticationService>();

            userManagerMock.Setup(u => u.FindByEmailAsync(String.Empty)).Returns(Task.FromResult<Usuario>(null));
            JWTKeyModel key = new JWTKeyModel()
            {
                JWT_Key = "jm.NK;}OS\"gZ^wK)u0+|\\v(ADRIAN?xG~Ln_Vd\"<=[7')YNg:Lb/94~+fQ^/CRR"
            };
            var iOptionsMock = new Mock<IOptions<JWTKeyModel>>();
            iOptionsMock.Setup(s => s.Value).Returns(key);

            var customAuthenticationStateProvider = new CustomAuthenticationStateProvider(signInManagerMock.Object,
                userManagerMock.Object,
                localMock.Object,
                authenticationServiceMock.Object,
                iOptionsMock.Object);

            var result = await customAuthenticationStateProvider.AuthenticateLogin(new LogInModel
            {
                Correo = String.Empty,
                Contraseña = String.Empty
            });

            Assert.False(result);
        }

        [Fact]
        public async void AuthenticateLoginOnInvalidPasswordSubmit()
        {
            var localMock = new Mock<ILocalStorageService>();
            var store = new Mock<IUserStore<Usuario>>();
            var userManagerMock = new Mock<UserManager<Usuario>>(new Mock<IUserStore<Usuario>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<Usuario>>().Object,
                new IUserValidator<Usuario>[0],
                new IPasswordValidator<Usuario>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<Usuario>>>().Object);
            var signInManagerMock = new Mock<SignInManager<Usuario>>(userManagerMock.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<Usuario>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<ILogger<SignInManager<Usuario>>>().Object,
                new Mock<IAuthenticationSchemeProvider>().Object,
                new Mock<IUserConfirmation<Usuario>>().Object);
            var authenticationServiceMock = new Mock<PRIME_UCR.Application.Services.UserAdministration.IAuthenticationService>();

            userManagerMock.Setup(u => u.FindByEmailAsync("test@test.com")).Returns(Task.FromResult(new Usuario
            {
                Id = "test-test",
                UserName = "test@test.com",
                Email = "test@test.com",
                PasswordHash = "Test.1234"
            }));

            signInManagerMock.Setup(s => s.CheckPasswordSignInAsync(It.IsAny<Usuario>(), String.Empty, false)).Returns(Task.FromResult(SignInResult.Failed));
            JWTKeyModel key = new JWTKeyModel()
            {
                JWT_Key = "jm.NK;}OS\"gZ^wK)u0+|\\v(ADRIAN?xG~Ln_Vd\"<=[7')YNg:Lb/94~+fQ^/CRR"
            };
            var iOptionsMock = new Mock<IOptions<JWTKeyModel>>();
            iOptionsMock.Setup(s => s.Value).Returns(key);

            var customAuthenticationStateProvider = new CustomAuthenticationStateProvider(signInManagerMock.Object,
                userManagerMock.Object,
                localMock.Object,
                authenticationServiceMock.Object,
                iOptionsMock.Object);

            var result = await customAuthenticationStateProvider.AuthenticateLogin(new LogInModel
            {
                Correo = "test@test.com",
                Contraseña = ""
            });

            Assert.False(result);
        }
        [Fact]
        public async void LogoutTest()
        {
            var localMock = new Mock<ILocalStorageService>();
            var store = new Mock<IUserStore<Usuario>>();
            var userManagerMock = new Mock<UserManager<Usuario>>(new Mock<IUserStore<Usuario>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<Usuario>>().Object,
                new IUserValidator<Usuario>[0],
                new IPasswordValidator<Usuario>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<Usuario>>>().Object);
            var signInManagerMock = new Mock<SignInManager<Usuario>>(userManagerMock.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<Usuario>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<ILogger<SignInManager<Usuario>>>().Object,
                new Mock<IAuthenticationSchemeProvider>().Object,
                new Mock<IUserConfirmation<Usuario>>().Object);
            var authenticationServiceMock = new Mock<PRIME_UCR.Application.Services.UserAdministration.IAuthenticationService>();

            localMock.Setup(s => s.RemoveItemAsync(It.IsAny<string>()));

            JWTKeyModel key = new JWTKeyModel()
            {
                JWT_Key = "jm.NK;}OS\"gZ^wK)u0+|\\v(ADRIAN?xG~Ln_Vd\"<=[7')YNg:Lb/94~+fQ^/CRR"
            };
            var iOptionsMock = new Mock<IOptions<JWTKeyModel>>();
            iOptionsMock.Setup(s => s.Value).Returns(key);

            var customAuthenticationStateProvider = new CustomAuthenticationStateProvider(signInManagerMock.Object,
                userManagerMock.Object,
                localMock.Object,
                authenticationServiceMock.Object,
                iOptionsMock.Object);

            var result = await customAuthenticationStateProvider.Logout();

            Assert.True(result);
        }
    }
}