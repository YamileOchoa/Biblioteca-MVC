<div align="center">

# 📚 Biblioteca MVC

### Sistema de gestión de biblioteca con ASP.NET Core MVC, Dapper y SQL Server

![.NET](https://img.shields.io/badge/.NET-ASP.NET%20Core%20MVC-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-Procedimientos%20Almacenados-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3-7952B3?style=for-the-badge&logo=bootstrap&logoColor=white)
![Dapper](https://img.shields.io/badge/Dapper-Micro%20ORM-6366F1?style=for-the-badge)

</div>

---

## ✨ Descripción

**Biblioteca MVC** es una aplicación web para administrar el catálogo de libros, los socios y los préstamos de una biblioteca. Está construida con **ASP.NET Core MVC** y accede a la base de datos **BibliotecaDB** mediante **Dapper**, ejecutando únicamente **procedimientos almacenados**. Todo el código vive en un único proyecto: `Biblioteca.Web`.

---

## 🚀 Funcionalidades

| Módulo | Qué se puede hacer |
|---|---|
| 📖 **Libros** | Listar, buscar por título, ver detalle, crear, editar y eliminar (eliminación **lógica**, `Activo = 0`) |
| 👥 **Socios** | Listar socios activos y registrar nuevos. Si el DNI ya existe, se muestra un mensaje de validación en el formulario |
| 📋 **Reporte de préstamos** | Filtro por rango de fechas (`desde` y `hasta`) con socio, libro, fecha límite y estado. Conserva el rango elegido |
| 🎨 **Diseño** | Bootstrap 5.3, iconos, tarjetas de estadísticas, badges de stock y **modo oscuro** con un clic |

---

## 🛠️ Tecnologías

- **ASP.NET Core MVC** (Razor Views)
- **Dapper** (`QueryAsync` / `ExecuteAsync` con `CommandType.StoredProcedure`)
- **Microsoft.Data.SqlClient**
- **SQL Server** (base de datos `BibliotecaDB`)
- **Bootstrap 5.3** + **Bootstrap Icons** + fuente **Inter**

---

## 🧱 Arquitectura

La petición recorre capas con responsabilidades separadas:

```
Navegador ➜ Ruta ➜ Controlador ➜ Repositorio (Dapper) ➜ Procedimiento almacenado ➜ BibliotecaDB
                       │
                       └──────────────➜ Vista Razor (Model / ViewData / TempData) ➜ Navegador
```

- **Controladores**: reciben la petición y coordinan. No contienen SQL ni abren conexiones.
- **Repositorios**: ejecutan los procedimientos almacenados con Dapper.
- **Procedimientos almacenados**: contienen todo el SQL.
- **Vistas**: fuertemente tipadas con `@model`.

### 📦 Cómo se pasan los datos a la vista

| Mecanismo | Uso en el proyecto |
|---|---|
| **Model** | `Libro`, `Socio`, `PrestamoReporte` y sus listas |
| **ViewData** | Lista de autores, texto de búsqueda, rango de fechas y títulos de página |
| **TempData** | Mensajes de éxito tras guardar (sobreviven a la redirección) |

---

## 📁 Estructura del proyecto

```
Biblioteca.Web/
├── Controllers/
│   ├── LibrosController.cs
│   ├── SociosController.cs
│   └── PrestamosController.cs
├── Models/
│   ├── Libro.cs
│   ├── Autor.cs
│   ├── Socio.cs
│   └── PrestamoReporte.cs
├── Repositorios/
│   ├── LibroRepositorio.cs
│   ├── SocioRepositorio.cs
│   └── PrestamoRepositorio.cs
├── Views/
│   ├── Libros/      (Index, Details, Create, Edit, Delete, _LibroForm, _LibroFila)
│   ├── Socios/      (Index, Create)
│   ├── Prestamos/   (Reporte)
│   └── Shared/      (_Layout)
├── wwwroot/css/site.css
├── appsettings.json
└── Program.cs
```

---

## 🗄️ Base de datos

Tablas: `Autores`, `Libros`, `Socios`, `Prestamos` y `DetallePrestamo`, con datos de prueba (8 autores, 20 libros, 10 socios y 5 préstamos).

Procedimientos almacenados utilizados:

| Módulo | Procedimientos |
|---|---|
| Libros | `usp_Libros_Listar`, `usp_Libros_BuscarPorTitulo`, `usp_Libros_ObtenerPorId`, `usp_Libros_Insertar`, `usp_Libros_Actualizar`, `usp_Libros_Eliminar` |
| Socios | `usp_Socios_Listar`, `usp_Socios_Insertar` |
| Autores | `usp_Autores_ListarActivos` |
| Reportes | `usp_Prestamos_ReportePorFechas` |

---

## ▶️ Cómo ejecutarlo

**1. Clonar el repositorio**

```bash
git clone https://github.com/YamileOchoa/Biblioteca-MVC.git
cd Biblioteca-MVC
```

**2. Crear la base de datos**

Abre SQL Server Management Studio y ejecuta, en este orden:

1. El script de tablas y datos de prueba de `BibliotecaDB`.
2. El script de procedimientos almacenados.

**3. Configurar la cadena de conexión**

Edita `appsettings.json` y ajusta `Server` según tu instalación (`localhost`, `.\SQLEXPRESS`, `(localdb)\MSSQLLocalDB`...):

```json
"ConnectionStrings": {
  "BibliotecaDB": "Server=localhost;Database=BibliotecaDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

**4. Ejecutar**

```bash
cd Biblioteca.Web
dotnet run
```

Abre la dirección que aparece en la consola (por ejemplo `https://localhost:xxxx`).

> 💡 **Tip:** en el reporte de préstamos usa el rango `2026-09-01` a `2026-09-30` para ver todos los datos de prueba.

---

## ✅ Requisitos cumplidos

- [x] Proyecto único `Biblioteca.Web` con Dapper y Microsoft.Data.SqlClient
- [x] Cadena de conexión en `appsettings.json`, leída con `IConfiguration`
- [x] Models con DataAnnotations (`[Required]`, `[StringLength]`, `[Range]`, `[DataType]`)
- [x] Repositorios con `QueryAsync` / `ExecuteAsync` y procedimientos almacenados
- [x] Repositorios registrados en `Program.cs` e inyectados por constructor
- [x] CRUD de libros con lista desplegable de autores
- [x] Patrón Post/Redirect/Get con `TempData`
- [x] Validación de DNI duplicado con `ModelState.AddModelError`
- [x] Acciones `async Task<IActionResult>` de punta a punta
- [x] Menú en `_Layout.cshtml`
- [x] Vista parcial para la fila de libro (punto extra)

---

## 👩‍💻 Autora

**Yamile Ochoa**

---

<div align="center">

⭐ Hecho con ASP.NET Core MVC, Dapper y mucho café ☕

</div>
