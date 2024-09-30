--CREATE DATABASE "BankDb"
--    WITH
--    OWNER = postgres
--    ENCODING = 'UTF8'
--    LC_COLLATE = 'Ukrainian_Ukraine.1251'
--    LC_CTYPE = 'Ukrainian_Ukraine.1251'
--    LOCALE_PROVIDER = 'libc'
--    TABLESPACE = pg_default
--    CONNECTION LIMIT = -1
--    IS_TEMPLATE = False;



--USE [BankDb];
--GO

DO $$
BEGIN
	CREATE TABLE IF NOT EXISTS Clients(
	ID UUID PRIMARY KEY DEFAULT gen_random_uuid(),
	LastName VARCHAR(20) NOT NULL,
	FirstName VARCHAR(20) NOT NULL
	);
	
	CREATE TABLE IF NOT EXISTS Ips(
	ID UUID PRIMARY KEY DEFAULT gen_random_uuid(),
	IpAddress VARCHAR(15) NOT NULL
	);
	
	CREATE TABLE IF NOT EXISTS ClientIps(
	ID UUID PRIMARY KEY DEFAULT gen_random_uuid(),
	ClientID UUID NOT NULL,
	IPID UUID NOT NULL,
	FOREIGN KEY (ClientID) REFERENCES Clients(ID) ON DELETE CASCADE,
	FOREIGN KEY (IPID) REFERENCES Ips(ID) ON DELETE CASCADE
	);
	
	CREATE TABLE IF NOT EXISTS Departments(
	ID UUID PRIMARY KEY DEFAULT gen_random_uuid(),
	AddressStr VARCHAR(100) NOT NULL UNIQUE
	);
	
	CREATE TABLE IF NOT EXISTS Currencies(
	ID UUID PRIMARY KEY DEFAULT gen_random_uuid(),
	TypeName VARCHAR(10) NOT NULL UNIQUE
	);
	
	CREATE TABLE IF NOT EXISTS Statuses(
	ID UUID PRIMARY KEY DEFAULT gen_random_uuid(),
	TypeName VARCHAR(20) NOT NULL UNIQUE,
	IsDefault BOOLEAN DEFAULT FALSE
	);

	CREATE TABLE IF NOT EXISTS Applications(
	ID UUID PRIMARY KEY DEFAULT gen_random_uuid(),
	Amount MONEY NOT NULL,
	DateCreate TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
	DateUpdate TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
	StatusID UUID NOT NULL,
	ClientIpID UUID NOT NULL,
	DepartmentID UUID NOT NULL,
	CurrencyID UUID NOT NULL,
	FOREIGN KEY (StatusID) REFERENCES Statuses(ID) ON DELETE CASCADE,
	FOREIGN KEY (ClientIpID) REFERENCES ClientIps(ID) ON DELETE CASCADE,
	FOREIGN KEY (DepartmentID) REFERENCES Departments(ID) ON DELETE CASCADE,
	FOREIGN KEY (CurrencyID) REFERENCES Currencies(ID) ON DELETE CASCADE,
	CHECK ((amount BETWEEN '100'::money AND '100000'::money))
	);
	
	INSERT INTO Currencies (TypeName) 
	VALUES ('UAH'),
	('USD'),
	('EUR'),
	('PLN');
	
	INSERT INTO Departments (AddressStr) 
	VALUES 
	('вулиця Івана Акінфієва, 14'),
	('проспект Дмитра Яворницького, 46'),
	('вулиця Олеся Гончара, 6'),
	('проспект Науки, 7'),
	('вулиця Мечникова, 10б'),
	('вулиця Набережна Перемоги, 30'),
	('проспект Героїв, 37'),
	('бульвар Слави, 46');
	
	INSERT INTO Statuses (TypeName, IsDefault) 
	VALUES ('В очікуванні', TRUE);

	INSERT INTO Statuses (TypeName) 
	VALUES ('Обробляється'),
	('В дорозі'),
	('У відділенні'),
	('Готова до видачі');
	
	INSERT INTO Clients(LastName, FirstName) 
	VALUES ('Pupkin', N'Vasiliy');

	
EXCEPTION
    WHEN undefined_table THEN
        ROLLBACK; 
		RAISE NOTICE 'An error occurred: table does not exist';
    WHEN unique_violation THEN
        RAISE NOTICE 'Unique constraint violation: %', SQLERRM;
        ROLLBACK; 
    WHEN OTHERS THEN
        RAISE NOTICE 'An unexpected error occurred: %', SQLERRM;
        ROLLBACK; 
END $$;

COMMIT;
