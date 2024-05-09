
INSERT INTO usuarios ( Login, Nombre, Paterno, Materno)--inserta los datos proporcionados en el archivo de excel de la hoja usuarios
SELECT  Login, Nombres, Paterno, Materno
FROM OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 12.0;Database=C:\Users\Mauricio\Downloads\DatosPracticaSQL.xlsx;HDR=YES;IMEX=1',
                'SELECT * FROM [Info usuarios$]');
GO





INSERT INTO empleados ( Login, Sueldo, FechaIngreso)--inserta los datos proporcionados en el archivo de excel de la hoja empleados 
SELECT Login, Sueldo, FechaIngreso
FROM OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 12.0;Database=C:\Users\Mauricio\Downloads\DatosPracticaSQL.xlsx;HDR=YES;IMEX=1',
                'SELECT * FROM [Info empleadosm$]');
GO


SELECT * 
FROM usuarios
WHERE ID NOT IN (6, 7, 9, 10);--Depurar solo los ID diferentes de 6,7,9 y 10 de la tabla usuarios


UPDATE empleados--Actualizar el dato Sueldo en un 10 porciento a los empleados que tienen fechas entre el año 2000 y 2001 
SET Sueldo = Sueldo * 1.1
WHERE YEAR(Fecha) BETWEEN 2000 AND 2001;


SELECT NombreUsuario, FechaIngreso--Realiza una consulta para traer el nombre de usuario y fecha de ingreso de los usuarios que gananen mas de 10000 y su apellido comience con T ordernado del mas reciente al mas antiguo 
FROM usuarios
WHERE Sueldo > 10000 AND Apellido LIKE 'T%'
ORDER BY FechaIngreso DESC;



SELECT --Realiza una consulta donde agrupes a los empleados por sueldo, un grupo con los que ganan menos de 1200 y uno mayor o igual a 1200, 
    CASE 
        WHEN Sueldo < 1200 THEN 'Menor de 1200'
        ELSE '1200 o más'
    END AS GrupoSueldo,
    COUNT(*) AS CantidadEmpleados
FROM empleados
GROUP BY 
    CASE 
        WHEN Sueldo < 1200 THEN 'Menor de 1200'
        ELSE '1200 o más'
    END;
