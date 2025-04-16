USE dbaudit;
GO

GRANT SELECT, INSERT, UPDATE, DELETE ON SCHEMA :: dbo TO dbaudit;

GRANT EXECUTE ON SCHEMA :: dbo TO dbaudit;
GRANT ALTER, CONTROL, REFERENCES, TAKE OWNERSHIP ON SCHEMA :: dbo TO dbaudit;

PRINT N'Przyznano pełne uprawnienia dla app_user w schemacie dbo.';

-- GRANT SELECT ON dbo.Users TO dbaudit;
