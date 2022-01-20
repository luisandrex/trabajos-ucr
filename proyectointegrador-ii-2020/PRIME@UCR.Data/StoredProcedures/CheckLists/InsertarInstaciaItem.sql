Create PROCEDURE [dbo].[InsertarInstaciaItem]
	 @itemId int, 
	 @plantillaId int, 
	 @incidenteCod varchar(50),
	 @iDItemPadre int
	 AS
	 BEGIN
		BEGIN TRY
			set transaction isolation level serializable;
			BEGIN TRANSACTION T2
			DECLARE @itemIdtemp int 
			DECLARE cursor1 CURSOR FOR
			Select i.Id From Item i Where i.IDLista = @plantillaId and i.IDSuperItem = @itemId Order by [Orden ] ASC 
			OPEN cursor1
			FETCH NEXT FROM cursor1 INTO @itemIdtemp 

			Insert into InstanciaItem (Id_Item,Id_Lista,Codigo_Incidente,Id_Item_Padre,Id_Lista_Padre,Codigo_Incidente_Padre)
			Values (@itemId,@plantillaId,@incidenteCod,@iDItemPadre,@plantillaId,@incidenteCod) 

			WHILE @@FETCH_STATUS = 0    
			 BEGIN
				
				Exec [dbo].[InsertarInstaciaItem] @itemId = @itemIdtemp, @plantillaId = @plantillaId, @incidenteCod = @incidenteCod,@iDItemPadre = @itemId
				FETCH NEXT FROM cursor1 INTO @itemIdtemp
			END
			close cursor1
			DEALLOCATE cursor1 
			COMMIT TRANSACTION T2
		END TRY
		BEGIN CATCH
			ROLLBACK TRANSACTION T2
		END CATCH
	 END 