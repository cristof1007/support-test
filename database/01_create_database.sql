-- Script de creación de base de datos para el sistema de gestión de incidentes
-- BUGS INTENCIONALES: Falta de índices, constraints y optimizaciones

-- Crear base de datos
CREATE DATABASE IncidentDB;
GO

USE IncidentDB;
GO

-- Crear tabla Users
CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) NOT NULL,
    IsActive BIT DEFAULT 1
    -- BUG: Falta UNIQUE constraint en Username y Email
    -- BUG: Falta campo CreatedDate para auditoría
);

-- Crear tabla Categories
CREATE TABLE Categories (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500),
    IsActive BIT DEFAULT 1
    -- BUG: Falta UNIQUE constraint en Name
    -- BUG: Falta campo CreatedDate para auditoría
);

-- Crear tabla Incidents
CREATE TABLE Incidents (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(1000),
    Status NVARCHAR(50) DEFAULT 'Open',
    Priority NVARCHAR(20) DEFAULT 'Medium',
    UserId INT NOT NULL,
    CategoryId INT NOT NULL,
    CreatedDate DATETIME2 DEFAULT GETUTCDATE(),
    ClosedDate DATETIME2 NULL,
    AssignedTo INT NULL
  
    -- BUG: Falta campo ModifiedDate para auditoría
);

