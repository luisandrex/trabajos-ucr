CREATE TABLE [dbo].[Unidad_De_Transporte]
(
	Matricula	VARCHAR(30),
	Estado		VARCHAR(30),
	Modalidad	VARCHAR(30) NOT NULL,
	PRIMARY KEY (Matricula),
	FOREIGN KEY (Modalidad) REFERENCES Modalidad(Tipo)
);
