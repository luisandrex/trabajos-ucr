CREATE TABLE [dbo].[Funcionario]
(
	Cédula		nvarchar(12)		NOT NULL,
	primary key (Cédula),
	foreign key (Cédula)
		references	Persona(Cédula)
);