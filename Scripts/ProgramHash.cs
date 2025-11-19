using BCrypt.Net;

// Contraseñas inventadas
string adminPassword = "Admin123!";
string usuario1Password = "Usuario123!";
string usuario2Password = "Usuario456!";

// Generar hashes
string hashAdmin = BCrypt.Net.BCrypt.HashPassword(adminPassword);
string hashUsuario1 = BCrypt.Net.BCrypt.HashPassword(usuario1Password);
string hashUsuario2 = BCrypt.Net.BCrypt.HashPassword(usuario2Password);

Console.WriteLine("=== HASHES GENERADOS ===");
Console.WriteLine($"Admin (Rafael Reyes): {hashAdmin}");
Console.WriteLine($"Usuario 1 (David Cruz): {hashUsuario1}");
Console.WriteLine($"Usuario 2 (Iván Orellana): {hashUsuario2}");

