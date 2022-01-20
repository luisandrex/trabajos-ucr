DELETE FROM AsignadoA
DELETE FROM CambioIncidente
DELETE FROM EstadoIncidente
DELETE FROM Estado
DELETE FROM Incidente
DELETE FROM MetricasCitaMedica
DELETE FROM CitaMedica
DELETE FROM Cita
DELETE FROM Expediente
DELETE FROM AdministradorCentroDeControl
DELETE FROM Trabaja_En
DELETE FROM Centro_Ubicacion
DELETE FROM Administrador
DELETE FROM Médico
DELETE FROM EspecialistaTécnicoMédico
DELETE FROM GerenteMédico
DELETE FROM AdministradorCentroDeControl
DELETE FROM CoordinadorTécnicoMédico
DELETE FROM Funcionario
DELETE FROM Paciente
DELETE FROM Pertenece
DELETE FROM Usuario
DELETE FROM AspNetUsers
DELETE FROM Permite
DELETE FROM Perfil
DELETE FROM Permiso
DELETE FROM NúmeroTeléfono
DELETE FROM Persona


INSERT INTO AspNetUsers
VALUES
    ('a6f7aa70-a038-419f-9945-7c77b093d58f','juan.guzman@prime.com','JUAN.GUZMAN@PRIME.COM','juan.guzman@prime.com','JUAN.GUZMAN@PRIME.COM',1,'AQAAAAEAACcQAAAAEKBfjZVSMkEvJ3kJikd/FETuy1hxI3csK3qM2EwHBlQpgixfBX3tUaxpposHbUfakg==','M7SUOG4MXMPBKLX2BN34HVOG7GRGNIDQ','8caf2844-e5ad-452c-b89f-016d71b5d09e',NULL,0,0,NULL,1,0),
    ('e5cb3edd-44c2-45bd-bd03-a35a4bca66fa','pedro.lopez@prime.com','PEDRO.LOPEZ@PRIME.COM','pedro.lopez@prime.com','PEDRO.LOPEZ@PRIME.COM',1,'AQAAAAEAACcQAAAAEE8FiiPlsBjDIWFC8BcdydBip1i4qXfCkrgIblFJLUl8w+6DXj4wgvdXEaa8yFoMew==','AKPWZUTG53SQX6MZO3HPRDYB5F3V7JVU','6c28fda7-48d1-4ea8-a63b-44d26bf09f6e',NULL,0,0,NULL,1,0),
    ('312e5edf-cf52-4dac-a4c9-1565cb73e13e','maria.loria@prime.com','MARIA.LORIA@PRIME.COM','maria.loria@prime.com','MARIA.LORIA@PRIME.COM',1,'AQAAAAEAACcQAAAAEJSZG3DWeI1LEpwB2rAT/vG2nnDHe8fcO55g5UbNdSkOVSScN08l1172YqVGJlJseA==','6BPGZU6X3TD65J6SJ4JMXGYJO5UW57PU','e6c7f00d-e907-4fad-866f-d45fcc496750',NULL,0,0,NULL,1,0),
    ('5c14b733-ae39-42da-aa31-3eccf37ae33f','diego.carmona@prime.com','DIEGO.CARMONA@PRIME.COM','diego.carmona@prime.com','DIEGO.CARMONA@PRIME.COM',1,'AQAAAAEAACcQAAAAELKV77rFE3S1T//lXdgEYOlHX8+yW3Hng36stZupTvU/bX3Gbcq6r7eBlAgLO+m7fw==','DXEUSGTKRDZISW4R2LKGCK377IHGR7NF','5155b9fd-a9d9-4324-b68b-08b31322b959',NULL,0,0,NULL,1,0),
    ('021d330b-5ffa-4bfc-8159-5393ee0c60d9','wilbert.lopez@prime.com','WILBERT.LOPEZ@PRIME.COM','wilbert.lopez@prime.com','WILBERT.LOPEZ@PRIME.COM',1,'AQAAAAEAACcQAAAAECCsm21knu8qOLl+dGlE6tA+Ut9m4CZ/ty5cNTeElQJDXr3JqeG3YMvNz5PzL8eSpg==','RXP7MNASZSLAKXBK36XF2S5NWX7ALBWH','ab435603-9545-4934-b7b6-1eb6d6cac19b',NULL,0,0,NULL,1,0),
    ('07f9b44f-157e-441b-a428-da0b8affed2e','fabiola.mora@prime.com','FABIOLA.MORA@PRIME.COM','fabiola.mora@prime.com','FABIOLA.MORA@PRIME.COM',1,'AQAAAAEAACcQAAAAEE8QhqiNIwplTUZAh5V/VL2KH/mgtqElC6ctlmlmnXAF+9qKpxSzN0WNC+cMPE536Q==','77K2TE54GPP2X5XZYN2D437NST66SI47','4d53a186-b20d-48ee-b382-f2e0b935e54d',NULL,0,0,NULL,1,0),
    ('25dd51e7-abf3-4efd-82b8-0c790433523c','ana.torres@prime.com','ANA.TORRES@PRIME.COM','ana.torres@prime.com','ANA.TORRES@PRIME.COM',1,'AQAAAAEAACcQAAAAEAK7urGc990Wj5FLe+8L5QRdlNYn0qf+b5LdmBYSJ4p3L+kpAgI6I1lMV7+6putYog==','CLXO4EI5VIBL2M3UEYKM66O6TTB6RQY3','6993e788-1079-4ce2-b7a7-6cf6d3a76f60',NULL,0,0,NULL,1,0),
    ('b97aac93-cfaf-4485-81d4-cb12e652ef68','fabian.hernandez@prime.com','FABIAN.HERNANDEZ@PRIME.COM','fabian.hernandez@prime.com','FABIAN.HERNANDEZ@PRIME.COM',1,'AQAAAAEAACcQAAAAELTZe/dQ0H81AzD632vv2DyJ4XnOrzO5uFlLUlS1KJuSXUhRh+6L2KQMe90GBK3KaQ==','XW7YV5A3STAFWJKB6LNHDKMYSTIFKYCM','43d5cb32-4d1a-4e1e-8285-c585d1c3670b',NULL,0,0,NULL,1,0),
    ('8af8648e-2ccc-4261-a69a-1ad92a691399','teodoro.barquero@prime.com','TEODORO.BARQUERO@PRIME.COM','teodoro.barquero@prime.com','TEODORO.BARQUERO@PRIME.COM',1,'AQAAAAEAACcQAAAAEHAmVY3K6Gp6Eck1SnW5ZGUsKZUTCnXumQIl57pnb60T1cOzifua1IxOUgNbynNopw==','V2RDSXVBAISGPR3DTGK7REY2DWTC3RPX','e20ff5fb-b260-4a6c-b82d-a0a59b369c77',NULL,0,0,NULL,1,0),
    ('df025dd6-57c9-4c3e-8ae2-e319080ca07b','shannon.zuniga@prime.com','SHANNON.ZUNIGA@PRIME.COM','shannon.zuniga@prime.com','SHANNON.ZUNIGA@PRIME.COM',1,'AQAAAAEAACcQAAAAEFK8Ee1g/O7ntPAXnnYKvwoNEwT290f/h+q/hBsz6ybcX1pVBrLG22rtZ78mScSV5g==','FYWVZRUHB46QLLFPFJIOTHJKJMYKAIMM','24e2c1eb-578b-4d2f-9fd4-7cf379447f61',NULL,0,0,NULL,1,0),
    ('e8b07151-040d-4b2c-95dd-03314508c40f','jaikel.rivas@prime.com','JAIKEL.RIVAS@PRIME.COM','jaikel.rivas@prime.com','JAIKEL.RIVAS@PRIME.COM',0,'AQAAAAEAACcQAAAAEII1jGldBK6jolZ2bPIvV84xZsAXe/+ODtiiVvbcKDWzd2QMRUUKnxgfkqCcSXI2pg==','IWPN2PPJ5GEAHFYDJVFRRNEM7PISBQXX','e8270c87-935a-4fcb-b096-8a048264b171',NULL,0,0,NULL,1,0),
    ('95b3d7ae-03ff-4b50-af8b-0e1582750640','irene.ruiz@prime.com','IRENE.RUIZ@PRIME.COM','irene.ruiz@prime.com','IRENE.RUIZ@PRIME.COM',1,'AQAAAAEAACcQAAAAEEDTux/AmLwnRZjedeXcuPSKa/LF1rEbGVb1xUTHzMpV2KDK32Mp8LcFoBfqcLFdmg==','OZOMTPSXPOE7ZI2UJXVKLWSRTU6TM6LE','d06ad0e2-973d-4b03-bdb2-50e0092eb97a',NULL,0,0,NULL,1,0);

INSERT INTO Persona (Cédula, Nombre, PrimerApellido, FechaNacimiento)
VALUES  ('12345678', 'Juan', 'Guzman','2020-10-10'),
        ('23456789', 'Pedro', 'Lopez','2020-10-10'),
        ('34567890', 'Maria', 'Loria','2020-10-10'),
        ('45678901', 'Diego', 'Carmona','2020-10-10'),
        ('56789012', 'Ana', 'Torres','2020-10-10'),
        ('67890123', 'Teodoro', 'Barquero','2020-10-10'),
        ('78901234', 'Shannon', 'Zuñiga','2020-10-10'),
        ('89012345', 'Wilbert', 'Lopez','2020-10-10'),
        ('90123456', 'Irene', 'Ruiz','2020-10-10'),
        ('01234567', 'Fabian', 'Hernandez','2020-10-10'),
        ('11111111', 'Jaikel', 'Rivas','2020-10-10'),
        ('22222222', 'Fabiola', 'Mora','2020-10-10');

INSERT INTO Paciente
VALUES  ('12345678'),
        ('23456789'),
        ('34567890'),
        ('45678901'),
        ('56789012'),
        ('67890123'),
        ('78901234'),
        ('89012345'),
        ('90123456');

INSERT INTO Usuario (Id, CédulaPersona)
VALUES
    ('a6f7aa70-a038-419f-9945-7c77b093d58f','12345678'),
    ('e5cb3edd-44c2-45bd-bd03-a35a4bca66fa','23456789'),
    ('312e5edf-cf52-4dac-a4c9-1565cb73e13e','34567890'),
    ('5c14b733-ae39-42da-aa31-3eccf37ae33f','45678901'),
    ('021d330b-5ffa-4bfc-8159-5393ee0c60d9','89012345'),
    ('07f9b44f-157e-441b-a428-da0b8affed2e','22222222'),
    ('25dd51e7-abf3-4efd-82b8-0c790433523c','56789012'),
    ('b97aac93-cfaf-4485-81d4-cb12e652ef68','01234567'),
    ('8af8648e-2ccc-4261-a69a-1ad92a691399','67890123'),
    ('df025dd6-57c9-4c3e-8ae2-e319080ca07b','78901234'),
    ('e8b07151-040d-4b2c-95dd-03314508c40f','11111111'),
    ('95b3d7ae-03ff-4b50-af8b-0e1582750640','90123456');

INSERT INTO Permiso (IdPermiso, Descripción_Permiso)
VALUES
    -- Administrador
    (1,'Puede administrar las cuentas de usuario'),
    (2,'Puede crear listas de chequeo'),
    (3,'Puede instanciar listas de chequeo'),
    -- Especialista técnico médico
    (4,'Puede ver el listado de incidentes'),
    (5,'Puede ver la información médica en el listado de incidentes'),
    (6,'Puede ver los detalles básicos de un incidente'),
    (7,'Puede ver los detalles médicos de un incidente'),
    (8,'Puede ver la información del paciente de un incidente'),
    -- Médico
    (9,'Ver expedientes de sus pacientes'),
    -- Gerente médico
    (10,'Puede ver todos los expedientes médicos'),
    (11,'Puede manejar la información del dashboard referente a expedientes'),
        -- Se repite el 13
    -- Coordinador técnico médico
    (12,'Puede manejar la información del dashboard referente a incidentes'),
        -- Se repite el 3 y 4
    (13,'Puede marcar items de las listas de chequeo'),
    (14,'Puede adjuntar multimedia en las listas de chequeo'),
        -- Se repite el 5, 6, 7, 8
    (15,'Puede editar los detalles básicos de los incidentes'),
    (16,'Puede editar los detalles médicos de los incidentes'),
    (17,'Puede revisar los incidentes'),
        -- Se repite 9
    (18,'Puede editar la información del paciente de un incidente'),
    (19,'Puede llevar a cabo la asignación de incidentes'),
    (20,'Puede crear incidentes'),
-- Se repite el 22
    -- Administrador del centro de control
        -- Se repite 5, 7, 16 y 21
    (21,'Puede administrar multimedia de un incidente'),
    (22,'Puede administrar listas de chequeo de un incidente'),
    (23,'Puede visualizar mapas en tiempo real');

INSERT INTO Perfil (NombrePerfil)
VALUES ('Administrador'),
       ('Especialista técnico médico'),
       ('Médico'),
       ('Gerente médico'),
       ('Coordinador técnico médico'),
       ('Administrador de la central de control');

INSERT INTO Permite (IdPermiso, NombrePerfil)
VALUES  (1,'Administrador'),
        (2,'Administrador'),
        (3,'Administrador'),
        (4,'Especialista técnico médico'),
        (5,'Especialista técnico médico'),
        (6,'Especialista técnico médico'),
        (7,'Especialista técnico médico'),
        (8,'Especialista técnico médico'),
        (13, 'Especialista técnico médico'),
        (21,'Especialista técnico médico'),
        (22,'Especialista técnico médico'),
        (4,'Médico'),
        (6,'Médico'),
        (8,'Médico'),
        (9,'Médico'),
        (10,'Médico'),
        (16,'Médico'),
        (18,'Médico'),
        (19,'Médico'),
        (20,'Médico'),
        (21,'Médico'),
        (10,'Gerente médico'),
        (11,'Gerente médico'),
        (12,'Gerente médico'),
        (16,'Gerente médico'),
        (18,'Gerente médico'),
        (19,'Gerente médico'),
        (20,'Gerente médico'),
        (2,'Coordinador técnico médico'),
        (3,'Coordinador técnico médico'),
        (4,'Coordinador técnico médico'),
        (5,'Coordinador técnico médico'),
        (6,'Coordinador técnico médico'),
        (7,'Coordinador técnico médico'),
        (8,'Coordinador técnico médico'),
        (9,'Coordinador técnico médico'),
        (10,'Coordinador técnico médico'),
        (12,'Coordinador técnico médico'),
        (13,'Coordinador técnico médico'),
        (14,'Coordinador técnico médico'),
        (15,'Coordinador técnico médico'),
        (16,'Coordinador técnico médico'),
        (17,'Coordinador técnico médico'),
        (18,'Coordinador técnico médico'),
        (19,'Coordinador técnico médico'),
        (20,'Coordinador técnico médico'),
        (21,'Coordinador técnico médico'),
        (22,'Coordinador técnico médico'),
        (23,'Coordinador técnico médico'),
        (4,'Administrador de la central de control'),
        (6,'Administrador de la central de control'),
        (15,'Administrador de la central de control'),
        (20,'Administrador de la central de control');

INSERT INTO Pertenece(IdUsuario, NombrePerfil)
VALUES ('a6f7aa70-a038-419f-9945-7c77b093d58f','Administrador'),
       ('e5cb3edd-44c2-45bd-bd03-a35a4bca66fa','Administrador'),
       ('312e5edf-cf52-4dac-a4c9-1565cb73e13e','Especialista técnico médico'),
       ('5c14b733-ae39-42da-aa31-3eccf37ae33f','Especialista técnico médico'),
       ('021d330b-5ffa-4bfc-8159-5393ee0c60d9','Médico'),
       ('07f9b44f-157e-441b-a428-da0b8affed2e','Médico'),
       ('25dd51e7-abf3-4efd-82b8-0c790433523c','Gerente médico'),
       ('b97aac93-cfaf-4485-81d4-cb12e652ef68','Gerente médico'),
       ('8af8648e-2ccc-4261-a69a-1ad92a691399','Coordinador técnico médico'),
       ('df025dd6-57c9-4c3e-8ae2-e319080ca07b','Coordinador técnico médico'),
       ('e8b07151-040d-4b2c-95dd-03314508c40f','Administrador de la central de control'),
       ('95b3d7ae-03ff-4b50-af8b-0e1582750640','Administrador de la central de control');
