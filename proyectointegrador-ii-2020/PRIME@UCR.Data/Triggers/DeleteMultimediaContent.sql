go
create trigger [BorrarContenidoMultimedia]
on [dbo].[MultimediaContent] instead of delete
as 
begin
begin transaction borrarMCTransaccion
set transaction isolation level serializable
declare @id int
select @id = d.Id
from deleted d
delete from Accion where MultContId = @id
delete from MultimediaContentItem where Id_MultCont = @id
delete from MultimediaContent where Id = @id
commit
end;