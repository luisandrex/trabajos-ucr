CREATE TABLE [dbo].[MultimediaContentItem]
(
	[Id_MultCont] INT NOT NULL,
	[Id_Item] INT NOT NULL,
	[Id_Lista] INT NOT NULL,
	[Codigo_Incidente] VARCHAR(50),
	PRIMARY KEY (Id_MultCont, Id_Item, Id_Lista, Codigo_Incidente),
	FOREIGN KEY (Id_MultCont)
		REFERENCES MultimediaContent(Id),
	FOREIGN KEY (Id_Item, Id_Lista, Codigo_Incidente)
		REFERENCES InstanciaItem(Id_Item, Id_Lista, Codigo_Incidente)
)
