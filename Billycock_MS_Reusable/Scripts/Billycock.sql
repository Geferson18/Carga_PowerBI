if OBJECT_ID('[dbo].[TOKENUSER]') is not null
Begin
	DELETE FROM [dbo].[TOKENUSER]

	INSERT [dbo].[TOKENUSER] ([Id], [userName], [password], [accessFailedCount], [lockoutEnabled]) VALUES (N'123456789', N'vlarosa', N'RFBHFVZ/S861Wjtt0jRIKg==', 0, 1)
End
GO

if OBJECT_ID('[dbo].[CORRELATIVE]') is not null
BEGIN
	DELETE FROM [dbo].[CORRELATIVE]

	INSERT INTO [dbo].[CORRELATIVE] ([guid],[idPlatformAccount],[idUserPlatform],[idUserPlatformAccount]) VALUES (N'30E84046-EA2B-4FDE-BA5E-4EEF50E2B251',0,0,0)
END
GO


IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'BILLYCOCK_TABLES')
BEGIN
	DROP PROCEDURE [dbo].[BILLYCOCK_TABLES]
END
GO

/****** Object:  StoredProcedure [dbo].[BILLYCOCK_TABLES]    Script Date: 17/03/2024 23:37:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[BILLYCOCK_TABLES](
    @TIPO VARCHAR(1),
    @PROCESO VARCHAR(1)
)
AS
BEGIN 
    SET NOCOUNT ON; -- Para evitar mensajes adicionales

    IF @PROCESO = 'R' --READ
    BEGIN
        IF @TIPO = 'N' --NECESSARY
        BEGIN
            SELECT * FROM [TOKENUSER];
            SELECT * FROM [STATE];
            SELECT * FROM [CORRELATIVE];
        END
        IF @TIPO = 'C' --COMPLEMENTARY
        BEGIN
            SELECT * FROM [HISTORY];
            SELECT * FROM [AUDIT];
        END
        IF @TIPO = 'T' --TRANSACTIONAL
        BEGIN
            SELECT * FROM [PLATFORM];
            SELECT * FROM [ACCOUNT];
            SELECT * FROM [PLATFORMACCOUNT];
            SELECT * FROM [USER]; 
            SELECT * FROM [USERPLATFORM];
            SELECT * FROM [USERPLATFORMACCOUNT];
        END
    END
    ELSE IF @PROCESO = 'D' --DELETE
    BEGIN
        BEGIN TRANSACTION;
        BEGIN TRY
            IF @TIPO = 'N' --NECESSARY
            BEGIN
                DELETE FROM [TOKENUSER];
                DELETE FROM [STATE];
                DBCC CHECKIDENT ([STATE], RESEED, 0);
                DELETE FROM [CORRELATIVE];
            END
            ELSE IF @TIPO = 'C' --COMPLEMENTARY
            BEGIN
                DELETE FROM [HISTORY];
                DBCC CHECKIDENT ([HISTORY], RESEED, 0);
                DELETE FROM [AUDIT];
                DBCC CHECKIDENT ([AUDIT], RESEED, 0);
            END
            ELSE IF @TIPO = 'T' --TRANSACTIONAL
            BEGIN
                DELETE FROM [PLATFORM];
                DBCC CHECKIDENT ([PLATFORM], RESEED, 0);
                DELETE FROM [ACCOUNT];
                DBCC CHECKIDENT ([ACCOUNT], RESEED, 0);
                DELETE FROM [PLATFORMACCOUNT];
                DELETE FROM [USER]; 
                DBCC CHECKIDENT ([USER], RESEED, 0); 
                DELETE FROM [USERPLATFORM];
                DELETE FROM [USERPLATFORMACCOUNT];
            END
            COMMIT TRANSACTION;
        END TRY
        BEGIN CATCH
            ROLLBACK TRANSACTION;
            -- Manejo de errores
        END CATCH
    END 
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'Tr' AND name = 'UPDATEPLATFORMACCOUNTTRIGGER')
BEGIN
	DROP TRIGGER [dbo].[UPDATEPLATFORMACCOUNTTRIGGER]
END
GO

CREATE TRIGGER UPDATEPLATFORMACCOUNTTRIGGER
ON UserPlatformAccount
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @IdPlatform INT,@IdAccount INT;

    SELECT @IdPlatform = idPlatform,@IdAccount = idAccount
    FROM deleted;

    UPDATE PlatformAccount
    SET freeUsers = freeUsers + 1
    WHERE idPlatform = @IdPlatform
	AND idAccount = @IdAccount;
END;
GO
