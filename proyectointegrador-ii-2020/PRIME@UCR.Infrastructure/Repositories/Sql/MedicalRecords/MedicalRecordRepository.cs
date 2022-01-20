using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PRIME_UCR.Application.Repositories.MedicalRecords;
using PRIME_UCR.Domain.Models.MedicalRecords;
using PRIME_UCR.Infrastructure.DataProviders;
using RepoDb;

namespace PRIME_UCR.Infrastructure.Repositories.Sql.MedicalRecords
{
    public class MedicalRecordRepository : GenericRepository<Expediente, int>, IMedicalRecordRepository
    {
        public MedicalRecordRepository(ISqlDataProvider dataProvider) : base(dataProvider)
        {
        }

        public override async Task<Expediente> InsertAsync(Expediente model)
        {
            model.FechaCreacion = DateTime.Now;
            _db.MedicalRecords.Add(model);
            await _db.SaveChangesAsync();
            return model;
        }

        public async Task<Expediente> GetByPatientIdAsync(string id)
        {
            return await _db.MedicalRecords.FirstOrDefaultAsync(mr => mr.CedulaPaciente == id);
        }

        public async Task<IEnumerable<Expediente>> GetByNameAndLastnameAsync(string name, string lastname) {

            return await _db.MedicalRecords
                .Include(p => p.Paciente)
                .Where(p => p.Paciente.Nombre == name && p.Paciente.PrimerApellido == lastname)
                .ToListAsync(); 
        }


        public async Task<IEnumerable<Expediente>> GetRecordsWithPatientAsync() {
            return await _db.MedicalRecords
                .Include(p => p.Paciente)
                .ToListAsync(); 
        }


        public async Task<Expediente> GetDetailsRecordWithPatientDoctorDatesAsync(int id) {
            return await _db.MedicalRecords
                .Include(p => p.Paciente)
                .Include(m => m.Medico)
                .Include(c => c.Citas)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<Expediente>> GetByNameAndLastnameLastnameAsync(string name, string lastname, string lastname2) {

            return await _db.MedicalRecords
                .Include(p => p.Paciente)
                .Where(p => p.Paciente.Nombre == name && p.Paciente.PrimerApellido == lastname && p.Paciente.SegundoApellido == lastname2)
                .ToListAsync(); 

        }

        public async Task<Expediente> UpdateMedicalRecordAsync(Expediente expediente) {

            await using var connection = new SqlConnection(_db.ConnectionString);

            if (expediente.CedulaMedicoDuenno != null && expediente.Clinica != null)
            {
                var result = await connection.ExecuteQueryAsync<Expediente>
                (@"UPDATE Expediente SET CedulaMedicoDuenno = @cedMed, Clinica = @clinic WHERE CedulaPaciente = 
                     @cedP", new
                {
                    cedMed = expediente.CedulaMedicoDuenno,
                    clinic = expediente.Clinica,
                    cedP = expediente.CedulaPaciente
                });
            }
            else {
                if (expediente.CedulaMedicoDuenno != null)
                {
                    var result = await connection.ExecuteQueryAsync<Expediente>
                    (@"UPDATE Expediente SET CedulaMedicoDuenno = @cedMed WHERE CedulaPaciente = 
                     @cedP", new
                    {
                        cedMed = expediente.CedulaMedicoDuenno,
                        cedP = expediente.CedulaPaciente
                    });
                }
                else {

                    var result = await connection.ExecuteQueryAsync<Expediente>
                    (@"UPDATE Expediente SET Clinica = @clinic WHERE CedulaPaciente = 
                     @cedP", new
                    {
                        clinic = expediente.Clinica,
                        cedP = expediente.CedulaPaciente
                    });
                }
            
            }
            return null; 
        }


        public async Task<Expediente> GetWithDetailsAsync(int id)
        {
            return await _db.MedicalRecords
                .Include(i => i.CedulaPaciente)
                .Include(i => i.CedulaMedicoDuenno)
                .Include(i => i.Id)
                .FirstOrDefaultAsync(i => i.Id == id);
        }
    }
}