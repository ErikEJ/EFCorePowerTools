﻿CREATE PROCEDURE [dbo].[sp_Procedure1]
	@param1 int = 0,
	@param2 int
AS
	SELECT @param1, @param2
	SELECT * FROM sys.objects
RETURN 0
