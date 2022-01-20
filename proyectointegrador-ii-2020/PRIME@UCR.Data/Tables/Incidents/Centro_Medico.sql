CREATE TABLE [dbo].[Centro_Medico]
(
	Id			INT IDENTITY(1,1),
	UbicadoEn	INT NOT NULL,
	Latitud		FLOAT,
	Longitud	FLOAT,
	Nombre		VARCHAR(100),
	PRIMARY KEY (Id),
	FOREIGN KEY (UbicadoEn) REFERENCES Distrito(Id)
);