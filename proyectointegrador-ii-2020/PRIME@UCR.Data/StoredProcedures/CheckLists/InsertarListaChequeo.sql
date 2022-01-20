CREATE PROCEDURE [dbo].[InsertarListaChequeo]
	@nombre				NVARCHAR(100),
	@tipo				NVARCHAR(20),
	@descripcion		NVARCHAR(200),
	@orden				INT,
	@imagenDescriptiva	NVARCHAR(MAX),
	@editable bit,
	@activada bit
AS
	BEGIN
		declare @MyTable TABLE (
			ListId INT
		)
		INSERT INTO CheckList
		OUTPUT inserted.id INTO @MyTable
		VALUES (
			@nombre,
			@tipo,
			@descripcion,
			@orden,
			@imagenDescriptiva,
			@editable ,
			@activada
		)

		SELECT ListId from @MyTable;
	END