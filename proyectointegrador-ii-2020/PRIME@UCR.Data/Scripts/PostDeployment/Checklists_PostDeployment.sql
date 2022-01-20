DELETE FROM CheckList
DELETE FROM Item
DELETE FROM InstanceChecklist
DELETE FROM InstanciaItem
DELETE FROM TipoListaChequeo

DBCC CHECKIDENT ('CheckList', RESEED, 0)
DBCC CHECKIDENT ('Item', RESEED, 0)

insert into TipoListaChequeo(Nombre)
Values('Colocación equipo');

insert into TipoListaChequeo(Nombre)
Values('Retiro equipo');

insert into TipoListaChequeo(Nombre)
Values('Paciente en origen');

insert into TipoListaChequeo(Nombre)
Values('Paciente en destino');

insert into TipoListaChequeo(Nombre)
Values('Paciente en traslado');

insert into CheckList(Nombre, Tipo, Descripcion, Orden)
Values('Salida de Paciente de la Unidad de Internamiento', 'Paciente en origen', 'Se debe realizar al pie de la cama del usuario, previa salida Unidad de Hospitalización', 1);

insert into Item(Nombre, Orden, IDLista)
Values('Asegurar TET, corrugado o interfase de VMNI', 2, 1);

insert into Item(Nombre, Orden, IDLista)
Values('Según dispositivo de oxígeno suplementario (Nasocanula, Venturi, Reservorio o Cánula de alto flujo), valorar la necesidad de colocación de mascarilla quirúrgica', 1, 1);

insert into Item(Nombre, Orden, IDLista)
Values('Asegurar y probar permeabilidad de accesos vasculares', 3, 1);

insert into Item(Nombre, Orden, IDLista)
Values('Colocar electrodos y realizar monitoreos', 4, 1);

insert into Item(Nombre, Orden, IDLista)
Values('Asegurar sondas y otras invasiones', 5, 1);

insert into Item(Nombre, Orden, IDLista)
Values('Frenar cama y camilla de transporte', 6, 1);

insert into Item(Nombre, Orden, IDLista)
Values('Toma de signos vitales antes de mover a la camilla de transporte', 7, 1);

insert into Item(Nombre, Orden, IDLista)
Values('Movilizar en bloque, de forma sincrónica con equipo de apoyo de la unidad', 8, 1);

insert into Item(Nombre, Orden, IDLista)
Values('Colocar dispositivos de sujeción de la camilla de transporte al paciente', 9, 1);

insert into Item(Nombre, Orden, IDLista)
Values('Asegurar bombas de infusión en caso necesario', 10, 1);

insert into Item(Nombre, Orden, IDLista)
Values('Comunicar al centro regulador y al receptor, el tiempo estimado de llegada', 11, 1);


insert into CheckList(Nombre, Tipo, Orden)
Values('BataEquipo y Protección Personal. Con Bata','Colocación equipo' , 2);

insert into Item(Nombre, Orden, IDLista)
Values('Higiene de las manos al menos por 60 segundos con agua jabón o solución alcohólica', 1, 2);

insert into Item(Nombre, Orden, IDLista)
Values('Botas descartables', 2, 2);

insert into Item(Nombre, Orden, IDLista)
Values('Par de guantes (interiores)', 3, 2);

insert into Item(Nombre, Orden, IDLista)
Values('Bata', 4, 2);

insert into Item(Nombre, Orden, IDLista)
Values('Segundo par de guantes (exteriores), extiéndalos de manera que cubran las mangas.', 5, 2);

insert into Item(Nombre, Orden, IDLista)
Values('Respirador N 95 (FFP2)', 6, 2);

insert into Item(Nombre, Orden, IDLista)
Values('Prueba de ajuste', 7, 2);

insert into Item(Nombre, Orden, IDLista)
Values('Lentes de seguridad', 8, 2);

insert into Item(Nombre, Orden, IDLista)
Values('Verificar que la colocación de los lentes no alteró el sello facial del respirador. Repetir prueba de ajuste', 9, 2);

insert into Item(Nombre, Orden, IDLista)
Values('Gorro', 10, 2);

insert into Item(Nombre, Orden, IDLista)
Values('Carreta', 11, 2);

insert into Item(Nombre, Orden, IDLista)
Values('Tercer par de guantes (exteriores), Extiéndalos de manera que cubran las mangas.', 12, 2);


insert into CheckList(Nombre, Tipo, Orden)
Values('Secuencia rápida de intubación', 'Paciente en traslado', 3);

insert into Item(Nombre, Orden, IDLista)		--24
Values('Preparacion', 1, 3);

insert into Item(Nombre, Descripcion, Orden, IDSuperItem, IDLista)		--25
Values('Colocación de EPP', 'Verificar lista de chequeo ', 1, 24, 3);

insert into Item(Nombre, Orden, IDSuperItem, IDLista)
Values('Colocación de EPP', 1, 25, 3);

insert into Item(Nombre, Descripcion, Orden, IDSuperItem, IDLista)
Values('Dispositivo de barrera', 'Covid Cubo/Covid Plástico', 2, 25, 3);
-------------------------------------------------
insert into Item(Nombre, Descripcion, Orden, IDSuperItem, IDLista)		--28
Values('Dispositivo De Barrera', 'Covid Cubo/Covid Plástico', 2, 24, 3);

insert into Item(Nombre, Orden, IDSuperItem, IDLista)
Values('Preoxigenación: Fuente de oxígeno/ NC + Reservorio (APOX) / CAF / VMNI / Bolsa Mascarilla Autoinflable (AMBU) + Válvula PEEP', 3, 24, 3);

insert into Item(Nombre, Descripcion, Orden, IDSuperItem, IDLista)
Values('Acceso Vascular', '2 Accesos Periféricos Permeables / Acceso Intraóseo', 4, 24, 3);

insert into Item(Nombre, Descripcion, Orden, IDSuperItem, IDLista)
Values('Soluciones y conexión', 'Funcionando', 5, 24, 3);

insert into Item(Nombre, Descripcion, Orden, IDSuperItem, IDLista)
Values('Aspiración', 'Funcionando', 6, 24, 3);

insert into Item(Nombre, Descripcion, Orden, IDSuperItem, IDLista)
Values('Monitor', 'Encendido y Funcionando ', 7, 24, 3);

insert into Item(Nombre, Descripcion, Orden, IDSuperItem, IDLista)
Values('Ventilador', 'Calibrado, Configurado y Listo- Filtro Colocado  ', 8, 24, 3);
----------------------------------------------------------
insert into Item(Nombre, Orden, IDLista)	--35
Values('Drogas', 2, 3);

insert into Item(Nombre, Descripcion, Orden, IDSuperItem, IDLista)
Values('Premedicación', '-Atropina 0.5-1MG IV Previo a Inducción/-Lidocaína 1.5MG/KG IV Previo a Inducción / -Fentanil 1UG/KG IV ', 1, 35, 3);

insert into Item(Nombre, Descripcion, Orden, IDSuperItem, IDLista)
Values('Inductores', '-Midazolam 0.1MG/KG IV/ -Ketamina 1-2MG / KG IV/ -Propofol 1MG/ KG IV ', 2, 35, 3);

insert into Item(Nombre, Descripcion, Orden, IDSuperItem, IDLista)
Values('Paralisis', 'Succinilcolina 1.5 MG/ KG IV/ -Atracurio 1MG/KG IV/ -Rocuronio 1MG / KG IV', 3, 35, 3);

insert into Item(Nombre, Descripcion, Orden, IDSuperItem, IDLista)
Values('Drogas Mantenimiento', '-Midazolam 1 MG/ CC(6 ampollas +72 CC SP)/-Fentanilo 10 UG/ CC (10 ampollas + 80 CC SF)/- Atracurio 24 ampollas Puras', 4, 35, 3);

insert into Item(Nombre, Descripcion, Orden, IDSuperItem, IDLista)
Values('Vasopresores PUSH', ' Adrenalina 1/10000/-Fenilefrina 1 AMP + 100 CC SF', 1, 35, 3);
------------------------------------
insert into Item(Nombre, Orden, IDLista)		--41
Values('Vía Aérea', 3, 3);

insert into Item(Nombre, Descripcion, Orden, IDSuperItem, IDLista)
Values('Dispositivos básicos', 'Cánula Nasofaríngea - Cánula Orofaringea -', 1, 41, 3);

insert into Item(Nombre, Descripcion, Orden, IDSuperItem, IDLista)
Values('Video Laringoscopio', 'Funcionando ', 2, 41, 3);

insert into Item(Nombre, Descripcion, Orden, IDSuperItem, IDLista)
Values('Insumos', 'Bougie Guia /- TET-Probados-Repuesto/-Jeringa de 10CC/-Filtro/-Fijación de tubo ', 3, 41, 3);

insert into Item(Nombre, Orden, IDSuperItem, IDLista)
Values('Dispositivos Supraglóticos', 4, 41, 3);

insert into Item(Nombre, Orden, IDSuperItem, IDLista)
Values('Vía Aérea Quirúrgica', 5, 41, 3);
--------------------------------------
insert into Item(Nombre, Orden, IDLista)		--47
Values('Procedimiento', 4, 3);

insert into Item(Nombre, Orden, IDSuperItem, IDLista)
Values('Visualizar/Valorar Monitor', 1, 47, 3);

insert into Item(Nombre, Orden, IDSuperItem, IDLista)
Values('Administración de fármacos', 2, 47, 3);

insert into Item(Nombre, Orden, IDSuperItem, IDLista)
Values('Inmovilización en línea', 3, 47, 3);

insert into Item(Nombre, Orden, IDSuperItem, IDLista)
Values('Colocar TET/ insuflar balón Neumotaponador ', 4, 47, 3);
----------------------------------------------
insert into Item(Nombre, Orden, IDLista)		--52
Values('Manejo post-intubación', 5, 3);

insert into Item(Nombre, Orden, IDSuperItem, IDLista)
Values('Fijación TET', 1, 52, 3);

insert into Item(Nombre, Orden, IDSuperItem, IDLista)
Values('Monitorización ', 2, 52, 3);

insert into Item(Nombre, Orden, IDSuperItem, IDLista)
Values('Capnometría/Grafía: Comprobación Lista de Chequeo ', 3, 52, 3);

insert into Item(Nombre, Orden, IDSuperItem, IDLista)
Values('Retiro de EPP', 4, 52, 3);

declare @code varchar(50);
Select @code = RIGHT(REPLICATE('0', 4) + CAST(YEAR(GETDATE()) AS varchar(10)), 4) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(MONTH(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(DAY(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 4) + '3', 4) + 
	'-' +
	'IT' +
	'-' +
	'AER';

EXEC InsertarListaIntanciada @plantillaId = 3 , @incidenteCod = @code;