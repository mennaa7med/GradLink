-- SQL Script to check if Internships table exists
-- Run this in SQL Server Management Studio or Azure Data Studio

-- Check if table exists
SELECT 
    CASE WHEN EXISTS (
        SELECT 1 
        FROM INFORMATION_SCHEMA.TABLES 
        WHERE TABLE_NAME = 'Internships'
    )
    THEN 'Internships table EXISTS ✓'
    ELSE 'Internships table DOES NOT EXIST ✗'
    END AS TableStatus;

-- List all tables in database
SELECT TABLE_NAME 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE'
ORDER BY TABLE_NAME;

-- If Internships table exists, show its structure
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Internships')
BEGIN
    SELECT 
        COLUMN_NAME,
        DATA_TYPE,
        IS_NULLABLE,
        COLUMN_DEFAULT
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'Internships'
    ORDER BY ORDINAL_POSITION;
END

-- Check if there's any data
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Internships')
BEGIN
    SELECT COUNT(*) AS RecordCount FROM Internships;
END













