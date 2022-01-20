/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
:r .\PostDeployment\Users_PostDeployment.sql
:r .\PostDeployment\Identity_PostDeployment.sql
:r .\PostDeployment\Appointments_PostDeployment.sql
:r .\PostDeployment\Incidents_PostDeployment.sql
:r .\PostDeployment\Checklists_PostDeployment.sql
:r .\PostDeployment\MedicalRecords_PostDeployment.sql
:r .\PostDeployment\Alergies_PostDeployment.sql
:r .\PostDeployment\MedAppointments_PostDeployment.sql
:r .\PostDeployment\Specialties_PostDeployment.sql
