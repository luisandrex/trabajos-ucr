CREATE TABLE [dbo].[AsignadoA]
(
    Codigo VARCHAR(50) NOT NULL,
    CedulaEspecialista NVARCHAR(12),
    PRIMARY KEY(Codigo, CedulaEspecialista),
    FOREIGN KEY(Codigo)
        REFERENCES Incidente(Codigo),
    FOREIGN KEY(CedulaEspecialista)
        REFERENCES EspecialistaTécnicoMédico(Cédula)
)
