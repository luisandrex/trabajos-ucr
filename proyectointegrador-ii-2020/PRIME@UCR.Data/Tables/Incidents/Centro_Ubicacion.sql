CREATE TABLE [dbo].[Centro_Ubicacion]
(
	Id					INT,
	IdCentro			INT					NOT NULL,
	NumeroCama			INT,
	CédulaMédico		nvarchar(12)		NOT NULL,
	PRIMARY KEY (Id),
	FOREIGN KEY (Id) REFERENCES Ubicacion(Id),
	FOREIGN KEY (IdCentro) REFERENCES Centro_Medico(Id),
	FOREIGN KEY (CédulaMédico) REFERENCES Médico(Cédula)

);
