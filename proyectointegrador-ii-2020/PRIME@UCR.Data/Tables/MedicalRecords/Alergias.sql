CREATE TABLE [dbo].[Alergias]
(
	[IdListaAlergia]	INT				NOT NULL,
	[IdExpediente]		INT				NOT NULL,
	[FechaCreacion]     DATETIME        NOT NULL,
	PRIMARY KEY(IdListaAlergia,IdExpediente),
	CONSTRAINT Alergia_Expediente_FK FOREIGN KEY (IdExpediente) REFERENCES Expediente(Id),
	CONSTRAINT Alergia_ListaAlergia_FK FOREIGN KEY (IdListaAlergia) REFERENCES ListaAlergia(Id)
)
