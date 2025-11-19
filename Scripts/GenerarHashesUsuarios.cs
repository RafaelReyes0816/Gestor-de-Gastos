using BCrypt.Net;

// Contraseñas para los usuarios
string passwordAdmin = "Admin2024!";
string passwordDavid = "David2024!";
string passwordIvan = "Ivan2024!";

// Generar hashes
string hashAdmin = BCrypt.Net.BCrypt.HashPassword(passwordAdmin);
string hashDavid = BCrypt.Net.BCrypt.HashPassword(passwordDavid);
string hashIvan = BCrypt.Net.BCrypt.HashPassword(passwordIvan);

Console.WriteLine("=== HASHES GENERADOS ===\n");
Console.WriteLine($"Rafael Reyes (Admin) - Contraseña: {passwordAdmin}");
Console.WriteLine($"Hash: {hashAdmin}\n");

Console.WriteLine($"David Cruz (Usuario) - Contraseña: {passwordDavid}");
Console.WriteLine($"Hash: {hashDavid}\n");

Console.WriteLine($"Iván Orellana (Usuario) - Contraseña: {passwordIvan}");
Console.WriteLine($"Hash: {hashIvan}\n");

