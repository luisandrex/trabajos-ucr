﻿CREATE TABLE [dbo].[GerenteMédico]
(
	Cédula		nvarchar(12)		NOT NULL,
	primary key (Cédula),
	foreign key (Cédula)
		references	Funcionario(Cédula)
);