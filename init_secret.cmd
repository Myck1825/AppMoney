docker swarm init

echo "Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=BankDb;Integrated Security=SSPI;" | docker secret create ConnectionString.MSSQL -

echo "This is a file secret." > ConnectionString.MSSQL.txt

