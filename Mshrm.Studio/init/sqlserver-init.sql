USE [master];
GO

IF NOT EXISTS (SELECT * FROM sys.sql_logins WHERE name = 'applicationconnection')
BEGIN
    CREATE LOGIN [applicationconnection] WITH PASSWORD = 'Password123!', CHECK_POLICY = OFF;
    ALTER SERVER ROLE [sysadmin] ADD MEMBER [applicationconnection];
END
GO

IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'mshrm-studio-db')
  BEGIN
    CREATE DATABASE [mshrm-studio-db]
  END
GO