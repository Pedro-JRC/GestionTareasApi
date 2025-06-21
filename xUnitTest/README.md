
# 🧪 xUnitTest – Pruebas Unitarias para Gestión de Tareas API

Este proyecto contiene un conjunto de **10 pruebas unitarias clave** diseñadas para validar la lógica principal de la API `GestionTareasApi`.

Las pruebas cubren funcionalidades críticas como autenticación, validación de usuarios, encolado de tareas, y comportamiento reactivo en la cola de tareas.

---

## 🚀 Tecnologías Usadas

- **.NET 9**
- **xUnit**
- **Moq**
- **EF Core InMemory**
- **Microsoft.Extensions.DependencyInjection**

---

## 📂 Estructura del Proyecto

```
xUnitTest/
├── EntornoPruebas.cs
├── DummyScopeFactory.cs
├── Tests/
│   ├── 01_Login_Valido.cs
│   ├── 02_Login_ContraseñaIncorrecta.cs
│   └── ... hasta la 10
└── xUnitTest.csproj
```

---

## 🧪 Pruebas Implementadas

| Nº  | Prueba                                     | Resultado Esperado                          |
|-----|--------------------------------------------|----------------------------------------------|
| 01  | Login válido                                | Devuelve token JWT correctamente             |
| 02  | Contraseña incorrecta                       | Rechaza el inicio de sesión                  |
| 03  | Refresh token                               | Renueva token si está por expirar           |
| 04  | Registro duplicado                          | Rechaza si el usuario ya existe             |
| 05  | Contraseñas no coinciden                    | No permite actualizar usuario               |
| 06  | Porcentaje tareas completadas              | Calcula correctamente el porcentaje          |
| 07  | Crear tarea válida                          | Encola correctamente la tarea                |
| 08  | Actualizar tarea inexistente                | No realiza ninguna acción                    |
| 09  | Validación de descripción muy corta         | Retorna error de validación                  |
| 10  | Cola Rx.NET procesa en orden (FIFO)         | Asegura procesamiento secuencial             |

---

## ▶️ Ejecutar las pruebas

### Visual Studio

1. Abrir solución `GestionTareasApi.sln`.
2. Ir al panel **Test Explorer** (`Ctrl+E, T`).
3. Click en **Run All Tests**.

### Terminal

Desde la raíz del proyecto:

```bash
dotnet test xUnitTest
```

---

## ℹ️ Notas

- Las pruebas usan `DbContext InMemory` y mocks con `Moq`.
- El archivo `EntornoPruebas.cs` centraliza la preparación del entorno.
- Cada archivo en `Tests/` está numerado y enfocado en un caso crítico.

---

**Autor:** Pedro Rosario  
**Módulo:** Pruebas Unitarias – Proyecto `GestionTareasApi`  
**Última actualización:** Junio 2025
