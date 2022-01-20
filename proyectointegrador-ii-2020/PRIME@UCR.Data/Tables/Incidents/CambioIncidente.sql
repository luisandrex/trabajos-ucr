CREATE TABLE [dbo].[CambioIncidente]
(
	CodigoIncidente     VARCHAR(50)     NOT NULL,
    CedFuncionario      NVARCHAR(12)    NOT NULL, 
    FechaHora           DateTime        NOT NULL,
    UltimoCambio        NVARCHAR(20)    NULL,
    FOREIGN KEY (CodigoIncidente)
        REFERENCES Incidente (Codigo),
    FOREIGN KEY (CedFuncionario)
        REFERENCES CoordinadorTécnicoMédico (Cédula),
    PRIMARY KEY (CodigoIncidente, CedFuncionario, FechaHora)
)
