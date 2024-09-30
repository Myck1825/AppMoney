USE BankDb
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE sp_GetApplicationByID_Procedure 
	@id UNIQUEIDENTIFIER
	
AS
BEGIN

SELECT app.ID, Amount, c.TypeName as 'Currency', s.TypeName as 'Status' FROM Applications as app
inner join Currencies as c
on c.ID = app.CurrencyID
inner join Statuses as s
on s.ID = app.StatusID
WHERE app.ID = @id

END
GO