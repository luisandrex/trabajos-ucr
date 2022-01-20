CREATE TABLE [dbo].[Provincia]
(
	Nombre	VARCHAR(30),
	NombrePais	VARCHAR(30) NOT NULL,
	PRIMARY KEY (Nombre),
	FOREIGN KEY (NombrePais) REFERENCES Pais(Nombre)
);
