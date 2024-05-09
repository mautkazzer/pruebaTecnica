-- Create a new database called 'nuxiba'
-- Connect to the 'master' database to run this snippet
USE master
GO
-- Create the new database if it does not exist already
IF NOT EXISTS (
    SELECT name
        FROM sys.databases
        WHERE name = N'nuxiba'
)
CREATE DATABASE nuxiba
GO



USE nuxiba;
GO

-- Crear la tabla usuarios
CREATE TABLE usuarios (
    userId INT PRIMARY KEY,
    Login VARCHAR(100),
    Nombre VARCHAR(100),
    Paterno VARCHAR(100),
    Materno VARCHAR(100)
);
GO

-- Crear la tabla empleados con la relación a la tabla usuarios
CREATE TABLE empleados (
    userId INT PRIMARY KEY,
    Sueldo FLOAT,
    FechaIngreso DATE,
    FOREIGN KEY (userId) REFERENCES usuarios(userId)
);
GO