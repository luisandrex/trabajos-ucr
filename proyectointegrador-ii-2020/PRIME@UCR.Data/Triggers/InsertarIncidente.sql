CREATE TRIGGER [InsertarIncidente]
	ON [dbo].[Incidente]
	INSTEAD OF INSERT
	AS
	BEGIN
		SET NOCOUNT ON
		DECLARE @Codigo varchar(50);
		DECLARE @MatriculaTrans varchar(30);
		DECLARE @CedulaAdmin nvarchar(12);
		DECLARE @CedulaTecnicoCoordinador nvarchar(12);
		DECLARE @CedulaTecnicoRevisor nvarchar(12);
		DECLARE @CodigoCita int;
		DECLARE @IdOrigen int;
		DECLARE @IdDestino int;
		DECLARE @Modalidad varchar(30);
		DECLARE ptr CURSOR FOR
			SELECT *
			FROM inserted;

		OPEN ptr

		FETCH NEXT FROM ptr INTO
			@Codigo,
			@MatriculaTrans,
			@CedulaAdmin,
			@CedulaTecnicoCoordinador,
			@CedulaTecnicoRevisor,
			@CodigoCita,
			@IdOrigen,
			@IdDestino,
			@Modalidad

		WHILE @@FETCH_STATUS = 0
		BEGIN
			BEGIN TRANSACTION;
            DECLARE @id int;
            DECLARE @next_id int;
            DECLARE @mod char(10);
			DECLARE @fecha datetime;

			SELECT @fecha = FechaHoraCreacion
			FROM Cita
			WHERE Id = @CodigoCita
            
            SELECT TOP 1 @id = MAX(CAST(SUBSTRING(i.Codigo, 12, 4) as int))
            FROM Incidente i
            WHERE YEAR(@fecha) = YEAR(GETDATE());


            IF @id IS NULL
                SET @next_id = 1
            ELSE
                SET @next_id = @id + 1

			SET @mod = UPPER(SUBSTRING(@Modalidad, 1, 3));
			SET @mod = REPLACE(@mod, 'Á', 'A');
			SET @mod = REPLACE(@mod, 'É', 'E');
			SET @mod = REPLACE(@mod, 'Í', 'I');
			SET @mod = REPLACE(@mod, 'Ó', 'O');
			SET @mod = REPLACE(@mod, 'Ú', 'U');

			-- build code
			DECLARE @code varchar(50);
			SET @code =
			RTRIM(
			    
				RIGHT(REPLICATE('0', 4) + CAST(YEAR(@fecha) AS varchar(10)), 4) + 
				'-' +
				RIGHT(REPLICATE('0', 2) + CAST(MONTH(@fecha) AS varchar(10)), 2) + 
				'-' +
				RIGHT(REPLICATE('0', 2) + CAST(DAY(@fecha) AS varchar(10)), 2) + 
				'-' +
				RIGHT(REPLICATE('0', 4) + CAST(@next_id AS varchar(10)), 4) + 
				'-' +
				'IT' +
				'-' +
				@mod
            );

            INSERT INTO Incidente
                OUTPUT INSERTED.Codigo
            VALUES
            (
                @code,
                @MatriculaTrans,
                @CedulaAdmin,
                @CedulaTecnicoCoordinador,
                @CedulaTecnicoRevisor,
                @CodigoCita,
                @IdOrigen,
                @IdDestino,
                @Modalidad
            )

			COMMIT

			FETCH NEXT FROM ptr INTO
				@Codigo,
				@MatriculaTrans,
				@CedulaAdmin,
				@CedulaTecnicoCoordinador,
				@CedulaTecnicoRevisor,
				@CodigoCita,
				@IdOrigen,
				@IdDestino,
				@Modalidad
		END

		CLOSE ptr
		DEALLOCATE ptr
	    
	END
