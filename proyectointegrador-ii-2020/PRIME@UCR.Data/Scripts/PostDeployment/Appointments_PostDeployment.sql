DELETE FROM Cita
DELETE FROM MetricasCitaMedica
DELETE FROM Accion
DELETE FROM TipoAccion
DELETE FROM EstadoCitaMedica
DELETE FROM RecetaMedica

DBCC CHECKIDENT ('Cita', RESEED, 0)
DBCC CHECKIDENT ('EstadoCitaMedica', RESEED, 0)

INSERT INTO TipoAccion (Nombre, EsDeCitaMedica, EsDeIncidente)
VALUES
	('Complicación en traslado', 0, 1),
	('Condición de colecta de paciente', 0, 1),
	('Condición de entrega de paciente', 0, 1),
	('Síntomas', 1, 1),
	('Revisiones', 1, 0),
	('Diagnóstico', 1, 0),
	('Antecedentes', 1, 0)


INSERT INTO EstadoCitaMedica (NombreEstado)
VALUES
	('Pendiente'),
	('Activa'),
	('Finalizada');


INSERT INTO RecetaMedica(NombreReceta)
VALUES 
	('Paracetamol'), 
	('Clorfenamina'),
	('Beclometasona'),
	('Dextrometorfano'), 
	('Omeprazol'),
	('Atorvastatina'), 
	('Aspirina'), 
	('Ramipril');

INSERT INTO Cita (FechaHoraCreacion, FechaHoraEstimada)
VALUES
	(GETDATE(),DATEADD(day, -1, getdate())),
	(GETDATE(),DATEADD(day, -2, getdate())),
	(GETDATE(),DATEADD(day, -2, getdate())),
	(GETDATE(),DATEADD(day, -2, getdate())),
	(GETDATE(),DATEADD(day, -3, getdate())),
	(GETDATE(),DATEADD(day, -3, getdate())),
	(GETDATE(),DATEADD(day, -4, getdate())),
	(GETDATE(),DATEADD(day, -5, getdate())),
	(GETDATE(),DATEADD(day, -6, getdate())),
	(GETDATE(),DATEADD(day, 0, getdate())),
	(GETDATE(),DATEADD(day, 0, getdate())),

	--Created by Atenineses
	(GETDATE(),DATEADD(day, 0, getdate())),
	(GETDATE(),DATEADD(day, 0, getdate())),
	(GETDATE(),DATEADD(day, 0, getdate())),
	(GETDATE(),DATEADD(day, 1, getdate())),
	(GETDATE(),DATEADD(day, 2, getdate())),
	(GETDATE(),DATEADD(day, 2, getdate())),
	(GETDATE(),DATEADD(day, 2, getdate())),
	(GETDATE(),DATEADD(day, 2, getdate())),
	(GETDATE(),DATEADD(day, 3, getdate())),
	(GETDATE(),DATEADD(day, 3, getdate())),
	(GETDATE(),DATEADD(day, 3, getdate())),
	(GETDATE(),DATEADD(day, 3, getdate())),
	(GETDATE(),DATEADD(day, 4, getdate())),
	(GETDATE(),DATEADD(day, 4, getdate())),
	(GETDATE(),DATEADD(day, 5, getdate())),
	(GETDATE(),DATEADD(day, 5, getdate())),
	(GETDATE(),DATEADD(day, 5, getdate())),
	(GETDATE(),DATEADD(day, 6, getdate())),
	(GETDATE(),DATEADD(day, 7, getdate())),
	(GETDATE(),DATEADD(day, 7, getdate())),
	(GETDATE(),DATEADD(day, 7, getdate())),
	(GETDATE(),DATEADD(day, 7, getdate())),
	(GETDATE(),DATEADD(day, 7, getdate())),
	(GETDATE(),DATEADD(day, 7, getdate())),
	(GETDATE(),DATEADD(day, 8, getdate())),
	(GETDATE(),DATEADD(day, 8, getdate())),
	(GETDATE(),DATEADD(day, 8, getdate())),
	(GETDATE(),DATEADD(day, 9, getdate())),
	(GETDATE(),DATEADD(day, 9, getdate())),
	(GETDATE(),DATEADD(day, 9, getdate())),
	(GETDATE(),DATEADD(day, 9, getdate())),
	(GETDATE(),DATEADD(day, 10, getdate())),
	(GETDATE(),DATEADD(day, 10, getdate()))
