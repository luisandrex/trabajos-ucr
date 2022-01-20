using PRIME_UCR.Domain.Models.MedicalRecords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIME_UCR.Pages.Records
{
    public partial class CreateMedicalRecord
    {
        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        RecordModel recordModel = new RecordModel();

        Expediente expediente = new Expediente();

        private void create_medical_record()
        {
            //expediente.FechaCreacion = DateTime.Now;

            //medical_record_service.InsertAsync(expediente); 
            //Console.WriteLine(expediente);
        }
    }
}
