using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PRIME_UCR.Application.DTOs.CheckLists;
using System.IO;

namespace PRIME_UCR.Application.Services.CheckLists
{
    public interface IPdfService
    {
        MemoryStream GenerateIncidentPdf(PdfModel information);
    }
}
