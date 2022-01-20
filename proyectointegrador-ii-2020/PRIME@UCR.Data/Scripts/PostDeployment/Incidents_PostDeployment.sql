DELETE FROM AsignadoA
DELETE FROM EstadoIncidente
DELETE FROM Estado
DELETE FROM DocumentacionIncidente
DELETE FROM Incidente
DELETE FROM Unidad_De_Transporte
DELETE FROM Modalidad
DELETE FROM Centro_Medico
DELETE FROM Internacional
DELETE FROM Domicilio
DELETE FROM Ubicacion
DELETE FROM Distrito
DELETE FROM Canton
DELETE FROM Provincia
DELETE FROM Pais

DBCC CHECKIDENT ('Canton', RESEED, 0)
DBCC CHECKIDENT ('Centro_Medico', RESEED, 0)
DBCC CHECKIDENT ('Distrito', RESEED, 0)
DBCC CHECKIDENT ('Ubicacion', RESEED, 0)
DBCC CHECKIDENT ('DocumentacionIncidente', RESEED, 0)
-- Pais
INSERT INTO Pais (Nombre)
VALUES
    ('Costa Rica'),
    ('Panamá'),
    ('Nicaragua'),
    ('Guatemala'),
    ('Honduras'),
    ('El Salvador');

-- Provincia
Insert Into Provincia (Nombre, NombrePais)
Values
    ('San José', 'Costa Rica'),
    ('Heredia', 'Costa Rica'),
    ('Cartago', 'Costa Rica'),
    ('Limón', 'Costa Rica'),
    ('Guanacaste', 'Costa Rica'),
    ('Puntarenas', 'Costa Rica'),
    ('Alajuela', 'Costa Rica');


-- Canton
INSERT INTO Canton (NombreProvincia, Nombre)
VALUES
    ('San José','San José'),
    ('San José','Escazú'),
    ('San José','Desamparados'),
    ('San José','Puriscal'),
    ('San José','Terrazú'),
    ('San José','Aserrí'),
    ('San José','Mora'),
    ('San José','Goicoechea'),
    ('San José','Santa Ana'),
    ('San José','Alajuelita'),
    ('San José','Vázquez de Coronado'),
    ('San José','Acosta'),
    ('San José','Tibás'),
    ('San José','Moravia'),
    ('San José','Montes de Oca'),
    ('San José','Turrubares'),
    ('San José','Dota'),
    ('San José','Curridabat'),
    ('San José','Pérez Zeledón'),
    ('San José','León Cortés Castro'),

    ('Alajuela','Alajuela'),
    ('Alajuela','San Ramón'),
    ('Alajuela','Grecia'),
    ('Alajuela','San Mateo'),
    ('Alajuela','Atenas'),
    ('Alajuela','Naranjo'),
    ('Alajuela','Palmares'),
    ('Alajuela','Poas'),
    ('Alajuela','Orotina'),
    ('Alajuela','San Carlos'),
    ('Alajuela','Zarcero'),
    ('Alajuela','Sarchí'),
    ('Alajuela','Upala'),
    ('Alajuela','Los Chiles'),
    ('Alajuela','Guatuso'),
    ('Alajuela','Río Cuarto'),

    ('Cartago','Cartago'),
    ('Cartago','Paraiso'),
    ('Cartago','La Unión'),
    ('Cartago','Jiménez'),
    ('Cartago','Turrialba'),
    ('Cartago','Alvarado'),
    ('Cartago','Oreamuno'),
    ('Cartago','El Guarco'),

    ('Heredia','Heredia'),
    ('Heredia','Barva'),
    ('Heredia','Santo Domingo'),
    ('Heredia','Santa Bárbara'),
    ('Heredia','San Rafael'),
    ('Heredia','San Isidro'),
    ('Heredia','Belén'),
    ('Heredia','Flores'),
    ('Heredia','San Pablo'),
    ('Heredia','Sarapiquí'),

    ('Guanacaste','Liberia'),
    ('Guanacaste','Nicoya'),
    ('Guanacaste','Santa Cruz'),
    ('Guanacaste','Bagaces'),
    ('Guanacaste','Carrillo'),
    ('Guanacaste','Cañas'),
    ('Guanacaste','Abangares'),
    ('Guanacaste','Tilarán'),
    ('Guanacaste','Nandayure'),
    ('Guanacaste','La Cruz'),
    ('Guanacaste','Hojancha'),

    ('Limón','Limón'),
    ('Limón','Pococí'),
    ('Limón','Siquirres'),
    ('Limón','Talamanca'),
    ('Limón','Matina'),
    ('Limón','Guácimo'),

    ('Puntarenas','Puntarenas'),
    ('Puntarenas','Esparza'),
    ('Puntarenas','Buenos Aires'),
    ('Puntarenas','Montes de Oro'),
    ('Puntarenas','Osa'),
    ('Puntarenas','Quepos'),
    ('Puntarenas','Golfito'),
    ('Puntarenas','Coto Brus'),
    ('Puntarenas','Parrita'),
    ('Puntarenas','Corredores'),
    ('Puntarenas','Garabito');

-- Distritos
INSERT INTO Distrito (IdCanton, Nombre)
VALUES
    (1, 'San José'),
    (1, 'Pavas'),
    (2, 'Escazú'),
    (3, 'Desamparados'),
    (4, 'Santiago'),
    (5, 'San Marcos'),
    (6, 'Aserrí'),
    (7, 'Ciudad Colon'),
    (8, 'Guadalupe'),
    (9, 'Santa Ana'),
    (10, 'Alajuelita'),
    (11, 'San Isidro'),
    (12, 'San Ignacio'),
    (13, 'San Ignacio'),
    (14, 'San Vicente'),
    (15, 'San Pedro'),
    (16, 'San Pablo'),
    (17, 'Santa María'),
    (18, 'Curridabat'),
    (19, 'San Isidro de El General'),
    (20, 'San Pablo'),

    (21, 'Alajuela'),
    (22, 'San Ramón'),
    (23, 'Grecia'),
    (24, 'San Mateo'),
    (25, 'Atenas'),
    (26, 'Naranjo'),
    (27, 'Palmares'),
    (28, 'San Pedro'),
    (29, 'Orotina'),
    (30, 'Quesada'),
    (31, 'Zarcero'),
    (32, 'Sarchí Norte'),
    (33, 'Upala'),
    (34, 'Los Chiles'),
    (35, 'San Rafael'),
    (36, 'Río Cuarto'),

    (37, 'Cartago'),
    (38, 'Paraíso'),
    (39, 'Tres Ríos'),
    (40, 'Juan Viñas'),
    (41, 'Turrialba'),
    (42, 'Pacayas'),
    (43, 'San Rafael'),
    (44, 'El Tejar'),

    (45, 'Heredia'),
    (46, 'Barva'),
    (47, 'Santo Domingo'),
    (48, ' Santa Bárbara'),
    (49, 'San Rafael'),
    (50, 'San Isidro'),
    (51, 'San Antonio'),
    (52, 'San Joaquín'),
    (53, 'San Pablo'),
    (54, 'Puerto Viejo'),

    (55, 'Liberia'),
    (56, 'Nicoya'),
    (57, 'Santa Cruz'),
    (58, 'Bagaces'),
    (59, 'Filadelfia'),
    (60, 'Cañas'),
    (61, 'Las Juntas'),
    (62, 'Tilarán'),
    (63, 'Carmona'),
    (64, 'La Cruz'),
    (65, 'Hojancha'),

    (66, 'Limón'),
    (67, 'Guápiles'),
    (68, 'Siquirres'),
    (69, 'Bribri'),
    (70, 'Matina'),
    (71, 'Guácimo'),

    (72, 'Puntarenas'),
    (73, 'Esparza'),
    (74, 'Buenos Aires'),
    (75, 'Miramar'),
    (76, 'Osa'),
    (77, 'Quepos'),
    (78, 'Golfito'),
    (79, 'Coto Brus'),
    (80, 'Parrita'),
    (81, 'Ciudad Neily'),
    (82, 'Jacó');


-- Ubicación
INSERT INTO Ubicacion DEFAULT VALUES
INSERT INTO Ubicacion DEFAULT VALUES
INSERT INTO Ubicacion DEFAULT VALUES
INSERT INTO Ubicacion DEFAULT VALUES
INSERT INTO Ubicacion DEFAULT VALUES
INSERT INTO Ubicacion DEFAULT VALUES
INSERT INTO Ubicacion DEFAULT VALUES
INSERT INTO Ubicacion DEFAULT VALUES
INSERT INTO Ubicacion DEFAULT VALUES

-- Domicilio
INSERT INTO Domicilio (Id, Direccion, DistritoId, Latitud, Longitud)
VALUES
    (1, 'Santa Ana 420 metros este de City Place', 9, 205, 200),
    (2, 'Pavas al lado del aeropuerto', 1, 124, 260);

-- Internacional
INSERT INTO Internacional (Id, NombrePais)
VALUES
    (3, 'Costa Rica'),
    (4, 'Panamá'),
    (5, 'Nicaragua');

-- Centro Médicos
INSERT INTO Centro_Medico (UbicadoEn, Latitud, Longitud, Nombre)
VALUES
    (8, 23, 23, 'Centro Nacional de Rehabilitación Humberto Araya Rojas'),
    (8, 12, 34.3,'Hospital México'),
    (2, 69, 42.0, 'Hospital Cima'),
    (3, 48, 23, 'Hospital CEACO');

-- Centro_Ubicacion
INSERT INTO Centro_Ubicacion (Id, IdCentro, NumeroCama, CédulaMédico)
VALUES
    (6, 1, 15, '89012345'),
    (7, 2, 6,  '89012345'),
    (8, 3, 12, '22222222'),
    (9, 4, 25, '22222222');

-- Trabaja_En
INSERT INTO Trabaja_En(CédulaMédico, CentroMedicoId)
VALUES
    ('89012345', 1),
    ('89012345', 2),
    ('22222222', 3),
    ('22222222', 4);

-- Modalidad
INSERT INTO Modalidad (Tipo)
VALUES
    ('Terrestre'),
    ('Marítimo'),
    ('Aéreo');

-- Unidad Transporte
INSERT INTO Unidad_De_Transporte (Matricula, Estado, Modalidad)
VALUES
    ('BPC086', 'Disponible', 'Terrestre'),
    ('FMM420', 'Disponible', 'Terrestre'),
    ('XRG430', 'Disponible', 'Marítimo'),
    ('XRG431', 'Disponible', 'Marítimo'),
    ('JPG777', 'Disponible', 'Aéreo'),
    ('PHP999', 'Disponible', 'Aéreo');

-- Incidente
INSERT INTO Incidente (CedulaAdmin, CodigoCita, IdOrigen, Modalidad)
VALUES
	('11111111', 1, 1, 'Terrestre'),
    ('11111111', 2, 2, 'Aéreo'),
    --Agregados por Atenienses
    ('11111111', 3, 3, 'Aéreo'),
    ('11111111', 4, 4, 'Aéreo'),
    ('11111111', 5, 5, 'Aéreo'),
    ('11111111', 6, 6, 'Aéreo'), 
    ('11111111', 7, 7, 'Aéreo'),
    ('11111111', 8, 8, 'Aéreo'),
    ('11111111', 9, 9, 'Aéreo'),
    ('11111111', 10, null, 'Marítimo'),
    ('11111111', 11, null, 'Marítimo'),
    ('11111111', 12, null, 'Marítimo'),
    ('11111111', 13, null, 'Aéreo'),
    ('11111111', 14, null, 'Marítimo'),
    ('11111111', 15, null, 'Marítimo'),
    ('11111111', 16, null, 'Marítimo'),
    ('11111111', 17, null, 'Terrestre'),
    ('11111111', 18, null, 'Terrestre'),
    ('11111111', 19, null, 'Terrestre'),
	('11111111', 20, null, 'Terrestre'),
    ('11111111', 21, null, 'Marítimo'),
    ('11111111', 22, null, 'Marítimo'),
    ('11111111', 23, null, 'Aéreo'),
    ('11111111', 24, null, 'Terrestre'),
    ('11111111', 25, null, 'Aéreo'),
    ('11111111', 26, null, 'Terrestre'),
    ('11111111', 27, null, 'Terrestre'),
    ('11111111', 28, null, 'Marítimo'),
    ('11111111', 29, null, 'Aéreo'),
    ('11111111', 30, null, 'Terrestre'),
    ('11111111', 31, null, 'Terrestre'),
    ('11111111', 32, null, 'Marítimo'),
    ('11111111', 33, null, 'Terrestre'),
    ('11111111', 34, null, 'Marítimo'),
    ('11111111', 35, null, 'Terrestre'),
    ('11111111', 36, null, 'Aéreo'),
    ('11111111', 37, null, 'Terrestre'),
    ('11111111', 38, null, 'Aéreo'),
    ('11111111', 39, null, 'Aéreo'),
    ('11111111', 40, null, 'Terrestre');


-- Estado
INSERT INTO Estado
VALUES
    ('En proceso de creación'),
    ('Creado'),
    ('Rechazado'),
    ('Aprobado'),
    ('Asignado'),
    ('En preparación'),
    ('En ruta a origen'),
    ('Paciente recolectado en origen'),
    ('En traslado'),
    ('Entregado'),
    ('Reactivación'),
    ('Finalizado')

-- EstadoIncidente
INSERT INTO EstadoIncidente( CodigoIncidente, NombreEstado, FechaHora, Activo, AprobadoPor)
VALUES
(
	RIGHT(REPLICATE('0', 4) + CAST(YEAR(GETDATE()) AS varchar(10)), 4) +
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(MONTH(GETDATE()) AS varchar(10)), 2) +
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(DAY(GETDATE()) AS varchar(10)), 2) +
	'-' +
	RIGHT(REPLICATE('0', 4) + '1', 4) +
	'-' +
	'IT' +
	'-' +
	'TER', 'En proceso de creación', GETDATE(), 1, '67890123'
),
(
	RIGHT(REPLICATE('0', 4) + CAST(YEAR(GETDATE()) AS varchar(10)), 4) +
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(MONTH(GETDATE()) AS varchar(10)), 2) +
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(DAY(GETDATE()) AS varchar(10)), 2) +
	'-' +
	RIGHT(REPLICATE('0', 4) + '2', 4) +
	'-' +
	'IT' +
	'-' +
	'AER', 'En proceso de creación', GETDATE(), 1, '67890123'
),
(
	RIGHT(REPLICATE('0', 4) + CAST(YEAR(GETDATE()) AS varchar(10)), 4) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(MONTH(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(DAY(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 4) + '3', 4) + 
	'-' +
	'IT' +
	'-' +
	'AER', 'En proceso de creación', GETDATE(), 1, '67890123'
),
(
	RIGHT(REPLICATE('0', 4) + CAST(YEAR(GETDATE()) AS varchar(10)), 4) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(MONTH(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(DAY(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 4) + '4', 4) + 
	'-' +
	'IT' +
	'-' +
	'AER', 'En proceso de creación', GETDATE(), 1, '67890123'
),
(
	RIGHT(REPLICATE('0', 4) + CAST(YEAR(GETDATE()) AS varchar(10)), 4) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(MONTH(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(DAY(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 4) + '5', 4) + 
	'-' +
	'IT' +
	'-' +
	'AER', 'En proceso de creación', GETDATE(), 1, '67890123'
),
(
	RIGHT(REPLICATE('0', 4) + CAST(YEAR(GETDATE()) AS varchar(10)), 4) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(MONTH(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(DAY(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 4) + '6', 4) + 
	'-' +
	'IT' +
	'-' +
	'AER', 'En proceso de creación', GETDATE(), 1, '67890123'
),
(
	RIGHT(REPLICATE('0', 4) + CAST(YEAR(GETDATE()) AS varchar(10)), 4) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(MONTH(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(DAY(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 4) + '7', 4) + 
	'-' +
	'IT' +
	'-' +
	'AER', 'En proceso de creación', GETDATE(), 1, '67890123'
),
(
	RIGHT(REPLICATE('0', 4) + CAST(YEAR(GETDATE()) AS varchar(10)), 4) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(MONTH(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(DAY(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 4) + '8', 4) + 
	'-' +
	'IT' +
	'-' +
	'AER', 'En proceso de creación', GETDATE(), 1, '67890123'
),
(
	RIGHT(REPLICATE('0', 4) + CAST(YEAR(GETDATE()) AS varchar(10)), 4) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(MONTH(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(DAY(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 4) + '9', 4) + 
	'-' +
	'IT' +
	'-' +
	'AER', 'En proceso de creación', GETDATE(), 1, '67890123'
),
(
	RIGHT(REPLICATE('0', 4) + CAST(YEAR(GETDATE()) AS varchar(10)), 4) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(MONTH(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(DAY(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 4) + '10', 4) + 
	'-' +
	'IT' +
	'-' +
	'MAR', 'En proceso de creación', GETDATE(), 1, '67890123'
),
(
	RIGHT(REPLICATE('0', 4) + CAST(YEAR(GETDATE()) AS varchar(10)), 4) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(MONTH(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(DAY(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 4) + '11', 4) + 
	'-' +
	'IT' +
	'-' +
	'MAR', 'En proceso de creación', GETDATE(), 1, '67890123'
),
(
	RIGHT(REPLICATE('0', 4) + CAST(YEAR(GETDATE()) AS varchar(10)), 4) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(MONTH(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(DAY(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 4) + '12', 4) + 
	'-' +
	'IT' +
	'-' +
	'MAR', 'En proceso de creación', GETDATE(), 1, '78901234'
),
(
	RIGHT(REPLICATE('0', 4) + CAST(YEAR(GETDATE()) AS varchar(10)), 4) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(MONTH(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(DAY(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 4) + '13', 4) + 
	'-' +
	'IT' +
	'-' +
	'AER', 'En proceso de creación', GETDATE(), 1, '78901234'
),
(
	RIGHT(REPLICATE('0', 4) + CAST(YEAR(GETDATE()) AS varchar(10)), 4) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(MONTH(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(DAY(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 4) + '14', 4) + 
	'-' +
	'IT' +
	'-' +
	'MAR', 'En proceso de creación', GETDATE(), 1, '78901234'
),
(
	RIGHT(REPLICATE('0', 4) + CAST(YEAR(GETDATE()) AS varchar(10)), 4) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(MONTH(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(DAY(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 4) + '15', 4) + 
	'-' +
	'IT' +
	'-' +
	'MAR', 'En proceso de creación', GETDATE(), 1, '78901234'
),
(
	RIGHT(REPLICATE('0', 4) + CAST(YEAR(GETDATE()) AS varchar(10)), 4) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(MONTH(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(DAY(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 4) + '16', 4) + 
	'-' +
	'IT' +
	'-' +
	'MAR', 'En proceso de creación', GETDATE(), 1, '78901234'
),
(
	RIGHT(REPLICATE('0', 4) + CAST(YEAR(GETDATE()) AS varchar(10)), 4) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(MONTH(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(DAY(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 4) + '17', 4) + 
	'-' +
	'IT' +
	'-' +
	'TER', 'En proceso de creación', GETDATE(), 1, '78901234'
),
(
	RIGHT(REPLICATE('0', 4) + CAST(YEAR(GETDATE()) AS varchar(10)), 4) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(MONTH(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(DAY(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 4) + '18', 4) + 
	'-' +
	'IT' +
	'-' +
	'TER', 'En proceso de creación', GETDATE(), 1, '78901234'
),
(
	RIGHT(REPLICATE('0', 4) + CAST(YEAR(GETDATE()) AS varchar(10)), 4) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(MONTH(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(DAY(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 4) + '19', 4) + 
	'-' +
	'IT' +
	'-' +
	'TER', 'En proceso de creación', GETDATE(), 1, '78901234'
),
(
	RIGHT(REPLICATE('0', 4) + CAST(YEAR(GETDATE()) AS varchar(10)), 4) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(MONTH(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(DAY(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 4) + '20', 4) + 
	'-' +
	'IT' +
	'-' +
	'TER', 'En proceso de creación', GETDATE(), 1, '78901234'
),
(
	RIGHT(REPLICATE('0', 4) + CAST(YEAR(GETDATE()) AS varchar(10)), 4) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(MONTH(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(DAY(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 4) + '21', 4) + 
	'-' +
	'IT' +
	'-' +
	'MAR', 'En proceso de creación', GETDATE(), 1, '78901234'
),
(
	RIGHT(REPLICATE('0', 4) + CAST(YEAR(GETDATE()) AS varchar(10)), 4) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(MONTH(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(DAY(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 4) + '22', 4) + 
	'-' +
	'IT' +
	'-' +
	'MAR', 'En proceso de creación', GETDATE(), 1, '78901234'
),
(
	RIGHT(REPLICATE('0', 4) + CAST(YEAR(GETDATE()) AS varchar(10)), 4) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(MONTH(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(DAY(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 4) + '23', 4) + 
	'-' +
	'IT' +
	'-' +
	'AER', 'En proceso de creación', GETDATE(), 1, '78901234'
),
(
	RIGHT(REPLICATE('0', 4) + CAST(YEAR(GETDATE()) AS varchar(10)), 4) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(MONTH(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(DAY(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 4) + '24', 4) + 
	'-' +
	'IT' +
	'-' +
	'TER', 'En proceso de creación', GETDATE(), 1, '78901234'
),
(
	RIGHT(REPLICATE('0', 4) + CAST(YEAR(GETDATE()) AS varchar(10)), 4) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(MONTH(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(DAY(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 4) + '25', 4) + 
	'-' +
	'IT' +
	'-' +
	'AER', 'En proceso de creación', GETDATE(), 1, '78901234'
),
(
	RIGHT(REPLICATE('0', 4) + CAST(YEAR(GETDATE()) AS varchar(10)), 4) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(MONTH(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(DAY(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 4) + '26', 4) + 
	'-' +
	'IT' +
	'-' +
	'TER', 'En proceso de creación', GETDATE(), 1, '78901234'
),
(
	RIGHT(REPLICATE('0', 4) + CAST(YEAR(GETDATE()) AS varchar(10)), 4) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(MONTH(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(DAY(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 4) + '27', 4) + 
	'-' +
	'IT' +
	'-' +
	'TER', 'En proceso de creación', GETDATE(), 1, '78901234'
),
(
	RIGHT(REPLICATE('0', 4) + CAST(YEAR(GETDATE()) AS varchar(10)), 4) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(MONTH(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(DAY(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 4) + '28', 4) + 
	'-' +
	'IT' +
	'-' +
	'MAR', 'En proceso de creación', GETDATE(), 1, '78901234'
),
(
	RIGHT(REPLICATE('0', 4) + CAST(YEAR(GETDATE()) AS varchar(10)), 4) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(MONTH(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(DAY(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 4) + '29', 4) + 
	'-' +
	'IT' +
	'-' +
	'AER', 'En proceso de creación', GETDATE(), 1, '78901234'
),
(
	RIGHT(REPLICATE('0', 4) + CAST(YEAR(GETDATE()) AS varchar(10)), 4) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(MONTH(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(DAY(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 4) + '30', 4) + 
	'-' +
	'IT' +
	'-' +
	'TER', 'En proceso de creación', GETDATE(), 1, '78901234'
),
(
	RIGHT(REPLICATE('0', 4) + CAST(YEAR(GETDATE()) AS varchar(10)), 4) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(MONTH(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(DAY(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 4) + '31', 4) + 
	'-' +
	'IT' +
	'-' +
	'TER', 'En proceso de creación', GETDATE(), 1, '78901234'
),
(
	RIGHT(REPLICATE('0', 4) + CAST(YEAR(GETDATE()) AS varchar(10)), 4) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(MONTH(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(DAY(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 4) + '32', 4) + 
	'-' +
	'IT' +
	'-' +
	'MAR', 'En proceso de creación', GETDATE(), 1, '78901234'
),
(
	RIGHT(REPLICATE('0', 4) + CAST(YEAR(GETDATE()) AS varchar(10)), 4) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(MONTH(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(DAY(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 4) + '33', 4) + 
	'-' +
	'IT' +
	'-' +
	'TER', 'En proceso de creación', GETDATE(), 1, '78901234'
),
(
	RIGHT(REPLICATE('0', 4) + CAST(YEAR(GETDATE()) AS varchar(10)), 4) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(MONTH(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(DAY(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 4) + '34', 4) + 
	'-' +
	'IT' +
	'-' +
	'MAR', 'En proceso de creación', GETDATE(), 1, '78901234'
),
(
	RIGHT(REPLICATE('0', 4) + CAST(YEAR(GETDATE()) AS varchar(10)), 4) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(MONTH(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(DAY(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 4) + '35', 4) + 
	'-' +
	'IT' +
	'-' +
	'TER', 'En proceso de creación', GETDATE(), 1, '78901234'
),
(
	RIGHT(REPLICATE('0', 4) + CAST(YEAR(GETDATE()) AS varchar(10)), 4) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(MONTH(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(DAY(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 4) + '36', 4) + 
	'-' +
	'IT' +
	'-' +
	'AER', 'En proceso de creación', GETDATE(), 1, '78901234'
),
(
	RIGHT(REPLICATE('0', 4) + CAST(YEAR(GETDATE()) AS varchar(10)), 4) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(MONTH(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(DAY(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 4) + '37', 4) + 
	'-' +
	'IT' +
	'-' +
	'TER', 'En proceso de creación', GETDATE(), 1, '78901234'
),
(
	RIGHT(REPLICATE('0', 4) + CAST(YEAR(GETDATE()) AS varchar(10)), 4) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(MONTH(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(DAY(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 4) + '38', 4) + 
	'-' +
	'IT' +
	'-' +
	'AER', 'En proceso de creación', GETDATE(), 1, '78901234'
),
(
	RIGHT(REPLICATE('0', 4) + CAST(YEAR(GETDATE()) AS varchar(10)), 4) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(MONTH(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(DAY(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 4) + '39', 4) + 
	'-' +
	'IT' +
	'-' +
	'AER', 'En proceso de creación', GETDATE(), 1, '78901234'
),
(
	RIGHT(REPLICATE('0', 4) + CAST(YEAR(GETDATE()) AS varchar(10)), 4) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(MONTH(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 2) + CAST(DAY(GETDATE()) AS varchar(10)), 2) + 
	'-' +
	RIGHT(REPLICATE('0', 4) + '40', 4) + 
	'-' +
	'IT' +
	'-' +
	'TER', 'En proceso de creación', GETDATE(), 1, '78901234'
);