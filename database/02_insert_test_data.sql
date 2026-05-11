-- Script para insertar datos de prueba
-- Estos datos ayudarán a identificar problemas de performance y funcionalidad

USE IncidentDB;
GO

-- Insertar usuarios de prueba
INSERT INTO Users (Username, Email) VALUES 
('admin', 'admin@techsolutions.com'),
('user1', 'user1@techsolutions.com'),
('user2', 'user2@techsolutions.com'),
('support1', 'support1@techsolutions.com'),
('support2', 'support2@techsolutions.com');

-- Insertar categorías de prueba
INSERT INTO Categories (Name, Description) VALUES 
('Hardware', 'Problemas relacionados con equipos físicos'),
('Software', 'Problemas relacionados con aplicaciones y sistemas'),
('Network', 'Problemas de conectividad y red'),
('Database', 'Problemas relacionados con bases de datos'),
('Security', 'Problemas de seguridad y acceso');

-- Insertar incidentes de prueba (esto ayudará a identificar problemas de performance)
INSERT INTO Incidents (Title, Description, Status, Priority, UserId, CategoryId, AssignedTo) VALUES 
('PC no enciende', 'La computadora no responde al botón de encendido, verificar fuente de poder', 'Open', 'High', 2, 1, 4),
('Error en login del sistema', 'No puedo acceder al sistema, aparece error de credenciales inválidas', 'In Progress', 'Medium', 3, 2, 4),
('Internet lento en oficina norte', 'La conexión está muy lenta en toda la oficina norte, verificar switch principal', 'Open', 'Low', 2, 3, 5),
('Base de datos no responde', 'El servidor de base de datos no responde a las consultas, verificar servicios', 'Open', 'Critical', 2, 4, 4),
('Acceso denegado a archivos', 'No puedo acceder a archivos compartidos, verificar permisos de usuario', 'In Progress', 'Medium', 3, 5, 5),
('Impresora no imprime', 'La impresora HP LaserJet no responde a comandos de impresión', 'Open', 'Medium', 2, 1, 4),
('Sistema lento', 'El sistema de gestión está muy lento, verificar recursos del servidor', 'In Progress', 'High', 3, 2, 5);

-- Insertar más incidentes para probar performance con grandes volúmenes
-- BUG: Estos datos adicionales expondrán problemas de consultas N+1 y falta de paginación
DECLARE @i INT = 8;
WHILE @i <= 100
BEGIN
    INSERT INTO Incidents (Title, Description, Status, Priority, UserId, CategoryId, AssignedTo)
    VALUES 
    ('Incidente de prueba ' + CAST(@i AS VARCHAR), 'Descripción del incidente ' + CAST(@i AS VARCHAR), 
     CASE WHEN @i % 4 = 0 THEN 'Open' WHEN @i % 4 = 1 THEN 'In Progress' WHEN @i % 4 = 2 THEN 'Closed' ELSE 'Open' END,
     CASE WHEN @i % 4 = 0 THEN 'Low' WHEN @i % 4 = 1 THEN 'Medium' WHEN @i % 4 = 2 THEN 'High' ELSE 'Medium' END,
     (@i % 3) + 2, (@i % 5) + 1, (@i % 2) + 4);
    SET @i = @i + 1;
END
