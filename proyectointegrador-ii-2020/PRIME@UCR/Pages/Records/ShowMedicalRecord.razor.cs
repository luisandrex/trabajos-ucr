using BlazorTable;
using MatBlazor;
using PRIME_UCR.Domain.Models.MedicalRecords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIME_UCR.Pages.Records
{
    public partial class ShowMedicalRecord
    {
        public int page_size { get; set; } = 5;

        public List<Expediente> medical_records { get; set; }

        public ITable<Expediente> MedicalRecordsModel { get; set; }

        private const string create_medical_record = "/create-medical-record";

        MatTheme AddButtonTheme = new MatTheme()
        {
            Primary = "white",
            Secondary = "#095290"
        };


        protected override async Task OnInitializedAsync()
        {
            await get_records();
        }

        private async Task get_records()
        {
            //getting medical records from the database. 
            IEnumerable<Expediente> records = await medical_record_service.GetAllAsync();
            medical_records = records.ToList();
        }


        private async Task size_changed(int new_size)
        {
            page_size = new_size;
        }

        public string get_record_link(int recordId)
        {
            //getting the link to redirect the component o a new page after a medical record is pushed. 
            return $"/medicalrecord/{recordId}";
        }

        void Redirect()
        {
            //allow the program to navigate through different pages. 
            NavManager.NavigateTo($"{create_medical_record}");
        }


    }
}
