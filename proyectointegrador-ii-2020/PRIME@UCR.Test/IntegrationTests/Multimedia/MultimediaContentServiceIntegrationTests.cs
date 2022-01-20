using Microsoft.Extensions.DependencyInjection;
using PRIME_UCR.Application.Services.Multimedia;
using PRIME_UCR.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PRIME_UCR.Test.IntegrationTests.Multimedia
{
    public class MultimediaContentServiceIntegrationTests : IClassFixture<IntegrationTestWebApplicationFactory<Startup>>
    {
        private readonly IntegrationTestWebApplicationFactory<Startup> _factory;
        private readonly IMultimediaContentService mcService;

        public MultimediaContentServiceIntegrationTests(IntegrationTestWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            mcService = _factory.Services.GetRequiredService<IMultimediaContentService>();
        }

        [Fact]
        public async Task GetMultimediaContentByIdReturnsNull()
        {
            /*
             * Case: Trying to get a MultimediaContent element that
             * doesn't exists (id == -1).
             * -> returns a null MultimediaContent
            */
            var result = await mcService.GetById(-1);
            Assert.Null(result);
        }

        [Fact]
        public async Task AddMultimediaContentTest()
        {
            /*
             * Case: Add a MultimediaContent instance using the MCService.
             * -> returns the stored Multimedia Content
             */
            MultimediaContent mc = new MultimediaContent
            {
                Nombre = "N_Test",
                Archivo = "A_Test",
                Tipo = "T_Test",
                Fecha_Hora = DateTime.Now
            };
            var result = await mcService.AddMultimediaContent(mc);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetByAppointmentActionReturnsEmptyList()
        {
            /*
             * Case: There are incidents in post deployment. 
             * Without MultimediaContent attached.
             * At the time of requesting the MC attached to
             * the incident it should return an empty list.
             */
            var result = (await mcService.GetByAppointmentAction(1, "Síntomas")).ToList();
            Assert.NotNull(result);
            Assert.True(result.Count == 0);
        }

    }
}
