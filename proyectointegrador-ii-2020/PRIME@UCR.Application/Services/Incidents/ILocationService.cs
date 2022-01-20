using System.Collections.Generic;
using System.Threading.Tasks;
using PRIME_UCR.Application.Dtos;
using PRIME_UCR.Application.Dtos.Incidents;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Domain.Models.Incidents;
using PRIME_UCR.Domain.Models.UserAdministration;


namespace PRIME_UCR.Application.Services.Incidents
{
    public interface ILocationService
    {
        Task<IEnumerable<Médico>> GetAllDoctorsByMedicalCenter(int medicalCenterId);
        Task<Pais> GetCountryByName(string name);
        Task<CentroMedico> GetMedicalCenterById(int id);
        Task<IEnumerable<CentroMedico>> GetAllMedicalCentersAsync();
        Task<LocationModel> GetLocationByDistrictId(int districtId);
        Task<IEnumerable<Pais>> GetAllCountriesAsync();
        Task<IEnumerable<Provincia>> GetProvincesByCountryNameAsync(string countryName);
        Task<IEnumerable<Canton>> GetCantonsByProvinceNameAsync(string provinceName);
        Task<IEnumerable<Distrito>> GetDistrictsByCantonIdAsync(int cantonId);
    }
}