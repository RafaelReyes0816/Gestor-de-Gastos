// Script para generar nuevos hashes de BCrypt
// Ejecutar este código en un proyecto de consola o en LINQPad
// Asegúrate de tener instalado el paquete BCrypt.Net-Next versión 4.0.3

using BCrypt.Net;

Console.WriteLine("=== GENERADOR DE HASHES BCrypt ===\n");

// Contraseñas para los usuarios
string passwordAdmin = "Admin2024!";
string passwordDavid = "David2024!";
string passwordIvan = "Ivan2024!";

// Generar hashes con BCrypt
// Usar enhancedEntropy: false para compatibilidad estándar
string hashAdmin = BCrypt.Net.BCrypt.HashPassword(passwordAdmin, workFactor: 11);
string hashDavid = BCrypt.Net.BCrypt.HashPassword(passwordDavid, workFactor: 11);
string hashIvan = BCrypt.Net.BCrypt.HashPassword(passwordIvan, workFactor: 11);

Console.WriteLine("=== HASHES GENERADOS ===\n");
Console.WriteLine($"Rafael Reyes (Admin) - Contraseña: {passwordAdmin}");
Console.WriteLine($"Hash: {hashAdmin}\n");

Console.WriteLine($"David Cruz (Usuario) - Contraseña: {passwordDavid}");
Console.WriteLine($"Hash: {hashDavid}\n");

Console.WriteLine($"Iván Orellana (Usuario) - Contraseña: {passwordIvan}");
Console.WriteLine($"Hash: {hashIvan}\n");

Console.WriteLine("\n=== SCRIPT SQL PARA ACTUALIZAR ===\n");
Console.WriteLine("USE GestorGastos;\n");
Console.WriteLine($"UPDATE Usuarios SET Password = '{hashAdmin}' WHERE Username = 'rafael.reyes';");
Console.WriteLine($"UPDATE Usuarios SET Password = '{hashDavid}' WHERE Username = 'david.cruz';");
Console.WriteLine($"UPDATE Usuarios SET Password = '{hashIvan}' WHERE Username = 'ivan.orellana';");

