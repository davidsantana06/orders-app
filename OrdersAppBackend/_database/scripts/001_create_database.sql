USE master;
GO

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
