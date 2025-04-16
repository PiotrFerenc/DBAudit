IF NOT EXISTS (SELECT *
               FROM sys.server_principals
               WHERE name = 'dbaudit')
    BEGIN
        -- Ensure the password meets your security requirements and replace with a strong password
        CREATE LOGIN dbaudit WITH PASSWORD = 'ilaW!yu345kfh7734';
        PRINT 'Login dbaudit utworzony.';
    END
ELSE
    PRINT N'Login dbaudit już istnieje.';