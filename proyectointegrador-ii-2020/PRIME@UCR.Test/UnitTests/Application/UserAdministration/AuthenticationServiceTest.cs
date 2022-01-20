using Moq;
using PRIME_UCR.Application.Implementations.UserAdministration;
using PRIME_UCR.Application.Repositories.UserAdministration;
using PRIME_UCR.Domain.Models.UserAdministration;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PRIME_UCR.Test.UnitTests.Application.UserAdministration
{
    public class AuthenticationServiceTest
    {
        [Fact]
        public async void GetAllProfilesWithDetailsAsyncEmptyList()
        {
            var repoMock = new Mock<IAuthenticationRepository>();
            repoMock.Setup(p => p.GetPerfilesWithDetailsAsync()).Returns(Task.FromResult(new List<Perfil>()));
            var authenticationService = new AuthenticationService(repoMock.Object);
            var result = await authenticationService.GetAllProfilesWithDetailsAsync();
            Assert.Empty(result);
        }

        [Fact]
        public async void GetAllProfilesWithDetailsAsyncNotEmptyList()
        {
            var repoMock = new Mock<IAuthenticationRepository>();
            repoMock.Setup(p => p.GetPerfilesWithDetailsAsync()).Returns(Task.FromResult(new List<Perfil>() {
                new Perfil
                {
                    NombrePerfil = "Admin"
                },
                new Perfil
                {
                    NombrePerfil = "Médico"
                },
                new Perfil
                {
                    NombrePerfil = "Especialista médico"
                }
            }));
            var authenticationService = new AuthenticationService(repoMock.Object);
            var result = await authenticationService.GetAllProfilesWithDetailsAsync();
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public async void GetAllUsersWithDetailsAsyncEmptyList()
        {
            var repoMock = new Mock<IAuthenticationRepository>();
            repoMock.Setup(p => p.GetAllUsersWithDetailsAsync()).Returns(Task.FromResult(new List<Usuario>()));
            var authenticationService = new AuthenticationService(repoMock.Object);
            var result = await authenticationService.GetAllUsersWithDetailsAsync();
            Assert.Empty(result);
        }

        [Fact]
        public async void GetAllUsersWithDetailsAsyncNotEmptyList()
        {
            var repoMock = new Mock<IAuthenticationRepository>();
            repoMock.Setup(p => p.GetAllUsersWithDetailsAsync()).Returns(Task.FromResult(new List<Usuario>() {
                new Usuario
                {
                    Email = "test1@test1.com"
                },
                new Usuario
                {
                    Email = "test1@test2.com"
                },
                new Usuario
                {
                    Email = "test1@test3.com"
                },
            }));
            var authenticationService = new AuthenticationService(repoMock.Object);
            var result = await authenticationService.GetAllUsersWithDetailsAsync();
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public async void GetUserByEmailAsyncNullEmailTest()
        {
            var repoMock = new Mock<IAuthenticationRepository>();
            repoMock.Setup(p => p.GetUserByEmailAsync(String.Empty)).Returns(Task.FromResult<Usuario>(null));
            var authenticationService = new AuthenticationService(repoMock.Object);
            var result = await authenticationService.GetUserByEmailAsync(String.Empty);
            Assert.Null(result);
        }

        [Fact]
        public async void GetUserByEmailAsyncValidEmailTest()
        {
            var repoMock = new Mock<IAuthenticationRepository>();
            repoMock.Setup(p => p.GetUserByEmailAsync("test@test.com")).Returns(Task.FromResult<Usuario>(new Usuario() { 
                Email = "test@test.com",
                UserName = "test@test.com",
                CedPersona = "12345678"
            }));
            var authenticationService = new AuthenticationService(repoMock.Object);
            var result = await authenticationService.GetUserByEmailAsync("test@test.com");
            Assert.Equal("test@test.com",result.Email);
            Assert.Equal("test@test.com", result.UserName);
            Assert.Equal("12345678", result.CedPersona);
        }

        [Fact]
        public async void GetUserWithDetailsAsyncNullTest()
        {
            var repoMock = new Mock<IAuthenticationRepository>();
            repoMock.Setup(p => p.GetWithDetailsAsync(String.Empty)).Returns(Task.FromResult<Usuario>(null));
            var authenticationService = new AuthenticationService(repoMock.Object);
            var result = await authenticationService.GetUserWithDetailsAsync(String.Empty);
            Assert.Null(result);
        }

        [Fact]
        public async void GetUserWithDetailsAsyncValidEmailTest()
        {
            var repoMock = new Mock<IAuthenticationRepository>();
            repoMock.Setup(p => p.GetWithDetailsAsync("test@test.com")).Returns(Task.FromResult<Usuario>(new Usuario()
            {
                Email = "test@test.com",
                UserName = "test@test.com",
                CedPersona = "12345678",
                Persona = new Persona
                {
                    Nombre = "Test",
                    PrimerApellido = "Test 1",
                    SegundoApellido = "Test 2",
                    Sexo = "M"
                }
            }));
            var authenticationService = new AuthenticationService(repoMock.Object);
            var result = await authenticationService.GetUserWithDetailsAsync("test@test.com");
            Assert.Equal("test@test.com", result.Email);
            Assert.Equal("test@test.com", result.UserName);
            Assert.Equal("12345678", result.CedPersona);
            Assert.Equal("Test", result.Persona.Nombre);
            Assert.Equal("Test 1", result.Persona.PrimerApellido);
            Assert.Equal("Test 2", result.Persona.SegundoApellido);
            Assert.Equal("M", result.Persona.Sexo);
            Assert.Equal("Test Test 1 Test 2", result.Persona.NombreCompleto);
        }
    }
}
