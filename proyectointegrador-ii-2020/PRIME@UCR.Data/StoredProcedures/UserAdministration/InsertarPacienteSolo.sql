CREATE PROCEDURE [dbo].[InsertarPacienteSolo]
	@cedula nvarchar(12)
AS
	INSERT INTO Paciente(Cédula)
	VALUES (@cedula)
