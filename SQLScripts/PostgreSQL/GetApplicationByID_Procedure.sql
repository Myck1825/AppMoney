CREATE OR REPLACE FUNCTION sp_GetApplicationByID_Procedure(appId UUID)
RETURNS TABLE(Id UUID, amount DECIMAL, currency varchar(10), status varchar(20)) 
LANGUAGE PLPGSQL AS $$
BEGIN
    RETURN QUERY
    SELECT app.ID, app.Amount::DECIMAL AS amount, c.TypeName AS currency, s.TypeName AS status
    FROM Applications AS app
    INNER JOIN Currencies AS c ON c.ID = app.CurrencyID
    INNER JOIN Statuses AS s ON s.ID = app.StatusID
    WHERE app.ID = appId;
END;
$$;
