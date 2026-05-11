
-- Realiza las siguientes consultas 1.(15%), 2.(25%)
--------------------------------------------------------------------------
-- 1.  Cantidad de casos en Estado 'En Proceso'por agente Se deben mostrar los campos 
--	   Nombre del agente, estado y la cantidad  

SELECT 
      ag.[Nombre]
      ,[Estado]
     ,count(ti.[TicketID]) as cantidad
  FROM [MesaAyudaDB].[dbo].[Tickets] ti
  inner join [MesaAyudaDB].[dbo].[Agentes] ag on ag.AgenteID = ti.AgenteID
  where Estado ='En Proceso'
  group by  ag.[Nombre],[Estado]
  order by cantidad  desc




--------------------------------------------------------------------------------------
-- 2.  El lider de la mesa de ayuda necesita saber los casos abiertos que sean del año 2025 para gestionarlos con su equipo
--     para esto requiere conocer el nombre del agente y la cantidad de casos con estado abierto para el 2025 para cada uno, 
--	   ordenar la información de mayor a menor y solo mostrar los 10 agentes con mas casos abiertos sin gestionar.
---------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------
 
 SELECT TOP 10
   ag.[Nombre] AS Agente,
    COUNT(ti.[TicketID]) AS CantidadCasosAbiertos
FROM [MesaAyudaDB].[dbo].[Tickets] ti
INNER JOIN [MesaAyudaDB].[dbo].[Agentes] ag
    ON  ag.AgenteID = ti.AgenteID
WHERE 
    ti.Estado = 'Abierto'
    AND YEAR(ti.[FechaCreacion]) = '2025'
GROUP BY 
   ag.[Nombre]
ORDER BY 
    CantidadCasosAbiertos DESC;