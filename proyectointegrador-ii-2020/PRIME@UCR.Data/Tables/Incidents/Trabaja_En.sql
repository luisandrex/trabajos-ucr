CREATE TABLE [dbo].[Trabaja_En]
(
	CédulaMédico		nvarchar(12)		NOT NULL,
	CentroMedicoId		INT					NOT NULL,
	PRIMARY KEY (CentroMedicoId, CédulaMédico),
	FOREIGN KEY (CentroMedicoId) REFERENCES Centro_Medico(Id),
	FOREIGN KEY (CédulaMédico) REFERENCES Médico(Cédula)
)
