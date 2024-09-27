docker build -t appmoney .

docker compose run -d -p 8080:80 --name appmoney-container appmoney