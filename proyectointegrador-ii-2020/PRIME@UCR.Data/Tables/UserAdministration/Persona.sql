CREATE TABLE [dbo].[Persona]
(
	Cédula				nvarchar(12)		NOT NULL,
	Nombre				nvarchar(20)		NOT NULL,
	PrimerApellido		nvarchar(20)		NOT NULL,
	SegundoApellido		nvarchar(20),
	Sexo				char(1),
	FechaNacimiento		date,
	primary key(Cédula)
);
