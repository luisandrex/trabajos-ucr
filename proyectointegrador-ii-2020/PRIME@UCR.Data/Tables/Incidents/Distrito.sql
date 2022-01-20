CREATE TABLE [dbo].[Distrito]
(
	Id				INT IDENTITY(1,1),
	IdCanton		INT NOT NULL,
	Nombre			VARCHAR(30),
	PRIMARY KEY (Id),
	FOREIGN KEY (IdCanton) REFERENCES Canton(Id)
);
