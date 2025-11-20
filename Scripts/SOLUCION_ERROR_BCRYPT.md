# Solución al Error "SaltParseException: Invalid salt version"

## Problema
El error indica que los hashes de contraseñas almacenados en la base de datos no tienen un formato BCrypt válido.

## Posibles Causas
1. Los hashes están truncados en la base de datos
2. Los hashes fueron generados con una versión diferente de BCrypt
3. Los hashes están en texto plano o en otro formato

## Solución

### Opción 1: Actualizar los hashes en la base de datos (Recomendado)

1. **Ejecutar el script SQL de actualización:**
   ```sql
   -- Ejecutar el archivo: Scripts/ActualizarHashesUsuarios.sql
   ```

2. **O ejecutar manualmente estos comandos en MySQL:**
   ```sql
   USE GestorGastos;
   
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
   ```

3. **Verificar que los hashes se actualizaron correctamente:**
   ```sql
   SELECT Username, 
          LEFT(Password, 7) AS HashPrefix, 
          LENGTH(Password) AS HashLength,
          CASE 
              WHEN Password LIKE '$2a$%' OR Password LIKE '$2b$%' OR Password LIKE '$2y$%' 
              THEN 'Formato válido' 
              ELSE 'Formato inválido' 
          END AS EstadoHash
   FROM Usuarios
   WHERE Activo = 1;
   ```

### Opción 2: Generar nuevos hashes

Si los hashes anteriores no funcionan, puedes generar nuevos hashes:

1. **Crear un proyecto de consola temporal** o usar LINQPad
2. **Instalar el paquete:** `BCrypt.Net-Next` versión 4.0.3
3. **Ejecutar el código del archivo:** `Scripts/GenerarHashNuevo.cs`
4. **Copiar los nuevos hashes generados** y actualizar la base de datos

### Opción 3: Verificar la longitud del campo Password

Asegúrate de que el campo `Password` en la tabla `Usuarios` tenga suficiente longitud:

```sql
-- Verificar la longitud actual
SHOW COLUMNS FROM Usuarios LIKE 'Password';

-- Si es necesario, aumentar la longitud
ALTER TABLE Usuarios MODIFY COLUMN Password VARCHAR(255) NOT NULL;
```

## Credenciales de Prueba

Después de actualizar los hashes, puedes usar estas credenciales:

- **Administrador:**
  - Usuario: `rafael.reyes`
  - Contraseña: `Admin2024!`

- **Usuario 1:**
  - Usuario: `david.cruz`
  - Contraseña: `David2024!`

- **Usuario 2:**
  - Usuario: `ivan.orellana`
  - Contraseña: `Ivan2024!`

## Notas Importantes

- Los hashes BCrypt siempre deben empezar con `$2a$`, `$2b$` o `$2y$`
- La longitud típica de un hash BCrypt es de 60 caracteres
- Nunca almacenes contraseñas en texto plano
- El código del `AccountController` ahora maneja estos errores de manera más elegante

