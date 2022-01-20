CREATE TRIGGER [AfterInsertInstance] ON [dbo].[InstanceChecklist]
AFTER insert AS
BEGIN
	declare @plantillaid int
	select  @plantillaid=i.PlantillaId
	from inserted i
	declare @cant int
	select @cant = COUNT(cl.Id)
	from CheckList cl join InstanceChecklist icl on cl.Id = icl.PlantillaId
	where cl.Id=@plantillaid
	declare @edit bit
	select @edit = cl.Editable
	from CheckList cl
	where cl.Id=@plantillaid
	if  @edit = 1 and @cant > 0-- deshabilitar edición
			begin
				update CheckList
				set Editable = 0
				where Id = @plantillaid
			end 
	else 
	begin
		if  @edit = 0 and  @cant = 0 --habilitar edición
			begin
				update CheckList
				set Editable = 1
				where Id = @plantillaid
			end
	end
END;