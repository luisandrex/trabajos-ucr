using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using PRIME_UCR.Application.Implementations.Incidents;
using PRIME_UCR.Application.Repositories.Incidents;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Application.Repositories.UserAdministration;
using PRIME_UCR.Domain.Models.Incidents;
using PRIME_UCR.Domain.Models.UserAdministration;
using Xunit;
using PRIME_UCR.Application.Dtos.Incidents;
using PRIME_UCR.Domain.Models.MedicalRecords;
using System;
using PRIME_UCR.Application.Repositories.Dashboard;
using PRIME_UCR.Application.Implementations.Dashboard;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Application.Implementations.UserAdministration;
using PRIME_UCR.Application.DTOs.Dashboard;
using PRIME_UCR.Components.Dashboard.Filters;
using PRIME_UCR.Domain.Constants;
using PRIME_UCR.Application.Permissions.Dashboard;

namespace PRIME_UCR.Test.UnitTests.Application.Dashboards
{
    public class DashboardServiceTests
    {
        [Fact]
        public async Task GetAllIncidentsAsyncReturnsEmptyList()
        {
            //arrange
            var mockRepo = new Mock<IDashboardRepository>();



            var data = new List<Incidente>();

            mockRepo
                .Setup(s => s.GetAllIncidentsAsync())
                .Returns(Task.FromResult<List<Incidente>>(data));
           
            var mockSecurity = new Mock<IPrimeSecurityService>();
            mockSecurity.Setup(s => s.CheckIfIsAuthorizedAsync(It.IsAny<AuthorizationPermissions[]>()));

            var service = new SecureDashboardService(
                mockRepo.Object, null, null, null, null, mockSecurity.Object, null, null);

            //act 
            var result = await service.GetAllIncidentsAsync();

            //assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllIncidentsAsyncReturnsValidList()
        {
            // arrange
            var mockRepo = new Mock<IDashboardRepository>();
            var data = new List<Incidente>
            {
                new Incidente {Codigo = "codigo1"},
                new Incidente {Codigo = "codigo2"},
                new Incidente {Codigo = "codigo3"},
                new Incidente {Codigo = "codigo4"},
                new Incidente {Codigo = "codigo5"},
                new Incidente {Codigo = "codigo6"},
            };

            mockRepo
                .Setup(d => d.GetAllIncidentsAsync())
                .Returns(Task.FromResult<List<Incidente>>(data));


            var mockSecurity = new Mock<IPrimeSecurityService>();
            mockSecurity.Setup(s => s.CheckIfIsAuthorizedAsync(It.IsAny<AuthorizationPermissions[]>()));

            var service = new SecureDashboardService(
                mockRepo.Object, null, null, null, null, mockSecurity.Object, null, null);

            // act
            var result = await service.GetAllIncidentsAsync();

            // assert
            Assert.Collection(result,
                                  incidente => Assert.Equal("codigo1", incidente.Codigo),
                                  incidente => Assert.Equal("codigo2", incidente.Codigo),
                                  incidente => Assert.Equal("codigo3", incidente.Codigo),
                                  incidente => Assert.Equal("codigo4", incidente.Codigo),
                                  incidente => Assert.Equal("codigo5", incidente.Codigo),
                                  incidente => Assert.Equal("codigo6", incidente.Codigo)
                              );
        }

        [Fact]
        public async Task GetAllDistrictsAsyncReturnsEmptyList()
        {
            //arrange
            var mockRepo = new Mock<IDashboardRepository>();
            var data = new List<Distrito>();

            mockRepo
                .Setup(s => s.GetAllDistrictsAsync())
                .Returns(Task.FromResult<List<Distrito>>(data));


            var mockSecurity = new Mock<IPrimeSecurityService>();
            mockSecurity.Setup(s => s.CheckIfIsAuthorizedAsync(It.IsAny<AuthorizationPermissions[]>()));

            var service = new SecureDashboardService(
                mockRepo.Object, null, null, null, null, mockSecurity.Object, null, null);

            //act 
            var result = await service.GetAllDistrictsAsync();

            //assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllDistrictsAsyncReturnsValidList()
        {
            // arrange
            var mockRepo = new Mock<IDashboardRepository>();
            var data = new List<Distrito>
            {
                new Distrito {Id = 1},
                new Distrito {Id = 2},
                new Distrito {Id = 3},
                new Distrito {Id = 4},
                new Distrito {Id = 5},
                new Distrito {Id = 6},
            };

            mockRepo
                .Setup(d => d.GetAllDistrictsAsync())
                .Returns(Task.FromResult<List<Distrito>>(data));


            var mockSecurity = new Mock<IPrimeSecurityService>();
            mockSecurity.Setup(s => s.CheckIfIsAuthorizedAsync(It.IsAny<AuthorizationPermissions[]>()));

            var service = new SecureDashboardService(
                mockRepo.Object, null, null, null, null, mockSecurity.Object, null, null);

            // act
            var result = await service.GetAllDistrictsAsync();

            // assert
            Assert.Collection(result,
                                  d => Assert.Equal(1, d.Id),
                                  d => Assert.Equal(2, d.Id),
                                  d => Assert.Equal(3, d.Id),
                                  d => Assert.Equal(4, d.Id),
                                  d => Assert.Equal(5, d.Id),
                                  d => Assert.Equal(6, d.Id)
                              );
        }


        [Fact]
        public async Task GetIncidentCounterAsyncReturnsZero()
        {
            // arrange
            var mockRepo = new Mock<IDashboardRepository>();
            int count = 0;

            mockRepo
                .Setup(d => d.GetIncidentsCounterAsync("modalidad", string.Empty))
                .Returns(Task.FromResult<int>(count));

            var mockSecurity = new Mock<IPrimeSecurityService>();
            mockSecurity.Setup(s => s.CheckIfIsAuthorizedAsync(It.IsAny<AuthorizationPermissions[]>()));


            var service = new SecureDashboardService(
                mockRepo.Object,
                null, null, null, null, mockSecurity.Object, null, null);

            // act
            var result = await service.GetIncidentCounterAsync("modalidad", String.Empty);

            // assert
            Assert.Equal(0, result);
        }


        [Fact]
        public async Task GetFilteredIncidentListAsyncReturnsValidList()
        {
            // arrange
            var mockRepo = new Mock<IDashboardRepository>();
            var mockCountry = new Mock<ICountryRepository>();
            var mockMedical = new Mock<IMedicalCenterRepository>();
            var data = new List<Incidente>
            {
                new Incidente {Codigo = "codigo1", Modalidad = "Maritimo"},
                new Incidente {Codigo = "codigo2", Modalidad = "Maritimo"},
                new Incidente {Codigo = "codigo3", Modalidad = "Maritimo"},
                new Incidente {Codigo = "codigo4", Modalidad = "Maritimo"},
                new Incidente {Codigo = "codigo5", Modalidad = "Terrestre"},
                new Incidente {Codigo = "codigo6", Modalidad = "Terrestre"},
            };

            mockRepo
                .Setup(d => d.GetAllIncidentsAsync())
                .Returns(Task.FromResult<List<Incidente>>(data));

            var mockSecurity = new Mock<IPrimeSecurityService>();
            mockSecurity.Setup(s => s.CheckIfIsAuthorizedAsync(It.IsAny<AuthorizationPermissions[]>()));

            var service = new SecureDashboardService(
                mockRepo.Object,
                 null, null, mockCountry.Object, mockMedical.Object, mockSecurity.Object, null, null);

            // act
            var modalityFilter = new Modalidad();
            modalityFilter.Tipo = "Terrestre";
            var model = new FilterModel { ModalityFilter = modalityFilter };
            var result = await service.GetFilteredIncidentsList(model);

            // assert
            Assert.Collection(result,
                                  i => Assert.Equal("Terrestre", i.Modalidad),
                                  i => Assert.Equal("Terrestre", i.Modalidad)
                              );
        }
    }
}