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


INSERT INTO Cita(FechaHoraCreacion, FechaHoraEstimada, IdExpediente)
VALUES
    (GETDATE(), DATEADD(month,-5,getdate()), 7),
	(GETDATE(), DATEADD(month,-4,getdate()), 7),
	(GETDATE(), DATEADD(month,-3,getdate()), 7),
	(GETDATE(), DATEADD(month,-2,getdate()), 7),
	(GETDATE(), DATEADD(month,-1,getdate()), 7),

    (GETDATE(), DATEADD(month,-4,getdate()), 8),
	(GETDATE(), DATEADD(month,-3,getdate()), 8),
	(GETDATE(), DATEADD(month,-2,getdate()), 8),
	(GETDATE(), DATEADD(month,-1,getdate()), 8),
	(GETDATE(), DATEADD(month,-1,getdate()), 8),

    (GETDATE(), DATEADD(month,-3,getdate()), 6),
	(GETDATE(), DATEADD(month,-2,getdate()), 6),
	(GETDATE(), DATEADD(month,-1,getdate()), 6),
	(GETDATE(), DATEADD(day,-15,getdate()), 6),
	(GETDATE(), DATEADD(day,-1,getdate()), 6),

    (GETDATE(), DATEADD(month,-5,getdate()), 5),
	(GETDATE(), DATEADD(month,-4,getdate()), 5),
	(GETDATE(), DATEADD(month,-3,getdate()), 5),
	(GETDATE(), DATEADD(month,-2,getdate()), 5),
	(GETDATE(), DATEADD(month,-1,getdate()), 5),

    (GETDATE(), DATEADD(month,-2,getdate()), 4),
	(GETDATE(), DATEADD(month,-1,getdate()), 4),
	(GETDATE(), DATEADD(day,-20,getdate()), 4),
	(GETDATE(), DATEADD(day,-10,getdate()), 4),

    (GETDATE(), DATEADD(month,-3,getdate()), 3),
	(GETDATE(), DATEADD(month,-2,getdate()), 3),
	(GETDATE(), DATEADD(month,-1,getdate()), 3),

    (GETDATE(), DATEADD(month,-2,getdate()), 2),
	(GETDATE(), DATEADD(day,-12,getdate()), 2),

	(GETDATE(), DATEADD(day,-1,getdate()), 1);

INSERT INTO CitaMedica(ExpedienteId, CedMedicoAsignado, CentroMedicoId, EstadoId, CitaId)
VALUES
    (7, 22222222, 2, 1, 45),
    (7, 22222222, 2, 1, 46),
    (7, 22222222, 2, 1, 47),
    (7, 22222222, 2, 1, 48),
    (7, 22222222, 2, 1, 49),

    (8, 22222222, 1, 1, 50),
    (8, 22222222, 1, 1, 51),
    (8, 22222222, 1, 1, 52),
    (8, 22222222, 1, 1, 53),
    (8, 22222222, 1, 1, 54),

    (6, 22222222, 3, 1, 55),
    (6, 22222222, 3, 1, 56),
    (6, 22222222, 3, 1, 57),
    (6, 22222222, 3, 1, 58),
    (6, 22222222, 3, 1, 59),

    (5, 22222222, 4, 1, 60),
    (5, 22222222, 4, 1, 61),
    (5, 22222222, 4, 1, 62),
    (5, 22222222, 4, 1, 63),
    (5, 22222222, 4, 1, 64),

    (4, 22222222, 1, 1, 65),
    (4, 22222222, 1, 1, 66),
    (4, 22222222, 1, 1, 67),
    (4, 22222222, 1, 1, 68),

    (3, 22222222, 2, 1, 69),
    (3, 22222222, 2, 1, 70),
    (3, 22222222, 2, 1, 71),

    (2, 22222222, 3, 1, 72),
    (2, 22222222, 3, 1, 73),

    (1, 22222222, 4, 1, 74);

/*Inserted by Atenienses ++*/

INSERT INTO MetricasCitaMedica(CitaId, Presion, Peso, Altura)
VALUES
    (50, 80, 41, 150),
    (51, 80, 40, 150),
    (52, 90, 43, 150),
    (53, 90, 42, 150),
    (54, 100, 42, 150),

    (45, 100, 55, 165),
    (46, 110, 57, 165),
    (47, 120, 60, 165),
    (48, 100, 62, 165),
    (49, 110, 60, 165),

    (55, 100, 80, 180),
    (56, 111, 90, 180),
    (57, 114, 95, 180),
    (58, 100, 89, 180),
    (59, 120, 90, 180),

    (60, 120, 45, 167),
    (61, 110, 46, 167),
    (62, 100, 47, 167),
    (63, 115, 50, 167),
    (64, 120, 49, 167),

    (65, 90, 75, 189),
    (66, 100, 78, 189),
    (67, 90, 74, 189),
    (68, 110, 80, 173),

    (69, 90, 75, 189),
    (70, 100, 78, 189),
    (71, 90, 74, 189),

     (72, 100, 80, 180),
    (73, 111, 90, 180),

     (74, 120, 63, 165);
