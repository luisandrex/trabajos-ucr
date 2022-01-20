CREATE TABLE [dbo].[Accion]
(
	[CitaId] INT NOT NULL,
	[NombreAccion] VARCHAR(50) NOT NULL,
	[MultContId] INT NOT NULL,
	[Descripcion] NCHAR(200) NULL, 
    PRIMARY KEY (CitaId, NombreAccion, MultContId),
	FOREIGN KEY (CitaId)
		REFERENCES Cita(Id),
	FOREIGN KEY (NombreAccion)
		REFERENCES TipoAccion(Nombre),
	FOREIGN KEY (MultContId)
		REFERENCES MultimediaContent(Id)
)
