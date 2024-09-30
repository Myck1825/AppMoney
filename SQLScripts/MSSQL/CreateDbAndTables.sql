--USE master
--GO
--ALTER DATABASE BankDb
--SET OFFLINE WITH ROLLBACK IMMEDIATE
--GO
--CREATE DATABASE BankDb;

--USE [BankDb];
--GO

BEGIN TRANSACTION [Tran1]

BEGIN TRY
	delete from Applications;
delete from ClientIps;
delete from Clients;
delete from Ips;
delete from Departments;
delete from Currencies;
delete from Statuses;

drop table Applications;
drop table ClientIps;
drop table Clients;
drop table Ips;
drop table Departments;
drop table Currencies;
drop table Statuses;
	
	CREATE TABLE Clients(
	ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT newsequentialid(),
	LastName NVARCHAR(20) NOT NULL,
	FirstName NVARCHAR(20) NOT NULL
	);
	
	CREATE TABLE Ips(
	ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT newsequentialid(),
	IpAddress NVARCHAR(15) NOT NULL
	);
	
	CREATE TABLE ClientIps(
	ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT newsequentialid(),
	ClientID UNIQUEIDENTIFIER NOT NULL,
	IPID UNIQUEIDENTIFIER NOT NULL,
	FOREIGN KEY (ClientID) REFERENCES Clients(ID),
	FOREIGN KEY (IPID) REFERENCES Ips(ID)
	);
	
	CREATE TABLE Departments(
	ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT newsequentialid(),
	AddressStr NVARCHAR(100) NOT NULL UNIQUE
	);
	
	CREATE TABLE Currencies(
	ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT newsequentialid(),
	TypeName NVARCHAR(10) NOT NULL UNIQUE
	);
	
	CREATE TABLE Statuses(
	ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT newsequentialid(),
	TypeName NVARCHAR(20) NOT NULL UNIQUE,
	IsDefault BIT DEFAULT 0
	);

	CREATE TABLE Applications(
	ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT newsequentialid(),
	Amount MONEY NOT NULL check(Amount between 100 and 100000),
	DateCreate DATETIME DEFAULT getdate(),
	DateUpdate DATETIME DEFAULT getdate(),
	StatusID UNIQUEIDENTIFIER NOT NULL,
	ClientIpID UNIQUEIDENTIFIER NOT NULL,
	DepartmentID UNIQUEIDENTIFIER NOT NULL,
	CurrencyID UNIQUEIDENTIFIER NOT NULL,
	FOREIGN KEY (StatusID) REFERENCES Statuses(ID),
	FOREIGN KEY (ClientIpID) REFERENCES ClientIps(ID),
	FOREIGN KEY (DepartmentID) REFERENCES Departments(ID),
	FOREIGN KEY (CurrencyID) REFERENCES Currencies(ID)
	);
	
	INSERT INTO Currencies (TypeName) 
	VALUES ('UAH'),
	('USD'),
	('EUR'),
	('PLN')
	
	INSERT INTO Departments (AddressStr) 
	VALUES (N'������ ����� ��������, 14'),
	(N'�������� ������ ������������, 46'),
	(N'������ ����� �������, 6'),
	(N'�������� �����, 7'),
	(N'������ ���������, 10�'),
	(N'������ ��������� ��������, 30'),
	(N'�������� �����, 37'),
	(N'������� �����, 46')
	
	INSERT INTO Statuses (TypeName, IsDefault) 
	VALUES (N'� ���������', 1)

	INSERT INTO Statuses (TypeName) 
	VALUES (N'������������'),
	(N'� �����'),
	(N'� �������'),
	(N'������ �� ������')
	
	INSERT INTO Clients(LastName, FirstName) 
	VALUES (N'Pupkin', N'Vasiliy')

	COMMIT TRANSACTION [Tran1]; 

END TRY

BEGIN CATCH
	ROLLBACK TRANSACTION [Tran1]
END CATCH  


