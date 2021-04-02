# Tareas App

Esta solución de ejemplo tiene cómo obejtivo explorar características y patrones de diseño relcionados al desarrollo de aplicaciones en .Net Core

---

# Instrucciones

## Preparación

1. Instalar .Net Core sdk versión 3.1 (recomendado) o superior
2. Instalar Docker y Docker componse (opcional)
3. Clonar este repositorio

### Windows

Si quiere ejecutar la solución usando Docker sobre Windows tenga en cuenta lo siguiente:

* Debe contar con Windows 10 versión 1903 compilación 18362.X o superior.
* [Debe instalar y configurar WSL 2](https://docs.microsoft.com/en-us/windows/wsl/install-win10#manual-installation-steps) (Windows Subsystem fo Linux).  También se puede WSL 1 aunque el redimiento es menor.
* Active el uso de [WSL 2 en Docker](https://docs.docker.com/docker-for-windows/wsl/)
* Si usa VS Code y quiere integrarlo con Docker, este [tutorial](https://docs.microsoft.com/en-us/windows/wsl/tutorials/wsl-containers) le dice cómo.

### Migraciones

Las migraciones son una característica de EF para administrar cambios en el modelo de datos y sincronizarlos con la base de datos.  Al desarrollar una aplicación nueva, el modelo de datos cambia con frecuencia y, cada vez que lo hace, se deja de sincronizar con la base de datos.  Cada vez que cambie el modelo de datos (agregar, quitar o cambiar las clases de entidad, o bien cambiar la clase DbContext), puede eliminar la base de datos y EF crea una que coincida con el modelo y la inicializa con datos de prueba.

1. Abra una sesión de terminal y ubíquese en la raíz de *TareasAPI*
2. Ejecute los siguientes comandos:

#### Preparar la herramienta

`dotnet tool install --global dotnet-ef`

`dotnet tool update --global dotnet-ef //opcional`

`dotnet add package Microsoft.EntityFrameworkCore.Design`

`dotnet add package Microsoft.EntityFrameworkCore.SqlServer //Este paquete va a variar dependiendo el DBMS que esté usando (Postgres, MySQL, etc)`

`dotnet add package Microsoft.EntityFrameworkCore.Tools`

#### Probar la herramienta
`dotnet ef`

#### Generar una Migración a partir del modelo de datos

`dotnet ef migrations add InitialCreate --project ..\Infrastructure\Infrastructure.csproj`

#### Aplicar la migración en la base de datos

`dotnet ef database update`

#### Eliminar la base de datos

`dotnet ef database drop`

#### Eliminar la migración

`dotnet ef migrations remove --project ..\Infrastructure\Infrastructure.csproj`

[Herramienta de migraciones](https://docs.microsoft.com/en-us/ef/core/cli/dotnet#installing-the-tools)

[Tutorial de migraciones](https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/migrations?view=aspnetcore-5.0#the-data-model-snapshot)

[Otro tutorial con instrcciones extras](https://thecodeblogger.com/2020/01/09/net-core-3-and-entity-framework-core-migrations/)