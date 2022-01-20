CREATE TABLE [dbo].[InstanciaItem]
(
	[Id_Item]					INT			NOT NULL,
	[Id_Lista]					INT			NOT NULL,
	[Codigo_Incidente]			VARCHAR(50)	NOT NULL,
	[Id_Item_Padre]				INT			NULL,
	[Id_Lista_Padre]			INT			NULL,
	[Codigo_Incidente_Padre]	VARCHAR(50)	NULL,
	[Completado]				BIT			NOT NULL DEFAULT 0,
	[FechaHoraInicio]			DATETIME	NULL,
	[FechaHoraFin]				DATETIME	NULL,
	PRIMARY KEY(Id_Item, Id_Lista, Codigo_Incidente),
	CONSTRAINT FK_IdItem FOREIGN KEY(Id_Item)
		REFERENCES Item(Id),
	CONSTRAINT FK_InstanciaLista FOREIGN KEY(Id_Lista, Codigo_Incidente)
		REFERENCES InstanceChecklist(PlantillaId, IncidentCod) ON DELETE CASCADE,
	CONSTRAINT FK_RelacionRecursiva FOREIGN KEY(Id_Item_Padre, Id_Lista_Padre, Codigo_Incidente_Padre)
		REFERENCES InstanciaItem(Id_Item, Id_Lista, Codigo_Incidente)
)
