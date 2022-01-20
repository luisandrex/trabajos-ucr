CREATE TABLE [dbo].[CitaMedica]
(
	[Id] INT NOT NULL IDENTITY(1,1),
	[EstadoId] INT NOT NULL,
	[CitaId] INT NOT NULL, 
	[Codigo] VARCHAR(30) NULL,
	[CedMedicoAsignado] NVARCHAR(12) NULL,
	[ExpedienteId] INT NOT NULL,
	[CentroMedicoId] INT NOT NULL,
	PRIMARY KEY(Id),
	FOREIGN KEY(CitaId) REFERENCES Cita(Id),
	FOREIGN KEY(CedMedicoAsignado) REFERENCES Médico(Cédula), 
	FOREIGN KEY(ExpedienteId) REFERENCES Expediente(Id),
	FOREIGN KEY(EstadoId) REFERENCES EstadoCitaMedica(Id),
	FOREIGN KEY(CentroMedicoId) REFERENCES Centro_Medico(Id)
)
