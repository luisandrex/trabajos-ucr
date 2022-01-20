CREATE TABLE [dbo].[SeEspecializa]
(
	[CedulaMedico] NVARCHAR(12) NOT NULL, 
	[NombreEspecialidad] NVARCHAR(50) NOT NULL,
	PRIMARY KEY(CedulaMedico, NombreEspecialidad),
	FOREIGN KEY(CedulaMedico) REFERENCES Médico(Cédula),
	FOREIGN KEY(NombreEspecialidad) REFERENCES EspecialidadMedica(Nombre)
)
