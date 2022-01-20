using Microsoft.Extensions.DependencyInjection;
using PRIME_UCR.Application.Services.CheckLists;
using PRIME_UCR.Domain.Models.CheckLists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace PRIME_UCR.Test.IntegrationTests.CheckLists
{
    public class InstanceCheckListServiceTests : IClassFixture<IntegrationTestWebApplicationFactory<Startup>>
    {
        private readonly IntegrationTestWebApplicationFactory<Startup> _factory;

        public InstanceCheckListServiceTests(IntegrationTestWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetByIncidentCodeReturnsEmpty()
        {
            /* Case: There is no valid instance checklist in post deployment
             * -> returns an empty list because there is no instance checklist with this code
             */
            var checkListService = _factory.Services.GetRequiredService<IInstanceChecklistService>();
            string incidentCode = "Pruebas";
            var result = await checkListService.GetByIncidentCod(incidentCode);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetItemsByIncidentCodAndCheckListIdReturnsEmpty()
        {
            /* Case: There is no valid instance checklist in post deployment
             * -> returns an empty list because there is no instance checklist with this code or with this id.
             */
            var checkListService = _factory.Services.GetRequiredService<IInstanceChecklistService>();
            string incidentCode = "Pruebas";
            var result = await checkListService.GetItemsByIncidentCodAndCheckListId(incidentCode, 1);
            Assert.Empty(result);
        }


        [Fact]
        public async Task GetAllReturnsNotEmpty()
        {
            /* Case: There is 1 checklist instance in post deployment
             * -> the list of all checklists instances won't be empty.
             */
            var checkListService = _factory.Services.GetRequiredService<IInstanceChecklistService>();
            var result = await checkListService.GetAll();
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task GetNumberOfItemsTest()
        {
            /* Case: There is a checklist instance in post deployment
             * -> returns the number of instance items of the existing list instance in post deployment
             */
            var checkListService = _factory.Services.GetRequiredService<IInstanceChecklistService>();
            string code = DateTime.Now.ToString("yyyy-MM-dd") + "-0003-IT-AER";

            var result = await checkListService.GetNumberOfItems(code, 3);

            Assert.Equal(33, result);
        }

        [Fact]
        public async Task GetNumberOfCompletedItemsTest()
        {
            /* Case: There is a checklist instance with 0 completed items in post deployment
             * -> returns the number of completed instance items of the list instance in post deployment
             */
            var checkListService = _factory.Services.GetRequiredService<IInstanceChecklistService>();
            string code = DateTime.Now.ToString("yyyy-MM-dd") + "-0003-IT-AER";

            var result = await checkListService.GetNumberOfCompletedItems(code, 3);
            
            Assert.Equal(6, result);
        }

        [Fact]
        public async Task GetCoreItemsTest()
        {
            /* Case: There is a checklist instance with 5 core items in post deployment
             * -> returns the number of instance items of the existing list instance in post deployment
             */
            var checkListService = _factory.Services.GetRequiredService<IInstanceChecklistService>();
            string code = DateTime.Now.ToString("yyyy-MM-dd") + "-0003-IT-AER";
            
            var result = await checkListService.GetCoreItems(code, 3);
            
            Assert.Equal(5, result.Count());
        }

        [Fact]
        public async Task GetItemsByFatherIdTest()
        {
            /* Case: There is a checklist instance with 5 core items in post deployment, the item with id 41 hast 5 subtimes
             * -> returns the number of subitems of the item with id 41 of the existing list instance in post deployment
             */
            var checkListService = _factory.Services.GetRequiredService<IInstanceChecklistService>();
            string code = DateTime.Now.ToString("yyyy-MM-dd") + "-0003-IT-AER";
            
            var result = await checkListService.GetItemsByFatherId(code, 3, 41);
            
            Assert.Equal(5, result.Count());
        }

        [Fact]
        public async Task GetItemsByIncidentCodAndCheckListIdTest()
        {
            /* Case: There is a checklist instance with 33 items in post deployment
             * -> returns the instance items of the existing list instance in post deployment
             */
            var checkListService = _factory.Services.GetRequiredService<IInstanceChecklistService>();
            string code = DateTime.Now.ToString("yyyy-MM-dd") + "-0003-IT-AER";
            
            var result = await checkListService.GetItemsByIncidentCodAndCheckListId(code, 3);
            
            Assert.Equal(33, result.Count());
        }

        [Fact]
        public async Task CheckItemTest()
        {
            /* Case: There is a checklist instance with 33 items in post deployment
             * -> an item will be checked automatically whe all of its subimtes are checked
             */
            var checkListService = _factory.Services.GetRequiredService<IInstanceChecklistService>();
            string code = DateTime.Now.ToString("yyyy-MM-dd") + "-0003-IT-AER";
            List<InstanciaItem> result = (List<InstanciaItem>) await checkListService.GetItemsByIncidentCodAndCheckListId(code, 3);

            for (int i = 12; i < 17; ++i)
            {
                result[i].Completado = true;
                await checkListService.UpdateItemInstance(result[i]);
            }

            Thread.Sleep(5000);
            
            // reload items
            result = (List<InstanciaItem>)await checkListService.GetItemsByIncidentCodAndCheckListId(code, 3);


            Assert.True(result[11].Completado);

            // undo changes
            for (int i = 12; i < 17; ++i)
            {
                result[i].Completado = false;
                await checkListService.UpdateItemInstance(result[i]);
            }
        }

        [Fact]
        public async Task LoadRelationsTest()
        {
            /* Case: There is a checklist instance with 33 items in post deployment
             * -> subitems are stored in their parent's Subitems list: item in position 11 has 5 subitems
             * -> subitems get a reference to their parent: items in positions 12 through 16 have an item assigned in MyFather
             */
            var checkListService = _factory.Services.GetRequiredService<IInstanceChecklistService>();
            string code = DateTime.Now.ToString("yyyy-MM-dd") + "-0003-IT-AER";
            List<InstanciaItem> result = (List<InstanciaItem>) await checkListService.GetItemsByIncidentCodAndCheckListId(code, 3);

            checkListService.LoadRelations(result);
            Thread.Sleep(5000);

            Assert.Equal(5, result[11].SubItems.Count());
            for (int i = 12; i < 17; ++i)
            {
                Assert.NotNull(result[12].MyFather);
            }
        }

    }
}
