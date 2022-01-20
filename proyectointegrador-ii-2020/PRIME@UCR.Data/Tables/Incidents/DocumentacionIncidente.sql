CREATE TABLE [dbo].[DocumentacionIncidente]
(
	Id			INT IDENTITY(1,1),
	CodigoIncidente VARCHAR(50)     NOT NULL,
	RazonDeRechazo     NVARCHAR(200)    NULL,
	PRIMARY KEY (Id),
	FOREIGN KEY (CodigoIncidente)
        REFERENCES Incidente (Codigo),
)
