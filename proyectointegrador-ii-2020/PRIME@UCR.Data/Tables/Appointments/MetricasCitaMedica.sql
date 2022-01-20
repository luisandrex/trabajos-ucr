CREATE TABLE [dbo].[MetricasCitaMedica]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[CitaId] INT NOT NULL,
	[Presion] NVARCHAR(50),
	[Peso] NVARCHAR(50),
	[Altura] NVARCHAR(50),
	PRIMARY KEY (Id),
	FOREIGN KEY(CitaId) REFERENCES Cita(Id)
)
