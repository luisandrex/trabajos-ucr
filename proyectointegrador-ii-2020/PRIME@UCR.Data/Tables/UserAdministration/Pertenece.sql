CREATE TABLE [dbo].[Pertenece]
(
	IdUsuario			nvarchar(450)		NOT NULL,
	NombrePerfil		nvarchar(60)		NOT NULL,
	primary key (IdUsuario,NombrePerfil),
	foreign key (IdUsuario)
		references Usuario(Id),
	foreign key (NombrePerfil)
		references Perfil(NombrePerfil)
);
