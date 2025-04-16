USE dbaudit;
GO

IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'dbaudit')
BEGIN
    CREATE USER dbaudit FOR LOGIN dbaudit;
    PRINT 'User dbaudit utworzony w bazie dbaudit.';
END
    ELSE
    PRINT 'User dbaudit już istnieje w bazie dbaudit.';