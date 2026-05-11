# CONFIGURACIÓN DE BASE DE DATOS

## OPCIÓN 1: DOCKER (RECOMENDADO)

### 1. Ejecutar SQL Server en Docker
```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=YourStrong@Passw0rd" \
  -p 1433:1433 --name sqlserver --hostname sqlserver \
  -d mcr.microsoft.com/mssql/server:2022-latest
```

### 2. Conectar a la base de datos
```bash
docker exec -it sqlserver /opt/mssql-tools/bin/sqlcmd \
  -S localhost,1433 -U sa -P YourStrong@Passw0rd
```

### 3. Ejecutar scripts de base de datos
```sql
-- Ejecutar en orden:
-- 01_create_database.sql
-- 02_insert_test_data.sql
```

## OPCIÓN 2: SQL SERVER LOCAL

### 1. Instalar SQL Server Express o Developer Edition
- Descargar desde: https://www.microsoft.com/en-us/sql-server/sql-server-downloads
- Usar autenticación de Windows

### 2. Conectar usando SQL Server Management Studio (SSMS)
- Server: `localhost` o `.\SQLEXPRESS`
- Authentication: Windows Authentication

### 3. Ejecutar scripts de base de datos
- Abrir y ejecutar `01_create_database.sql`
- Abrir y ejecutar `02_insert_test_data.sql`

## OPCIÓN 3: CONEXIÓN PROPORCIONADA POR EL EVALUADOR

Si el evaluador proporciona una conexión específica:
1. Actualizar `appsettings.json` con la connection string proporcionada
2. Verificar conectividad antes de comenzar la prueba
3. Los scripts de base de datos ya estarán ejecutados

## VERIFICACIÓN DE CONECTIVIDAD

### 1. Probar conexión desde la aplicación
```bash
dotnet run
```

### 2. Verificar en Swagger UI
- Abrir: http://localhost:5000/swagger
- Probar endpoint GET /api/Incident

### 3. Verificar logs de la aplicación
- Los logs mostrarán errores de conexión si hay problemas

## NOTAS IMPORTANTES

- **Connection String**: Verificar que coincida con la configuración elegida
- **Puerto**: SQL Server por defecto usa puerto 1433
- **Firewall**: Asegurar que el puerto esté abierto
- **TrustServerCertificate**: Necesario para conexiones locales sin certificados válidos
- **Datos de Prueba**: Los scripts insertan 100+ incidentes para probar performance

## SOLUCIÓN DE PROBLEMAS

### Error: "Login failed for user"
- Verificar credenciales
- Usar autenticación de Windows si es local

### Error: "Server not found"
- Verificar que SQL Server esté ejecutándose
- Verificar puerto y nombre del servidor

### Error: "Database does not exist"
- Ejecutar primero `01_create_database.sql`
- Verificar nombre de la base de datos en connection string
