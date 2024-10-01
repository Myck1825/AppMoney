CREATE OR REPLACE FUNCTION sp_GetApplicationByCliendIdAndDepartementAddress_Procedure(client_id UUID, department_address VARCHAR(100))
RETURNS TABLE(app_id UUID, amount DECIMAL, currency VARCHAR(10), status VARCHAR(20)) 
LANGUAGE PLPGSQL AS $$
DECLARE
    department_id UUID;
BEGIN
    -- Get the department ID based on the provided address
    SELECT d.ID 
    INTO department_id
    FROM Departments AS d 
    WHERE d.AddressStr = department_address
    AND department_address IS NOT NULL;
    
	-- Return the query results
    RETURN QUERY
    WITH clientIpId AS (
        SELECT cl.ID, cip.ID AS client_ip_id 
        FROM Clients AS cl
        INNER JOIN ClientIps AS cip ON cl.ID = cip.ClientID
        WHERE cl.ID = client_id
    )
	
    SELECT app.ID, app.Amount::DECIMAL AS amount, c.TypeName AS currency, s.TypeName AS status 
    FROM Applications AS app
    INNER JOIN clientIpId AS cip ON app.ClientIpID = cip.client_ip_id
    INNER JOIN Currencies AS c ON c.ID = app.CurrencyID
    INNER JOIN Statuses AS s ON s.ID = app.StatusID
    WHERE app.DepartmentID = department_id;

END;
$$;