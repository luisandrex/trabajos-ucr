using System.Data;
using Microsoft.Extensions.DependencyInjection;
using PRIME_UCR.Application.Implementations.Multimedia;
using PRIME_UCR.Application.Repositories;
using PRIME_UCR.Application.Repositories.Appointments;
using PRIME_UCR.Application.Repositories.CheckLists;
using PRIME_UCR.Application.Repositories.Dashboard;
using PRIME_UCR.Application.Repositories.Incidents;
using PRIME_UCR.Application.Repositories.MedicalRecords;
using PRIME_UCR.Application.Repositories.Multimedia;
using PRIME_UCR.Application.Repositories.UserAdministration;
using PRIME_UCR.Application.Services.Multimedia;
using PRIME_UCR.Infrastructure.DataProviders;
using PRIME_UCR.Infrastructure.DataProviders.Implementations;
using PRIME_UCR.Infrastructure.Permissions.Dashboard;
using PRIME_UCR.Infrastructure.Permissions.Incidents;
using PRIME_UCR.Infrastructure.Permissions.UserAdministration;
using PRIME_UCR.Infrastructure.Repositories.External;
using PRIME_UCR.Infrastructure.Repositories.Sql;
using PRIME_UCR.Infrastructure.Repositories.Sql.Appointments;
using PRIME_UCR.Infrastructure.Repositories.Sql.CheckLists;
using PRIME_UCR.Infrastructure.Repositories.Sql.Dashboard;
using PRIME_UCR.Infrastructure.Repositories.Sql.Incidents;
using PRIME_UCR.Infrastructure.Repositories.Sql.MedicalRecords;
using PRIME_UCR.Infrastructure.Repositories.Sql.Multimedia;
using PRIME_UCR.Infrastructure.Repositories.Sql.UserAdministration;

namespace PRIME_UCR.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services)
        {
            RepoDb.SqlServerBootstrap.Initialize();

            // data providers
            services.AddTransient<ISqlDataProvider, ApplicationDbContext>();

            // repositories
            // generic repositories
            services.AddTransient(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
            services.AddTransient(typeof(IRepoDbRepository<,>), typeof(RepoDbRepository<,>));

            // checklists
            services.AddTransient<ICheckListRepository, SqlCheckListRepository>();
            services.AddTransient<ICheckListTypeRepository, SqlCheckListTypeRepository>();
            services.AddTransient<IItemRepository, SqlItemRepository>();
            services.AddTransient<IInstanceChecklistRepository, SqlInstanceChecklistRepository>();
            services.AddTransient<IInstanceItemRepository, SqlInstanceItemRepository>();

            // appointments
            services.AddTransient<IActionTypeRepository, ActionTypeRepository>();
            services.AddTransient<IAssignmentRepository, AssignmentRepository>();
            services.AddTransient<IAppointmentRepository, AppointmentRepository>();
            services.AddTransient<IUbicationCenterRepository, UbicationCenterRepository>();
            services.AddTransient<IMedCenterRepository, MedCenterRepository>();
            services.AddTransient<IMedicalAppointmentRepository, MedicalAppointmentRepository>();
            services.AddTransient<IHavePrescriptionRepository, HavePrescriptionRepository>();
            services.AddTransient<IDrugRepository, DrugRepository>();
            services.AddTransient<IDrugRepository, DrugRepository>();
            services.AddTransient<IMedAppMetricRepository, MedAppMetricRepository>();
            services.AddTransient<IMedicalSpecialtyRepository, MedicalSpecialtyRepository>();
            services.AddTransient<ISpecializesRepository, SpecializesRepository>();
            services.AddTransient<IAppointmentReferenceRepository, AppointmentReferenceRepository>();
            services.AddTransient<IAppointmentStatusRepository, AppointmentStatusRepository>();
            // incidents repositories
            services.AddTransient<IStateRepository, SecureStateRepository>();
            services.AddTransient<ICountryRepository, CountryRepository>();
            services.AddTransient<IProvinceRepository, ProvinceRepository>();
            services.AddTransient<ICantonRepository, CantonRepository>();
            services.AddTransient<IDistrictRepository, DistrictRepository>();
            services.AddTransient<IMedicalCenterRepository, MedicalCenterRepository>();
            services.AddTransient<IModesRepository, ModesRepository>();
            services.AddTransient<IIncidentRepository, IncidentRepository>();
            services.AddTransient<IIncidentStateRepository, IncidentStateRepository>();
            services.AddTransient<ILocationRepository, LocationRepository>();
            services.AddTransient<ITransportUnitRepository, TransportUnitRepository>();
            services.AddTransient<IActionTypeRepository, ActionTypeRepository>();
            services.AddTransient<IDocumentacionIncidenteRepository, DocumentacionIncidenteRepository>();

            // medical records
            services.AddTransient<IMedicalRecordRepository, MedicalRecordRepository>();
            services.AddTransient<IMedicalBackgroundRepository, MedicalBackgroundRepository>();
            services.AddTransient<IMedicalBackgroundListRepository, MedicalBackgroundListRepository>();
            services.AddTransient<IAlergyRepository, AlergyRepository>();
            services.AddTransient<IAlergyListRepository, AlergyListRepository>();
            services.AddTransient<IChronicConditionRepository, ChronicConditionRepository>();
            services.AddTransient<IChronicConditionListRepository, ChronicConditionListRepository>();

            // multimedia
            services.AddTransient<IMultimediaContentRepository, MultimediaContentRepository>();
            services.AddTransient<IActionRepository, ActionRepository>();
            services.AddTransient<IFileService, FileService>();
            services.AddTransient<IMultimediaContentItemRepository, MultimediaContentItemRepository>();

            // user administration repositories
            services.AddTransient<ICoordinadorTécnicoMédicoRepository, SecureCoordinadorTécnicoMédicoRepository>();
            services.AddTransient<IDoctorRepository, SecureDoctorRepository>();
            services.AddTransient<IEspecialistaTécnicoMédicoRepository, SecureEspecialistaTécnicoMédicoRepository>();
            services.AddTransient<IFuncionarioRepository, SecureFuncionarioRepository>();
            services.AddTransient<INumeroTelefonoRepository, SecureNumeroTelefonicoRepository>();
            services.AddTransient<IPacienteRepository, SecurePacienteRepository>();
            services.AddTransient<IPermisoRepository, SecurePermisoRepository>();
            services.AddTransient<IPermiteRepository, SecurePermiteRepository>();
            services.AddTransient<IPersonaRepository, SecurePersonaRepository>();
            services.AddTransient<IPerteneceRepository, SecurePerteneceRepository>();
            services.AddTransient<IUsuarioRepository, SecureUsuarioRepository>();
            services.AddTransient<IAdministradorRepository, AdministradorRepository>();
            services.AddTransient<IAdministradorCentroDeControlRepository, AdministradorCentroDeControlRepository>();
            services.AddTransient<IGerenteMédicoRepository, GerenteMédicoRepository>();
            services.AddTransient<IPerfilRepository, PerfilRepository>();
            services.AddTransient<IAuthenticationRepository, AuthenticationRepository>();

            //dashboard repositories
            services.AddTransient<IDashboardRepository, SecureDashboardRepository>();

            // external
            services.AddScoped<IGpsDataRepository, GpsDataRepository>();

            return services;
        }
    }
}
