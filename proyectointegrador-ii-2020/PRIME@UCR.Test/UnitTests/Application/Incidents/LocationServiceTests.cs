using Moq;
using PRIME_UCR.Application.Implementations.Incidents;
using PRIME_UCR.Application.Repositories.Incidents;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Domain.Models.UserAdministration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using NuGet.Frameworks;
using PRIME_UCR.Application.Dtos.Incidents;

namespace PRIME_UCR.Test.UnitTests.Application.Incidents
{
    public class LocationServiceTest
    {
        [Fact]
        public async Task GetCountryByNameReturnsNull()
        {
            var mockRepo = new Mock<ICountryRepository>();
            mockRepo.Setup(p => p.GetByKeyAsync(String.Empty)).Returns(Task.FromResult<Pais>(null));
            var locationService = new LocationService(mockRepo.Object, null, null, null, null);
            var result = await locationService.GetCountryByName(String.Empty);
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllDoctorsByMedicalCenterReturnsEmpty()
        {
            /*If a medical center has no active medical staff, the method should return an empty enumerable object. 
             */
            var mockRepo = new Mock<IMedicalCenterRepository>();
            mockRepo
                .Setup(p =>
                   p.GetDoctorsByMedicalCenterId(0))
                    .Returns(Task.FromResult<IEnumerable<Médico>>(new List<Médico>())
                );
            var locationService = new LocationService(null, null, null, null, mockRepo.Object);
            var result = await locationService.GetAllDoctorsByMedicalCenter(0);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllDoctorsByMedicalCenterReturnsQuantity()
        {
            /*If a medical center has active medical staff, the method should return an non empty list with the correct
             *number of employees.  
             */
            var mockRepo = new Mock<IMedicalCenterRepository>();
            List<Médico> MyList = new List<Médico>
            {
                new Médico(),
                new Médico(),           
                new Médico(),            
                new Médico(),
            };
            mockRepo
                .Setup(p => 
                    p.GetDoctorsByMedicalCenterId(0))
                        .Returns(Task.FromResult<IEnumerable<Médico>>(MyList)
                );
            var locationService = new LocationService(null, null, null, null, mockRepo.Object);
            var result = await locationService.GetAllDoctorsByMedicalCenter(0);

            Assert.NotEmpty(result);
            Assert.Equal(MyList, result.ToList());
        }

        [Fact]
        public async Task GetAllCountriesAsyncReturnsNonEmpty()
        {
            /*If there are countries registered, the service should return an non empty list.*/
            var mockRepo = new Mock<ICountryRepository>();
            List<Pais> MyList = new List<Pais>
            {
                new Pais(),
                new Pais(),
                new Pais(),
                new Pais(),
                new Pais()
            };
            mockRepo
                .Setup(p =>
                    p.GetAllAsync())
                        .Returns(Task.FromResult<IEnumerable<Pais>>(MyList)
                );
            var locationService = new LocationService(mockRepo.Object, null, null, null, null);
            var result = await locationService.GetAllCountriesAsync();

            Assert.NotEmpty(result);
            Assert.Equal(MyList, result.ToList());
        }

        [Fact]
        public async Task GetAllCountriesAsyncReturnsEmpty()
        {
            /*If there are no countries registered, the service should return an non empty list.*/
            var mockRepo = new Mock<ICountryRepository>();
            mockRepo
                .Setup(p =>
                    p.GetAllAsync())
                        .Returns(Task.FromResult<IEnumerable<Pais>>(new List<Pais>())
                );
            var locationService = new LocationService(mockRepo.Object, null, null, null, null);
            var result = await locationService.GetAllCountriesAsync();
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetCantonsByProvinceNameAsyncReturnsEmptyList()
        {
            // arrange
            var mockRepo = new Mock<ICantonRepository>();
            var data = new List<Canton>();

            mockRepo
                .Setup(r => r.GetCantonsByProvinceNameAsync("provincia inválida"))
                .Returns(Task.FromResult<IEnumerable<Canton>>(data));

            var service = new LocationService(
                null,
                null, mockRepo.Object, null, null);

            // act 
            var result = await service.GetCantonsByProvinceNameAsync("provincia inválida");
            
            // assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetCantonsByProvinceNameAsyncReturnsValidList()
        {
            // arrange
            var mockRepo = new Mock<ICantonRepository>();
            var data = new List<Canton>
            {
                new Canton{ Id = 1,},
                new Canton{ Id = 2,},
                new Canton{ Id = 3,},
                new Canton{ Id = 4,},
                new Canton{ Id = 5,},
                new Canton{ Id = 6,},
            };

            mockRepo
                .Setup(r => r.GetCantonsByProvinceNameAsync("provincia válida"))
                .Returns(Task.FromResult<IEnumerable<Canton>>(data));

            var service = new LocationService(
                null,
                null, mockRepo.Object, null, null);

            // act 
            var result = await service.GetCantonsByProvinceNameAsync("provincia válida");
            
            // assert
            Assert.Collection(result,
                                  canton => Assert.Equal(1, canton.Id),
                                  canton => Assert.Equal(2, canton.Id),
                                  canton => Assert.Equal(3, canton.Id),
                                  canton => Assert.Equal(4, canton.Id),
                                  canton => Assert.Equal(5, canton.Id),
                                  canton => Assert.Equal(6, canton.Id)
                              );
        }

        [Fact]
        public async Task GetMedicalCenterByIdReturnsNull()
        {
            // arrange
            var mockRepo = new Mock<IMedicalCenterRepository>();
            CentroMedico data = null;

            mockRepo
                .Setup(p => p.GetByKeyAsync(-1))
                .Returns(Task.FromResult(data));

            var service = new LocationService(
                null,
                null, null, null, mockRepo.Object);

            // act 
            var result = await service.GetMedicalCenterById(-1);

            // assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetMedicalCenterByIdReturnsValid()
        {
            // arrange
            var mockRepo = new Mock<IMedicalCenterRepository>();
            var data = new CentroMedico { Id = 1 };

            mockRepo
                .Setup(p => p.GetByKeyAsync(1))
                .Returns(Task.FromResult(data));

            var service = new LocationService(
                null,
                null, null, null, mockRepo.Object);

            // act 
            var result = await service.GetMedicalCenterById(1);

            // assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task GetProvincesByCountryNameReturnsEmpty()
        {
            /* Invalid Country name test case ->
             * returns empty list because the Country doesnt exist
            */
            var mockRepo = new Mock<IProvinceRepository>();
            var data = new List<Provincia>();
            mockRepo
                .Setup(r => r.GetProvincesByCountryNameAsync("pais inválido"))
                .Returns(Task.FromResult<IEnumerable<Provincia>>(data));
            var service = new LocationService(null, mockRepo.Object, null, null, null);
            var result = await service.GetProvincesByCountryNameAsync("pais inválido");
            Assert.Empty(result);
        }
    }
}
