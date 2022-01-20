CREATE TABLE [dbo].[PoseeReceta]
(
	[IdRecetaMedica] INT NOT NULL, 
	[IdCitaMedica] INT NOT NULL,
	[Dosis] NVARCHAR(100) NOT NULL, 
	PRIMARY KEY (IdRecetaMedica, IdCitaMedica),
	FOREIGN KEY (IdRecetaMedica) REFERENCES RecetaMedica(Id),
	FOREIGN KEY (IdCitaMedica) REFERENCES CitaMedica(Id)
)
