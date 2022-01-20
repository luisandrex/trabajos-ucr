CREATE TABLE [dbo].[Expediente]
(
	[Id] INT NOT NULL IDENTITY(1, 1),
	[CedulaPaciente] NVARCHAR(12) NOT NULL,
	[CedulaMedicoDuenno] NVARCHAR(12) NULL,
	[FechaCreacion] DATETIME NULL,
	[Clinica] NVARCHAR(50) NULL,
	PRIMARY KEY (Id),
	FOREIGN KEY (CedulaPaciente) REFERENCES Paciente(Cédula),
	FOREIGN KEY (CedulaMedicoDuenno) REFERENCES Médico(Cédula)
)
