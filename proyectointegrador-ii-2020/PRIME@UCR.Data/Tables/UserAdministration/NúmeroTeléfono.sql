CREATE TABLE [dbo].[NúmeroTeléfono]
(
	CédulaPersona		nvarchar(12)		NOT NULL,
	Número				char(8)				NOT NULL,
	primary key (CédulaPersona, Número),
	foreign key (CédulaPersona)
		references Persona(Cédula)
);