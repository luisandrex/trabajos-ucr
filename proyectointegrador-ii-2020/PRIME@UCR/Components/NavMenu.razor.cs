using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PRIME_UCR.Domain.Models;

namespace PRIME_UCR.Components
{
    public partial class NavMenu
    {
        private bool collapseNavMenu = true;

        private string NavMenuCssClass => collapseNavMenu ? "collapse" : null;

        private string moduleToShow = "Dashboard";

        private string classOfModule = "oi oi-dashboard";

        private string showDashboard = "hide";

        private string showMedicalRecords = "show";

        private string showIncidents = "show";

        private string showMap = "show";

        private string showUserAdmin = "show";

        private string showCheckList = "show";

        private string incidentUrl = "incidents";
        private string mapUrl = "map";
        private string checklistUrl = "checklist";
        private string dashboardUrl = "dashboard";
        private string usersUrl = "user_administration/profiles";
        private string recordsUrl = "show-medical-record";

        protected string GetUrl()
        {
            switch (moduleToShow)
            {
                case "Dashboard":
                    return dashboardUrl;
                case "Expedientes":
                    return recordsUrl;
                case "Administración de usuarios":
                    return usersUrl;
                case "Listas de chequeo":
                    return checklistUrl;
                case "Incidentes":
                    return incidentUrl;
                case "Mapa de incidentes":
                    return mapUrl;
                default:
                    return null;
            }

        }

        protected override void OnInitialized()
        {
            string uri = MyNavegationManager.ToBaseRelativePath(MyNavegationManager.Uri);

            if (uri != "")
            {
                uri = uri.Split('/').First();
                if (uri == "dashboard")
                {
                    showDashboardMenu();
                }
                else if (uri == "incidents")
                {
                    showIncidentsMenu();
                }
                else if(uri == "map")
                {
                    showMapMenu();
                }
                else if (uri == "user_administration")
                {
                    showUserAdminMenu();
                }
                else if (uri == "show-medical-record")
                {
                    showMedicalRecordsMenu();
                }
                else if (uri == "checklist")
                {
                    showCheckListMenu();
                }
            }
        }

        private void ToggleNavMenu()
        {
            collapseNavMenu = !collapseNavMenu;
        }

        private void showIncidentsMenu()
        {
            moduleToShow = "Incidentes";
            showIncidents = "hide";
            showMap = showMedicalRecords = showUserAdmin = showCheckList = showDashboard = "show";
            classOfModule = "oi oi-map";
        }

        private void showMapMenu()
        {
            moduleToShow = "Mapa de incidentes";
            showMap = "hide";
            showIncidents = showMedicalRecords = showUserAdmin = showCheckList = showDashboard = "show";
            classOfModule = "oi oi-map-marker";
        }

        private void showCheckListMenu()
        {
            moduleToShow = "Listas de chequeo";
            showCheckList = "hide";
            showMedicalRecords = showUserAdmin = showIncidents = showMap = showDashboard = "show";
            classOfModule = "oi oi-list";
        }

        private void showMedicalRecordsMenu()
        {
            moduleToShow = "Expedientes";
            showMedicalRecords = "hide";
            showCheckList = showUserAdmin = showIncidents = showMap = showDashboard = "show";
            classOfModule = "oi oi-list";
        }

        private void showDashboardMenu()
        {
            moduleToShow = "Dashboard";
            showDashboard = "hide";
            showCheckList = showUserAdmin = showIncidents = showMap = showMedicalRecords = "show";
            classOfModule = "oi oi-dashboard";
        }

        private void showUserAdminMenu()
        {
            moduleToShow = "Administración de usuarios";
            showUserAdmin = "hide";
            showCheckList = showDashboard = showIncidents = showMap = showMedicalRecords = "show";
            classOfModule = "oi oi-book";
        }
    }
}
