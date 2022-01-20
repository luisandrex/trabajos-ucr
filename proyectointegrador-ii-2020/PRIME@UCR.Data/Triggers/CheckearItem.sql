CREATE TRIGGER [CheckearItem] ON [dbo].[InstanciaItem]
AFTER UPDATE AS
BEGIN
set nocount on
	IF UPDATE(Completado) BEGIN TRY
		set transaction isolation level repeatable read;
		BEGIN TRANSACTION TCheckearitem
					-- Datos del item chequeado
			DECLARE @Id_Item INT,
					@Completado BIT,
					@FechaHoraInicio DATETIME,
					@Id_Lista INT,
					@Codigo_Incidente VARCHAR(50),

					-- Datos del padre del item chequeado
					@Id_Item_Padre INT,
					@Id_Lista_Padre INT,
					@Codigo_Incidente_Padre VARCHAR(50),
					@Completado_Padre BIT,
				
					@FechaHoraInicio_Padre DATETIME,

					-- Datos de los items en el mismo nivel
					@Completados_Nivel BIT,
					@Completados_Nivel_MAX BIT;
		
			DECLARE ptr_actualizados CURSOR FOR 
				SELECT Id_Item, Id_Lista, Codigo_Incidente, Id_Item_Padre, Id_Lista_Padre, Codigo_Incidente_Padre, Completado, FechaHoraInicio
				FROM INSERTED;
			OPEN ptr_actualizados

			FETCH NEXT FROM ptr_actualizados INTO @Id_Item, @Id_Lista, @Codigo_Incidente, @Id_Item_Padre, @Id_Lista_Padre, @Codigo_Incidente_Padre, @Completado, @FechaHoraInicio
			WHILE @@FETCH_STATUS = 0 BEGIN

				DECLARE ptr_nivel CURSOR FOR
					WITH instanciasSuperiores (Id_Item, Id_Lista, Codigo_Incidente, Id_Item_Padre, Id_Lista_Padre, Codigo_Incidente_Padre)
					AS (
						SELECT Id_Item, Id_Lista, Codigo_Incidente, Id_Item_Padre, Id_Lista_Padre, Codigo_Incidente_Padre
						FROM InstanciaItem
						WHERE Id_Item = @Id_Item AND Codigo_Incidente = @Codigo_Incidente AND Id_Lista = @Id_Lista
					
						UNION ALL
						SELECT iAn.Id_Item, iAn.Id_Lista, iAn.Codigo_Incidente, iAn.Id_Item_Padre, iAn.Id_Lista_Padre, iAn.Codigo_Incidente_Padre
						FROM InstanciaItem iAn INNER JOIN instanciasSuperiores iDes ON iAn.Id_Item = iDes.Id_Item_Padre AND iAn.Codigo_Incidente = iDes.Codigo_Incidente_Padre AND iAn.Id_Lista = iDes.Id_Lista_Padre
					) --Toma todas las tuplas antecesoras de este item (incluye a este item)
					SELECT Id_Item, Id_Lista, Codigo_Incidente, Id_Item_Padre, Id_Lista_Padre, Codigo_Incidente_Padre
					FROM instanciasSuperiores;

				--no tomar completado ni fechahorafin de la tabla temporal que no se actualiza
				OPEN ptr_nivel

				FETCH NEXT FROM ptr_nivel INTO @Id_Item, @Id_Lista, @Codigo_Incidente, @Id_Item_Padre, @Id_Lista_Padre, @Codigo_Incidente_Padre
				WHILE @@FETCH_STATUS = 0 BEGIN
			
					SELECT @Completado = Completado, @FechaHoraInicio = FechaHoraInicio
					FROM InstanciaItem
					WHERE @Id_Item = Id_Item AND @Id_Lista = Id_Lista AND @Codigo_Incidente = Codigo_Incidente

					-- Si tiene padre
					IF @Id_Item_Padre IS NOT NULL BEGIN
						--  busca entre los items que tienen al mismo padre ('hermanos')
						SELECT @Completados_Nivel = MIN(CASE(Completado) WHEN 1 THEN 1 ELSE 0 END), @Completados_Nivel_MAX = MAX(CASE(Completado) WHEN 1 THEN 1 ELSE 0 END)
						FROM InstanciaItem
						WHERE Id_Lista = @Id_Lista AND Codigo_Incidente = @Codigo_Incidente AND Id_Item_Padre = @Id_Item_Padre

						SELECT @Completado_Padre = Completado, @FechaHoraInicio_Padre = FechaHoraInicio
						FROM InstanciaItem
						WHERE @Id_Item_Padre = Id_Item AND @Id_Lista_Padre = Id_Lista AND @Codigo_Incidente_Padre = Codigo_Incidente
					
						
						IF (@Completados_Nivel_MAX = 0) BEGIN
							UPDATE InstanciaItem
							SET FechaHoraInicio = NULL
							WHERE @Id_Item_Padre = Id_Item AND @Id_Lista_Padre = Id_Lista AND @Codigo_Incidente_Padre = Codigo_Incidente
						END
						ELSE IF (@Completados_Nivel <> @Completado_Padre) BEGIN
							IF (@Completado = 0) BEGIN
								SET @FechaHoraInicio = NULL
							END
							UPDATE InstanciaItem
							SET Completado = @Completado, FechaHoraFin = @FechaHoraInicio
							WHERE @Id_Item_Padre = Id_Item AND @Id_Lista_Padre = Id_Lista AND @Codigo_Incidente_Padre = Codigo_Incidente
						END
						ELSE IF (@Completado = 1 AND @FechaHoraInicio_Padre IS NULL) BEGIN
							UPDATE InstanciaItem
							SET Completado = 0, FechaHoraInicio = @FechaHoraInicio
							WHERE @Id_Item_Padre = Id_Item AND @Id_Lista_Padre = Id_Lista AND @Codigo_Incidente_Padre = Codigo_Incidente
						END
					END --Nivel superior
				
					-- Si no tiene padre, revisa si se debe actualizar la lista
					ELSE BEGIN
						--  busca entre los items que no tienen padre (nivel 1 del arbol)
						SELECT @Completados_Nivel = MIN(CASE(Completado) WHEN 1 THEN 1 ELSE 0 END), @Completados_Nivel_MAX = MAX(CASE(Completado) WHEN 1 THEN 1 ELSE 0 END)
						FROM InstanciaItem
						WHERE Id_Lista = @Id_Lista AND Codigo_Incidente = @Codigo_Incidente AND Id_Item_Padre IS NULL
					
						SELECT @Completado_Padre = Completado, @FechaHoraInicio_Padre = FechaHoraInicio
						FROM InstanceChecklist
						WHERE @Id_Lista = PlantillaId AND @Codigo_Incidente = IncidentCod

						
						IF (@Completados_Nivel_MAX = 0) BEGIN
							UPDATE InstanceChecklist
							SET FechaHoraInicio = NULL
							WHERE @Id_Lista = PlantillaId AND @Codigo_Incidente = IncidentCod
						END
						ELSE IF (@Completados_Nivel <> @Completado_Padre) BEGIN
							IF (@Completado = 0) BEGIN
								SET @FechaHoraInicio = NULL
							END
							UPDATE InstanceChecklist
							SET Completado = @Completado, FechaHoraFinal = @FechaHoraInicio
							WHERE @Id_Lista = PlantillaId AND @Codigo_Incidente = IncidentCod
						END
						ELSE IF (@Completado = 1 AND @FechaHoraInicio_Padre IS NULL) BEGIN
							UPDATE InstanceChecklist
							SET Completado = 0, FechaHoraInicio = @FechaHoraInicio
							WHERE @Id_Lista = PlantillaId AND @Codigo_Incidente = IncidentCod
						END
					END --Instancia Lista de chequeo
				
					FETCH NEXT FROM ptr_nivel INTO @Id_Item, @Id_Lista, @Codigo_Incidente, @Id_Item_Padre, @Id_Lista_Padre, @Codigo_Incidente_Padre
				END --WHILE cursor ptr_nivel
				CLOSE ptr_nivel
				DEALLOCATE ptr_nivel

				FETCH NEXT FROM ptr_actualizados INTO @Id_Item, @Id_Lista, @Codigo_Incidente, @Id_Item_Padre, @Id_Lista_Padre, @Codigo_Incidente_Padre, @Completado, @FechaHoraInicio
			END --WHILE cursor ptr_actualizados
			CLOSE ptr_actualizados
			DEALLOCATE ptr_actualizados
			
		COMMIT TRANSACTION TCheckearitem
	END TRY  --IF UPDATE(Completado)
	BEGIN CATCH
		rollback Transaction TCheckearitem;
		THROW;
	END CATCH
END;