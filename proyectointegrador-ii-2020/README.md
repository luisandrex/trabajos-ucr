# Universidad de Costa Rica

## README.MD

# PRIME@UCR

#### Version 2.0
  
  
    
## Tabla de contenidos 

[TOC]

## Definiciones, acrónimos y abreviaciones

CEACO: Centro Especializado de Atención de Pacientes con COVID-19 

PRIME: Primera Respuesta Médica Especializada

Visión del producto: proyección al futuro de qué será el producto y por qué se creará.

Mapa de ruta del producto: plan de acción que muestra cómo un producto evolucionará a lo largo del tiempo.

Definición de listo: serie de criterios que toda historia de usuario debe cumplir antes de considerarse completada.

Lista de chequeo: elemento utilizado para poder controlar el cumplimiento satisfactorio de una serie de pasos que se siguen en una actividad, por lo general repetitiva.

Instancia de una lista: es una copia de la lista de chequeo que fue creada con anterioridad y que se le muestra al especialista técnico médico para que este llene

Plantilla: lista de chequeo base no instanciada, disponible para editar según el nivel de autorización

Listas de incidentes: listas de chequeo que están en la categoría de sucesos inesperados y a las que se puede acceder en todo momento del incidente.

Ítem: elemento que forma parte de una lista de chequeo. Es una tarea que se debe seguir según el objetivo de lista de chequeo en cuestión

Sub-ítem: elemento que se desprende un ítem. Es una subtarea que debe seguir el usuario para completar un ítem.

Imagen descriptiva: imagen asociada a un ítem o lista para un mejor reconocimiento.

Lista de chequeo predeterminada: son listas que se aplican a todos los incidentes existentes.

Check: acción de marcar un ítem dentro de una lista de chequeo instanciada como completado.

## Introducción

La función de este documento es explicar y detallar las tecnologías, diseños y funcionalidades de la aplicación PRIME@UCR, que consiste en una aplicación que permite una mejor administración de los transportes de pacientes a CEACO, Centro Especializado de Atención de Pacientes con COVID-19. Este documento tiene como objetivo dar un mejor entendimiento de la estructura de la aplicación, las diferentes funciones que cumple y los objetivos de cada una para mejorar la experiencia de los usuarios.

El documento está compuesto por una portada, introducción, miembros del equipo de desarrollo, descripciones del sistema, artefactos de la base de datos utilizados, decisiones técnicas y bibliografía.

## Listado de equipos y miembros de los equipos.

### Atenienses++
Integrantes:

- Jose Andrés Víquez Ramírez B88635

- Daniela Vargas Sauma B88306 

- Luis Andrés Sánchez Romero B87367

- Fernando Morales B85338

- Elian Ortega Velasquez B85791

### Diosvier

Integrantes:

- Adrián Sibaja Retana  B87561

- Erik Kühlmann Salazar  B84175

- José Esteban Marín Masís  B84594

- Daniel Salazar Mora B87214

- Ricardo Franco Rodríguez B83050

### Legados

- Kevin Obando Molina               B55121

- Erick Rojas Zúñiga                B66310

- Stephanie María Leitón Ramírez    B74106

- Juan Pablo Gamboa Legados         B83135

- Alejandro Mairena Jiménez         B84561

### Drim Team
- Adrian Cordoba      B82348

- Gustavo Jimenez     B84060

- Jose Ignacio Cruz   B62230

- Javier Sandoval     B56762

- Isaac Zuñiga        B68038


### Asignación de roles para cada una de las iteraciones

#### Sprint 0:

- Atenienses++

| Integrante                     | Rol             |
| -------------------------------| ----------------| 
| Luis Andrés Sánchez Romero     | Developer       |
| Daniela Vargas Sauma           | Scrum Master    |
| José Andrés Víquez Ramírez     | Developer       |
| Fernando Ezequiel Morales      | Scrum Ambassador|
| Elian Ortega Velásquez         | Developer       |


- Diosvier:

| Integrante                     | Rol             |
| -------------------------------| ----------------| 
| Adrián Sibaja Retana           | Scrum Master    |
| Erik Kühlmann Salazar          | Scrum Ambassador|
| José Esteban Marín Masís       | Developer       |
| Daniel Salazar Mora            | Developer       |
| Ricardo Franco Rodríguez       | Developer       |


- Legados:

| Integrante                     | Rol             |
| -------------------------------| ----------------| 
| Kevin Obando Molina            | Scrum Master    |
| Juan Pablo Gamboa Legados      | Scrum Ambassador|
| Stephanie María Leitón Ramírez | Developer       |
| Erick Rojas Zúñiga             | Developer       |
| Alejandro Mairena Jiménez      | Developer       |


- Drim Team:

| Integrante                     | Rol             |
| -------------------------------| ----------------| 
| Adrian Cordoba                 | Scrum Master    |
| Javier Sandoval                | Scrum Ambassador|
| Jose Ignacio Cruz              | Developer       |
| Gustavo Jimenez                | Developer       |
| Isaac Zuñiga                   | Developer       |

#### Sprint 1:

    
- Atenienses++

| Integrante                     | Rol             |
| -------------------------------| ----------------| 
| Luis Andrés Sánchez Romero     | Developer       |
| Daniela Vargas Sauma           | Scrum Master    |
| José Andrés Víquez Ramírez     | Developer       |
| Fernando Ezequiel Morales      | Scrum Ambassador|
| Elian Ortega Velásquez         | Developer       |

- Diosvier:

| Integrante                     | Rol             |
| -------------------------------| ----------------| 
| Adrián Sibaja Retana           | Scrum Master    |
| Erik Kühlmann Salazar          | Scrum Ambassador|
| José Esteban Marín Masís       | Developer       |
| Daniel Salazar Mora            | Developer       |
| Ricardo Franco Rodríguez       | Developer       |

## Descripción general del sistema a desarrollar
La aplicación debe desarrollar e integrar las funciones necesarias para la sistematización de los procesos del equipo PRIME, desde una sola aplicación integrada. Por lo tanto, el sistema consiste en una aplicación web con una base de datos, que permita a los usuarios la sistematización de los procesos de atención y la gestión en tiempo real de la información de los procesos entre los equipos de atención y el centro de control. Esto con el fin de permitir que todos los usuarios autorizados cuenten con información actualizada por medio de la aplicación web.

El sistema permitirá a los usuarios realizar la administración de los traslados e incidentes para la movilización desde centros de atención de bajo nivel de complejidad a hospitales de mayor complejidad, el control de los procedimientos establecidos durante los traslados de pacientes, la caracterización de los traslados y la condición de los pacientes, la gestión de envío de información en tiempo real por los miembros del equipo PRIME al centro de control, el seguimiento en tiempo real de los incidentes, la administración de la información de los usuarios especialistas médicos, de los equipos de trabajo humano, los equipos técnicos y las unidades de atención, y finalmente, la generación de estadísticas sobre los procesos de atención, entre otros. 

Debido a la sensibilidad de la información que manejaría esta aplicación(Datos médicos de pacientes), esta debe almacenarse de forma segura, y solo personal autorizado debe poder accederla. Debido a que se manejan procesos médicos, el tiempo de respuesta es un aspecto crucial de la aplicación.

Hasta el momento para esto se han utilizado otras aplicaciones de mensajería web para complir con estos objetivos, que al no estar diseñados para esto no cumplen con todas las funciones que se requieren. La aplicación también debe tener la escalabilidad para ser usada en situaciones similares en el futuro, y no solo para esta función actual.




### Problema que resuelve

El servicio de Terapia Respiratoria del CEACO por medio del equipo de Primera Respuesta Médica Especializada (PRIME), se encarga de la movilización de todos los pacientes con COVID-19 del país, pero de forma más importante de pacientes críticos, que necesitan de una movilización de centros de bajo nivel de complejidad a hospitales de mayor complejidad.

En la actualidad el equipo PRIME, en conjunto con un equipo de la UCR, ya se encuentra trabajando en las funcionalidades iniciales de una aplicación móvil a partir de las prioridades de los usuarios especialistas respiratorios.

Dada que la emergencia sanitaria se encuentra en uno de sus picos de contagio, y se esperan nuevas olas de transmisión, el equipo PRIME requiere además una primera versión de una aplicación web para administrar sus procesos de atención, y que complemente la funcionalidad que ofrecerá la aplicación móvil. El presente producto corresponde a la implementación de la aplicación web.

### Interesados del proyecto y tipos de usuarios.

Administrador del sistema: acceso total a las funciones de la aplicación.

Médico: en los hospitales, tiene acceso a los expedientes de los pacientes (multimedia).

Especialista técnico-médico: montados en ambulancias, se encargan de los incidentes de traslado.

Administrador de la central mando: no ve información de archivos médicos, no diagnostica, ve las unidades de traslado.

Coordinador de equipo: coordina un equipo de traslado, recibe solicitudes.

Gerente médico: médico que se interesa en las estadísticas de los procesos de atención.

### Solución propuesta

Ante el problema descrito anteriormente se da como solución la implementación de una aplicación web por medio de la cual se permite gestionar de mejor manera el proceso ante la atención de incidentes de pacientes COVID-19, inclusive el traslado de los mismos. La idea es brindar un medio digital que permite crear, monitorear, atender y gestionar los incidentes del equipo PRIME del CEACO ante la atención de pacientes. Esta aplicación permitiría llevar el registro desde el momento del reporte del incidente hasta el momento en que se culmine la atención de dicho paciente.

La aplicación a desarrollar se compone de 4 módulos principales; el primero de ellos es el despacho, seguimiento y monitoreo en tiempo real tanto del equipo PRIME como de los pacientes COVID. El segundo la creación de listas de chequeo parametrizables para cada uno de los procesos de atención. El tercero la gestión del envío de información en tiempo real entre el equipo PRIME en campo y el centro de control, tanto para archivos de texto como de multimedia. Y finalmente la autorización y administración gráfica, por medio de cuadros de control, de los diferentes tipos de granularidad de la información.

El primero módulo permitiría inicialmente la creación de nuevos incidentes de pacientes COVID-19 para su respectiva atención de parte del equipo PRIME. La idea consiste en brindar un medio por el cual se pueden notificar al equipo PRIME del CEACO la lista de incidentes en espera de ser atendidos de acuerdo a su prioridad para que se pueda llevar un mejor control de atenciones. Igualmente, este módulo se encargaría de automatizar el proceso de despacho de pacientes de un centro de salud al siguiente en los casos que sea requerido; o bien desde cualquier zona geográfica donde sea necesaria la atención de pacientes COVID-19. Finalmente, este módulo se encargaría de monitorear en tiempo real las unidades PRIME del CEACO, así como la de pacientes COVID por medio de un mapa nacional que permita visualizar la movilización de unidades.

Posteriormente el módulo de creación de listas de chequeo parametrizables por cada uno de los procesos de atención a pacientes COVID, permite para cada uno de los procesos de atención de un incidente, tener listas de chequeos de requerimientos obligatorios para cada uno de los tipos de traslado. Por ejemplo, para un traslado de un paciente permite tener la serie de pasos que los miembros de equipo PRIME debe cumplir antes de atender el incidente con la finalidad de satisfacer las condiciones impuestas por los medios de salud. Es decir, contar con las distintas capas de guantes, mascarillas, batas de protección, entre otros.

El módulo de administración de la información de las listas de chequeo permite garantizar la correcta emisión de mensajes informativos del proceso de atención a la central de mando del equipo PRIME del CEACO. Es decir, este módulo se encarga de garantizar el correcto envío de información presente en listas de chequeo, o en las actualizaciones del estado de un traslado, por medio de mensajes de texto o mensajes multimedia desde el equipo móvil del PRIME hasta el equipo del centro de control para su correcta gestión de la información entre todos los entes responsables de los traslados.

Y finalmente el módulo de autorización y administración gráfica de los distintos niveles de granularidad permite el acceso a la aplicación de un usuario previamente identificado y con sus permisos para poder gestionar, monitorear y administrar cada uno de procesos de atención de incidentes de pacientes COVID de acuerdo a sus permisos sobre el acceso a la información. Igualmente permite mostrar resúmenes visuales, por medio de gráficos, de los procesos de atención de pacientes COVID donde entonces se permitiría llevar una mejor administración del equipo de respuesta PRIME para garantizar su eficiencia en los procesos de atención.

En síntesis, la solución del problema dado se puede resumir por medio de un diagrama de flujos de la aplicación a partir del cual se puede visualizar el proceso de atención de pacientes COVID-19. 

![](https://i.imgur.com/BTWHypT.png)

*Figura 1: Flujo de la aplicación web*

El primer paso corresponde a la creación de un nuevo incidente de atención de un paciente COVID; luego el equipo PRIME del CEACO hace el despacho del incidente, ya sea por medio de una visita a la residencia del paciente para administrar su traslado o a un hospital. Para esto el equipo de atención sigue una serie de procedimientos previos a la atención del paciente para verificar el seguimiento de los protocolos de salud indicados. Igualmente, durante la atención del paciente el equipo PRIME puede validar el cumplimiento de una serie de procedimientos médicos a aplicarle a dicho paciente de acuerdo a su condición; y finalmente una validación de los cumplimientos al finalizar la atención del paciente. 

Todo esto se realiza por medio de las listas de chequeo y el envío de información en tiempo real entre el equipo PRIME y la central de control o los respectivos centros de salud asociados al traslado. Además, la aplicación permite monitorear en tiempo real la movilización de las unidades del equipo PRIME durante cualquier momento de la atención de un incidente.

Además de esto, la aplicación propuesta como solución permite que se dé una administración gráfica, por medio de cuadros de control de mando, en la cual se pueden visualizar gráficos sobre los procesos de atención de pacientes COVID donde se resuma información administrativa de interés. Además de que se manejaría la seguridad de la aplicación por medio de la asignación de usuarios y perfiles a cada una de las personas con acceso a la aplicación para que se pueda manejar los distintos niveles de granularidad y acceso a la información de acuerdo al perfil asociado.

### Visión del producto

Puede encontrar nuestra visión del producto en el link: https://docs.google.com/spreadsheets/d/1XnhCmkLnF6gNzaVUzOPjGgIScgK2AZjcDtBU81P5B8c/edit#gid=1003342746

### Relación con los sistemas externos

El sistema PRIME@UCR interactúa directamente con la aplicación web, una de las principales funciones de la aplicación web es del tipo “back office”, la cual se usa para la  administración de los procesos que se coordinan desde el centro de control de incidentes y el personal médico en campo y  puede ser un medio por el cual se le pueda mandar información en tiempo real a los usuarios que utilicen la aplicación móvil en campo. Además otra de las funciones es que mediante contenido multimedia, la aplicación móvil que está diseñada para los usuarios de campo, pueda enviar información en tiempo real que va a ser visto por los usuarios de la web, esto para mantener informados a todos los entes relacionados en el traslado de pacientes de la CEACO.

### Descripción  de  los  temas  (módulos)  asignados  a  cada  equipo

#### Atenienses++

El tema del que se encarga de desarrollar Atenienses++ es el de estadística y autenticación. Este tema está caracterizado por el desarrollo de la autenticación de usuarios, es decir, que aquellos que tengan un usuario asignado, puedan ingresar a la aplicación con este, además de los diversos asuntos pertinentes a este tema, como la seguridad, la regulación de los tipos de usuario (sus características y permisos), además, en la parte de estadística, se ve todo lo que tiene que ver con la representación visual de las estadisticas a mostrar al usuario, mediante el uso de gráficos.

Los epics a desarrollar para este tema son los siguientes:

1.  **Administración de usuarios y perfiles**:

El administrador de la aplicación debe contar con métodos de autenticación de los usuarios de la aplicación para garantizar la confiabilidad del acceso a los datos de la aplicación.

2.  **Administración del dashboard**:

El administrador debe poder visualizar un dashboard con estadísticas referentes a la información obtenida a través de la aplicación para tener una representación gráfica de dicha información.

3.  **Vista gráfica de usuarios y perfiles**:

El administrador debe poder visualizar la informacion y permisos asignados a cada usuario de la aplicacion. 

#### Diosvier 

Para la administración de los traslados (e incidentes) se requiere la implementación de una plataforma que permita el despacho, seguimiento y monitoreo en tiempo real por medio de GPS y mapas tanto para las unidades terrestres, marítimas y aéreas. Los epics asociados a este tema son:

1. **Crear incidente con datos básicos:** para cada incidente, se debe digitar el origen, destino, nombre del paciente, síntomas y demás datos útiles para su atención.

2. **Asociar expediente con incidente nuevo:** para cada incidente, se consulta en la base de datos si fue creado un expediente anteriormente o si debe generarse uno nuevo.

3. **Asignación de incidente aprobado:** una vez se ha revisado que el incidente se ingresó correctamente, se aprueba y se procede a asignar una unidad de transporte y un equipo para su atención.
    
Con el módulo Dashboard, se debe coordinar el despliegue consistente y limpio de la información en la interfaz de la aplicación. Con el módulo Expedientes Médicos, se encuentran las consultas a los distintos expedientes generados con anterioridad y las solicitudes para crear nuevos documentos. Por último, con el módulo Listas de chequeo, se requiere coordinar el despliegue de esta información en la aplicación para su uso durante el traslado.

#### Legados

Para el control de los procedimientos durante los traslados de pacientes se deben implementar listas de chequeo parametrizables para cada uno de los subprocesos que los componen. Cada uno de los procedimientos, los subprocesos y sus actividades son definidas por los administradores de la aplicación para su posterior uso en los incidentes de atención.

1. **Creación de plantillas de listas de chequeo:** como administrador especializado en salud, registrado y con permisos, debo ser capaz de crear una plantilla de una lista de chequeo para los especialistas de transporte de pacientes.

2. **Administrar las listas de chequeo:** Este Epic tiene como objetivo colaborar con médicos, pacientes y especialistas de la CEACO en la automatización del proceso de traslado de pacientes, ofreciendo una serie de funcionalidad que permita un ágil y efectivo control de procedimientos mediante el uso de listas de chequeo.

3. **Interacción con procesos para incidentes:** Como coordinador técnico médico debo tener alguna forma de almacenar listas de chequeo que se usan solo en ocasiones específicas y que los especialistas técnico médicos tengan acceso a estas.

#### Drim Team

Para la gestión de envío de información en tiempo real por los miembros del equipo PRIME al centro de contro se debe permitir que para cada una de las actividades de las listas de chequeo de los procesos, se pueda enviar información por medio de texto, imágenes, vídeo y voz.

1. **Administración de Contenido Multimedia:**  Este Epic está relacionado a la acceso, visualización y almacenamiento seguro del contentido multimedia.

2. **Adjuntar Contenido Multimedia:** Este está epic relacionado a poder adjuntar contenido, ya sea imagen, texto, video o audio, a las listas de chequeo, o a una cita.

3. **Administración de Expedientes:** Este epic está relacionado a la creación de expedientes, así como la distribución de la información que tiene cada funcionario en referencia a sus permisos. 
 

### Mapa de ruta del producto (Product Road Map)

Puede encontrar nuestro Product Roadmap en el link: https://docs.google.com/spreadsheets/d/1XnhCmkLnF6gNzaVUzOPjGgIScgK2AZjcDtBU81P5B8c/edit#gid=1653817941

### Requerimientos no funcionales que debe cumplir toda la aplicación web

-El programa debe mostrar mensajes de error que sean informativos y orientados a un usuario final.
-El programa debe tener una interfaz gráfica responsiva.


#### Requerimientos de Usabilidad

El programa en su pantalla de inicio le muestra al usuario solamente un menú lateral, cada elemento de este menú tiene un nombre corto y conciso con una imágen de referencia que es visible en todo momento, este menú, al igual que el encabezado, tiene el mismo diseño en todas las ventanas y módulos de la aplicación.

##### Atenienses ++/Dashboards
- La diseño y apariencia de la aplicacion deben de estar implementados en base a los lineamientos propuestos por la UCR.

- La aplicacion debe de tener una interzas sencilla que permita a usuario entenderla intuitivamente.

- El panel de navegacion izquierdo debe de ser facil de usar y poseer nombres de los diferentes modulos a los que se tiene acceso, todo esto siguiento las guias de diseño UCR.

- En el proceso de autenticacion el usuario debe identificarse usando un email y contraseña. Solo los usuarios autenticados pueden tener acceso a la aplicacion.

- El formulario debe de mostrar mensajes de claros cuando haya un error de autenticacion o un campo invalido en el formulario.

- Los usuarios tienen acceso a la informacion de acuerdo a los permisos dados por el administrador.

- El sistema debe de presentar mensajes de error informativos orientados al usuario.

##### Diosvier/Administración de Traslados

- La aplicación debe mostrar un menú lateral a la izquierda, con las pestañas o tabs respectivas a la funcionalidad del módulo Administración de Incidentes. Cada pestaña debe tener un nombre corto y significativo, de manera que el usuario pueda saber qué información está completando en todo momento. Este menú y estas pestañas debe seguir el mismo diseño que todas las demás ventanas y módulos de la aplicación.

- Si el usuario registrara algún incidente incorrectamente, se le debe notificar con un mensaje claro y conciso su error.

- La aplicación debe seguir todos los lineamientos del Manual de Identidad Visual de la Universidad de Costa Rica.

##### Legados/Control de procedimientos

- En la pantalla de registro de listas de chequeo se puede ver cada lista junto a detalles de la misma en orden descendiente, además de enlaces directos en el menú lateral para una búsqueda más veloz de la lista deseada.  

- Para crear una lista se clickea el botón “Crear lista de chequeo” en el menú lateral de la pantalla, esto abrirá una ventana titulada “Crear plantilla de lista de chequeo” (definición previamente explicada en el apartado “Definiciones, acrónimos y abreviaciones”), en esta se ven las características principales de la lista marcadas en un cuadro blanco en el centro de la ventana y marcadas con un asterisco las que son fundamentales, la leyenda del asterisco también se muestra en la parte inferior; el botón de guardar lista está inhabilitado hasta que la ventanas necesarias, Nombre y Tipo, sean correctamente llenadas y esto se ve reflejado en el color del mismo que se hace más intenso y diferenciable cuando se encuentra habilitado, además de tener la opción de cancelar la creación si así lo desea.  

- Al crear una lista esta se ve inmediatamente reflejada en la pantalla de listas de chequeo.

- Para crear un item se se clickea el botón de "Crear item", se abre una pantalla para los detalles del mismo y son guardados asociados a la lista de checkeo actual mostrada.

- Al crear un item este se ve mostrado inmediatamente en pantalla.

- A cada item se pueden añadir subitems mediante el menú desplegable asociado a cada item en pantalla.

- Los subitems son mostrados en pantalla inmediatamente y se ve en pantalla en que nivel se encuentran para asociarlos a su item padre.

#### Drim Team/Contenido Multimedia-Expedientes

-Cuando hay algún error a la hora de subir un archivo, se le debe dar retroalimentación al usuario para que decida si quiere intentarlo de nuevo.
-El usuario debe poder subir archivos de tipo .pdf, .doc, .docx, .xls, .txt, .mp3, .jpg, .png, .mp4, .wmv, .avi.
-Una vez adjuntado el archivo, este debe poder ser visualizado.

#### Requerimientos de Eficiencia

El sistema PRIME@UCR es capaz de soportar la gran cantidad de incidentes que ocurren simultáneamente en todo momento y es capaz de abrir y cerrar los incidentes en menos de 5 segundos, además almacena los archivos multimedia que sean enviados y los enlaza a la instancia de ítem con el que fueron creados, este envío se hace además con algoritmos de compresión para dar la mayor velocidad posible que la conexión presente permita.  

El Sistema Prime@UCR deberá ser capaz de soportar todos los incidentes de traslado de pacientes que se tengan que abrir.  

El sistema deberá ser lo suficientemente eficiente para que un incidente de traslado se pueda abrir en un segundo.  

El sistema deberá contar con algoritmos de autocompresión para que el envío de archivos multimedia sea lo más rápido posible y en tiempo real.

#### Requerimientos de Confiabilidad 

Ante un cierre involuntario del usuario y si este estaba haciendo alguna tarea, el sistema al abrirse nuevamente deberá restaurarse en el punto en el que el usuario lo dejó.  

El sistema deberá contar con una opción de “reintentar” ante la existencia de una falla al enviar un archivo multimedia.

#### Requerimientos de Seguridad

Las claves y usuarios son encriptadas mediante una función Hash y los datos se encriptan al enviarlos y recibirlos, ya que contienen información delicada de los pacientes transportados.  

El sistema PRIME@UCR deberá mantener una jerarquía de usuarios para que estos tengan acceso solo a ciertos tipos de datos.  

Las comunicaciones y contenido multimedia que se intercambia deben estar encriptadas mediante algoritmos para dicho fin, esto para que no haya fugas o robo de información delicada.

#### Requerimientos de desarrollo

El sistema está implementado usando Core.NET y con interfaz creada con Blazor y la base de datos sigue un modelo relacional escrita con SQL y diseñada con un modelo ER y su diagrama lógico.

#### Requerimientos operacionales

Los usuarios deben iniciar sesión para la mayoría de funciones del sistema, los niveles de acceso son muy variados dando acceso a algunos usuarios solo a algunas funciones y a otros a las mismas funciones pero a mayor información, las funciones están divididas en módulos que dan acceso a cada usuario de manera independiente.

#### Requerimientos legales

El sistema PRIME@UCR, al manejar datos sensibles de carácter médico, debe contar con una serie de medidas de seguridad para que cumpla con la Ley 8968 o también conocida como “Protección de la Persona frente al tratamiento de sus datos personales”.  

El sistema PRIME@UCR, mantendrá un control de quién tiene acceso a ciertos datos ya sea del paciente, médicos u otros entes relacionados con el traslado de pacientes COVID positivos y velará porque esto cumpla con los principios del secreto profesional de las Ciencias de la Salud. Este primer requerimiento es necesarios para cumplir la ley 8968, capítulo 2, sección 3, artículo 11.[1]

#### Requerimientos éticos

Los datos manejados por el sistema son encriptados y almacenados de manera segura, solo un pequeño grupo de personas tienen acceso selectivo a los datos almacenados, el acceso a información confidencial es registrada para futuras revisiones.  

El equipo de desarrollo no tiene, tuvo ni tendrá ningún acceso a esta información y trabajó en todo momento con datos hipotéticos y de fantasía.  

Las contraseñas de los distintos usuarios solo deben ser conocidas por ellos mismos y en caso de una emergencia, el administrador técnico médico podría llegar a tener acceso a ellos.  

El contenido multimedia que se tome al hacer un traslado por ejemplo foto del paciente que está siendo trasladado, deberá manejarse con extrema cautela y no debe ser vista por nadie que no sean médicos o especialistas técnicos médicos.  

La condición en la que se traslada el paciente solo deberá ser conocida por médicos y especialistas técnicos.  

Información personal de los entes relacionados no debe fugarse ni ser vista por nadie más que los usuarios que tengan el nivel de jerarquía necesario.

#### Requerimientos regulatorios

El diseño de la página sigue la paleta de colores, diseño del header y footer propuesta por la Universidad de Costa Rica, siguiendo sus regulaciones  del manual de desarrollo de sitios web de la Oficina de Divulgación e Información ODI [8] y también siguiendo ejemplos de la página oficial de la UCR así como de la ECCI.

## Artefactos de base de datos

### Esquema conceptual de la base de datos

A continuación se muestra el esquema conceptual:

![](https://app.lucidchart.com/invitations/accept/6de27e38-f785-4942-9cc4-9defbde00e09)

### Esquema lógico de la base de datos

![](https://i.imgur.com/J4bu9GS.png)

A continuación se muestra el esquema lógico dividido en los correspondientes a cada equipo: 

Atenienses++ (Administración de Usuarios):

![](https://i.imgur.com/69sm8RJ.png)

Diosvier (Administración de Translados) :

![](https://i.imgur.com/Z5rxjQK.png)

Legados (Control del Procedimientos) :

![](https://i.imgur.com/Qwl0XXB.png)

Drim Team (Gestión de Información) :

![](https://i.imgur.com/rcEkHBA.png)

## Decisiones técnicas.

### Metodologías utilizadas y procesos definidos.

1. **Git:** mecanismo para control de versiones. Se optó por una rama por equipo y una rama por cada desarrollador. Además, se acordaron reglas para subir código a la rama *master*, las cuales están especificadas en la definición de listo.

2. **Scrum:** metodología ágil para el desarrollo de software. Se trabajó con un *scrum of scrums*, con cada equipo auto-organizado trabajando en un módulo específico de la aplicación. En los links adjuntos, se pueden consultar los distintos procesos que engloba esta metodología.

### Artefactos utilizados en el desarrollo del proyecto

Jira software: herramienta de desarrollo de software para equipos ágiles que los ayuda a planificar, supervisar y  publicar software de alta calidad.[2]

Bitbucket: Sitio para planificar proyectos,  programación colaborativamente del código, probar y colocar.Integrado con Jira software [3]

Visual Studio 2019:  Entorno de desarrollo integrado.[4] IDE donde se creará el programa de la aplicación web en .Net core y Blazor en su versión de Visual Studio Enterprise

SQL Server: Plataforma de inteligencia de datos, segura y de buen rendimiento, ayuda a analizar todo tipo de datos, obtener información valiosa consultando datos relacionales, no relacionales sea que estén o no estructurados. [7]

Blazor: Es un marco para crear una interfaz de usuario web interactiva del lado del cliente utilizando .NET que tiene las ventajas:
  - Permite crear interfaces de usuario interactivas utilizando C# en lugar de JavaScript.
  
  - Comparte lógica de aplicación del lado del servidor y del lado del cliente escrito en .NET.
  
  - Renderizar la interfaz de usuario cómo HTML y CSS para una amplia compatibilidad entre navegadores.
  
  - Integra plataformas de alojamiento modernas cómo Docker. [5]

CORE .net: es la versión de código abierto de ASP.NET que se ejecuta en machOS, Linux y Windows. Fue lanzada a inicios del año 2016 y es un rediseno de versiones anteriores de ASP.NET que solo se podían utilizar en Windows. [6]

### Tecnologías utilizadas con sus respectivas versiones.

Para el presente proyecto, se decidió utilizar las siguientes tecnologías:

#### Blazor

Se optó por el uso de Blazor como framework para el desarrollo de la aplicación web utilizando .NET, desarrollado por microsoft. 

#### Core 3.1

Es un franework de desarrollo de aplicaciones desarrollado por Microsoft.

#### Blazor server 3.1

Esto corresponde a un modelo de alojamiento, de manera que la aplicación se ejecute desde un servidor desde la aplicación ASP.NET

#### Sql server 2019

Corresponde a un sistema de manejo de bases de datos relacionales desarrollado por Microsoft.

#### Bootstrap 4.5.2

Corresponde a un framework de CSS. Es una biblioteca de herramientas para el diseño de aplicaciones web. 

### Repositorio de código y estrategia git para el proyecto

#### Repositorio 

Se usó Bitbucket para guardar el repositorio que contiene el código del desarrollo del proyecto, el cual se encuentra en el siguiente enlace: https://bitbucket.org/cristian_quesadalopez/ecci_ci0128_ii2020_g01_pi/src/master/. 

#### Estrategia git para el proyecto

La estrategia de git que se utlizó fue la de branching; en esta forma de trabajo, la estructura es la siguiente: inicialmente, se tiene el branch de master, luego por equipo, se tiene un branch aparte y, por cada desarrollador indivual, se tiene un branch personal. Al ser 4 equipos que forman parte del proyecto, se tendrá, además del branch master, 1 branch por equipo + 5 branches por los integrantes de los equipos; por lo que, en total, se tendrían 25 branches diferentes. 

Cabe destacar que en la branch master únicamente se le harán commits y merge al final de las iteraciones; esto para tener releases funcionales en el master y poder asegurar que el la aplicación haya sido testeada correctamente antes de ser agregada al branch. 

En más detalle, los desarrolladores que vayan implementando funcionalidades a la aplicación van realizando commits a su branch por la duración de un sprint. Cuando sea necesario, estos desarrolladores pueden realizar merges con su branch de equipo, ya que otros miembros del equipo pueden ir utilizando las funcionalidades que sus compañeros hayan implementado. Una vez que se termine la iteración, se hará merge entre los diferentes branches de los equipos principales, para de esta forma, poder hacer un único merge con el master, para así evitar la máxima cantidad de errores que pueden ocurrir. 



### Definición de listo (DoD)

Puede encontrar nuestra definición de listo en el link: https://docs.google.com/spreadsheets/d/1XnhCmkLnF6gNzaVUzOPjGgIScgK2AZjcDtBU81P5B8c/edit#gid=115914780

## Referencias Bibliográficas

[1] Sistema Costarricense de Información Jurídica. (2022, 23 septiembre). Procuraduría General de la República. http://www.pgrweb.go.cr/scij/Busqueda/Normativa/Normas/nrm_texto_completo.aspx?param1=NRTC&nValor1=1&nValor2=70975&nValor3=85989&strTipM=TC#:%7E:text=Esta%20ley%20es%20de%20orden,y%20dem%C3%A1s%20derechos%20de%20la

[2] Jira Software. (n.d.). Atlassian. Retrieved Septiembre 24, 2020, from https://www.atlassian.com/es/software/jira?&aceid=&adposition=&adgroup=109687540504&campaign=10332064761&creative=443576046665&device=c&keyword=jira%20software&matchtype=e&network=g&placement=&ds_kids=p55122863597&ds_e=GOOGLE&ds_eid=700000001550060&ds_e1=G

[3] Bitbucket. (n.d.). Alassian. Retrieved Septiembre 24, 2020, from https://www.atlassian.com/es/software/bitbucket

[4] Visual Studio. (n.d.). Microsoft. Retrieved Septiembre 24, 2020, from https://visualstudio.microsoft.com/es/

[5] Introduction to ASP.NET CORE Blazor. (n.d.). Microsoft. Retrieved Septiembre 24, 2020, from https://docs.microsoft.com/en-us/aspnet/core/blazor/?view=aspnetcore-3.1

[6] What is ASP.NET Core? (n.d.). Microsoft. Retrieved Septiembre 24, 2020, from https://dotnet.microsoft.com/learn/aspnet/what-is-aspnet-core

[7] Pruebe SQL server 2019. (n.d.). Microsoft. Retrieved Septiembre 24, 2020, from https://www.microsoft.com/en-us/sql-server/sql-server-2019

[8] Manual de lineamientos de Sitios web UCR. (n.d.). Oficina de Divulgación e Información. Retrieved Setiembre 27, 2020, from https://odi.ucr.ac.cr/

[9] Ouellette, A. (2017, 20 septiembre). What is Bootstrap: A Beginners Guide. CareerFoundry. https://getbootstrap.com/docs/4.5/getting-started/introduction/

[10] Roth, D. (2020, 11 agosto). ASP.NET Core Blazor hosting models. Microsoft Docs. https://docs.microsoft.com/en-us/aspnet/core/blazor/hosting-models?view=aspnetcore-3.1

[11] Roth, D., & Latham, L. (2020, 19 junio). Introduction to ASP.NET Core Blazor. Microsoft Docs. https://docs.microsoft.com/en-us/aspnet/core/blazor/?view=aspnetcore-3.1

[12] Elmasri, R., & Navathe, S. B. (2010). Fundamentals of Database Systems (6th ed. ed.). Addison Wesley Longman.

[13] Cohn, M. (2005). Agile Estimating And Planning (1.a ed.). Prentice Hall.

[14] Quesada Lopez C. (2020) Descripción general del sistema PRIME@UCR Recuperado de: https://mv1.mediacionvirtual.ucr.ac.cr/pluginfile.php/1167974/mod_folder/content/0/1-Descripci%C3%B3n%20general%20de%20la%20aplicaci%C3%B3n%20web_CI0128_001_II-2020.pdf?forcedownload=1 