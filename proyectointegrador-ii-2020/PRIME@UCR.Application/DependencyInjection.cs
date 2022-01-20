using Microsoft.Extensions.DependencyInjection;
using PRIME_UCR.Application.Implementations;
using PRIME_UCR.Application.Implementations.Appointments;
using PRIME_UCR.Application.Implementations.CheckLists;
using PRIME_UCR.Application.Services.CheckLists;
using PRIME_UCR.Application.Implementations.Incidents;
using PRIME_UCR.Application.Implementations.MedicalRecords;
using PRIME_UCR.Application.Services;
using PRIME_UCR.Application.Services.Incidents;
using PRIME_UCR.Application.Services.Multimedia;
using PRIME_UCR.Application.Implementations.Multimedia;
using PRIME_UCR.Application.Implementations.UserAdministration;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Application.Repositories.Incidents;
using PRIME_UCR.Application.Services.Appointments;
using PRIME_UCR.Application.Services.MedicalRecords;
using PRIME_UCR.Application.Services.Dashboard;
using PRIME_UCR.Application.Implementations.Dashboard;
using Microsoft.AspNetCore.Components.Authorization;
using PRIME_UCR.Infrastructure.Repositories.Sql.MedicalRecords;
using PRIME_UCR.Application.Permissions.UserAdministration;
using PRIME_UCR.Application.Permissions.Incidents;
using PRIME_UCR.Application.Permissions.Dashboard;
using PRIME_UCR.Application.Permissions.CheckLists;
using PRIME_UCR.Application.Permissions.Appointments;
using PRIME_UCR.Application.Permissions.MedicalRecord;

namespace PRIME_UCR.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            // services
            services.AddTransient<ICheckListService, SecureCheckListService>();
            services.AddTransient<IInstanceChecklistService, SecureInstanceChecklistService>();
            services.AddTransient<IPdfService, PdfService>();
            // incidents
            services.AddTransient<IAssignmentService, SecureAssignmentService>();
            services.AddTransient<IIncidentService, SecureIncidentService>();
            services.AddTransient<IStateService, SecureStateService>();
            services.AddTransient<ILocationService, LocationService>();
            services.AddTransient<IGpsDataService, GpsDataService>();

            // medical records
            services.AddTransient<IMedicalRecordService, SecureMedicalRecordService>();
            services.AddTransient<IMedicalBackgroundService, SecureMedicalBackgroundService>();
            services.AddTransient<IAlergyService, SecureAlergyService>();
            services.AddTransient<IChronicConditionService, SecureChronicConditionService>();
            services.AddTransient<IAppointmentService, SecureAppointmentService>();
            // multimedia
            services.AddTransient<IMultimediaContentService, MultimediaContentService>();
            services.AddSingleton<IEncryptionService, EncryptionService>();

            // user administration
            services.AddScoped<IPermissionsService, SecurePermissionService>();
            services.AddScoped<IProfilesService, SecureProfilesService>();
            services.AddScoped<IUserService, SecureUserService>();
            services.AddTransient<IPermiteService, SecurePermiteService>();
            services.AddTransient<IDoctorService, SecureDoctorService>();
            services.AddTransient<IPerteneceService, SecurePerteneceService>();
            services.AddTransient<IPersonService, SecurePersonService>();
            services.AddTransient<IPatientService, SecurePatientService>();
            services.AddTransient<INumeroTelefonoService, SecureNumeroTelefonoService>();

            services.AddTransient<IAuthenticationService, AuthenticationService>();

            //Dashboard
            services.AddTransient<IDashboardService, SecureDashboardService>();
            services.AddTransient<IFileManagerService, FileManagerService>();
            services.AddTransient<IMailService, MailService>();
            return services;
        }
    }
}
