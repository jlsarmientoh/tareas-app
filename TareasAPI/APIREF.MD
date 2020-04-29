|API  |Description  |Request body  |Response body  |
|---------|---------|---------|---------|
|GET /api/tareas         |Retorna todas las tareas|None|Array tareas|
|GET /api/tareas/{id}    |Retorna una tarea a partir de su id|None|tarea|
|POST /api/tareas        |Agrega una nueva tarea|tarea a crear|tarea creada|
|PUT /api/tareas/{id}    |Actualiza una tarea|la tarea a actualizar|None|
|DELETE /api/tareas/{id} |Elimina una tarea a partir de su id|None|None|


**DIseño**

![MVC referencia](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api/_static/architecture.png)