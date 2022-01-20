using System.ComponentModel;

namespace PRIME_UCR.Domain.Constants
{
    /**
     * Enumeration used to know each of the policies names to be used in the authorization.
     * The idea is to used in a page a structure like the following to make the authorization:
     * Ej:      @attribute [HasPermission(AuthorizationPolicies.CanDoAnything)]
     *
     * Or in a piece of code a structure like the following:
     * Ej:
     * <AuthorizeView Policy="@AuthorizationPolicies.CanDoAnything">
            <Authorized>
                <h1>
                    Dashboard
                </h1>
            </Authorized>
            <NotAuthorized>
                <h1>
                    No dashboard
                </h1>
            </NotAuthorized>
        </AuthorizeView>
     * In both of them is needed to include the next namespace:
     * using PRIME_UCR.Application.DTOs.UserAdministration;
     * using PRIME_UCR.Application.Implementations.UserAdministration;
     */

    public enum AuthorizationPermissions
    {
        [Description("Administrar Usuarios")]
        CanManageUsers = 1,
        [Description("crear listas de checkeo")]
        CanCreateChecklist,
        [Description("instanciar lista de checkeo")]
        CanInstantiateChecklist,
        [Description("ver listado de incidentes")]
        CanSeeIncidentsList,
        [Description("observar la información médica del incidente")]
        CanSeeMedicalInfoInIncidentsList,
        [Description("observar los detalles básicos del incidente")]
        CanSeeBasicDetailsOfIncidents,
        [Description("observar los detalles médicos del incidente")]
        CanSeeMedicalDetailsOfIncidents,
        [Description("observar la información del paciente del incidente")]
        CanSeeInfoOfIncidentsPatient,
        [Description("ver la información de pacientes")]
        CanSeeMedicalRecordsOfHisPatients,
        [Description("observar listado de los expedientes")]
        CanSeeAllMedicalRecords,
        [Description("observar información médica en el dashboard")]
        CanSeeMedicalInfoOnDashboard,
        [Description("ver información de incidentes en el dashboard")]
        CanSeeIncidentsInfoOnDashboard,
        [Description("revisar los items en las listas de checkeo")]
        CanCheckItemsInChecklists,
        [Description("adjuntar multimadia a las listas de checkeo")]
        CanAttachMultimediaInChecklists,
        [Description("observar los detalles básicos del incidente")]
        CanEditBasicDetailsOfIncident,
        [Description("editar los detalles básicos del incidente")]
        CanEditMedicalDetailsOfIncident,
        [Description("revisar incidentes")]
        CanReviewIncidents,
        [Description("editar la información del paciente del indicente")]
        CanEditMedicalInfoOfIncidentsPatient,
        [Description("asignar personal al incindente")]
        CanAssignIncidents,
        [Description("crear incidentes")]
        CanCreateIncidents,
        [Description("ver el contenido multimedia del incidente")]
        CanManageIncidentMultimediaContent,
        [Description("manejar las listas de checkeo del incidente")]
        CanManageIncidentChecklists,
        [Description("visualizar mapas en tiempo real")]
        CanViewMapsInRealTime,
    }
}
