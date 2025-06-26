USE [SocialNetworkProject];
GO

PRINT '==> Desactivando todas las restricciones de clave externa...';
EXEC sp_msforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL';
GO

PRINT '==> Borrando datos de todas las tablas...';
EXEC sp_msforeachtable 'SET QUOTED_IDENTIFIER ON; DELETE FROM ?';
GO

PRINT '==> Reactivando todas las restricciones de clave externa...';
EXEC sp_msforeachtable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL';
GO

PRINT '==> ¡Proceso completado! Todas las tablas han sido limpiadas.';
GO