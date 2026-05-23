# 🐾 Patinhas em Dia — Challenge Cloud Computing 2026

> Solução cloud-native para gerenciamento de tutores, pets e eventos veterinários — executada de forma 100% containerizada na Microsoft Azure.

---

## 👥 Equipe

| Nome | RM |
|------|----|
| Enzo Franchin de Souza | RM 565677 |
| Lucas da Silva Lima | RM 562118 |
| Riquelme Nascimento | RM 565468 |
| Yasmin Nathalin Miranda dos Santos | RM 561565 |

---

## 📌 Visão Geral

O **Patinhas em Dia** é uma API RESTful desenvolvida com **ASP.NET Core (.NET 9)** e banco de dados **Oracle XE**, voltada ao acompanhamento completo da jornada de cuidados de animais domésticos — desde o cadastro de tutores e pets até o registro de eventos veterinários.

A aplicação permite:

- **Gerenciamento de Tutores** — Cadastro e manutenção de responsáveis pelos pets
- **Gestão de Pets** — Registro completo de cães, gatos e outros animais domésticos
- **Eventos de Cuidado** — Agendamento e rastreamento de vacinações, check-ups, tosas, limpeza dental e outros procedimentos
- **Relatórios de Saúde** — Resumo consolidado de eventos pendentes, realizados e atrasados por pet
- **Notificação de Tarefas** — Identificação automática de cuidados preventivos em atraso

Esta solução foi **conteinerizada via Docker Compose** e implantada em uma **máquina virtual Linux na Microsoft Azure**, garantindo escalabilidade, reprodutibilidade e conformidade com boas práticas de infraestrutura em nuvem.

### Stack Tecnológica

- **Backend:** ASP.NET Core Web API (.NET 9)
- **Banco de Dados:** Oracle Database XE
- **ORM:** Entity Framework Core (com Migrations)
- **Containerização:** Docker + Docker Compose
- **Cloud:** Microsoft Azure (VM Linux + IP Público + NSG)

---

## 💼 Benefícios para o Negócio

### Agilidade no Deploy
- Orquestração completa via **Docker Compose**: subir toda a infraestrutura com um único comando (`docker compose up -d --build`)
- CI/CD pronto para pipelines de integração contínua
- Redução no tempo de implementação em novos ambientes

### Portabilidade Garantida
- Ambiente de produção **idêntico** ao desenvolvimento
- Eliminação do erro clássico "funciona na minha máquina"
- Facilita onboarding de novos membros do time

### Integridade e Persistência de Dados
- **Volume nomeado** (`oracle_persistence_volume`) garante que os dados sobrevivem a reinicializações de containers
- Backup automático gerenciado pela Azure
- Recuperação em caso de falhas

### Eficiência Operacional
- Automação de infraestrutura via **Azure CLI** — criação e exclusão dinâmica de recursos
- Otimização de custos: recursos provisionados sob demanda
- **Health checks** automáticos garantem disponibilidade da API

### Conformidade e Segurança
- Usuário não-root dentro do container
- Credenciais gerenciadas via variáveis de ambiente
- Isolamento de rede via Docker Network (Bridge)
- Registros auditáveis via logs centralizados

---

## 🏗️ Arquitetura da Solução
<img width="1105" height="768" alt="arquitetura-Challenge-cloud" src="https://github.com/user-attachments/assets/8771ede6-adb0-430c-8e80-7afbaad3af53" />



### Recursos Azure Provisionados

- Resource Group
- Máquina Virtual Linux
- IP Público (Standard SKU)
- Network Security Group (portas 8080, 1521, 3389)
- Docker Engine + Docker Compose
- Oracle XE containerizado
- API ASP.NET Core containerizada

---

## 📁 Estrutura do Projeto

```
challenge-cloud-computing-1-2-2026/
│
├── Controllers/
│   ├── EventosController.cs
│   ├── PetsController.cs
│   └── TutoresController.cs
│
├── Data/
│   └── AppDbContext.cs
│
├── Migrations/
│   ├── 20260430112854_InitialCreate.cs
│   ├── 20260430112854_InitialCreate.Designer.cs
│   └── AppDbContextModelSnapshot.cs
│
├── Models/
│   ├── EventoCuidado.cs
│   ├── Pet.cs
│   └── Tutor.cs
│
├── Properties/
├── init-db/
│
├── Dockerfile
├── docker-compose.yml
├── criar-vm.sh
├── Program.cs
├── appsettings.json
├── appsettings.Development.json
└── README.md
```

---

## 🐳 Dockerfile

Estratégia de **multi-stage build** para reduzir o tamanho da imagem final e garantir segurança com usuário `non-root`.

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
RUN useradd -m appuser
USER appuser
EXPOSE 8080
ENTRYPOINT ["dotnet", "patinhasemdia.dll"]
```

**Práticas implementadas:**
- Multi-stage build (imagem final enxuta)
- Build em modo Release otimizado
- Execução com usuário `appuser` (non-root)

---

## 🐳 Docker Compose

```yaml
version: '3.8'

services:
  oracle-db:
    image: gvenzl/oracle-xe:latest
    container_name: oracle-db
    ports:
      - "1521:1521"
    environment:
      - ORACLE_PASSWORD=310106
      - APP_USER=RM565468
      - APP_USER_PASSWORD=310106
    volumes:
      - oracle_data:/opt/oracle/oradata
      - ./init-db:/container-entrypoint-initdb.d
    restart: always
    healthcheck:
      test: ["CMD", "healthcheck.sh"]
      interval: 30s
      timeout: 10s
      retries: 20
      start_period: 3m

  patinhasemdia-api:
    build:
      context: .
      dockerfile: Dockerfile
    container_name: patinhasemdia-api
    ports:
      - "8080:8080"
    environment:
      - ConnectionStrings__OracleConnection=User Id=RM565468;Password=310106;Data Source=oracle-db:1521/XEPDB1;
      - ASPNETCORE_ENVIRONMENT=Development
    depends_on:
      oracle-db:
        condition: service_healthy
    restart: always

volumes:
  oracle_data:
    name: oracle_persistence_volume
```

---

## 🔐 Credenciais Oracle

| Item | Valor |
|------|-------|
| Usuário | `RM565468` |
| Senha | `Fiap@2tdsmvs` |
| Porta | `1521` |
| Service Name | `XEPDB1` |

---

## 📋 Rotas da API

### 👨‍💼 Tutores — `/api/Tutores`

| Método | Rota | Descrição |
|--------|------|-----------|
| `GET` | `/api/Tutores` | Listar todos os tutores |
| `GET` | `/api/Tutores/{id}` | Buscar tutor por ID |
| `POST` | `/api/Tutores` | Cadastrar novo tutor |
| `PUT` | `/api/Tutores/{id}` | Atualizar tutor |
| `DELETE` | `/api/Tutores/{id}` | Remover tutor |

### 🐾 Pets — `/api/Pets`

| Método | Rota | Descrição |
|--------|------|-----------|
| `GET` | `/api/Pets` | Listar todos os pets |
| `GET` | `/api/Pets/{id}` | Buscar pet por ID |
| `POST` | `/api/Pets` | Cadastrar novo pet |
| `PUT` | `/api/Pets/{id}` | Atualizar pet |
| `DELETE` | `/api/Pets/{id}` | Remover pet |

### 🩺 Eventos de Cuidado — `/api/Eventos`

| Método | Rota | Descrição |
|--------|------|-----------|
| `GET` | `/api/Eventos` | Listar todos os eventos |
| `GET` | `/api/Eventos/{id}` | Buscar evento por ID |
| `POST` | `/api/Eventos` | Cadastrar novo evento |
| `PUT` | `/api/Eventos/{id}` | Atualizar evento |
| `DELETE` | `/api/Eventos/{id}` | Remover evento |

---

## 🧪 Exemplo de Payload

Cadastro completo de **Tutor + Pet + Evento** em uma única requisição:

```json
{
  "id": 0,
  "nome": "Riquelme Nascimento - Tutor Oficial Challenge Cloud",
  "email": "riquelme.nascimento@fiap.com.br",
  "telefone": "11999999999",
  "pets": [
    {
      "id": 0,
      "nome": "Thor",
      "especie": "Cachorro",
      "raca": "Golden Retriever",
      "idade": 4,
      "tutorId": 0,
      "tutor": "Riquelme Nascimento",
      "eventos": [
        {
          "id": 0,
          "petId": 0,
          "pet": "Thor",
          "tipoCuidado": "Vacinação",
          "dataPrevista": "2026-06-15T10:00:00.000Z",
          "status": "Agendado",
          "prioridade": "Alta",
          "observacao": "Vacina anual V10 e avaliação veterinária completa"
        }
      ]
    }
  ]
}
```

---


## ☁️ Script de Provisionamento Azure

O arquivo `criar-vm.sh` automatiza toda a criação da infraestrutura na Azure via CLI:

```bash
#!/bin/bash

RESOURCE_GROUP="rg-challenge-565468"
LOCATION="eastus2"
VM_NAME="vmdocker565468"
IMAGE="dockerinc1694120899427:devbox_azuremachine:devboxlicensefpromo:4.41.2"
SIZE="Standard_D2s_v3"
ADMIN_USERNAME="rm565468"
ADMIN_PASSWORD="Fiap@2tdsmvs"
DISK_SKU="StandardSSD_LRS"
PORT=3389
SHUTDOWN_TIME="0230"

echo "Criando grupo de recursos..."
az group create --name $RESOURCE_GROUP --location $LOCATION

echo "Aceitando os termos legais da imagem..."
az vm image terms accept --urn $IMAGE

echo "Criando máquina virtual..."
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

echo "Abrindo portas..."
az vm open-port --port 3389 --resource-group $RESOURCE_GROUP --name $VM_NAME
az vm open-port --port 8080 --resource-group $RESOURCE_GROUP --name $VM_NAME
az vm open-port --port 1521 --resource-group $RESOURCE_GROUP --name $VM_NAME

echo "Configurando desligamento automático..."
az vm auto-shutdown \
  --resource-group $RESOURCE_GROUP \
  --name $VM_NAME \
  --time $SHUTDOWN_TIME

echo "Provisionamento concluído!"
```

---

## 🛠️ Como Executar — Passo a Passo

### 1️⃣ Limpar o Ambiente Docker 

```bash
docker compose down -v
docker system prune -a -f --volumes
```

### 2️⃣ Clonar o Repositório

```bash
git clone https://github.com/riquelmee06/challenge-cloud-computing-1-2-2026.git
cd /challenge-cloud-computing-1-2-2026
```

### 2️⃣ Limpar o Ambiente Docker (opcional)

```bash
docker compose down -v
docker system prune -a -f --volumes
```

### 3️⃣ Subir os Containers

```bash
docker compose up -d --build
```

### 4️⃣ Verificar Containers Ativos

```bash
docker ps
```

Containers esperados:
- `patinhasemdia-api`
- `oracle-db`

### 5️⃣ Validar Segurança do Container

```bash
docker exec -it patinhasemdia-api /bin/bash
whoami
# Resultado esperado: appuser
```

### 6️⃣ Validar Banco Oracle

```bash
docker exec -it oracle-db sqlplus RM565468/310106@//localhost:1521/XEPDB1
```

```sql
SELECT * FROM TUTOR;
```

### 7️⃣ Acessar o Swagger

```
http://52.247.24.18:8080/swagger
```

---


## ✅ Checklist de Evidências

- [x] Build completo da aplicação
- [x] Orquestração via Docker Compose
- [x] Persistência Oracle com volume nomeado
- [x] Healthcheck do serviço Oracle
- [x] Segurança com usuário non-root no container
- [x] Swagger funcional e acessível
- [x] Persistência validada via `SELECT` no banco
- [x] Infraestrutura provisionada na Azure
- [x] Reprodutibilidade completa do ambiente

---

## 📦 Entity Framework Core — Migrations

O projeto utiliza **EF Core Migrations** para controle de versionamento do esquema do banco Oracle, garantindo:

- Controle de versão estrutural do banco
- Reprodutibilidade do esquema entre ambientes
- Evolução controlada e rastreável da aplicação
- Sincronização automática entre aplicação e banco de dados

---

*Projeto desenvolvido para o Challenge Cloud Computing 2026 — FIAP*
