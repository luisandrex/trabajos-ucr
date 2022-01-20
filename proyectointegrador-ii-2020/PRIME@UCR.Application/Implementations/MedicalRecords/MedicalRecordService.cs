using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using PRIME_UCR.Application.DTOs.MedicalRecords;
using PRIME_UCR.Application.Repositories.Incidents;
using PRIME_UCR.Application.Repositories.MedicalRecords;
using PRIME_UCR.Application.Repositories.UserAdministration;
using PRIME_UCR.Application.Services.MedicalRecords;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Domain.Models.MedicalRecords;
using PRIME_UCR.Domain.Models.UserAdministration;
using PRIME_UCR.Infrastructure.Repositories.Sql.MedicalRecords;
using PRIME_UCR.Application.Repositories.Appointments;

namespace PRIME_UCR.Application.Implementations.MedicalRecords
{
    internal class MedicalRecordService : IMedicalRecordService
    {
        private readonly IMedicalRecordRepository _repo;
        private readonly IPersonaRepository _personRepo;
        private readonly IPacienteRepository _patientrepo;
        private readonly IFuncionarioRepository _medicrepo;
        private readonly IIncidentRepository _incidentrepo;
        private readonly IMedicalCenterRepository _mcrepo;
        private readonly IMedicalBackgroundRepository _mbrepo;
        private readonly IAlergyRepository _arepo;
        private readonly IAlergyListRepository _larepo;
        private readonly IMedicalBackgroundListRepository _lantrepo;
        private readonly IUbicationCenterRepository _ubcrepo;
        private readonly IMedCenterRepository _medcenrepo; 
        
        public MedicalRecordService(IMedicalRecordRepository repo, 
                                    IPersonaRepository personRepo, 
                                    IPacienteRepository repo1, 
                                    IFuncionarioRepository repo2, 
                                    IIncidentRepository incidentrepo, 
                                    IMedicalCenterRepository mcs, 
                                    IMedicalBackgroundRepository mbs,
                                    IAlergyRepository als,
                                    IAlergyListRepository alls,
                                    IMedicalBackgroundListRepository mbls,
                                    IUbicationCenterRepository ubc, 
                                    IMedCenterRepository mcr)
        {
            _repo = repo;
            _personRepo = personRepo;
            _patientrepo = repo1;
            _medicrepo = repo2;
            _incidentrepo = incidentrepo;
            _mcrepo = mcs;
            _mbrepo = mbs;
            _arepo = als;
            _larepo = alls;
            _lantrepo = mbls;
            _ubcrepo = ubc;
            _medcenrepo = mcr; 
        }



        public async Task<IEnumerable<CentroMedico>> GetMedicalCentersAsync() {
            return await _mcrepo.GetAllAsync(); 
        }


        public async Task<IEnumerable<Expediente>> GetByConditionAsync(string name)
        {

            string patient_name = "";
            string lastname1 = "";
            string lastname2 = "";

            int index_names = 0;

            int identification;

            bool is_numeric = int.TryParse(name, out identification);
            if (is_numeric)
            {
                return await _repo.GetByConditionAsync(_repo => _repo.Paciente.Cédula.Contains(name));
            }
            else
            {
                for (int index = 0; index < name.Length; ++index)
                {
                    if (name[index] != ' ')
                    {
                        switch (index_names)
                        {
                            case 0:
                                patient_name += name[index];
                                break;

                            case 1:
                                lastname1 += name[index];
                                break;

                            case 2:
                                lastname2 += name[index];
                                break;
                        }
                    }
                    else
                    {
                        ++index_names;
                    }
                }


                if (index_names == 0)
                {
                    return await _repo.GetByConditionAsync(_repo => _repo.Paciente.Nombre.Contains(name));
                }
                else
                {
                    if (index_names == 1)
                    {
                        return await _repo.GetByNameAndLastnameAsync(patient_name, lastname1);
                    }
                    else
                    {
                        return await _repo.GetByNameAndLastnameLastnameAsync(patient_name, lastname1, lastname2);
                    }
                }
            }
        }

        public async Task<IEnumerable<Expediente>> GetAllAsync()
        {
            return await _repo.GetRecordsWithPatientAsync(); 

        }

        public async Task<CentroMedico> GetMedicalCenterByUbicationCenterIdAsync(int id) {

            CentroUbicacion cu = await _ubcrepo.GetByKeyAsync(id);
            return await _medcenrepo.GetByKeyAsync(cu.CentroMedicoId); 
        }

        public async Task<IEnumerable<Paciente>> GetPatients()
        {
            return await _patientrepo.GetAllAsync();
        }

        public async Task<IEnumerable<Funcionario>> GetMedics()
        {
            return await _medicrepo.GetAllAsync();
        }

        public async Task<Expediente> GetByPatientIdAsync(string id)
        {
            return await _repo.GetByPatientIdAsync(id);
        }

        public async Task<Expediente> InsertAsync(Expediente expediente)
        {
            expediente.FechaCreacion = DateTime.Now;
            return await _repo.InsertAsync(expediente);
        }

        public async Task<Expediente> CreateMedicalRecordAsync(Expediente entity)
        {
            entity.FechaCreacion = DateTime.Now;
            return await _repo.InsertAsync(entity);
        }

        public async Task<Expediente> GetByIdAsync(int id)
        {
            return await _repo.GetByKeyAsync(id);
        }

        public async Task<IEnumerable<Incidente>> GetMedicalRecordIncidents(int recordId)
        {
            return await _incidentrepo.GetByConditionAsync(i => i.Cita.IdExpediente == recordId);
        }


        public async Task<Expediente> GetMedicalRecordDetailsLinkedAsync(int identification) {

            return await _repo.GetDetailsRecordWithPatientDoctorDatesAsync(identification); 
        }


        public async Task<Expediente> UpdateMedicalRecordAsync(Expediente expediente) {
            return await _repo.UpdateMedicalRecordAsync(expediente); 
        }

        public async Task<RecordViewModel> GetIncidentDetailsAsync(int id)
        {
            var record = await _repo.GetByKeyAsync(id);
            var person = await _personRepo.GetByKeyPersonaAsync(record.CedulaPaciente);
            var doctor = await _personRepo.GetByKeyPersonaAsync(record.CedulaMedicoDuenno);
            if (record != null)
            {
                var model = new RecordViewModel
                {
                    Cedula = person.Cédula,
                    Nombre = person.Nombre,
                    PrimerApellido = person.PrimerApellido,
                    SegundoApellido = person.SegundoApellido,
                    Sexo = person.Sexo,
                    FechaNacimiento = person.FechaNacimiento,
                    NombreMedico = doctor?.Nombre,
                    PrimerApellidoMedico = doctor?.PrimerApellido,
                    SegundoApellidoMedico = doctor?.SegundoApellido,
                    IdExpediente = record.Id,
                    FechaCreacion = record.FechaCreacion,
                    Clinica = record.Clinica

                };

                return model;
            }

            return null;
        }
        public async Task<IEnumerable<Antecedentes>> GetBackgroundByRecordId(int recordId)
        {
            IEnumerable<Antecedentes> test = await _mbrepo.GetByConditionAsync(i => i.IdExpediente == recordId);
            return test;
        }

        public async Task<IEnumerable<Alergias>> GetAlergyByRecordId(int recordId)
        {
            return await _arepo.GetByConditionAsync(i => i.IdExpediente == recordId);
        }

        public async Task<IEnumerable<ListaAlergia>> GetAllAlergies()
        {
            return await _larepo.GetAllAsync();
        }
        public async Task<IEnumerable<ListaAntecedentes>> GetAll()
        {
            return await _lantrepo.GetAllAsync();
        }
    }
}