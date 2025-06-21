
<h1 align="center">🧠 Gestión de Tareas API + Pruebas Unitarias</h1>
<p align="center">
  Plataforma completa para la gestión de tareas con arquitectura limpia, programación reactiva, autenticación JWT
  y pruebas unitarias profesionales con xUnit y Moq.
</p>

<p align="center">
  <img src="https://img.shields.io/badge/.NET-9.0-blueviolet" />
  <img src="https://img.shields.io/badge/Tests-xUnit%20%2B%20Moq-green" />
  <img src="https://img.shields.io/badge/Arquitectura-Clean--Architecture-blue" />
  <img src="https://img.shields.io/badge/Status-En%20Desarrollo-yellow" />
</p>

---

## 🌐 Descripción General

Este repositorio contiene **dos proyectos** integrados en una solución completa:

| Proyecto          | Descripción                                                                 |
|-------------------|-----------------------------------------------------------------------------|
| `GestionTareasApi`| API REST desarrollada en .NET 9 con autenticación, validaciones, SignalR y Rx.NET |
| `xUnitTest`       | Proyecto de pruebas unitarias con 10 casos clave usando xUnit y Moq        |

Diseñada para ser **modular, escalable y profesional**, ideal para desarrollos reales o educativos.

---

## 📌 Funcionalidades Destacadas

✅ Autenticación con JWT y Refresh Token  
✅ Encolado y ejecución secuencial de tareas (Rx.NET)  
✅ Validaciones dinámicas con delegados y expresiones  
✅ Pruebas unitarias con cobertura funcional y lógica  
✅ Patrones de diseño: Factory, Reactive, Clean Code  
✅ CRUD completo de Usuarios y Tareas con DTOs limpios  
✅ Memorización de cálculos frecuentes con cache interna

---

## 🧱 Estructura de la Solución

```
📦 GestionTareasApi/
├── GestionTareasApi/             # API principal (.NET 9)
│   ├── Controllers/
│   ├── Models/
│   ├── Servicios/
│   └── DTOs/
├── xUnitTest/                    # Proyecto de pruebas
│   ├── EntornoPruebas.cs
│   ├── DummyScopeFactory.cs
│   └── Tests/
│       ├── 01_Login_Valido.cs
│       └── ... hasta la 10
├── GestionTareasApi.sln          # Solución principal
└── README.md                     # Este archivo
```

---

## 🧪 Pruebas Unitarias (xUnitTest)

Pruebas clave implementadas para cubrir casos críticos del sistema:

| Nº | Prueba                                           | Resultado esperado                                |
|----|--------------------------------------------------|---------------------------------------------------|
| 01 | Login válido                                     | Devuelve token JWT correctamente                 |
| 02 | Contraseña incorrecta                            | Rechaza acceso                                   |
| 03 | Token por expirar                                | Se renueva automáticamente                       |
| 04 | Registro duplicado                               | Rechaza usuario ya existente                     |
| 05 | Actualización de usuario inválida                | Rechaza por contraseñas no coincidentes          |
| 06 | Porcentaje de tareas completadas                 | Calculado correctamente                          |
| 07 | Encolado de tareas válidas                       | Tarea se encola exitosamente                     |
| 08 | Actualizar tarea inexistente                     | No realiza la operación                          |
| 09 | Validación de campos                              | Rechaza descripciones muy cortas                 |
| 10 | Procesamiento secuencial FIFO                    | Se respeta orden de ejecución con Rx.NET         |

```bash
# Ejecutar pruebas
dotnet test xUnitTest
```

---

## 🛠️ Tecnologías Clave

- **.NET 9 / C# 13**
- **Entity Framework Core (InMemory)**
- **JWT Authentication + Refresh Tokens**
- **Rx.NET + SignalR**
- **xUnit + Moq**
- **Patrones: Factory, Delegates, Memorización**
- **Arquitectura Limpia y modular**

---



## 🧑‍💻 Autor

**Pedro J. Rosario**  
Desarrollador .NET • Enfocado en buenas prácticas, automatización y calidad de código.

---

<p align="center"><strong>¡Explora, ejecuta y contribuye a este proyecto profesional de ejemplo! 💼</strong></p>
