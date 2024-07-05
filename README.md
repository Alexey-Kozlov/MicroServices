# MicroServices
Микросервисы + RabbitMq + React

Из папки MicroServices:
docker build -f ./CategoryApi/Dockerfile -t kozlovas/ms-category-api .
docker build -f ./front/Dockerfile -t kozlovas/ms-front .
docker build -f ./Identity/Dockerfile -t kozlovas/ms-identity-api .
docker build -f ./Identity/client-app/Dockerfile -t kozlovas/ms-identity-front .
docker build -f ./MainAPI/Dockerfile -t kozlovas/ms-main-api .
docker build -f ./Orders/Dockerfile -t kozlovas/ms-order-api .
docker build -f ./ProductAPI/Dockerfile -t kozlovas/ms-product-api .
docker build -f ./RabbitConsumer/Dockerfile -t kozlovas/ms-rabbitconsumer .
docker build -f ./RabbitProducer/Dockerfile -t kozlovas/ms-rabbitproducer .