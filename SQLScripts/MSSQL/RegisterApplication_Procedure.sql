USE BankDb
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE sp_RegisterApplication_Procedure 
	@client_id UNIQUEIDENTIFIER,
	@ip_address nvarchar(15),
	@department_address nvarchar(100),
	@amount money, 
	@currency nvarchar(10),
	@id UNIQUEIDENTIFIER OUTPUT,
	@error_code int OUTPUT,
	@error_message nvarchar(100) OUTPUT
	
AS
BEGIN
SET NOCOUNT on;
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE -- встановив це усвідомленно, так як вважаю що ізольованність в банку має бути на високому рівні, але при цьому буде страждати продуктивність
BEGIN TRANSACTION [Tran1];  

BEGIN TRY
	
	DECLARE @ALREADY_EXIST_MESSAGE nvarchar(100), @ARGUMENTS_NOT_CORRECT_MESSAGE nvarchar(100), @INTERNAL_ERROR_MESSAGE nvarchar(100),
	@ARGUMENTS_NOT_CORRECT_CODE INT, @CONFLICT_CODE INT, @INTERNAL_ERROR_CODE INT, @error_format nvarchar(100);
	SET @error_format = 'Message: %s. Error Code: %d'
	SET @ALREADY_EXIST_MESSAGE = 'Application already exist.';
	SET @INTERNAL_ERROR_MESSAGE = 'Fill table with statuses.';
	SET @ARGUMENTS_NOT_CORRECT_MESSAGE = ' is not correct or does not exist';
	SET @ARGUMENTS_NOT_CORRECT_CODE = 400;
	SET @CONFLICT_CODE = 409;
	SET @INTERNAL_ERROR_CODE = 500;

	-- Get id ip_address. If not exist add to table.
	DECLARE @ip_id UNIQUEIDENTIFIER
	SET @ip_id = (select ID from Ips where IpAddress = @ip_address);
	
	IF(@ip_id is NULL)
	BEGIN
		INSERT INTO Ips (IpAddress) VALUES (@ip_address);
		SET @ip_id = (select ID from Ips where IpAddress = @ip_address);
	END
	
	-- Get id client. If not exist -> throw exception.
	DECLARE @exist_client_id UNIQUEIDENTIFIER
	SET @exist_client_id = (select ID from Clients where ID = @client_id);
	
	IF(@exist_client_id is NULL)
	BEGIN
		SET @error_code = @ARGUMENTS_NOT_CORRECT_CODE;
		SET @error_message = CONCAT('Client_id', @ARGUMENTS_NOT_CORRECT_MESSAGE);
		RAISERROR(@error_format, 16, 1, @error_message, @error_code);
	END
	
	-- Get id client and ip. If not exist add to table.
	DECLARE @client_ip_id UNIQUEIDENTIFIER
	SET @client_ip_id = (select ID from ClientIps where ClientID = @client_id and IPID = @ip_id);
	
	IF(@client_ip_id is NULL)
	BEGIN
		INSERT INTO ClientIps (ClientID, IPID) VALUES (@client_id, @ip_id);
		SET @client_ip_id = (select ID from ClientIps where ClientID = @client_id and IPID = @ip_id);
	END
	
	-- Get id department.  If not exist -> throw exception.
	DECLARE @department_id UNIQUEIDENTIFIER
	SET @department_id = (select ID from Departments where AddressStr = @department_address);
	
	IF(@department_id is NULL)
	BEGIN
		SET @error_code = @ARGUMENTS_NOT_CORRECT_CODE;
		SET @error_message = CONCAT('Departament address', @ARGUMENTS_NOT_CORRECT_MESSAGE);
		RAISERROR(@error_format, 16, 1, @error_message, @error_code);
	END
	
	-- Get id currency.  If not exist -> throw exception.
	DECLARE @currency_id UNIQUEIDENTIFIER
	SET @currency_id = (select ID from Currencies where TypeName = @currency);
	
	IF(@currency_id is NULL)
	BEGIN
		SET @error_code = @ARGUMENTS_NOT_CORRECT_CODE;
		SET @error_message = CONCAT('Currency', @ARGUMENTS_NOT_CORRECT_MESSAGE);
		RAISERROR(@error_format, 16, 1, @error_message, @error_code);
	END
	
	-- Get id currency.  If not exist -> throw exception.
	DECLARE @status_id UNIQUEIDENTIFIER
	SET @status_id = (select ID from Statuses where IsDefault = 1);
	
	IF(@status_id is NULL)
	BEGIN
		SET @error_format = @INTERNAL_ERROR_CODE;
		SET @error_message = @INTERNAL_ERROR_MESSAGE;
		RAISERROR(@error_format, 16, 1, @error_message, @error_code);
	END

	DECLARE @applicationCount INT;
	set @applicationCount = (select COUNT(app.ID) from Applications as app
							 inner join ClientIps as cip
							 on app.ClientIpID = cip.Id
							 where cip.ClientID = @client_id 
							 and app.CurrencyID = @currency_id 
							 and app.StatusID = @status_id)
	IF(@applicationCount  > 0)
	BEGIN
		SET @error_code = @CONFLICT_CODE;
		SET @error_message = @ALREADY_EXIST_MESSAGE;
		RAISERROR(@error_format, 16, 1, @error_message, @error_code);
	END

	INSERT INTO Applications(Amount, StatusID, ClientIpID, DepartmentID, CurrencyID)
	OUTPUT inserted.ID
	VALUES (@amount, @status_id, @client_ip_id, @department_id, @currency_id)
	
	SET @error_code = 0;
	COMMIT TRANSACTION [Tran1]; 
END TRY

BEGIN CATCH

	ROLLBACK TRANSACTION [Tran1];
    DECLARE @error_severity INT;
    DECLARE @error_state INT;
	SET @error_message = ERROR_MESSAGE();
	SET @error_severity = ERROR_SEVERITY()
	SET @error_state = ERROR_STATE();


	RAISERROR(@error_format, @error_severity, @error_state, @error_message, @error_code);

END CATCH  

END
GO