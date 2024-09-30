CREATE
OR REPLACE PROCEDURE "sp_RegisterApplication_Procedure" 
(client_id UUID,
 ip_address nvarchar(15),
	department_address varchar(100),
	amount decimal, 
	currency varchar(10),
	OUT appid UUID,
	OUT error_code int,
	OUT error_message varchar(100)
) LANGUAGE PLPGSQL AS $$
DECLARE
    ALREADY_EXIST_MESSAGE varchar(100),
	ARGUMENTS_NOT_CORRECT_MESSAGE varchar(100), 
	INTERNAL_ERROR_MESSAGE varchar(100),
	ARGUMENTS_NOT_CORRECT_CODE INT,
	CONFLICT_CODE INT,
	INTERNAL_ERROR_CODE INT,
	error_format varchar(100),
	ip_id UUID,
	exist_client_id UUID,
	client_ip_id UUID,
	department_id UUID,
	currency_id UUID,
	status_id UUID,
	applicationCount INT
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
DO $$
BEGIN
	error_format := 'Message: %s. Error Code: %d'
	SET ALREADY_EXIST_MESSAGE := 'Application already exist.';
	SET INTERNAL_ERROR_MESSAGE := 'Fill table with statuses.';
	SET ARGUMENTS_NOT_CORRECT_MESSAGE := ' is not correct or does not exist';
	SET ARGUMENTS_NOT_CORRECT_CODE := 400;
	SET CONFLICT_CODE := 409;
	SET INTERNAL_ERROR_CODE := 500;

	-- Get id ip_address. If not exist add to table.
	SELECT ip.ID 
	INTO ip_id
	FROM Ips 
	WHERE IpAddress AS ip = ip_address;

	IF(ip_id IS NULL)
		INSERT INTO Ips (IpAddress) VALUES (ip_address);
		SELECT ip.ID
		INTO ip_id
		FROM Ips AS ip 
		WHERE IpAddress = ip_address);
	END IF
	
	-- Get id client. If not exist -> throw exception.
	
	SELECT c.ID 
	INTO exist_client_id
	FROM Clients AS c
	WHERE c.ID = client_id
	RETURN;

	IF(exist_client_id IS NULL)
		SET error_code := ARGUMENTS_NOT_CORRECT_CODE;
		SET error_message := CONCAT('Client_id', ARGUMENTS_NOT_CORRECT_MESSAGE);
		RAISE EXCEPTION error_format, error_message, error_code;
	END IF

	-- Get id client and ip. If not exist add to table.
	SELECT cip.ID 
	INTO client_ip_id
	FROM ClientIps AS cip 
	WHERE ClientID = client_id AND IPID = ip_id

	IF(client_ip_id IS NULL)
		INSERT INTO ClientIps (ClientID, IPID)
		VALUES (client_id, ip_id);
		
		SELECT ID
		INTO client_ip_id
		FROM ClientIps 
		WHERE ClientID = client_id AND IPID = ip_id);
	END IF

	-- Get id department.  If not exist -> throw exception.
	SELECT d.ID 
	INTO department_id
	FROM Departments AS d 
	WHERE AddressStr = department_address;

	IF(department_id IS NULL)
		SET error_code := ARGUMENTS_NOT_CORRECT_CODE;
		SET error_message := CONCAT('Departament address', ARGUMENTS_NOT_CORRECT_MESSAGE);
		RAISE EXCEPTION error_format, error_message, error_code;
	END IF

	-- Get id currency.  If not exist -> throw exception.
	SELECT c.ID 
	INTO currency_id
	FROM Currencies AS c 
	WHERE TypeName = currency;

	IF(currency_id IS NULL)
		SET error_code := ARGUMENTS_NOT_CORRECT_CODE;
		SET error_message := CONCAT('Currency', ARGUMENTS_NOT_CORRECT_MESSAGE);
		RAISE EXCEPTION error_format, error_message, error_code;
	END IF

	-- Get id status.  If not exist -> throw exception.
	SELECT c.ID 
	INTO status_id
	FROM Statuses AS c 
	WHERE IsDefault;

	IF(status_id IS NULL)
		SET error_code := INTERNAL_ERROR_CODE;
		SET error_message := INTERNAL_ERROR_MESSAGE;
		RAISE EXCEPTION error_format, error_message, error_code;
	END IF

	SELECT COUNT(app.ID)
	INTO applicationCount
	FROM Applications AS app
	INNER JOIN ClientIps AS cip
	ON app.ClientIpID = cip.Id
	WHERE cip.ClientID = client_id 
	AND app.CurrencyID = currency_id 
	AND app.StatusID = status_id

	IF(applicationCount > 0)
		SET error_code := CONFLICT_CODE;
		SET error_message := ALREADY_EXIST_MESSAGE;
		RAISE EXCEPTION error_format, error_message, error_code;
	END IF

	INSERT INTO Applications(Amount, StatusID, ClientIpID, DepartmentID, CurrencyID)
	OUTPUT INSERTED.ID
	VALUES (amount, status_id, client_ip_id, department_id, currency_id)
	
	SET error_code = 0;
	EXCEPTION

	RAISE EXCEPTION error_format, error_message, error_code;
	ROLLBACK;
	
END $$;
COMMIT;
