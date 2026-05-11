# CÓDIGO CON BUGS - SISTEMA DE GESTIÓN DE INCIDENTES

## DESCRIPCIÓN
Este proyecto contiene un sistema de gestión de incidentes desarrollado en .NET Core 8 con múltiples bugs y problemas de rendimiento que el candidato debe identificar y corregir.

## PROBLEMAS REPORTADOS
1. **Incidente Crítico**: Los usuarios reportan que el sistema tarda más de 30 segundos en cargar la lista de incidentes
2. **Error de Funcionalidad**: Al intentar crear un nuevo incidente, el sistema muestra error "500 Internal Server Error"
3. **Problema de Datos**: Los incidentes no se están cerrando correctamente, quedando en estado "En Proceso" indefinidamente
4. **Solicitud de Mejora**: Implementar un dashboard que muestre métricas de incidentes por estado

## BUGS IMPLEMENTADOS INTENCIONALMENTE

### 1. PROBLEMAS DE PERFORMANCE
- Consultas N+1 en IncidentService
- Falta de índices en base de datos
- No hay paginación en listados
- Falta de Include() en consultas Entity Framework

### 2. PROBLEMAS DE VALIDACIÓN
- Enums no validados (Status, Priority)
- Falta de validaciones de negocio

### 3. PROBLEMAS DE MANEJO DE ERRORES
- Excepciones genéricas (500 Internal Server Error)
- Falta de logging estructurado
- No hay transacciones en operaciones críticas
- Manejo pobre de errores de base de datos

### 4. PROBLEMAS DE CONFIGURACIÓN
- Servicios no registrados correctamente
- No hay configuración de logging

### 5. PROBLEMAS DE ARQUITECTURA
- Falta de navegación inversa en modelos
- No hay auditoría de cambios
- Falta de manejo de permisos
- No hay caché para métricas

## INSTRUCCIONES PARA EL CANDIDATO
1. Analizar el código fuente para identificar los bugs
2. Corregir todos los problemas encontrados
3. Implementar las mejoras solicitadas
4. Documentar las correcciones realizadas
5. Presentar la solución funcionando

## ESTRUCTURA DEL PROYECTO
```
codigo_con_bugs/
├── Controllers/
│   ├── IncidentController.cs
│   └── DashboardController.cs
├── Models/
│   ├── Incident.cs
│   ├── User.cs
│   └── Category.cs
├── Services/
│   ├── IncidentService.cs
│   ├── DatabaseService.cs
│   └── IIncidentService.cs
├── Data/
│   └── ApplicationDbContext.cs
├── DTOs/
│   ├── IncidentDto.cs
│   └── DashboardMetricsDto.cs
├── Program.cs
├── appsettings.json
└── IncidentManagementSystem.csproj
```

## BASE DE DATOS
- Scripts SQL en la carpeta `database/`
- Datos de prueba incluidos
- Estructura básica sin optimizaciones

## NOTAS IMPORTANTES
- El código está diseñado para fallar en ciertos escenarios
- Los logs están configurados para mostrar información útil para debugging
- La base de datos usa SQL Server (local o Docker)
- El proyecto debe compilar y ejecutar antes de las correcciones
