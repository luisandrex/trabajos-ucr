using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using PRIME_UCR.Application.Implementations.CheckLists;
using PRIME_UCR.Application.Repositories.CheckLists;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Domain.Models.CheckLists;
using Xunit;
using System;
using PRIME_UCR.Application.Permissions.CheckLists;

namespace PRIME_UCR.Test.UnitTests.Application.CheckLists
{
    public class CheckListServiceTests
    {
        [Fact]
        public async Task GetAllReturnsEmpty()
        {
            // arrange
            var mockRepo = new Mock<ICheckListRepository>();
            var data = new List<CheckList>();
            mockRepo
                .Setup(p => p.GetAllAsync())
                .Returns(Task.FromResult<IEnumerable<CheckList>>(data));
            var service = new SecureCheckListService(
                mockRepo.Object, null, null, new CheckListAuthMock().Object);

            // act
            var result = await service.GetAll();

            // assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllReturnsValidList()
        {
            // arrange
            var mockRepo = new Mock<ICheckListRepository>();
            var data = new List<CheckList>
            {
                new CheckList {Id = 1},
                new CheckList {Id = 2},
                new CheckList {Id = 3},
                new CheckList {Id = 4}
            };
            mockRepo
                .Setup(p => p.GetAllAsync())
                .Returns(Task.FromResult<IEnumerable<CheckList>>(data));
            var service = new SecureCheckListService(
                mockRepo.Object, null, null, new CheckListAuthMock().Object);

            // act
            var result = await service.GetAll();

            // assert
            Assert.Collection(result,
                                    checkList => Assert.Equal(1, checkList.Id),
                                    checkList => Assert.Equal(2, checkList.Id),
                                    checkList => Assert.Equal(3, checkList.Id),
                                    checkList => Assert.Equal(4, checkList.Id)
                            );
        }

        [Fact]
        public async Task GetTypesReturnsEmpty()
        {
            // arrange
            var mockRepo = new Mock<ICheckListTypeRepository>();
            var data = new List<TipoListaChequeo>();
            mockRepo
                .Setup(p => p.GetAllAsync())
                .Returns(Task.FromResult<IEnumerable<TipoListaChequeo>>(data));
            var service = new SecureCheckListService(
                null, mockRepo.Object, null, new CheckListAuthMock().Object);

            // act
            var result = await service.GetTypes();

            // assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetTypesReturnsValidList()
        {
            // arrange
            var mockRepo = new Mock<ICheckListTypeRepository>();
            var data = new List<TipoListaChequeo>
            {
                new TipoListaChequeo {Nombre = "Colocación equipo"},
                new TipoListaChequeo {Nombre = "Retiro equipo"},
                new TipoListaChequeo {Nombre = "Paciente en origen"},
                new TipoListaChequeo {Nombre = "Paciente en destino"},
                new TipoListaChequeo {Nombre = "Paciente en traslado"}
            };
            mockRepo
                .Setup(p => p.GetAllAsync())
                .Returns(Task.FromResult<IEnumerable<TipoListaChequeo>>(data));
            var service = new SecureCheckListService(
                null, mockRepo.Object, null, new CheckListAuthMock().Object);

            // act
            var result = await service.GetTypes();

            // assert
            Assert.Collection(result,
                                    type => Assert.Equal("Colocación equipo", type.Nombre),
                                    type => Assert.Equal("Retiro equipo", type.Nombre),
                                    type => Assert.Equal("Paciente en origen", type.Nombre),
                                    type => Assert.Equal("Paciente en destino", type.Nombre),
                                    type => Assert.Equal("Paciente en traslado", type.Nombre)
                            );
        }

        [Fact]
        public async Task GetByIdReturnsNull()
        {
            // arrange
            var mockRepo = new Mock<ICheckListRepository>();
            CheckList data = null;
            mockRepo
                .Setup(p => p.GetByKeyAsync(1))
                .Returns(Task.FromResult(data));
            var service = new SecureCheckListService(
                mockRepo.Object, null, null, new CheckListAuthMock().Object);

            // act
            var result = await service.GetById(1);

            // assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetByIdReturnsValid()
        {
            // arrange
            var mockRepo = new Mock<ICheckListRepository>();
            CheckList data = new CheckList { Id = 1 };
            mockRepo
                .Setup(p => p.GetByKeyAsync(1))
                .Returns(Task.FromResult(data));
            var service = new SecureCheckListService(
                mockRepo.Object, null, null, new CheckListAuthMock().Object);

            // act
            var result = await service.GetById(1);

            // assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task GetItemByIdReturnsNull()
        {
            var mockRepo = new Mock<IItemRepository>();
            Item data = null;
            mockRepo
                .Setup(p => p.GetByKeyAsync(1))
                .Returns(Task.FromResult(data));
            var service = new SecureCheckListService(
                 null, null, mockRepo.Object, new CheckListAuthMock().Object);
            var result = await service.GetItemById(1);

            Assert.Null(result);
        }


        [Fact]
        public async Task GetItemByIdReturnsValid()
        {
            var mockRepo = new Mock<IItemRepository>();
            Item data = new Item { Id = 1 };
            mockRepo
                .Setup(p => p.GetByKeyAsync(1))
                .Returns(Task.FromResult(data));
            var service = new SecureCheckListService(
                 null, null, mockRepo.Object, new CheckListAuthMock().Object);
            var result = await service.GetItemById(1);
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]

        public async Task GetItemsBySuperItemIDEmpty() {
            var mockRepo = new Mock<IItemRepository>();
            var data = new List<Item>();
            mockRepo
                .Setup(p => p.GetBySuperitemId(1))
                .Returns(Task.FromResult<IEnumerable<Item>>(data));
            var service = new SecureCheckListService(
                null, null, mockRepo.Object, new CheckListAuthMock().Object);
            var result = await service.GetItemsBySuperitemId(1);

            Assert.Empty(result);
        }
        [Fact]
        public async Task GetItemsBySuperItemIDReturnsValid()
        {
            var mockRepo = new Mock<IItemRepository>();
            var data = new List<Item>()
            {
                new Item {Id=1,IDSuperItem=1},
                new Item { Id=2,IDSuperItem=1}

            };
            mockRepo
                .Setup(p => p.GetBySuperitemId(1))
                .Returns(Task.FromResult<IEnumerable<Item>>(data));
            var service = new SecureCheckListService(
                null, null, mockRepo.Object, new CheckListAuthMock().Object);

            var result = await service.GetItemsBySuperitemId(1);
          
            Assert.Collection(result,
                item => Assert.Equal(1, item.Id),
                item => Assert.Equal(2, item.Id)
                );
        }

        [Fact]
        public async Task GetItemsByCheckListIdReturnsValid()
        {
            var mockRepo = new Mock<IItemRepository>();
            var data = new List<Item>()
            {
                new Item {Id=1,IDLista=1},
                new Item { Id=2,IDLista=1},

            };
            mockRepo
                .Setup(p => p.GetByCheckListId(1))
                .Returns(Task.FromResult<IEnumerable<Item>>(data));
            var service = new SecureCheckListService(
                null, null, mockRepo.Object, new CheckListAuthMock().Object);

            var result = await service.GetItemsByCheckListId(1);

            Assert.Collection(result,
                item => Assert.Equal(1,item.Id),
                item => Assert.Equal(2, item.Id)
                );
        }
        [Fact]
        public async Task GetItemsByCheckListIdReturns()
        {
            var mockRepo = new Mock<IItemRepository>();
            var data = new List<Item>();
            mockRepo
                .Setup(p => p.GetByCheckListId(1))
                .Returns(Task.FromResult<IEnumerable<Item>>(data));
            var service = new SecureCheckListService(
                null, null, mockRepo.Object, new CheckListAuthMock().Object);
            var result = await service.GetItemsByCheckListId(1);

            Assert.Empty(result);
        }

    }
}

