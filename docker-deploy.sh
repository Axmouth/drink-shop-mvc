touch .env
docker build . --file asp.net-core-mvc-drink-shop/Dockerfile --tag axmouth/asp-drinks-mvc
docker-compose build
docker-compose up -d --remove-orphans