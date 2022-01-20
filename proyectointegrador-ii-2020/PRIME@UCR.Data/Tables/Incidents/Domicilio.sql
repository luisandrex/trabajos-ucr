CREATE TABLE [dbo].[Domicilio]
(
	Id          INT,
	Direccion	VARCHAR(150),
	DistritoId	INT NOT NULL,
	Latitud		FLOAT,
	Longitud	FLOAT,
	PRIMARY KEY (Id),
	FOREIGN KEY (Id)
		REFERENCES Ubicacion(Id),
	FOREIGN KEY (DistritoId)
		REFERENCES Distrito(Id)
);
