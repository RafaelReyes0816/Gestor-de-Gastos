
USE GestorGastos;

-- Rafael Reyes (Administrador)
-- Contraseña: Admin2024!
INSERT INTO Usuarios (Username, Password, Email, Nombre, Apellido, Rol, FechaCreacion, Activo)
VALUES ('rafael.reyes', '$2a$11$JUFJzqLUL2M/NfZ5BwJFwe.OxnR.vdpUHERlZPsDaCCPPwfxdkHF6', 'rafael.reyes@example.com', 'Rafael', 'Reyes', 'Administrador', NOW(), 1);

-- David Cruz (Usuario)
-- Contraseña: David2024!
INSERT INTO Usuarios (Username, Password, Email, Nombre, Apellido, Rol, FechaCreacion, Activo)
VALUES ('david.cruz', '$2a$11$3Srb7xxSd5Jvn1NqFdVBlupHRqpaIc9tLt0xuF6np26GgCdlMWjvu', 'david.cruz@example.com', 'David', 'Cruz', 'Usuario', NOW(), 1);

-- Iván Orellana (Usuario)
-- Contraseña: Ivan2024!
INSERT INTO Usuarios (Username, Password, Email, Nombre, Apellido, Rol, FechaCreacion, Activo)
VALUES ('ivan.orellana', '$2a$11$fuWWm5lZ39ZdzX5JnlgsEOEZ9MoDzUwshQoPlKXKZo9qnzvQush2O', 'ivan.orellana@example.com', 'Iván', 'Orellana', 'Usuario', NOW(), 1);

-- NOTA: Las contraseñas y usuarios son:
-- rafael.reyes: Admin2024!
-- david.cruz: David2024!
-- ivan.orellana: Ivan2024!