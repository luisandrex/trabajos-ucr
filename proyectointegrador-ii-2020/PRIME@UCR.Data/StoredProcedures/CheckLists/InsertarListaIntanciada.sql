Create PROCEDURE [dbo].[InsertarListaIntanciada]
	@plantillaId int, 
	@incidenteCod varchar(50)
	AS 
	BEGIN
			BEGIN TRY
				set transaction isolation level serializable;
				BEGIN TRANSACTION T1 
					Insert into InstanceChecklist (PlantillaId,IncidentCod) values (@plantillaId,@incidenteCod)
					
					DECLARE @itemId int 
					DECLARE cursor1 CURSOR FOR
					Select i.Id From Item i Where i.IDLista = @plantillaId and i.IDSuperItem is null Order by [Orden ] ASC 
					OPEN cursor1
					FETCH NEXT FROM cursor1 INTO @itemId 
					WHILE @@FETCH_STATUS = 0     -- mientras el cursor tenga más datos que recuperar
						BEGIN
							--query
							Exec [dbo].[InsertarInstaciaItem] @itemId = @itemId, @plantillaId = @plantillaId, @incidenteCod = @incidenteCod, @iDItemPadre = null
							FETCH NEXT FROM cursor1 INTO @itemId
						END
					close cursor1
					DEALLOCATE cursor1
				COMMIT TRANSACTION T1
			END TRY
			BEGIN CATCH
			 rollback Transaction T1;
			THROW;
			END CATCH
	END