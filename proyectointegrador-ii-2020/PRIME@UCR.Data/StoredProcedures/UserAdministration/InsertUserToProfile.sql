CREATE PROCEDURE [dbo].[InsertUserToProfile]
    @idUsuario nvarchar(450),
    @nombrePerfil nvarchar(60)
AS
    BEGIN
        BEGIN TRY
            INSERT INTO Pertenece
            VALUES (
                @idUsuario,
                @NombrePerfil
            );
		END TRY
		BEGIN CATCH
			PRINT('El usuario que intentó insertar al perfil ya había sido insertado.')
		END CATCH

    END