CREATE TABLE [dbo].[Canton]
(	
	Nombre			VARCHAR(30),
	NombreProvincia	VARCHAR(30) NOT NULL,
	Id				INT IDENTITY(1,1),
	PRIMARY KEY (Id),
	FOREIGN KEY (NombreProvincia) REFERENCES Provincia(Nombre)
);
