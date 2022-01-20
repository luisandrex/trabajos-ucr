CREATE PROCEDURE [dbo].[InsertarAccion]
	@CitaId INT,
	@NombreAccion VARCHAR(50),
	@MultContId INT
AS
	INSERT INTO Accion(CitaId, NombreAccion, MultContId)
	VALUES(@CitaId, @NombreAccion, @MultContId)