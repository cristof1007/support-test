------------------------------------------
-- (50%)(
-- Los usuarios informan que al consultar los tickets por el reporte se generan duplicados
-- adicional el usuario informa que el campo porcentaje de carga esta calculando mal
-- Validas y encuentras que este SP genera el reporte.
-- exec ObtenerTicketsAvanzado

CREATE or alter PROCEDURE ObtenerTicketsAvanzado
AS
BEGIN
 SET NOCOUNT ON;

    ;WITH AgenteNivel AS (
        SELECT 
            NivelSoporte,
            COUNT(*) AS CantidadAgentes
        FROM Agentes
        WHERE Activo = 1
        GROUP BY NivelSoporte
    ),

	 TicketsNivel AS
    (
        SELECT 
            a.NivelSoporte,
            COUNT(t.TicketID) AS TotalTicketsNivel
        FROM Tickets t
        INNER JOIN Agentes a
            ON a.AgenteID = t.AgenteID
        GROUP BY a.NivelSoporte
    )

    SELECT 
        t.TicketID,
        t.Titulo,
        t.Estado,
        u.Nombre AS Usuario,
        a.Nombre AS Agente,
        an.CantidadAgentes,
    	   CAST(
            COUNT(t.TicketID) OVER (PARTITION BY a.AgenteID) * 100.0
            / tn.TotalTicketsNivel
            AS DECIMAL(10,2)
        ) AS PorcentajeCarga
    FROM Tickets t
    INNER JOIN Usuarios u 
        ON u.UsuarioID = t.UsuarioID
    INNER JOIN Agentes a 
        ON a.AgenteID = t.AgenteID
    LEFT JOIN AgenteNivel an
        ON an.NivelSoporte = a.NivelSoporte
	LEFT JOIN TicketsNivel tn
        ON tn.NivelSoporte = a.NivelSoporte;
END;