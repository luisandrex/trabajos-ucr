using PRIME_UCR.Domain.Attributes;
using PRIME_UCR.Domain.Constants;
using PRIME_UCR.Domain.Models.UserAdministration;
using PRIME_UCR.Application.DTOs.Incidents;
using PRIME_UCR.Domain.Models.Incidents;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PRIME_UCR.Domain.Models.MedicalRecords;
using PRIME_UCR.Application.Implementations.Appointments;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Application.Repositories.Appointments;
using PRIME_UCR.Application.Repositories.MedicalRecords;
using PRIME_UCR.Application.Services.Appointments;
using PRIME_UCR.Domain.Models.Appointments;
using PRIME_UCR.Domain.Models;

namespace PRIME_UCR.Application.Permissions.Appointments
{

    public class SecureAppointmentService : IAppointmentService
    {

        private readonly IActionTypeRepository _actionTypeRepo;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMedicalRecordRepository _medicalRecordRepository;
        private readonly IMedicalAppointmentRepository _medapprepo;
        private readonly IHavePrescriptionRepository _havepresc;
        private readonly IDrugRepository _drugrepo;
        private readonly IMedCenterRepository _medcenrepo;
        private readonly IMedAppMetricRepository _metapprepo;
        private readonly IMedicalSpecialtyRepository _medspecirepo;
        private readonly ISpecializesRepository _speciarepo;
        private readonly IAppointmentReferenceRepository _appreferepo;
        private readonly IPrimeSecurityService _primeSecurityService;
        private readonly IAppointmentStatusRepository _appostatusrep;
        private readonly AppointmentService appointmentService;

        public SecureAppointmentService(IActionTypeRepository actionTypeRepo,
            IAppointmentRepository appointmentRepository,
            IMedicalRecordRepository medicalRecordRepository,
            IMedicalAppointmentRepository medapp,
            IHavePrescriptionRepository havepres,
            IDrugRepository drugrep,
            IMedCenterRepository medcenrepo,
            IMedAppMetricRepository metapprepo,
            IMedicalSpecialtyRepository medspeci,
            ISpecializesRepository speciarepo,
            IAppointmentReferenceRepository appreferepo,
            IAppointmentStatusRepository appstatusrepo,
            IPrimeSecurityService primeSecurityService)
        {
            _actionTypeRepo = actionTypeRepo;
            _appointmentRepository = appointmentRepository;
            _medicalRecordRepository = medicalRecordRepository;
            _primeSecurityService = primeSecurityService;
            _medapprepo = medapp;
            _havepresc = havepres;
            _drugrepo = drugrep;
            _medcenrepo = medcenrepo;
            _metapprepo = metapprepo;
            _medspecirepo = medspeci;
            _speciarepo = speciarepo;
            _appreferepo = appreferepo;
            _appostatusrep = appstatusrepo;
            appointmentService = new AppointmentService(_actionTypeRepo, _appointmentRepository, _medicalRecordRepository,
                                                        _medapprepo, _havepresc, _drugrepo, _medcenrepo,
                                                        _metapprepo, _medspecirepo, _speciarepo, _appreferepo, _appostatusrep);

        }

        public async Task<MetricasCitaMedica> GetMetricsMedAppointmentByAppId(int id)
        {
            await _primeSecurityService.CheckIfIsAuthorizedAsync(new AuthorizationPermissions[0]);
            return await appointmentService.GetMetricsMedAppointmentByAppId(id);
        }

        public async Task<IEnumerable<ReferenciaCita>> GetReferencesByAppId(int id)
        {
            await _primeSecurityService.CheckIfIsAuthorizedAsync(new AuthorizationPermissions[0]);
            return await appointmentService.GetReferencesByAppId(id);
        }

        public async Task<IEnumerable<Persona>> GetDoctorsBySpecialtyNameAsync(string specialty_name)
        {
            await _primeSecurityService.CheckIfIsAuthorizedAsync(new AuthorizationPermissions[0]);
            return await appointmentService.GetDoctorsBySpecialtyNameAsync(specialty_name);
        }

        public async Task InsertAppointmentReference(ReferenciaCita reference)
        {
            await _primeSecurityService.CheckIfIsAuthorizedAsync(new AuthorizationPermissions[0]);
            await appointmentService.InsertAppointmentReference(reference);
        }

        public async Task<IEnumerable<EspecialidadMedica>> GetMedicalSpecialtiesAsync()
        {
            await _primeSecurityService.CheckIfIsAuthorizedAsync(new AuthorizationPermissions[0]);
            return await appointmentService.GetMedicalSpecialtiesAsync();
        }

        public async Task InsertMetrics(MetricasCitaMedica metrics)
        {
            await _primeSecurityService.CheckIfIsAuthorizedAsync(new AuthorizationPermissions[0]);
            await appointmentService.InsertMetrics(metrics);
        }

        public async Task UpdateMetrics(MetricasCitaMedica metrics)
        {
            await _primeSecurityService.CheckIfIsAuthorizedAsync(new AuthorizationPermissions[0]);
            await appointmentService.UpdateMetrics(metrics);
        }

        public async Task<CitaMedica> GetMedicalAppointmentWithAppointmentByIdAsync(int id)
        {
            await _primeSecurityService.CheckIfIsAuthorizedAsync(new AuthorizationPermissions[0]);
            return await appointmentService.GetMedicalAppointmentWithAppointmentByIdAsync(id);
        }

        public async Task<CentroMedico> GetMedCenterByKeyAsync(int id)
        {
            await _primeSecurityService.CheckIfIsAuthorizedAsync(new AuthorizationPermissions[0]);
            return await appointmentService.GetMedCenterByKeyAsync(id);
        }

        public async Task UpdateAppointmentStatus(int id)
        {
            await _primeSecurityService.CheckIfIsAuthorizedAsync(new AuthorizationPermissions[0]);
            appointmentService.UpdateAppointmentStatus(id);
        }

        public async Task UpdateAsync(PoseeReceta prescription)
        {
            await _primeSecurityService.CheckIfIsAuthorizedAsync(new AuthorizationPermissions[0]);
            await appointmentService.UpdateAsync(prescription);
        }

        public async Task<IEnumerable<RecetaMedica>> GetDrugsByConditionAsync(string drugname)
        {
            await _primeSecurityService.CheckIfIsAuthorizedAsync(new AuthorizationPermissions[0]);
            return await appointmentService.GetDrugsByConditionAsync(drugname);
        }

        public async Task<IEnumerable<RecetaMedica>> GetDrugsByFilterAsync(string filter)
        {
            await _primeSecurityService.CheckIfIsAuthorizedAsync(new AuthorizationPermissions[0]);
            return await appointmentService.GetDrugsByFilterAsync(filter);
        }

        public async Task<PoseeReceta> UpdatePrescriptionDosis(int idMedicalPrescription, int idMedicalAppointment, string dosis)
        {
            await _primeSecurityService.CheckIfIsAuthorizedAsync(new AuthorizationPermissions[0]);
            return await appointmentService.UpdatePrescriptionDosis(idMedicalPrescription, idMedicalAppointment, dosis);
        }

        public async Task<PoseeReceta> GetDrugByConditionAsync(int drug_id, int appointmentId)
        {
            await _primeSecurityService.CheckIfIsAuthorizedAsync(new AuthorizationPermissions[0]);
            return await appointmentService.GetDrugByConditionAsync(drug_id, appointmentId);
        }

        public async Task<CitaMedica> GetMedicalAppointmentByAppointmentId(int id)
        {
            await _primeSecurityService.CheckIfIsAuthorizedAsync(new AuthorizationPermissions[0]);
            return await appointmentService.GetMedicalAppointmentByAppointmentId(id);
        }

        public async Task<CitaMedica> GetMedicalAppointmentByKeyAsync(int id)
        {
            await _primeSecurityService.CheckIfIsAuthorizedAsync(new AuthorizationPermissions[0]);
            return await appointmentService.GetMedicalAppointmentByKeyAsync(id);
        }

        public async Task<CitaMedica> GetMedicalAppointmentWithAppointmentByKeyAsync(int id)
        {
            await _primeSecurityService.CheckIfIsAuthorizedAsync(new AuthorizationPermissions[0]);
            return await appointmentService.GetMedicalAppointmentWithAppointmentByKeyAsync(id);
        }

        public async Task<Cita> GetAppointmentByKeyAsync(int id)
        {
            await _primeSecurityService.CheckIfIsAuthorizedAsync(new AuthorizationPermissions[0]);
            return await appointmentService.GetAppointmentByKeyAsync(id);
        }

        public async Task<PoseeReceta> InsertPrescription(int idMedicalPrescription, int idMedicalAppointment)
        {
            await _primeSecurityService.CheckIfIsAuthorizedAsync(new AuthorizationPermissions[0]);
            return await appointmentService.InsertPrescription(idMedicalPrescription, idMedicalAppointment);
        }

        public async Task<IEnumerable<RecetaMedica>> GetDrugsAsync()
        {
            await _primeSecurityService.CheckIfIsAuthorizedAsync(new AuthorizationPermissions[0]);
            return await appointmentService.GetDrugsAsync();
        }

        public async Task<IEnumerable<PoseeReceta>> GetPrescriptionsByAppointmentId(int id)
        {
            await _primeSecurityService.CheckIfIsAuthorizedAsync(new AuthorizationPermissions[0]);
            return await appointmentService.GetPrescriptionsByAppointmentId(id);
        }

        public async Task<IEnumerable<TipoAccion>> GetActionTypesAsync(bool isIncident = true)
        {
            await _primeSecurityService.CheckIfIsAuthorizedAsync(new AuthorizationPermissions[0]);
            return await appointmentService.GetActionTypesAsync();
        }

        public async Task<IEnumerable<TipoAccion>> GetActionsTypesMedicalAppointmentAsync(bool isMedAppointment = true)
        {
            await _primeSecurityService.CheckIfIsAuthorizedAsync(new AuthorizationPermissions[0]);
            return await appointmentService.GetActionsTypesMedicalAppointmentAsync();
        }

        public async Task<Expediente> AssignMedicalRecordAsync(int appointmentId, Paciente patient)
        {
            await _primeSecurityService.CheckIfIsAuthorizedAsync(new AuthorizationPermissions[0]);
            return await appointmentService.AssignMedicalRecordAsync(appointmentId, patient);
        }

        public async Task<Cita> GetLastAppointmentDateAsync(int id)
        {
            await _primeSecurityService.CheckIfIsAuthorizedAsync(new AuthorizationPermissions[0]);
            return await appointmentService.GetLastAppointmentDateAsync(id);
        }

        public async Task<CitaMedica> InsertMedicalAppointmentAsync(CitaMedica model)
        {
            await _primeSecurityService.CheckIfIsAuthorizedAsync(new AuthorizationPermissions[0]);
            return await appointmentService.InsertMedicalAppointmentAsync(model);
        }

        public async Task<EstadoCitaMedica> GetStatusById(int id) {
            await _primeSecurityService.CheckIfIsAuthorizedAsync(new AuthorizationPermissions[0]);
            return await appointmentService.GetStatusById(id);
        }
        public async Task<Cita> InsertAppointmentAsync(Cita model)
        {
            await _primeSecurityService.CheckIfIsAuthorizedAsync(new AuthorizationPermissions[0]);
            return await appointmentService.InsertAppointmentAsync(model);
        }
    }
}