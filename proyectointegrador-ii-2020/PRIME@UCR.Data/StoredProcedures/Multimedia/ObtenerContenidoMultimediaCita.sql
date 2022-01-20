CREATE PROCEDURE [dbo].[ObtenerContenidoMultimediaCita]
	@CitaId INT,
	@NombreAccion VARCHAR(50)
AS
	SELECT mc.Id, mc.Nombre, mc.Archivo, mc.Descripcion, mc.Fecha_Hora, mc.Tipo FROM MultimediaContent mc
	JOIN Accion a on mc.Id = a.MultContId
	WHERE a.CitaId = @CitaId and a.NombreAccion = @NombreAccion;
RETURN