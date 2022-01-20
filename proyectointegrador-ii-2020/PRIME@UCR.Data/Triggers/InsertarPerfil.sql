CREATE TRIGGER [InsertarPerfil] ON [dbo].[Pertenece]
AFTER INSERT
AS
BEGIN

	-- Se escogió este nivel de aislamiento para las transacciones, debido al caso que se intente insertar al mismo funcionario
	-- dos veces concurrentemente.

	SET IMPLICIT_TRANSACTIONS OFF;
	SET TRANSACTION ISOLATION LEVEL
	SERIALIZABLE;

	DECLARE  @IdUsuario NVARCHAR(450),
		     @NombrePerfil NVARCHAR(60);
	
	DECLARE ptr CURSOR FOR 
		SELECT i.IdUsuario, i.NombrePerfil 
		FROM inserted as i;

	OPEN ptr

	FETCH NEXT FROM ptr INTO @IdUsuario, @NombrePerfil
	WHILE @@FETCH_STATUS = 0 BEGIN
		DECLARE @cedula nvarchar(12);
		SELECT @cedula = p.Cédula
		FROM Usuario as u JOIN Persona as p on u.CédulaPersona = p.Cédula
		WHERE u.Id = @IdUsuario
				
		DECLARE @amount INT 
		BEGIN TRANSACTION t1;
		SELECT @amount = COUNT(*)
			FROM Funcionario as f
			WHERE f.Cédula = @cedula

		IF @amount = 0
		BEGIN
			BEGIN TRY
				INSERT INTO Funcionario VALUES (@cedula)				
			END TRY
			BEGIN CATCH
				ROLLBACK TRANSACTION t1;
			END CATCH
		END
		COMMIT TRANSACTION t1;
		BEGIN TRY
			IF @NombrePerfil = 'Administrador'
			BEGIN
				INSERT INTO Administrador VALUES (@cedula)
			END
			IF @NombrePerfil = 'Especialista técnico médico'
			BEGIN
				INSERT INTO EspecialistaTécnicoMédico VALUES (@cedula)
			END
			IF @NombrePerfil = 'Médico'
			BEGIN
				INSERT INTO Médico VALUES (@cedula)
			END
			IF @NombrePerfil = 'Gerente médico'
			BEGIN
				INSERT INTO GerenteMédico VALUES (@cedula)
			END
			IF @NombrePerfil = 'Coordinador técnico médico'
			BEGIN
				INSERT INTO CoordinadorTécnicoMédico VALUES (@cedula)
			END
			IF @NombrePerfil = 'Administrador de la central de control'
			BEGIN
				INSERT INTO AdministradorCentroDeControl VALUES (@cedula)
			END
		END TRY
		BEGIN CATCH
			PRINT('El perfil que está intentando insertar ya fue previamente insertado.')
		END CATCH
		FETCH NEXT FROM ptr INTO @IdUsuario, @NombrePerfil
	END
	CLOSE ptr
	DEALLOCATE ptr

END;
	    
	