CREATE
OR REPLACE PROCEDURE "sp_GetApplicationByID_Procedure" (appId UUID) LANGUAGE PLPGSQL AS $$
DECLARE
    app_id UUID;
    amount decimal;
    currency TEXT;
    status TEXT;
BEGIN
    SELECT app.ID, app.Amount::decimal AS amount, c.TypeName, s.TypeName FROM Applications as app
	INTO app_id, amount, currency, status
	inner join Currencies as c
	on c.ID = app.CurrencyID
	inner join Statuses as s
	on s.ID = app.StatusID
	WHERE app.ID = appId;

	RAISE NOTICE 'ID: %, Amount: %, Currency: %, Status: %', app_id, amount, currency, status;
	RETURN;
END;
$$;