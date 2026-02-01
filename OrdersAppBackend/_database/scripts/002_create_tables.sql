USE OrdersAppDatabase;
GO

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
        Quantity INT NOT NULL DEFAULT 1,
        UnitPrice DECIMAL(18,2) NOT NULL,
        CONSTRAINT FK_OrderItems_Orders FOREIGN KEY (OrderId) 
            REFERENCES Orders(Id) ON DELETE CASCADE,
        CONSTRAINT CHK_OrderItems_Year CHECK (Year >= 1900 AND Year <= 2100),
        CONSTRAINT CHK_OrderItems_Quantity CHECK (Quantity >= 1),
        CONSTRAINT CHK_OrderItems_UnitPrice CHECK (UnitPrice >= 0)
    );
    PRINT 'Table OrderItems created successfully.';
END
ELSE
BEGIN
    PRINT 'Table OrderItems already exists.';
END
GO
