CREATE TABLE [dbo].[EstadoIncidente]
(
    CodigoIncidente VARCHAR(50)     NOT NULL,
    NombreEstado    VARCHAR(50)     NOT NULL,
    FechaHora       DATETIME        NOT NULL,
    Activo          BIT             NOT NULL,
    AprobadoPor     NVARCHAR(12)    NULL,    
    FOREIGN KEY (NombreEstado)
        REFERENCES Estado (Nombre),
    FOREIGN KEY (CodigoIncidente)
        REFERENCES Incidente (Codigo),
    FOREIGN KEY (AprobadoPor)
        REFERENCES CoordinadorTécnicoMédico (Cédula),
    PRIMARY KEY (CodigoIncidente, NombreEstado)
);
