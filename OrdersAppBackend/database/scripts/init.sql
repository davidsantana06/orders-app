USE master;
GO

-- Create database
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'OrdersAppDatabase')
BEGIN
    CREATE DATABASE OrdersAppDatabase;
    PRINT 'Database OrdersAppDatabase created successfully.';
END
ELSE
BEGIN
    PRINT 'Database OrdersAppDatabase already exists.';
END
GO

USE OrdersAppDatabase;
GO

-- Create tables
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Orders' AND type = 'U')
BEGIN
    CREATE TABLE Orders (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        TotalValue DECIMAL(18,2) NOT NULL DEFAULT 0,
        Status NVARCHAR(50) NOT NULL DEFAULT 'Solicitado',
        CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
        UpdatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
        CONSTRAINT CHK_Orders_Status CHECK (
            Status IN ('Solicitado', 'Em andamento', 'Concluído', 'Cancelado')
        )
    );
    PRINT 'Table Orders created successfully.';
END
ELSE
BEGIN
    PRINT 'Table Orders already exists.';
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'OrderItems' AND type = 'U')
BEGIN
    CREATE TABLE OrderItems (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        OrderId INT NOT NULL,
        Make NVARCHAR(100) NOT NULL,
        Model NVARCHAR(100) NOT NULL,
        Year INT NOT NULL,
        UnitPrice DECIMAL(18,2) NOT NULL,
        SubTotal DECIMAL(18,2) NULL,
        CONSTRAINT FK_OrderItems_Orders FOREIGN KEY (OrderId) 
            REFERENCES Orders(Id) ON DELETE CASCADE,
        CONSTRAINT CHK_OrderItems_Year CHECK (Year >= 1900 AND Year <= 2100),
        CONSTRAINT CHK_OrderItems_UnitPrice CHECK (UnitPrice >= 0)
    );
    PRINT 'Table OrderItems created successfully.';
END
ELSE
BEGIN
    PRINT 'Table OrderItems already exists.';
END
GO

-- Create trigger for automatic totalization
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
    
    -- Declare variable to store affected OrderIds
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
            SELECT SUM(
                CASE 
                    WHEN oi.SubTotal IS NOT NULL THEN oi.SubTotal
                    ELSE oi.UnitPrice
                END
            )
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

-- Create stored procedure for filtered search
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
            oi.UnitPrice,
            oi.SubTotal
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
        oi.UnitPrice,
        oi.SubTotal
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

-- Seed data (for testing)
IF NOT EXISTS (SELECT 1 FROM Orders)
BEGIN
    -- Sample Order 1
    INSERT INTO Orders (Status, CreatedAt, UpdatedAt)
    VALUES ('Solicitado', GETDATE(), GETDATE());
    
    DECLARE @Order1Id INT = SCOPE_IDENTITY();
    
    INSERT INTO OrderItems (OrderId, Make, Model, Year, UnitPrice)
    VALUES 
        (@Order1Id, 'Toyota', 'Corolla', 2023, 95000.00),
        (@Order1Id, 'Toyota', 'Hilux', 2023, 250000.00);
    
    -- Sample Order 2
    INSERT INTO Orders (Status, CreatedAt, UpdatedAt)
    VALUES ('Em andamento', GETDATE(), GETDATE());
    
    DECLARE @Order2Id INT = SCOPE_IDENTITY();
    
    INSERT INTO OrderItems (OrderId, Make, Model, Year, UnitPrice)
    VALUES 
        (@Order2Id, 'Honda', 'Civic', 2022, 120000.00),
        (@Order2Id, 'Honda', 'HR-V', 2023, 135000.00);
    
    -- Sample Order 3
    INSERT INTO Orders (Status, CreatedAt, UpdatedAt)
    VALUES ('Concluído', GETDATE(), GETDATE());
    
    DECLARE @Order3Id INT = SCOPE_IDENTITY();
    
    INSERT INTO OrderItems (OrderId, Make, Model, Year, UnitPrice)
    VALUES 
        (@Order3Id, 'Ford', 'Ranger', 2023, 280000.00);
    
    PRINT 'Sample data inserted successfully.';
END
ELSE
BEGIN
    PRINT 'Sample data skipped - Orders table already contains data.';
END
GO
