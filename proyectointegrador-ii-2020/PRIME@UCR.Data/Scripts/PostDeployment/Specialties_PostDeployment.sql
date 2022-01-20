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
/*
DELETE FROM SeEspecializa
DELETE FROM EspecialidadMedica

INSERT INTO EspecialidadMedica(Nombre)
VALUES
    ('Dermatología'),
    ('Psiquiatría'),
    ('Oftalmología'),
    ('Radiología'),
    ('Pediatría'),
    ('Neurocirugía');

INSERT INTO SeEspecializa(NombreEspecialidad, CedulaMedico)
VALUES
    ('Dermatología', '22222222'),
    ('Psiquiatría', '22222222'),
    ('Oftalmología', '22222222'),
    ('Radiología', '89012345'),
    ('Pediatría', '89012345'),
    ('Neurocirugía', '89012345');
*/