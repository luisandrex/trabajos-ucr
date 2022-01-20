CREATE INDEX ix_incidente_activo
ON EstadoIncidente(Activo)
INCLUDE(CodigoIncidente, NombreEstado)
WHERE Activo = 1