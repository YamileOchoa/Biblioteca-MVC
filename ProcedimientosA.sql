USE BibliotecaDB;
GO

/* ===================== LIBROS ===================== */

CREATE OR ALTER PROCEDURE usp_Libros_Listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT l.LibroId, l.Titulo, l.ISBN, l.AutorId, a.Nombre AS AutorNombre,
           l.Ejemplares, l.Activo
    FROM dbo.Libros l
    INNER JOIN dbo.Autores a ON a.AutorId = l.AutorId
    WHERE l.Activo = 1
    ORDER BY l.Titulo;
END
GO

CREATE OR ALTER PROCEDURE usp_Libros_BuscarPorTitulo
    @Titulo NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT l.LibroId, l.Titulo, l.ISBN, l.AutorId, a.Nombre AS AutorNombre,
           l.Ejemplares, l.Activo
    FROM dbo.Libros l
    INNER JOIN dbo.Autores a ON a.AutorId = l.AutorId
    WHERE l.Activo = 1
      AND l.Titulo LIKE '%' + @Titulo + '%'
    ORDER BY l.Titulo;
END
GO

CREATE OR ALTER PROCEDURE usp_Libros_ObtenerPorId
    @LibroId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT l.LibroId, l.Titulo, l.ISBN, l.AutorId, a.Nombre AS AutorNombre,
           l.Ejemplares, l.Activo
    FROM dbo.Libros l
    INNER JOIN dbo.Autores a ON a.AutorId = l.AutorId
    WHERE l.LibroId = @LibroId;
END
GO

CREATE OR ALTER PROCEDURE usp_Libros_Insertar
    @Titulo     NVARCHAR(200),
    @ISBN       VARCHAR(20),
    @AutorId    INT,
    @Ejemplares INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.Libros (Titulo, ISBN, AutorId, Ejemplares, Activo)
    VALUES (@Titulo, @ISBN, @AutorId, @Ejemplares, 1);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS NuevoId;
END
GO

CREATE OR ALTER PROCEDURE usp_Libros_Actualizar
    @LibroId    INT,
    @Titulo     NVARCHAR(200),
    @ISBN       VARCHAR(20),
    @AutorId    INT,
    @Ejemplares INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Libros
    SET Titulo = @Titulo,
        ISBN = @ISBN,
        AutorId = @AutorId,
        Ejemplares = @Ejemplares
    WHERE LibroId = @LibroId;
END
GO

-- Eliminación LÓGICA (nunca DELETE)
CREATE OR ALTER PROCEDURE usp_Libros_Eliminar
    @LibroId INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Libros SET Activo = 0 WHERE LibroId = @LibroId;
END
GO

/* ===================== SOCIOS ===================== */

CREATE OR ALTER PROCEDURE usp_Socios_Listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT SocioId, DNI, Nombre, Email, Activo
    FROM dbo.Socios
    WHERE Activo = 1
    ORDER BY Nombre;
END
GO

-- Si el DNI existe lanza el error 50001 (lo capturaremos en C#)
CREATE OR ALTER PROCEDURE usp_Socios_Insertar
    @DNI    CHAR(8),
    @Nombre NVARCHAR(100),
    @Email  NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM dbo.Socios WHERE DNI = @DNI)
        THROW 50001, 'Ya existe un socio con ese DNI.', 1;

    INSERT INTO dbo.Socios (DNI, Nombre, Email, Activo)
    VALUES (@DNI, @Nombre, @Email, 1);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS NuevoId;
END
GO

/* ===================== AUTORES ===================== */

CREATE OR ALTER PROCEDURE usp_Autores_ListarActivos
AS
BEGIN
    SET NOCOUNT ON;
    SELECT AutorId, Nombre
    FROM dbo.Autores
    WHERE Activo = 1
    ORDER BY Nombre;
END
GO

/* ===================== REPORTE DE PRÉSTAMOS ===================== */

CREATE OR ALTER PROCEDURE usp_Prestamos_ReportePorFechas
    @Desde DATE,
    @Hasta DATE
AS
BEGIN
    SET NOCOUNT ON;
    SELECT p.PrestamoId,
           s.Nombre AS SocioNombre,
           l.Titulo AS LibroTitulo,
           p.FechaPrestamo,
           p.FechaLimite,
           p.Estado
    FROM dbo.Prestamos p
    INNER JOIN dbo.DetallePrestamo d ON d.PrestamoId = p.PrestamoId
    INNER JOIN dbo.Libros l          ON l.LibroId    = d.LibroId
    INNER JOIN dbo.Socios s          ON s.SocioId    = p.SocioId
    WHERE p.FechaPrestamo BETWEEN @Desde AND @Hasta
    ORDER BY p.FechaPrestamo, p.PrestamoId;
END
GO
