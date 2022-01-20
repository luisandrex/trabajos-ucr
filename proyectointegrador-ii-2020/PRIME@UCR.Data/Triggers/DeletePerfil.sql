CREATE TRIGGER [BorrarPerfil] ON [dbo].[Pertenece]
AFTER DELETE
AS
BEGIN

	DECLARE  @IdUsuario NVARCHAR(450),
		     @NombrePerfil NVARCHAR(60);
	
	DECLARE ptr CURSOR FOR 
		SELECT d.IdUsuario, d.NombrePerfil 
		FROM deleted as d;

	OPEN ptr

	FETCH NEXT FROM ptr INTO @IdUsuario, @NombrePerfil
	WHILE @@FETCH_STATUS = 0 BEGIN
		 DECLARE @cedula nvarchar(12);
		 SELECT @cedula = p.Cédula
		 FROM Usuario as u JOIN Persona as p on u.CédulaPersona = p.Cédula
		 WHERE u.Id = @IdUsuario
		 
		 BEGIN TRY
 			 IF @NombrePerfil = 'Administrador'
			 BEGIN
				DELETE FROM Administrador WHERE Cédula = @cedula
			 END
			 IF @NombrePerfil = 'Especialista técnico médico'
			 BEGIN
				DELETE FROM EspecialistaTécnicoMédico WHERE Cédula = @cedula
			 END
			 IF @NombrePerfil = 'Médico'
			 BEGIN
				DELETE FROM Médico WHERE Cédula = @cedula
			 END
			 IF @NombrePerfil = 'Gerente médico'
			 BEGIN
				DELETE FROM GerenteMédico WHERE Cédula = @cedula
			 END
			 IF @NombrePerfil = 'Coordinador técnico médico'
			 BEGIN
				DELETE FROM CoordinadorTécnicoMédico WHERE Cédula = @cedula
			 END
			 IF @NombrePerfil = 'Administrador de la central de control'
			 BEGIN
				DELETE FROM AdministradorCentroDeControl WHERE Cédula = @cedula
			 END
		 END TRY
		 BEGIN CATCH
			PRINT('El perfil que se estaba buscando ya fue borrado.')
		 END CATCH
		 FETCH NEXT FROM ptr INTO @IdUsuario, @NombrePerfil
	END
	CLOSE ptr
	DEALLOCATE ptr
END;
	    
	