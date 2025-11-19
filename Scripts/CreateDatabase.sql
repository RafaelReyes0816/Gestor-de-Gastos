-- Crear base de datos
CREATE DATABASE IF NOT EXISTS GestorGastos
CHARACTER SET utf8mb4
COLLATE utf8mb4_unicode_ci;

USE GestorGastos;

-- Tabla: Usuarios
CREATE TABLE IF NOT EXISTS Usuarios (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Username VARCHAR(100) NOT NULL UNIQUE,
    Password VARCHAR(255) NOT NULL,
    Email VARCHAR(255) NOT NULL UNIQUE,
    Nombre VARCHAR(100) NOT NULL,
    Apellido VARCHAR(100) NOT NULL,
    Rol VARCHAR(50) NOT NULL,
    FechaCreacion DATETIME NOT NULL,
    Activo BIT DEFAULT 1,
    INDEX idx_username (Username),
    INDEX idx_email (Email)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Tabla: Categorias
CREATE TABLE IF NOT EXISTS Categorias (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(255),
    Color VARCHAR(7),
    Activo BIT DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Tabla: Gastos
CREATE TABLE IF NOT EXISTS Gastos (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    UsuarioId INT NOT NULL,
    CategoriaId INT NOT NULL,
    Monto DECIMAL(18,2) NOT NULL,
    Descripcion VARCHAR(500),
    FechaGasto DATETIME NOT NULL,
    FechaCreacion DATETIME NOT NULL,
    FechaModificacion DATETIME,
    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id) ON DELETE RESTRICT,
    FOREIGN KEY (CategoriaId) REFERENCES Categorias(Id) ON DELETE RESTRICT,
    INDEX idx_usuario_id (UsuarioId),
    INDEX idx_fecha_gasto (FechaGasto),
    INDEX idx_categoria_id (CategoriaId)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- VISTAS
-- Vista: Usuarios con estadísticas de gastos
-- Útil para el administrador para ver resumen de cada usuario
CREATE OR REPLACE VIEW vw_usuarios_estadisticas AS
SELECT 
    u.Id,
    u.Username,
    u.Email,
    u.Nombre,
    u.Apellido,
    CONCAT(u.Nombre, ' ', u.Apellido) AS NombreCompleto,
    u.Rol,
    u.FechaCreacion,
    u.Activo,
    COUNT(g.Id) AS TotalGastos,
    COALESCE(SUM(g.Monto), 0) AS TotalMontoGastado,
    COALESCE(AVG(g.Monto), 0) AS PromedioGasto,
    MAX(g.FechaGasto) AS UltimoGasto,
    MIN(g.FechaGasto) AS PrimerGasto
FROM Usuarios u
LEFT JOIN Gastos g ON u.Id = g.UsuarioId
GROUP BY u.Id, u.Username, u.Email, u.Nombre, u.Apellido, u.Rol, u.FechaCreacion, u.Activo;

-- Vista: Gastos con información completa
CREATE OR REPLACE VIEW vw_gastos_completos AS
SELECT 
    g.Id,
    g.UsuarioId,
    u.Username AS UsuarioUsername,
    CONCAT(u.Nombre, ' ', u.Apellido) AS UsuarioNombreCompleto,
    u.Email AS UsuarioEmail,
    g.CategoriaId,
    c.Nombre AS CategoriaNombre,
    c.Color AS CategoriaColor,
    g.Monto,
    g.Descripcion,
    g.FechaGasto,
    g.FechaCreacion,
    g.FechaModificacion,
    DATE(g.FechaGasto) AS FechaGastoDate,
    YEAR(g.FechaGasto) AS AnioGasto,
    MONTH(g.FechaGasto) AS MesGasto,
    WEEK(g.FechaGasto) AS SemanaGasto,
    DAY(g.FechaGasto) AS DiaGasto
FROM Gastos g
INNER JOIN Usuarios u ON g.UsuarioId = u.Id
INNER JOIN Categorias c ON g.CategoriaId = c.Id;

-- Vista: Estadísticas de gastos por categoría
-- Útil para reportes y gráficos
CREATE OR REPLACE VIEW vw_estadisticas_categorias AS
SELECT 
    c.Id AS CategoriaId,
    c.Nombre AS CategoriaNombre,
    c.Color AS CategoriaColor,
    COUNT(g.Id) AS TotalGastos,
    COALESCE(SUM(g.Monto), 0) AS TotalMonto,
    COALESCE(AVG(g.Monto), 0) AS PromedioMonto,
    COALESCE(MAX(g.Monto), 0) AS MontoMaximo,
    COALESCE(MIN(g.Monto), 0) AS MontoMinimo,
    COUNT(DISTINCT g.UsuarioId) AS UsuariosQueUsan
FROM Categorias c
LEFT JOIN Gastos g ON c.Id = g.CategoriaId
WHERE c.Activo = 1
GROUP BY c.Id, c.Nombre, c.Color;

-- Vista: Gastos por período (útil para filtros diario, semanal, mensual, anual)
CREATE OR REPLACE VIEW vw_gastos_por_periodo AS
SELECT 
    g.Id,
    g.UsuarioId,
    u.Username AS UsuarioUsername,
    CONCAT(u.Nombre, ' ', u.Apellido) AS UsuarioNombreCompleto,
    g.CategoriaId,
    c.Nombre AS CategoriaNombre,
    g.Monto,
    g.Descripcion,
    g.FechaGasto,
    DATE(g.FechaGasto) AS FechaGastoDate,
    YEAR(g.FechaGasto) AS Anio,
    MONTH(g.FechaGasto) AS Mes,
    WEEK(g.FechaGasto) AS Semana,
    DAY(g.FechaGasto) AS Dia,
    DAYNAME(g.FechaGasto) AS DiaSemana,
    MONTHNAME(g.FechaGasto) AS NombreMes
FROM Gastos g
INNER JOIN Usuarios u ON g.UsuarioId = u.Id
INNER JOIN Categorias c ON g.CategoriaId = c.Id;

-- Vista: Resumen de gastos por usuario y mes
-- Útil para comparativas mensuales
CREATE OR REPLACE VIEW vw_gastos_usuario_mes AS
SELECT 
    g.UsuarioId,
    u.Username,
    CONCAT(u.Nombre, ' ', u.Apellido) AS NombreCompleto,
    YEAR(g.FechaGasto) AS Anio,
    MONTH(g.FechaGasto) AS Mes,
    COUNT(g.Id) AS TotalGastos,
    SUM(g.Monto) AS TotalMonto,
    AVG(g.Monto) AS PromedioMonto
FROM Gastos g
INNER JOIN Usuarios u ON g.UsuarioId = u.Id
GROUP BY g.UsuarioId, u.Username, u.Nombre, u.Apellido, YEAR(g.FechaGasto), MONTH(g.FechaGasto);
