CREATE TABLE [dbo].[ReferenciaCita]
(
	[IdCita1] INT NOT NULL,
	[IdCita2] INT NOT NULL,
	[Especialidad] NVARCHAR(50) NOT NULL,
	PRIMARY KEY(IdCita1, IdCita2),
	FOREIGN KEY (IdCita1) REFERENCES Cita(Id),
	FOREIGN KEY (IdCita2) REFERENCES Cita(Id),
	FOREIGN KEY (Especialidad) REFERENCES EspecialidadMedica(Nombre)
)
