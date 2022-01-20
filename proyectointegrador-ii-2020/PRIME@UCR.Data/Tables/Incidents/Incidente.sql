CREATE TABLE [dbo].[Incidente]
(
    Codigo                   VARCHAR(50)  NOT NULL DEFAULT('TEMP'),
    MatriculaTrans           VARCHAR(30)  NULL,
    CedulaAdmin              NVARCHAR(12) NULL,
    CedulaTecnicoCoordinador NVARCHAR(12) NULL,
    CedulaRevisor            NVARCHAR(12) NULL,
    CodigoCita               INT          NOT NULL UNIQUE,
    IdOrigen                 INT          NULL,
    IdDestino                INT          NULL,
    Modalidad                VARCHAR(30)  NOT NULL,
    PRIMARY KEY (Codigo),
    FOREIGN KEY (CedulaAdmin) REFERENCES Persona(Cédula),
    FOREIGN KEY (CedulaTecnicoCoordinador) REFERENCES CoordinadorTécnicoMédico(Cédula),
    FOREIGN KEY (CedulaRevisor) REFERENCES Persona(Cédula),
    FOREIGN KEY (Modalidad) REFERENCES Modalidad (Tipo),
    FOREIGN KEY (IdDestino) REFERENCES Ubicacion (Id),
    FOREIGN KEY (IdOrigen) REFERENCES Ubicacion (Id),
    FOREIGN KEY (MatriculaTrans) REFERENCES Unidad_De_Transporte (Matricula),
    FOREIGN KEY (CodigoCita) REFERENCES Cita(Id)
)
