CREATE INDEX ix_checklist_activas
ON CheckList(Activada)
INCLUDE(Id, Nombre, Tipo, Descripcion,Orden)
WHERE Activada = 1
