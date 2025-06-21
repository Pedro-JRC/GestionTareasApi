
<h1 align="center">🧪 Proyecto de Pruebas Unitarias - xUnitTest</h1>
<p align="center">
  Este proyecto contiene un conjunto de pruebas automatizadas con <strong>xUnit</strong> y <strong>Moq</strong> para garantizar la calidad y correcto funcionamiento de la API <code>GestionTareasApi</code>.
</p>

<p align="center">
  <img src="https://img.shields.io/badge/Tested%20With-xUnit%20%2B%20Moq-green" />
  <img src="https://img.shields.io/badge/Coverage-10%20Pruebas%20Críticas-blue" />
  <img src="https://img.shields.io/badge/Estado-Estable-brightgreen" />
</p>

---

## 🎯 Objetivo

Este proyecto tiene como objetivo validar los casos críticos de negocio, autenticación, lógica de tareas y validaciones del sistema. Está desarrollado con **xUnit** como framework de testing y utiliza **Moq** para simular dependencias externas como servicios, base de datos o loggers.

---

## 🔍 Pruebas implementadas

El directorio `Tests/` contiene 10 clases, cada una validando un escenario específico:

| Nº | Prueba                                           | Verifica que...                                   |
|----|--------------------------------------------------|---------------------------------------------------|
| 01 | Login válido                                     | Devuelva correctamente el token JWT              |
| 02 | Contraseña incorrecta                            | El acceso sea denegado                           |
| 03 | Token próximo a expirar                          | Se renueve automáticamente                       |
| 04 | Registro duplicado                               | El usuario no pueda ser registrado dos veces     |
| 05 | Contraseñas no coinciden                         | Se rechace la actualización                      |
| 06 | Cálculo de porcentaje completado                 | Sea correcto                                      |
| 07 | Tarea válida se encola                           | Y sea confirmada como encolada                   |
| 08 | Actualización de tarea inexistente               | No cause errores y retorne false                 |
| 09 | Validación de descripción                        | Rechace descripciones cortas                     |
| 10 | Procesamiento FIFO (Rx.NET)                      | Las tareas se ejecuten en el orden correcto      |

---

## 🧪 Cómo ejecutar las pruebas

Puedes ejecutar todas las pruebas desde la terminal:

```bash
dotnet test xUnitTest
```

O bien desde **Visual Studio**, abriendo la ventana `Test Explorer` y ejecutando cada caso individualmente.

---

## 🧱 Estructura

```
xUnitTest/
├── EntornoPruebas.cs           # Contexto común con mocks, contexto en memoria, helpers
├── Tests/
│   ├── 01_Login_Valido.cs
│   ├── 02_Login_ContraseñaIncorrecta.cs
│   ├── ...
│   └── 10_ColaTareasRx_ProcesaSecuencialmente.cs
├── xUnitTest.csproj
└── README.md                   # Este archivo
```

---

## 🛠️ Tecnologías utilizadas

- **xUnit 2.9.3**
- **Moq 4.20+**
- **Microsoft.EntityFrameworkCore.InMemory**
- **.NET 9**

---

## ✍️ Autor

**Pedro Rosario**  
Proyecto creado para validar la arquitectura y lógica de la API GestiónTareasApi de forma aislada, modular y profesional.

---

<p align="center"><strong>✅ Código probado es código confiable. ¡Aporta o expande estas pruebas con confianza!</strong></p>
