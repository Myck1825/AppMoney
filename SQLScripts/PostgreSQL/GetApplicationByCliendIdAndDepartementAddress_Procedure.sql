CREATE
OR REPLACE PROCEDURE "sp_GetApplicationByCliendIdAndDepartementAddress_Procedure" (client_id UUID, department_address varchar(100)) LANGUAGE PLPGSQL AS $$
DECLARE
    app_id UUID;
    amount decimal;
    currency TEXT;
    status TEXT;
	department_id UUID;
BEGIN
	
	SELECT d.ID 
	INTO department_id
	FROM Departments AS d 
	WHERE d.AddressStr = department_address
	AND department_address IS NOT NULL;
				   
	with clientIpId  
	as
	(select cl.ID, cip.ID AS client_ip_id from Clients as cl
	inner join ClientIps as cip
	on cl.ID = cip.ClientID
	where cl.ID = client_id)

    SELECT app.ID, app.Amount::decimal AS amount, c.TypeName, s.TypeName FROM Applications as app
	INTO app_id, amount, currency, status
	inner join clientIpId as cip
	on app.ClientIpID = cip.client_ip_id
	inner join Currencies as c
	on c.ID = app.CurrencyID
	inner join Statuses as s
	on s.ID = app.StatusID
	WHERE app.DepartmentID = department_id;

	RAISE NOTICE 'ID: %, Amount: %, Currency: %, Status: %', app_id, amount, currency, status;
	RETURN;
END;
$$;

