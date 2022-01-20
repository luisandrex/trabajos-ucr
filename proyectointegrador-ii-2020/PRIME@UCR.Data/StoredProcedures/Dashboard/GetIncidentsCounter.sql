CREATE PROCEDURE [dbo].[GetIncidentsCounter]
(
	@modality		VARCHAR(30),
	@filter			VARCHAR(30)
)
AS
BEGIN
	-- Se eligió ese nivel de aislamiento de transacciones, ya que queremos tener la cantidad real de incidentes actualizada, por lo que 
	-- si un incidente ya se registró en la base de datos, sea tomado en cuenta para mostrar los datos más reales en el dashboard.

	SET IMPLICIT_TRANSACTIONS OFF;
	SET TRANSACTION ISOLATION LEVEL
	READ COMMITTED;
	
	BEGIN TRANSACTION t1;

	IF @modality = ''
	BEGIN
		IF @filter = 'Día'
		BEGIN
			SELECT COUNT(*)
			FROM Incidente I 
				JOIN Modalidad AS M ON I.Modalidad = M.Tipo
				JOIN Cita AS C ON C.Id = I.CodigoCita
			WHERE DATEDIFF(DAY,GETDATE(), C.FechaHoraEstimada) = 0;
			COMMIT TRANSACTION t1;
			RETURN
		END
		IF @filter = 'Semana'
		BEGIN
			SELECT COUNT(*)
			FROM Incidente I 
				JOIN Modalidad AS M ON I.Modalidad = M.Tipo
				JOIN Cita AS C ON C.Id = I.CodigoCita
			WHERE DATEDIFF(DAY,GETDATE(), C.FechaHoraEstimada) <= 6;
			COMMIT TRANSACTION t1;
			RETURN
		END
		IF @filter = 'Mes'
		BEGIN
			SELECT COUNT(*)
			FROM Incidente I 
				JOIN Modalidad AS M ON I.Modalidad = M.Tipo
				JOIN Cita AS C ON C.Id = I.CodigoCita
			WHERE DATEDIFF(DAY,GETDATE(), C.FechaHoraEstimada) <= 29;
			COMMIT TRANSACTION t1;
			RETURN
		END
		IF @filter = 'Año'
		BEGIN
			SELECT COUNT(*)
			FROM Incidente I 
				JOIN Modalidad AS M ON I.Modalidad = M.Tipo
				JOIN Cita AS C ON C.Id = I.CodigoCita
			WHERE DATEDIFF(DAY,GETDATE(), C.FechaHoraEstimada) <= 364;
			COMMIT TRANSACTION t1;
			RETURN
		END
	END
	ELSE
	BEGIN	
		IF @filter = 'Día'
		BEGIN 
			SELECT COUNT(*)
			FROM Incidente I 
				JOIN Modalidad AS M ON I.Modalidad = M.Tipo
				JOIN Cita AS C ON C.Id = I.CodigoCita
			WHERE DATEDIFF(DAY,GETDATE(), C.FechaHoraEstimada) = 0 AND I.Modalidad = @modality;
			COMMIT TRANSACTION t1;
			RETURN
		END
		IF @filter = 'Semana'
		BEGIN 
			SELECT COUNT(*)
			FROM Incidente I 
				JOIN Modalidad AS M ON I.Modalidad = M.Tipo
				JOIN Cita AS C ON C.Id = I.CodigoCita
			WHERE DATEDIFF(DAY,GETDATE(), C.FechaHoraEstimada) <= 6 AND I.Modalidad = @modality;
			COMMIT TRANSACTION t1;
			RETURN
		END
		IF @filter = 'Mes'
		BEGIN 
			SELECT COUNT(*)
			FROM Incidente I 
				JOIN Modalidad AS M ON I.Modalidad = M.Tipo
				JOIN Cita AS C ON C.Id = I.CodigoCita
			WHERE DATEDIFF(DAY,GETDATE(), C.FechaHoraEstimada) <= 29 AND I.Modalidad = @modality;
			COMMIT TRANSACTION t1;
			RETURN
		END
		IF @filter = 'Año'
		BEGIN 
			SELECT COUNT(*)
			FROM Incidente I 
				JOIN Modalidad AS M ON I.Modalidad = M.Tipo
				JOIN Cita AS C ON C.Id = I.CodigoCita
			WHERE DATEDIFF(DAY,GETDATE(), C.FechaHoraEstimada) <= 364 AND I.Modalidad = @modality;
			COMMIT TRANSACTION t1;
			RETURN
		END
	END
END;