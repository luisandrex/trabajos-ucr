CREATE TABLE [dbo].[MultimediaContent]
(
	[Id] int NOT NULL PRIMARY KEY IDENTITY(1,1),
	[Nombre] varchar(200) NOT NULL,
	[Archivo] varchar(500) NOT NULL,
	[Descripcion] varchar(2000),
	[Fecha_Hora] datetime NOT NULL,
	[Tipo] varchar(100) NOT NULL,
	[Extension] varchar(10) NOT NULL
)
