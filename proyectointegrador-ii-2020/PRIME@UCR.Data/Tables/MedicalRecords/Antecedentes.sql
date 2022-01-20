CREATE TABLE [dbo].[Antecedentes]
(
	[IdListaAntecedentes] INT NOT NULL,
	[IdExpediente] INT NOT NULL,
	[FechaCreacion] DATETIME NOT NULL,
	PRIMARY KEY (IdListaAntecedentes, IdExpediente),
	CONSTRAINT Antecedente_Expediente_FK FOREIGN KEY (IdExpediente) REFERENCES Expediente(Id),
	CONSTRAINT Antecedente_ListaAntecedente_FK FOREIGN KEY (IdListaAntecedentes) REFERENCES ListaAntecedentes(Id)
)
