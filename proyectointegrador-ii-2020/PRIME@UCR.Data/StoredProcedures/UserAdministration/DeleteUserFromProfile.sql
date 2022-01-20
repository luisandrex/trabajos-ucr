CREATE PROCEDURE [dbo].[DeleteUserFromProfile]
	@idUser nvarchar(450),
	@idProfile nvarchar(60)
AS
	BEGIN
		BEGIN TRY
			DELETE FROM Pertenece
			WHERE Pertenece.IdUsuario = @idUser AND Pertenece.NombrePerfil = @idProfile;
		END TRY
		BEGIN CATCH
			PRINT('El usuario que intentó remover del perfil ya había sido removido.')
		END CATCH
	END