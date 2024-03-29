USE [CoreSQLServer]
GO
/****** Object:  StoredProcedure [dbo].[USER.GetJSON]    Script Date: 30/11/2019 13:12:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<RGAV>
-- Create date: <2019-11-30>
-- Description:	<Procedimiento para autenticar usuario y retornar JSON>
-- =============================================
ALTER PROCEDURE [dbo].[USER.GetJSON] 
	@Nick varchar(250),
	@Password varchar(256)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Resultado varchar(max)

	SET @Resultado =
	(
		SELECT [user_id] AS Id,[user_name] AS Name, 
		user_createDate AS CreateDate
		FROM dbo.[User]
		WHERE user_nick = @Nick ANd user_password = @Password 
		AND user_delete = 0
		FOR JSON PATH,WITHOUT_ARRAY_WRAPPER
	)
	SELECT @Resultado AS Usuario
END
