using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PRIME_UCR.Application.Services.Incidents;
using PRIME_UCR.Domain.Models;
using Xunit;
using PRIME_UCR.Domain.Models.Incidents;

namespace PRIME_UCR.Test.IntegrationTests.Incidents
{
    public class StateServiceIntegrationTests : IClassFixture<IntegrationTestWebApplicationFactory<Startup>>
    {
        private readonly IntegrationTestWebApplicationFactory<Startup> _factory;
        public StateServiceIntegrationTests(IntegrationTestWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }
        [Fact]
        public async Task GetAllStatesReturnsValid()
        {
            var stateService = _factory.Services.GetRequiredService<IStateService>();
            var result = (await stateService.GetAllStates()).ToList();
            List<Estado> stateList = new List<Estado>
            {
                new Estado{Nombre = "Aprobado"},
                new Estado{Nombre = "Asignado" },
                new Estado{Nombre = "Creado" },
                new Estado{Nombre = "En preparación" },
                new Estado{Nombre = "En proceso de creación" },
                new Estado{Nombre = "En ruta a origen" },
                new Estado{Nombre = "En traslado" },
                new Estado{Nombre = "Entregado" },
                new Estado{Nombre = "Finalizado" },
                new Estado{Nombre = "Paciente recolectado en origen" },
                new Estado{Nombre = "Reactivación" },
                new Estado{Nombre = "Rechazado" }
            };
            for (var indexState = 0; indexState < stateList.Count(); ++indexState)
            {
                Assert.Equal(stateList[indexState].Nombre,result[indexState].Nombre);
            }
        }

    }
}