USE OrdersAppDatabase;
GO

IF NOT EXISTS (SELECT 1 FROM Orders)
BEGIN
    -- Sample Order 1
    INSERT INTO Orders (Status, CreatedAt, UpdatedAt)
    VALUES ('Solicitado', GETDATE(), GETDATE());
    
    DECLARE @Order1Id INT = SCOPE_IDENTITY();
    
    INSERT INTO OrderItems (OrderId, Make, Model, Year, Quantity, UnitPrice)
    VALUES 
        (@Order1Id, 'Toyota', 'Corolla', 2023, 2, 95000.00),
        (@Order1Id, 'Toyota', 'Hilux', 2023, 1, 250000.00);
    
    -- Sample Order 2
    INSERT INTO Orders (Status, CreatedAt, UpdatedAt)
    VALUES ('Em andamento', GETDATE(), GETDATE());
    
    DECLARE @Order2Id INT = SCOPE_IDENTITY();
    
    INSERT INTO OrderItems (OrderId, Make, Model, Year, Quantity, UnitPrice)
    VALUES 
        (@Order2Id, 'Honda', 'Civic', 2022, 1, 120000.00),
        (@Order2Id, 'Honda', 'HR-V', 2023, 3, 135000.00);
    
    -- Sample Order 3
    INSERT INTO Orders (Status, CreatedAt, UpdatedAt)
    VALUES ('Concluído', GETDATE(), GETDATE());
    
    DECLARE @Order3Id INT = SCOPE_IDENTITY();
    
    INSERT INTO OrderItems (OrderId, Make, Model, Year, Quantity, UnitPrice)
    VALUES (@Order3Id, 'Ford', 'Ranger', 2023, 1, 280000.00);
    
    PRINT 'Sample data inserted successfully.';
END
ELSE
BEGIN
    PRINT 'Sample data skipped - Orders table already contains data.';
END
GO
