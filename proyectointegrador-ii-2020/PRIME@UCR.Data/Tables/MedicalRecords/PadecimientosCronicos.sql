CREATE TABLE [dbo].[PadecimientosCronicos]
(
	[IdListaPadecimiento]	INT			NOT NULL,
	[IdExpediente]			INT			NOT NULL,
	[FechaCreacion]			DATETIME	NOT NULL,
	PRIMARY KEY(IdListaPadecimiento,IdExpediente),
	CONSTRAINT Padecimiento_Expediente_FK FOREIGN KEY (IdExpediente) REFERENCES Expediente(Id),
	CONSTRAINT Padecimiento_ListaAlergia_FK FOREIGN KEY (IdListaPadecimiento) REFERENCES ListaPadecimiento(Id)
)
