using Moq;
using PRIME_UCR.Application.Implementations.Incidents;
using PRIME_UCR.Application.Repositories.CheckLists;
using PRIME_UCR.Application.Services.CheckLists;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using NuGet.Frameworks;
using PRIME_UCR.Domain.Models;
using Castle.DynamicProxy.Generators;
using Microsoft.AspNetCore.DataProtection;
using PRIME_UCR.Domain.Models.CheckLists;
using PRIME_UCR.Application.Implementations.CheckLists;
using PRIME_UCR.Application.Permissions.CheckLists;

namespace PRIME_UCR.Test.UnitTests.Application.CheckLists
{
    public class ItemTest
    {
        [Fact]
        public void GetItemByIDServiceTest()
        {
            //arrange
            var mockRepo = new Mock<IItemRepository>();
            var data = new Item();
            mockRepo
                .Setup(p => p.GetByKeyAsync(0))
                .Returns(Task.FromResult<Item>(data));
            var service = new SecureCheckListService(
                null, null, mockRepo.Object, new CheckListAuthMock().Object);

            // act
            var result = service.GetItemsByCheckListId(-1);

            // assert
            Assert.NotEqual(result.Id, -1);
        }

        [Fact]
        public void GetItemByIDServiceTestIncorrect()
        {
            //arrange
            var mockRepo = new Mock<IItemRepository>();
            var data = new Item();
            mockRepo
                .Setup(p => p.GetByKeyAsync(0))
                .Returns(Task.FromResult<Item>(data));
            var service = new SecureCheckListService(
                null, null, mockRepo.Object, new CheckListAuthMock().Object);

            // act
            var result = service.GetItemsByCheckListId(1);

            // assert
            Assert.Equal(result.Id, 5);
        }

        [Fact]
        public void InsertItemServiceTest()
        {
            //arrange
            var mockRepo = new Mock<IItemRepository>();
            var data = new Item();
            data.Id = 100;
            mockRepo
                .Setup(p => p.InsertCheckItemAsync(data))
                .Returns(Task.FromResult<Item>(new Item()));
            var service = new SecureCheckListService(
                null, null, mockRepo.Object, new CheckListAuthMock().Object);

            // act
            var result = service.GetItemById(data.Id);

            // assert
            Assert.Equal(result.Id, 3);
        }

        [Fact]
        public void GetItemByChecklistIDServiceTest()
        {
            //arrange
            var mockRepo = new Mock<IItemRepository>();
            var data = new List<Item>();
            mockRepo
                .Setup(p => p.GetByCheckListId(1))
                .Returns(Task.FromResult<IEnumerable<Item>>(data));
            var service = new SecureCheckListService(
                null, null, mockRepo.Object, new CheckListAuthMock().Object);

            // act
            var result = service.GetItemsByCheckListId(1);

            // assert
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public void GetItemBySuperItemIDServiceTest()
        {
            //arrange
            var mockRepo = new Mock<IItemRepository>();
            var data = new List<Item>();
            mockRepo
                .Setup(p => p.GetByCheckListId(1))
                .Returns(Task.FromResult<IEnumerable<Item>>(data));
            var service = new SecureCheckListService(
                null, null, mockRepo.Object, new CheckListAuthMock().Object);

            // act
            var result = service.GetItemsBySuperitemId(24);

            // assert
            Assert.Equal(4, result.Id);
        }

        [Fact]
        public void GetCoreItemsTest()
        {
            //arrange
            var mockRepo = new Mock<IItemRepository>();
            var data = new List<Item>();
            mockRepo
                .Setup(p => p.GetByCheckListId(1))
                .Returns(Task.FromResult<IEnumerable<Item>>(data));
            var service = new SecureCheckListService(
                null, null, mockRepo.Object, new CheckListAuthMock().Object);

            // act
            var result = service.GetCoreItems(1);

            // assert
            Assert.Equal(2, result.Id);
        }

        [Fact]
        public void UpdateItemTest()
        {
            //arrange
            var mockRepo = new Mock<IItemRepository>();
            var data = new List<Item>();
            var temp = new Item();
            mockRepo
                .Setup(p => p.GetByCheckListId(1))
                .Returns(Task.FromResult<IEnumerable<Item>>(data));
            var service = new SecureCheckListService(
                null, null, mockRepo.Object, new CheckListAuthMock().Object);

            // act
            var result = service.UpdateItem(temp);

            // assert
            Assert.Equal(6, result.Id);
        }
    }
}
