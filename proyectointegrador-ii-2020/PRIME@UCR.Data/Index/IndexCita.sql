/*
Atenienses++ Index - Homework 3 
Post-Deployment Script - Index for Cita						
--------------------------------------------------------------------------------------			
--------------------------------------------------------------------------------------
*/

CREATE INDEX ix_fecha_cita
ON Cita(FechaHoraEstimada,FechaHoraCreacion)
WHERE 
FechaHoraEstimada IS NOT NULL 
AND FechaHoraCreacion IS NOT NULL
