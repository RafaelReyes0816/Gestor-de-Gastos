-- Script para actualizar los hashes de contraseñas de los usuarios
-- Ejecutar este script si hay errores de "Invalid salt version"

USE GestorGastos;

-- Paso 1: Verificar la estructura de la tabla
-- Asegurarse de que el campo Password tenga suficiente longitud (mínimo 255 caracteres)
ALTER TABLE Usuarios MODIFY COLUMN Password VARCHAR(255) NOT NULL;

-- Paso 2: Verificar el estado actual de los hashes
SELECT 
    Username,
    LEFT(Password, 7) AS HashPrefix,
    LENGTH(Password) AS HashLength,
    CASE 
        WHEN Password LIKE '$2a$%' OR Password LIKE '$2b$%' OR Password LIKE '$2y$%' 
        THEN 'Formato válido' 
        ELSE 'Formato inválido - REQUIERE ACTUALIZACIÓN' 
    END AS EstadoHash
FROM Usuarios
WHERE Activo = 1;

-- Paso 3: Actualizar los hashes con valores correctos
-- Hashes generados con BCrypt.Net-Next versión 4.0.3
-- Estos hashes son compatibles con la librería BCrypt.Net-Next

-- Actualizar hash de Rafael Reyes (Administrador)
-- Contraseña: Admin2024!
UPDATE Usuarios 
SET Password = '$2a$11$JUFJzqLUL2M/NfZ5BwJFwe.OxnR.vdpUHERlZPsDaCCPPwfxdkHF6'
WHERE Username = 'rafael.reyes';

-- Actualizar hash de David Cruz (Usuario)
-- Contraseña: David2024!
UPDATE Usuarios 
SET Password = '$2a$11$3Srb7xxSd5Jvn1NqFdVBlupHRqpaIc9tLt0xuF6np26GgCdlMWjvu'
WHERE Username = 'david.cruz';

-- Actualizar hash de Iván Orellana (Usuario)
-- Contraseña: Ivan2024!
UPDATE Usuarios 
SET Password = '$2a$11$fuWWm5lZ39ZdzX5JnlgsEOEZ9MoDzUwshQoPlKXKZo9qnzvQush2O'
WHERE Username = 'ivan.orellana';

-- Paso 4: Verificar que los hashes se actualizaron correctamente
SELECT 
    Username,
    LEFT(Password, 7) AS HashPrefix,
    LENGTH(Password) AS HashLength,
    CASE 
        WHEN Password LIKE '$2a$%' OR Password LIKE '$2b$%' OR Password LIKE '$2y$%' 
        THEN '✓ Formato válido' 
        ELSE '✗ Formato inválido' 
    END AS EstadoHash
FROM Usuarios
WHERE Activo = 1;

-- Paso 5: Verificar que todos los usuarios tienen hashes válidos
SELECT 
    COUNT(*) AS TotalUsuarios,
    SUM(CASE WHEN Password LIKE '$2a$%' OR Password LIKE '$2b$%' OR Password LIKE '$2y$%' THEN 1 ELSE 0 END) AS UsuariosConHashValido,
    SUM(CASE WHEN Password LIKE '$2a$%' OR Password LIKE '$2b$%' OR Password LIKE '$2y$%' THEN 0 ELSE 1 END) AS UsuariosConHashInvalido
FROM Usuarios
WHERE Activo = 1;

