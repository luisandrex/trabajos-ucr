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
DELETE FROM Expediente
DBCC CHECKIDENT ('Expediente', RESEED, 0)


INSERT INTO Expediente(CedulaPaciente, CedulaMedicoDuenno, FechaCreacion, Clinica)
VALUES
    ('12345678',NULL,'10/26/2020','clinica'),
    ('23456789',NULL,'10/26/2020','clinica'),
    ('34567890',NULL,'10/26/2020','clinica'),
    ('45678901',NULL,'10/26/2020','clinica'),
    ('56789012',NULL,'10/26/2020','clinica'),
    ('67890123',NULL,'10/26/2020','clinica'),
    ('78901234',NULL,'10/26/2020','clinica'),
    ('89012345',NULL,'10/26/2020','clinica'),
    ('90123456',NULL,'10/26/2020','clinica');



