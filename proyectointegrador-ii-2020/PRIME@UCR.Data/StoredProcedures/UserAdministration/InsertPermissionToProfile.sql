CREATE PROCEDURE [dbo].[InsertPermissionToProfile]
	@idPermission int,
	@idProfile nvarchar(60)
AS
	BEGIN
		BEGIN TRY
			INSERT INTO Permite
			VALUES (
				@idProfile,
				@idPermission
			);
		END TRY
		BEGIN CATCH
			PRINT('El permiso que intentó insertar al perfil ya había sido insertado.')
		END CATCH
	END
