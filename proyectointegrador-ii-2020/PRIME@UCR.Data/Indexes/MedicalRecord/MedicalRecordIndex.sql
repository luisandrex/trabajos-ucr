CREATE INDEX CedulaPaciente_Expediente 
ON Expediente(CedulaPaciente);

CREATE INDEX IdExpediente_Cita 
ON Cita(IdExpediente);