
-- 10%

CREATE DATABASE MesaAyudaDB;
GO

USE MesaAyudaDB;
GO

-- Tabla de usuarios (quien reporta el ticket)
CREATE TABLE Usuarios (
    UsuarioID INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100),
    Email NVARCHAR(100),
    FechaCreacion DATETIME DEFAULT GETDATE()
);

-- Tabla de agentes (quien atiende)
CREATE TABLE Agentes (
    AgenteID INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100),
    NivelSoporte INT, -- 1, 2, 3
    Activo BIT DEFAULT 1
);

-- Tabla de tickets
CREATE TABLE Tickets (
    TicketID INT IDENTITY(1,1) PRIMARY KEY,
    UsuarioID INT,
    AgenteID INT,
    Titulo NVARCHAR(200),
    Descripcion NVARCHAR(MAX),
    Estado NVARCHAR(50), -- Abierto, En Proceso, Cerrado
    FechaCreacion DATETIME DEFAULT GETDATE(),

    CONSTRAINT FK_Tickets_Usuarios FOREIGN KEY (UsuarioID)
        REFERENCES Usuarios(UsuarioID),

    CONSTRAINT FK_Tickets_Agentes FOREIGN KEY (AgenteID)
        REFERENCES Agentes(AgenteID)
);



-- Insertar Usuarios (100)
DECLARE @i INT = 1;

WHILE @i <= 100
BEGIN
    INSERT INTO Usuarios (Nombre, Email)
    VALUES (
        CONCAT('Usuario ', @i),
        CONCAT('usuario', @i, '@correo.com')
    );

    SET @i = @i + 1;
END;

-- Insertar Agentes (100)
SET @i = 1;

WHILE @i <= 100
BEGIN
    INSERT INTO Agentes (Nombre, NivelSoporte)
    VALUES (
        CONCAT('Agente ', @i),
        (ABS(CHECKSUM(NEWID())) % 3) + 1
    );

    SET @i = @i + 1;
END;

-- Insertar Tickets (100)
SET @i = 1;

WHILE @i <= 100
BEGIN
    INSERT INTO Tickets (UsuarioID, AgenteID, Titulo, Descripcion, Estado, FechaCreacion)
    VALUES (
        (ABS(CHECKSUM(NEWID())) % 100) + 1,
        (ABS(CHECKSUM(NEWID())) % 100) + 1,
        CONCAT('Problema ', @i),
        CONCAT('Descripción del problema ', @i),
        CASE (ABS(CHECKSUM(NEWID())) % 3)
            WHEN 0 THEN 'Abierto'
            WHEN 1 THEN 'En Proceso'
            ELSE 'Cerrado'
        END,
        dateadd(day, -(@i+30), Getdate())
    );

    SET @i = @i + 1;
END;