
docker swarm leave --force
docker swarm init

echo "Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=BankDb;Integrated Security=SSPI;" | docker secret create MSConnectionString -

echo "Host=localhost;Port=5432;Database=BankDb;Username=postgres;Password=admin;" | docker secret create PostgreConnectionString -

docker-compose build --no-cache

docker stack deploy --compose-file docker-compose.yaml appmoney_stack

docker-compose up

docker service create --name appmoney --secret MSConnectionString appmoney-appmoney
docker service create --name appmoney --secret MSConnectionString --network my_network appmoney-appmoney

docker network create appmoney_network

docker service create --name rabbitmq --network appmoney_network --publish 5672:5672 rabbitmq:3-management

docker service create --name appmoney --secret MSConnectionString appmoney-appmoney

docker service logs appmoney

docker service rm appmoney