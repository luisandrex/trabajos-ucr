CREATE TABLE [dbo].[Cita]
(
	[Id] INT IDENTITY(1, 1) NOT NULL,
	[FechaHoraEstimada] DATETIME NOT NULL,
	[FechaHoraCreacion] DATETIME NOT NULL,
	[IdExpediente] INT NULL,
	PRIMARY KEY (Id),
	FOREIGN KEY (IdExpediente) REFERENCES Expediente(Id)
)
