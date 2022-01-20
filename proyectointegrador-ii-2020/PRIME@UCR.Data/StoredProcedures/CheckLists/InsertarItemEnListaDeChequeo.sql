CREATE PROCEDURE [dbo].[InsertarItemEnListaDeChequeo]
	@nombre				NVARCHAR(150),
	@imagenDescriptiva	NVARCHAR(MAX),
	@descripcion		NVARCHAR(150),
	@orden				INT,
	@IdSuperItem INT,
	@IdLista INT
AS
	BEGIN
		declare @MyTable TABLE (
			ListId INT
		)
		INSERT INTO Item
		OUTPUT inserted.id INTO @MyTable
		VALUES (
			@nombre,				
			@imagenDescriptiva,	
			@descripcion,		
			@orden,				
			@IdSuperItem, 
			@IdLista 
		)

		SELECT ListId from @MyTable;
	END