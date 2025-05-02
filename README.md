# API de Gestión de Tareas

Esta API permite gestionar tareas y usuarios, incluyendo funcionalidades como autenticación con JWT, asignación de tareas, filtros avanzados y almacenamiento de datos adicionales personalizados.

## 🚀 Tecnologías Usadas

- **.NET 9**
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **SQL Server**
- **JWT (Json Web Token)** para autenticación
- **Swagger** para pruebas y documentación
- **PasswordHasher** para cifrado de contraseñas

## 📂 Estructura del Proyecto

- `1-Modelos`: Modelos de entidad como `UsuariosModel` y `TareaGeneral`.
- `2-DTOs`: Objetos de transferencia para creación, actualización y respuesta.
- `3-Servicios`: Lógica de negocio y acceso a datos.
- `4-Controllers`: Endpoints de la API.
- `Utilidades`: Helpers como `SeguridadHelper` y `DatosAdicionalesHelper`.
- `Middleware`: Manejador de excepciones personalizado.
- `Data`: `AppDbContext` para EF Core.
- `Program.cs`: Configuración principal del host, servicios y middlewares.

## ⚙️ Configuración

1. Asegúrate de tener instalado:
   - .NET 9 SDK
   - SQL Server
   - Visual Studio 2022+ o VS Code

2. Clona el repositorio y configura la cadena de conexión en `appsettings.json`:

```json
"ConnectionStrings": {
  "ConexionPorDefecto": "Server=localhost;Database=GestionTareasDb;Trusted_Connection=True;TrustServerCertificate=True;"
},
"Jwt": {
  "Key": "TU_CLAVE_SECRETA_MUY_SEGURA",
  "Issuer": "GestionTareasApi",
  "Audience": "GestionTareasApiUsuarios",
  "ExpiresInMinutes": "60"
}
```

## 🛠️ Ejecutar la Aplicación

1. 📦 Ejecuta las migraciones:

- Crear migración
```bash
Add-Migration Inicial
 ```
- Aplica la migración
```bash
Update-Database
```
3. Corre la aplicación desde Visual Studio:


4. Accede a la documentación Swagger en:  
   [https://localhost:{puerto}/swagger](https://localhost:{puerto}/swagger)

## 🔐 Endpoints Principales

### Autenticación

| Método | Ruta                  | Descripción             |
|--------|-----------------------|-------------------------|
| POST   | /api/auth/login       | Iniciar sesión (JWT)    |
| POST   | /api/auth/refresh     | Refrescar token JWT     |

### Usuarios

| Método | Ruta                     | Descripción                  |
|--------|--------------------------|------------------------------|
| GET    | /api/usuarios            | Listar usuarios              |
| GET    | /api/usuarios/{id}       | Obtener usuario por ID       |
| POST   | /api/usuarios/registrar  | Crear usuario                |
| PUT    | /api/usuarios/{id}       | Actualizar usuario           |
| DELETE | /api/usuarios/{id}       | Eliminar usuario             |

### Tareas

| Método | Ruta                     | Descripción                     |
|--------|--------------------------|---------------------------------|
| GET    | /api/tareas              | Listar tareas                   |
| GET    | /api/tareas/{id}         | Obtener tarea por ID            |
| POST   | /api/tareas              | Crear tarea                     |
| PUT    | /api/tareas/{id}         | Actualizar tarea                |
| DELETE | /api/tareas/{id}         | Eliminar tarea                  |

### Filtros de Tareas

| Método | Ruta                                | Descripción                        |
|--------|-------------------------------------|------------------------------------|
| GET    | /api/filtrostareas/por-estado       | Filtrar por estado                 |
| GET    | /api/filtrostareas/por-fecha        | Filtrar por fecha de vencimiento  |
| GET    | /api/filtrostareas/por-categoria    | Filtrar por categoría              |
| GET    | /api/filtrostareas/por-etiqueta     | Buscar en DatosAdicionales (lista)|
| GET    | /api/filtrostareas/por-prioridad-datos | Buscar número en DatosAdicionales|

## 📋 Ejemplo de Payload para Crear Tarea

```json
{
  "titulo": "Corregir login",
  "descripcion": "Revisar bug en el login con JWT",
  "fechaVencimiento": "2025-06-01T23:59:00",
  "estado": "Pendiente",
  "datosAdicionales": [ "etiqueta1", "urgente", "QA" ],
  "prioridad": 2,
  "categoria": "Desarrollo",
  "asignadoA": "juan23"
}
```

## ✅ Pruebas

1. Usa Swagger para hacer login:
   - `POST /api/auth/login`
   - Obtén el token JWT.

2. Copia el token y haz clic en "Authorize" en Swagger.
3. Usa cualquier endpoint autenticado como `GET /api/tareas`.

---

**Autor:** Pedro Rosario  
**Proyecto Educativo** – Curso C# .NET Avanzado
