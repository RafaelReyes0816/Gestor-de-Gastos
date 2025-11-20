# Instrucciones para Crear la Base de Datos en XAMPP

## Requisitos Previos
- XAMPP instalado y funcionando
- MySQL activo en XAMPP
- phpMyAdmin accesible (normalmente en http://localhost/phpmyadmin)

## Método 1: Usando phpMyAdmin (Recomendado para principiantes)

### Paso 1: Abrir phpMyAdmin
1. Inicia XAMPP Control Panel
2. Asegúrate de que **Apache** y **MySQL** estén corriendo (botones en verde)
3. Haz clic en **Admin** junto a MySQL, o abre en tu navegador: `http://localhost/phpmyadmin`

### Paso 2: Ejecutar el Script SQL
1. En phpMyAdmin, haz clic en la pestaña **"SQL"** en la parte superior
2. Abre el archivo `Scripts/Database_Completa.sql` con un editor de texto (Notepad++, VS Code, etc.)
3. **Copia todo el contenido** del archivo SQL
4. **Pega el contenido** en el área de texto de phpMyAdmin
5. Haz clic en el botón **"Continuar"** o **"Ejecutar"**

### Paso 3: Verificar
1. En el panel izquierdo, deberías ver la base de datos **"GestorGastos"**
2. Haz clic en ella para ver las tablas:
   - `Usuarios`
   - `Categorias`
   - `Gastos`
3. También deberías ver las vistas (en la pestaña "Vistas"):
   - `vw_usuarios_estadisticas`
   - `vw_gastos_completos`
   - `vw_estadisticas_categorias`
   - `vw_gastos_por_periodo`
   - `vw_gastos_usuario_mes`

## Método 2: Usando la Línea de Comandos (MySQL CLI)

### Paso 1: Abrir MySQL Command Line
1. Abre el símbolo del sistema (CMD) o PowerShell
2. Navega a la carpeta de MySQL de XAMPP:
   ```cmd
   cd C:\xampp\mysql\bin
   ```
   (Ajusta la ruta según donde tengas instalado XAMPP)

### Paso 2: Conectar a MySQL
```cmd
mysql -u root -p
```
- Si no tienes contraseña, presiona Enter
- Si tienes contraseña, ingrésala

### Paso 3: Ejecutar el Script
```cmd
source D:\Gestor-de-Gastos\Scripts\Database_Completa.sql
```
(Ajusta la ruta según donde tengas tu proyecto)

### O ejecutar directamente:
```cmd
mysql -u root -p < D:\Gestor-de-Gastos\Scripts\Database_Completa.sql
```

## Método 3: Usando MySQL Workbench

1. Abre MySQL Workbench
2. Conecta a tu servidor MySQL (localhost, puerto 3306)
3. File → Open SQL Script
4. Selecciona el archivo `Scripts/Database_Completa.sql`
5. Haz clic en el botón de ejecutar (⚡) o presiona `Ctrl+Shift+Enter`

## Verificar la Conexión desde la Aplicación

Después de crear la base de datos, verifica que tu archivo `appsettings.json` tenga la cadena de conexión correcta:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=GestorGastos;User=root;Password=;Port=3306;CharSet=utf8mb4;"
  }
}
```

**Nota:** Si tu MySQL tiene contraseña, agrega `Password=tu_contraseña;` en la cadena de conexión.

## Credenciales de Prueba

Una vez creada la base de datos, puedes usar estas credenciales para iniciar sesión:

### Administrador
- **Usuario:** `rafael.reyes`
- **Contraseña:** `Admin2024!`

### Usuario Regular 1
- **Usuario:** `david.cruz`
- **Contraseña:** `David2024!`

### Usuario Regular 2
- **Usuario:** `ivan.orellana`
- **Contraseña:** `Ivan2024!`

## Solución de Problemas

### Error: "Access denied for user 'root'@'localhost'"
- Verifica que MySQL esté corriendo en XAMPP
- Intenta con contraseña vacía o la contraseña que configuraste

### Error: "Database already exists"
- El script usa `CREATE DATABASE IF NOT EXISTS`, así que no debería dar error
- Si quieres empezar desde cero, descomenta la línea `DROP DATABASE IF EXISTS GestorGastos;` al inicio del script

### Error: "Table already exists"
- El script usa `CREATE TABLE IF NOT EXISTS`, así que no debería dar error
- Si quieres recrear las tablas, primero elimina la base de datos y vuelve a ejecutar el script

### Error: "Foreign key constraint fails"
- Asegúrate de que las tablas se creen en el orden correcto (Usuarios → Categorias → Gastos)
- El script ya está en el orden correcto

## Estructura de la Base de Datos

### Tablas
- **Usuarios:** Almacena información de usuarios y administradores
- **Categorias:** Categorías de gastos (Alimentación, Transporte, etc.)
- **Gastos:** Registro de gastos de los usuarios

### Vistas
- **vw_usuarios_estadisticas:** Resumen de cada usuario con estadísticas
- **vw_gastos_completos:** Gastos con información completa
- **vw_estadisticas_categorias:** Estadísticas por categoría
- **vw_gastos_por_periodo:** Gastos con campos calculados para filtros
- **vw_gastos_usuario_mes:** Resumen mensual por usuario

## Categorías Incluidas

El script incluye 9 categorías predefinidas:
1. Alimentación (Rojo)
2. Transporte (Turquesa)
3. Servicios (Azul)
4. Entretenimiento (Salmón)
5. Salud (Verde claro)
6. Educación (Amarillo)
7. Ropa (Morado)
8. Hogar (Azul claro)
9. Otros (Gris)

¡Listo! Tu base de datos está configurada y lista para usar. 🎉

