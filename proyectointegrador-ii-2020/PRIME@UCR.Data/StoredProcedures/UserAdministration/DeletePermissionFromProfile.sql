CREATE PROCEDURE [dbo].[DeletePermissionFromProfile]
	@idPermission int,
	@idProfile nvarchar(60)
AS
	BEGIN
		
		BEGIN TRY
			DELETE FROM Permite
			WHERE Permite.IdPermiso = @idPermission AND Permite.NombrePerfil = @idProfile;
		END TRY
		BEGIN CATCH
			PRINT('El permiso que intentó remover del perfil ya había sido removido.')
		END CATCH

	END