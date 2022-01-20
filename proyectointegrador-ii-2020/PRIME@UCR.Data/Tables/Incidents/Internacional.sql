CREATE TABLE [dbo].[Internacional]
(
	Id			INT,
	NombrePais	VARCHAR(30) NOT NULL,
	PRIMARY KEY (Id),
	FOREIGN KEY (Id) REFERENCES Ubicacion(Id),
	FOREIGN KEY (NombrePais) REFERENCES Pais(Nombre)
);
