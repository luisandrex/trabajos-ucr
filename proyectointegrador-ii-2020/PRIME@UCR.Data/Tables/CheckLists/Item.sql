CREATE TABLE [dbo].[Item]
(
	[Id] INT IDENTITY (1, 1) NOT NULL,
    [Nombre] NVARCHAR(200) NOT NULL, 
    [ImagenDescriptiva] NVARCHAR(MAX) NOT NULL DEFAULT '/images/defaultCheckList.svg', 
    [Descripcion] NVARCHAR(500) NULL, 
    [Orden ] INT NOT NULL, 
    [IDSuperItem] INT NULL, 
    [IDLista] INT NULL,
	PRIMARY KEY CLUSTERED ([Id] ASC),
    Foreign key (IDSuperItem) References Item (Id) 	On Delete No Action, 
    Foreign key (IDLista) References CheckList (Id)	On Delete Cascade
)
