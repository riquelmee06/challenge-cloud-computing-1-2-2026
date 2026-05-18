#!/bin/bash

RESOURCE_GROUP="rg-challenge-565468"
LOCATION="eastus2"
VM_NAME="vmdocker565468"
IMAGE="dockerinc1694120899427:devbox_azuremachine:devboxlicensefpromo:4.41.2"

SIZE="Standard_D2s_v3"
ADMIN_USERNAME="rm565468"
ADMIN_PASSWORD="Fiap@2tdsvms"
DISK_SKU="StandardSSD_LRS"

PORT=3389
SHUTDOWN_TIME="0230" 

echo "Criando grupo de recursos: $RESOURCE_GROUP..."
az group create --name $RESOURCE_GROUP --location $LOCATION

echo "Aceitando os Termos Legais da Imagem..."
az vm image terms accept --urn $IMAGE

echo "Criando a máquina virtual: $VM_NAME..."
az vm create \
  --resource-group $RESOURCE_GROUP \
  --name $VM_NAME \
  --image $IMAGE \
  --size $SIZE \
  --authentication-type password \
  --admin-username $ADMIN_USERNAME \
  --admin-password $ADMIN_PASSWORD \
  --storage-sku $DISK_SKU \
  --public-ip-sku Standard

echo "Abrindo portas para RDP, API (8080) e Banco Oracle (1521)..."
az vm open-port --port $PORT --resource-group $RESOURCE_GROUP --name $VM_NAME
az vm open-port --port 8080 --resource-group $RESOURCE_GROUP --name $VM_NAME
az vm open-port --port 1521 --resource-group $RESOURCE_GROUP --name $VM_NAME

echo "Configurando desligamento automático às $SHUTDOWN_TIME (UTC)..."
az vm auto-shutdown \
  --resource-group $RESOURCE_GROUP \
  --name $VM_NAME \
  --time $SHUTDOWN_TIME

echo "Provisionamento completo!"
