USE BankDb
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE sp_GetApplicationByCliendIdAndDepartementAddress_Procedure 
	@client_id UNIQUEIDENTIFIER,
	@department_address nvarchar(100)
	
AS
BEGIN

Declare @department_id UNIQUEIDENTIFIER
set @department_id = (select ID from Departments where AddressStr = @department_address);

with clientIpId  
as
(select cl.ID as 'ClientID', cip.ID from Clients as cl
inner join ClientIps as cip
on cl.ID = cip.ClientID
where cl.ID = @client_id)

SELECT app.ID, Amount, c.TypeName as 'Currency', s.TypeName as 'Status' FROM Applications as app
inner join clientIpId as cip
on app.ClientIpID = cip.ID
inner join Currencies as c
on c.ID = app.CurrencyID
inner join Statuses as s
on s.ID = app.StatusID
WHERE app.DepartmentID = @department_id

END
GO