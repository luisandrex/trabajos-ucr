using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PRIME_UCR.Application.DTOs.Incidents;
using PRIME_UCR.Application.Repositories.Incidents;
using PRIME_UCR.Domain.Constants;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Domain.Models.Incidents;
using PRIME_UCR.Domain.Models.MedicalRecords;
using PRIME_UCR.Domain.Models.UserAdministration;
using PRIME_UCR.Infrastructure.DataProviders;
using RepoDb;
using RepoDb.Extensions;

namespace PRIME_UCR.Infrastructure.Repositories.Sql.Incidents
{
    public class IncidentRepository : RepoDbRepository<Incidente, string>, IIncidentRepository
    {
        public IncidentRepository(ISqlDataProvider dataProvider) : base(dataProvider)
        {
        }

        public override async Task<Incidente> InsertAsync(Incidente model)
        {
            using (var connection = new SqlConnection(_db.ConnectionString))
            {
                var parameters = new Dictionary<string, object>
                {
                    {"CedulaAdmin", model.CedulaAdmin},
                    {"Modalidad", model.Modalidad},
                    {"FechaHoraRegistro", DateTime.Now},
                    {"FechaHoraEstimada", model.Cita.FechaHoraEstimada},
                    {"CedulaTecnicoCoordinador", null},
                    {"CedulaTecnicoRevisor", null},
                    {"IdOrigen", null},
                    {"IdDestino", null},
                    {"MatriculaTrans", null}
                };

                var result = await connection.ExecuteScalarAsync(
                    "dbo.InsertarNuevoIncidente", parameters, CommandType.StoredProcedure);

                model.Codigo = result.ToString();

                return model;
            }
        }

        public async Task<Incidente> GetIncidentByDateCodeAsync(int id) {
            return await _db.Incidents
                    .Include(p => p.Origen)
                    .Include(p => p.Destino)
                    .Include(p => p.EstadoIncidentes)
                    .FirstOrDefaultAsync(i => i.CodigoCita == id);
        }

        public async Task<Incidente> GetWithDetailsAsync(string code)
        {
            Incidente incident;
            using (var connection = new SqlConnection(_db.ConnectionString))
            {
                incident =
                   (await connection.QueryAsync<Incidente>(code))
                   .FirstOrDefault();

                if (incident != null)
                {
                    incident.Cita =
                        (await connection.QueryAsync<Cita>(incident.CodigoCita))
                        .FirstOrDefault();


                    if (incident.Cita?.IdExpediente != null)
                        incident.Cita.Expediente =
                            (await connection.QueryAsync<Expediente>(incident.Cita.IdExpediente))
                            .FirstOrDefault();
                }
            }

            var modelWithLocations = await _db.Incidents
                .AsNoTracking()
                .Include(i => i.Origen)
                .Include(i => i.Destino)
                .FirstOrDefaultAsync(i => i.Codigo == code);

            if (incident != null)
            {
                incident.Origen = modelWithLocations.Origen;
                incident.Destino = modelWithLocations.Destino;
            }

            return incident;
        }

        /*
         * Function: Obtains all the incidents registered in the system, with their respective details
         * @Return: A list with all the incidents registered in the system
         * @TransactionLevel: 
         *      If another transactions tries to register an incident but fails to do so, it would be an error to show that incident, since it wasn't registered correctly in the system
         *          Therefore, we can't set the isolation level to read uncommitted
         *      Since the application manages medical information (which is sometimes critical), we can't slow down the process of registering new incidents with the isolation level set as serializable
         *      Since the incidents are not used in any aggregate function (mean, max, etc), we don't need to guarantee repeteable read
         *          Therefore, the transaction isolation level is set as read committed
         */
        public async Task<IEnumerable<IncidentListModel>> GetIncidentListModelsAsync()
        {
            using (var connection = new SqlConnection(_db.ConnectionString))
            {
                var sql =
                    // Incidente
                    @"
                        set implicit_transactions off;
	                    set transaction isolation level read committed;
	                    begin transaction t1;
                        select *
                        from Incidente I
                        Order by I.Codigo Desc;
                    " +
                    // Cita
                    @"
                        select C.*
                        from Incidente I
                        join Cita C on C.Id = I.CodigoCita;
                    " +
                    // Destino - CentroUbicacion
                    @"
                        select CU.*
                        from Incidente
                        left join Ubicacion U on Incidente.IdDestino = U.Id
                        left join Centro_Ubicacion CU on U.Id = CU.Id
                    " +
                    // Estado
                    @"
                        select EI.*
                        from Incidente
                        join EstadoIncidente EI on Incidente.Codigo = EI.CodigoIncidente
                        where EI.Activo = 1;
                        commit transaction t1;
                    ";

                using (var result = await connection.ExecuteQueryMultipleAsync(sql))
                {
                    var incidents = await result.ExtractAsync<Incidente>();
                    var appointments = await result.ExtractAsync<Cita>();
                    var destinations = await result.ExtractAsync<CentroUbicacion>();
                    var medicalCenters = await connection.QueryAllAsync<CentroMedico>();
                    var states = await result.ExtractAsync<EstadoIncidente>();
                    var origins =
                        _db.Incidents
                            .AsNoTracking()
                            .Include(i => i.Origen)
                            .Select(i => i.Origen);

                    return from i in incidents
                           join a in appointments
                               on i.CodigoCita equals a.Id
                           join s in states
                               on i.Codigo equals s.CodigoIncidente
                           join add in origins
                               on i.IdOrigen equals add?.Id
                               into orgs
                           from o in orgs.DefaultIfEmpty() // left join
                           join add in destinations
                               on i.IdDestino equals add?.Id
                               into cUs
                           from d in cUs.DefaultIfEmpty() // left join
                           join add in medicalCenters
                               on d?.CentroMedicoId equals add?.Id
                               into cMs
                           from mc in cMs.DefaultIfEmpty() // left join
                           select new IncidentListModel
                           {
                               Codigo = i.Codigo,
                               FechaHoraRegistro = a.FechaHoraCreacion,
                               Modalidad = i.Modalidad,
                               Origen = o?.DisplayName,
                               Estado = s.NombreEstado,
                               IdDestino = i.IdDestino,
                               Destino = mc?.Nombre
                           };
                }
            }
        }

        public async Task<Médico> GetAssignedOriginDoctor(string code)
        {
            var incident = await _db.Incidents
                .Include(i => i.Origen)
                .FirstOrDefaultAsync(i => i.Codigo == code);

            if (incident?.Origen is CentroUbicacion)
            {
                var cu = incident.Origen as CentroUbicacion;
                if (cu != null) return await GetDoctorById(cu.CedulaMedico);
            }

            return null;
        }

        public async Task<Médico> GetAssignedDestinationDoctor(string code)
        {
            var incident = await _db.Incidents
                .Include(i => i.Destino)
                .FirstOrDefaultAsync(i => i.Codigo == code);

            if (incident?.Destino is CentroUbicacion)
            {
                var cu = incident.Destino as CentroUbicacion;
                if (cu != null) return await GetDoctorById(cu.CedulaMedico);
            }

            return null;
        }

        public async Task<IEnumerable<OngoingIncident>> GetOngoingIncidentsAsync(Modalidad unitType, Estado state)
        {
            // query all incidents with required data
            var data = _db.Incidents
                .AsNoTracking()
                .Include(i => i.Origen)
                .Include(i => i.Destino)
                .Include(i => i.UnidadDeTransporte)
                .Include(i => i.EstadoIncidentes)
                    .ThenInclude(i => i.Estado);

            Expression<Func<Incidente, bool>> query;
            if (unitType != null)
                query = i => i.Modalidad == unitType.Tipo;
            else
                query = (_) => true;

            var filteredData = await data
                .Where(query)
                .Select(i => new OngoingIncident // create OngoingIncident DTO to return
                {
                    Code = i.Codigo,
                    Incident = i,
                    Origin = i.Origen,
                    Destination = i.Destino,
                    TransportUnit = i.UnidadDeTransporte,
                    UnitType = new Modalidad { Tipo = i.Modalidad }
                })
                .ToListAsync();

            if (state != null)
                return filteredData.Where(i =>
                    IncidentStates.OngoingStates.Exists(s =>
                        i.Incident.EstadoActivo.Nombre == s.Nombre)
                    && i.Incident.EstadoActivo.Nombre == state.Nombre);

            return filteredData.Where(i =>
                IncidentStates.OngoingStates.Exists(s =>
                    i.Incident.EstadoActivo.Nombre == s.Nombre));
        }

        public override async Task<Incidente> GetByKeyAsync(string key)
        {
            using (var connection = new SqlConnection(_db.ConnectionString))
            {
                var incident =
                    (await connection.QueryAsync<Incidente>(i => i.Codigo == key))
                    .FirstOrDefault();

                if (incident != null)
                    incident.Cita =
                        (await connection.QueryAsync<Cita>(c => c.Id == incident.CodigoCita))
                        .FirstOrDefault();

                return incident;
            }
        }

        private async Task<Médico> GetDoctorById(string id)
        {
            using (var connection = new SqlConnection(_db.ConnectionString))
            {
                return (await connection.ExecuteQueryAsync<Médico>(@"
                    select Persona.* from Persona
                    join Funcionario F on Persona.Cédula = F.Cédula
                    join Médico M on F.Cédula = M.Cédula
                    where M.Cédula = @Id
                ", new { Id = id }))
                .FirstOrDefault();
            }
        }

        /*
         * Function: Query to get the last change in an specific incident.
         * @Return: A table of CambioIncidente that provides the date, the responsible and the tab of the last change in the incident.
         * @TransactionLevel: 
         *      If another transactions tries to make a change in an incident (in Patient, Origin, Destination or Assignment tabs) but fails to do so, it would be an 
         *      error to show that change as the last change in an incident, since it wasn't actually a change.Therefore, we can't set the isolation 
         *      level to read uncommitted
         *      
         *      Since the application needs to be able to handle several changes from various authorized users as quick as possible, we can't slow down the process of
         *      making changes to an incidents with the isolation level set as serializable. Since the incidents are not used in any aggregate function (mean, max, etc),
         *      we don't need to guarantee repeteable read. Therefore, the transaction isolation level is set as read committed
         */
        public async Task<CambioIncidente> GetLastChange(string code)
        {
            using (var connection = new SqlConnection(_db.ConnectionString))
            {
                return (await connection.ExecuteQueryAsync<CambioIncidente>(@"
                    set implicit_transactions off;
	                set transaction isolation level read committed;
	                begin transaction t1;
                    select CambioIncidente.* from CambioIncidente
                    where CodigoIncidente = @Id
                    order by FechaHora desc
                    commit transaction t1;
                ", new { Id = code }))
                .FirstOrDefault();
            }
        }

        public async Task UpdateLastChange(CambioIncidente change)
        {
            using (var connection = new SqlConnection(_db.ConnectionString))
            {
                var incident =
                    (await connection.QueryAsync<Incidente>(i => i.Codigo == change.CodigoIncidente))
                    .FirstOrDefault();
                if (incident != null)
                    change.Incidente = incident;
                await connection.InsertAsync(change);
            }
        }

        /*
         * Function: Obtains the Incident´s codes for which a specific Doctor is assigned to the incident´s origin
         * @Params: The id (cedula) of the doctor to be checked
         * @Return: A list with all the incidents' codes where the specified Id is assigned as origin doctor
         * @Story ID: PIG01IIC20-712
         */
        public async Task<IEnumerable<string>> GetAuthorizedCodesForOriginDoctor(string id)
        {
            using (var connection = new SqlConnection(_db.ConnectionString))
            {
                var authorizedCodes = await connection.ExecuteQueryAsync<string>(@"
                set implicit_transactions off;
	            set transaction isolation level read committed;
	            begin transaction t1;
                select i.Codigo from Incidente i
                join Centro_Ubicacion c on i.IdOrigen = c.Id
                where c.CédulaMédico = @Id
                commit transaction t1;
                ", new { Id = id });
                return authorizedCodes;
            }
        }

        /*
         * Function: Obtains the Incident´s codes for which a specific Doctor is assigned to the incident´s destination
         * @Params: The id (cedula) of the doctor to be checked
         * @Return: A list with all the incidents' codes where the specified Id is assigned as destination doctor
         * @Story ID: PIG01IIC20-712
         */
        public async Task<IEnumerable<string>> GetAuthorizedCodesForDestinationDoctor(string id)
        {
            using (var connection = new SqlConnection(_db.ConnectionString))
            {
                var authorizedCodes = await connection.ExecuteQueryAsync<string>(@"
                set implicit_transactions off;
	            set transaction isolation level read committed;
	            begin transaction t1;
                select i.Codigo from Incidente i
                join Centro_Ubicacion c on i.IdDestino = c.Id
                where c.CédulaMédico = @Id
                commit transaction t1;
                ", new { Id = id });
                return authorizedCodes;
            }
        }


        /*
         * Function: Obtains the incident's codes for which a specific Technical Specialist is assigned to
         * @Params: The id (cedula) of the specialist to be checked
         * @Return: A list with all the incidents' codes where the specified Id is assigned to
         * @Story ID: PIG01IIC20-712
         */
        public async Task<IEnumerable<string>> GetAuthorizedCodesForSpecialist(string id)
        {
            using (var connection = new SqlConnection(_db.ConnectionString)) 
            {
                
                var authorizedCodes = await connection.ExecuteQueryAsync<string>(@"
                    set implicit_transactions off;
	                set transaction isolation level read committed;
	                begin transaction t1;
                    select i.Codigo from Incidente i
                    join AsignadoA a on a.Codigo = i.Codigo
                    where a.CedulaEspecialista = @Id
                    commit transaction t1;
                    ", new { Id = id });
                return authorizedCodes;
                
            } 
        }

        /*
         * Function: Returns a list with all the incidents and their details that are assigned to a specified technical specialist
         * @Params: The id (cedula) of the specialist to be checked
         * @Return: A list with all the incidents' details where the specified id is assigned to
         * @Story ID: PIG01IIC20-712
         */
        public async Task<IEnumerable<IncidentListModel>> GetAuthorizedSpecialistIncidentListModelsAsync(string id)
        {
            var authorizedCodes = await GetAuthorizedCodesForSpecialist(id); // Get only the authorized incidents for this user 
            var incidentsList = await GetIncidentListModelsAsync(); // Get all incidents
            return incidentsList.Where(i => authorizedCodes.Contains(i.Codigo));  // Return the details of the incidents assigned to the user
        }

        /*
        * Function: Returns a list with all the incidents and their details that are assigned to a specified doctor
         * @Params: The id (cedula) of the doctor to be checked
         * @Return: A list with all the incidents' details where the specified id is assigned to
         * @Story ID: PIG01IIC20-712
        */
        public async Task<IEnumerable<IncidentListModel>> GetAuthorizedDoctorIncidentListModelsAsync(string id)
        {
            var authorizedOriginCodes = await GetAuthorizedCodesForOriginDoctor(id); // Get only the authorized incidents for this user 
            var authorizedDestinationCodes = await GetAuthorizedCodesForDestinationDoctor(id); // Get only the authorized incidents for this user 
            var incidentsList = await GetIncidentListModelsAsync(); // Get all incidents
            return incidentsList.Where(i => authorizedOriginCodes.Contains(i.Codigo) || authorizedDestinationCodes.Contains(i.Codigo)); // Return the details of the incidents assigned to the user
        }
    }

 }