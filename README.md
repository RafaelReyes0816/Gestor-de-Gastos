# Sistema Gestor de Gastos

Aplicación web desarrollada con ASP.NET MVC Core 8.0 para la gestión de gastos personales. Permite a los usuarios registrar, organizar y analizar sus gastos según diferentes períodos de tiempo (diario, semanal, mensual, anual), con un panel de administración para supervisión de usuarios.

## Tecnologías Utilizadas

- **Backend:**
  - ASP.NET MVC Core 8.0
  - C# 12
  - Entity Framework Core 8.0
  - Pomelo.EntityFrameworkCore.MySql 8.0
  - BCrypt.Net-Next (para hash de contraseñas)

- **Base de Datos:**
  - MySQL 8.0+

- **Frontend:**
  - Bootstrap 5
  - jQuery 3.x
  - Chart.js 4.4.0
  - HTML5, CSS3

## Características Principales

### Para Usuarios
- Autenticación segura con hash BCrypt
- CRUD completo de gastos
- Filtros por período (diario, semanal, mensual, anual)
- Sistema de categorización de gastos
- Dashboard interactivo con gráficos
- Estadísticas y reportes de gastos
- Visualización de gastos por categoría

### Para Administradores
- Panel de administración
- Vista de todos los usuarios registrados
- Detalle de gastos por usuario
- Estadísticas generales del sistema
- Análisis de uso del sistema

## Requisitos Previos

- .NET 8.0 SDK
- MySQL 8.0 o superior
- Visual Studio 2022 o VS Code (recomendado)

## Instalación y Configuración

### 1. Clonar el repositorio
```bash
git clone [url-del-repositorio]
cd Gestor-Gastos
```

### 2. Configurar la base de datos

Ejecutar los scripts SQL en el siguiente orden:

1. **Crear base de datos y tablas:**
   ```bash
   mysql -u root -p < Scripts/CreateDatabase.sql
   ```

2. **Insertar usuarios iniciales (opcional):**
   ```bash
   mysql -u root -p < Scripts/InsertarUsuariosEjemplo.sql
   ```

### 3. Configurar la cadena de conexión

Editar `appsettings.json` con tus credenciales de MySQL:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=GestorGastos;User=root;Password=tu_password;Port=3306;"
  }
}
```

### 4. Restaurar paquetes NuGet
```bash
dotnet restore
```

### 5. Ejecutar el proyecto
```bash
dotnet run
```

La aplicación estará disponible en `http://localhost:5030`

## Usuarios de Prueba

Si ejecutaste el script `InsertarUsuariosEjemplo.sql`, puedes usar estas credenciales:

**Administrador:**
- Usuario: `rafael.reyes`
- Contraseña: `Admin2024!`

**Usuarios:**
- Usuario: `david.cruz` / Contraseña: `David2024!`
- Usuario: `ivan.orellana` / Contraseña: `Ivan2024!`

## Estructura del Proyecto

```
Gestor-Gastos/
├── Controllers/          # Controladores MVC
│   ├── AccountController.cs
│   ├── AdminController.cs
│   ├── CategoriasController.cs
│   ├── GastosController.cs
│   └── HomeController.cs
├── Models/              # Modelos de entidad y ViewModels
│   ├── Categoria.cs
│   ├── Gasto.cs
│   ├── Usuario.cs
│   └── ViewModels/
├── Views/               # Vistas Razor
│   ├── Account/
│   ├── Admin/
│   ├── Categorias/
│   ├── Gastos/
│   ├── Home/
│   └── Shared/
├── Data/                # DbContext
│   └── ApplicationDbContext.cs
├── Helpers/             # Helpers personalizados
│   └── AuthorizeAttribute.cs
├── Scripts/             # Scripts SQL
│   ├── CreateDatabase.sql
│   └── InsertarUsuariosEjemplo.sql
├── wwwroot/             # Archivos estáticos
│   ├── css/
│   ├── js/
│   └── lib/
└── Program.cs           # Configuración principal
```

## Base de Datos

### Tablas Principales
- **Usuarios**: Información de usuarios y administradores
- **Categorias**: Categorías de gastos
- **Gastos**: Registro de gastos de usuarios

### Vistas Optimizadas
El proyecto incluye 5 vistas SQL para optimizar consultas:
- `vw_usuarios_estadisticas`: Estadísticas por usuario
- `vw_gastos_completos`: Gastos con información completa
- `vw_estadisticas_categorias`: Estadísticas por categoría
- `vw_gastos_por_periodo`: Gastos con campos calculados para filtros
- `vw_gastos_usuario_mes`: Resumen mensual por usuario

## Seguridad

- Contraseñas hasheadas con BCrypt
- Autenticación basada en sesiones
- Autorización por roles (Usuario/Administrador)
- Validación de datos en cliente y servidor
- Protección contra inyección SQL mediante Entity Framework Core

## Funcionalidades del Dashboard

- Resumen de gastos por período (día, semana, mes, año)
- Gráfico de pastel: Gastos por categoría
- Gráfico de línea: Evolución de gastos (últimos 6 meses)
- Tabla de resumen por categoría
- Lista de últimos gastos registrados
- Estadísticas generales (promedio, máximo, mínimo)

## Desarrollo

### Tecnologías y Patrones
- **Arquitectura**: MVC (Model-View-Controller)
- **ORM**: Entity Framework Core
- **Validación**: Data Annotations y jQuery Validation
- **Autenticación**: Sesiones con verificación de roles

### Características Técnicas
- Separación de responsabilidades por capas
- ViewModels para transferencia de datos
- Helpers personalizados para autorización
- Vistas SQL para optimización de consultas
- Diseño responsive con Bootstrap 5

## Notas

- Las categorías son compartidas entre todos los usuarios
- Los usuarios solo pueden ver y gestionar sus propios gastos
- Los administradores tienen acceso completo al sistema
- El proyecto no utiliza migraciones de Entity Framework, se usan scripts SQL directos

## Licencia

Este proyecto es de uso educativo.

## Autores

Grupo-01

---

**Desarrollado con ASP.NET MVC Core 8.0**

