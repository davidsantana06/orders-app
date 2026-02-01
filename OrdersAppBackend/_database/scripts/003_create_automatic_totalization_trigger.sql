USE OrdersAppDatabase;
GO

IF EXISTS (SELECT * FROM sys.triggers WHERE name = 'trg_UpdateOrderTotalValue')
BEGIN
    DROP TRIGGER trg_UpdateOrderTotalValue;
    PRINT 'Trigger trg_UpdateOrderTotalValue dropped for recreation.';
END
GO

CREATE TRIGGER trg_UpdateOrderTotalValue
ON OrderItems
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @AffectedOrders TABLE (OrderId INT);
    
    -- Get OrderIds from inserted records (INSERT/UPDATE)
    INSERT INTO @AffectedOrders (OrderId)
    SELECT DISTINCT OrderId FROM inserted;
    
    -- Get OrderIds from deleted records (DELETE/UPDATE)
    INSERT INTO @AffectedOrders (OrderId)
    SELECT DISTINCT OrderId FROM deleted
    WHERE OrderId NOT IN (SELECT OrderId FROM @AffectedOrders);
    
    -- Update TotalValue and UpdatedAt for all affected Orders
    UPDATE o
    SET 
        o.TotalValue = ISNULL((
            SELECT SUM(oi.Quantity * oi.UnitPrice)
            FROM OrderItems oi
            WHERE oi.OrderId = o.Id
        ), 0),
        o.UpdatedAt = GETDATE()
    FROM Orders o
    INNER JOIN @AffectedOrders ao ON o.Id = ao.OrderId;
END
GO

PRINT 'Trigger trg_UpdateOrderTotalValue created successfully.';
GO
