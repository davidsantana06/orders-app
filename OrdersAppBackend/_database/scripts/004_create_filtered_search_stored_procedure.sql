USE OrdersAppDatabase;
GO

IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'sp_GetOrdersWithFilters')
BEGIN
    DROP PROCEDURE sp_GetOrdersWithFilters;
    PRINT 'Stored Procedure sp_GetOrdersWithFilters dropped for recreation.';
END
GO

CREATE PROCEDURE sp_GetOrdersWithFilters
    @Make NVARCHAR(100) = NULL,
    @Model NVARCHAR(100) = NULL,
    @Year INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    -- If no filters provided, return all orders with their items
    IF @Make IS NULL AND @Model IS NULL AND @Year IS NULL
    BEGIN
        SELECT 
            o.Id AS OrderId,
            o.Status,
            o.TotalValue,
            o.CreatedAt AS OrderCreatedAt,
            o.UpdatedAt AS OrderUpdatedAt,
            oi.Id AS ItemId,
            oi.Make,
            oi.Model,
            oi.Year,
            oi.Quantity,
            oi.UnitPrice
        FROM Orders o
        LEFT JOIN OrderItems oi ON o.Id = oi.OrderId
        ORDER BY o.Id DESC, oi.Id;
        RETURN;
    END
    
    -- With filters: return only orders that have at least one matching item
    SELECT DISTINCT
        o.Id AS OrderId,
        o.Status,
        o.TotalValue,
        o.CreatedAt AS OrderCreatedAt,
        o.UpdatedAt AS OrderUpdatedAt,
        oi.Id AS ItemId,
        oi.Make,
        oi.Model,
        oi.Year,
        oi.Quantity,
        oi.UnitPrice
    FROM Orders o
    INNER JOIN OrderItems oi ON o.Id = oi.OrderId
    WHERE 
        (@Make IS NULL OR oi.Make LIKE '%' + @Make + '%')
        AND (@Model IS NULL OR oi.Model LIKE '%' + @Model + '%')
        AND (@Year IS NULL OR oi.Year = @Year)
    ORDER BY o.Id DESC, oi.Id;
END
GO

PRINT 'Stored Procedure sp_GetOrdersWithFilters created successfully.';
GO
