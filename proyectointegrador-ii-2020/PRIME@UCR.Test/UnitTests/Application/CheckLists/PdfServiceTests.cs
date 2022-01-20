using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using PRIME_UCR.Application.Implementations.CheckLists;
using PRIME_UCR.Application.Repositories.CheckLists;
using PRIME_UCR.Application.DTOs.CheckLists;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Domain.Models.CheckLists;
using Xunit;
using System;
using PRIME_UCR.Application.Permissions.CheckLists;
using System.IO;
using PRIME_UCR.Application.DTOs.Incidents;
using PRIME_UCR.Application.Dtos.Incidents;
using PRIME_UCR.Domain.Models.UserAdministration;

namespace PRIME_UCR.Test.UnitTests.Application.CheckLists
{
    public class PdfServiceTests
    {
        [Fact]
        public void GenerateIncidentPdfReturnsNull()
        {
            // arrange
            PdfModel testModel = new PdfModel();
            testModel.Incident = null;
            var service = new PdfService();

            // act
            var result = service.GenerateIncidentPdf(testModel);

            // assert
            Assert.Null(result);
        }

        [Fact]
        public void GenerateIncidentPdfReturnsNotNull()
        {   
            // arrange
            PdfModel testModel = new PdfModel();
            testModel.Incident = new IncidentDetailsModel();
            var service = new PdfService();

            // act
            var result = service.GenerateIncidentPdf(testModel);

            // assert
            Assert.NotNull(result);
            Assert.IsType<MemoryStream>(result);
        }
    }
}
