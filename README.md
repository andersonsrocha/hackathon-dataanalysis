<div align="center">

<h1>
  <br/>
  <br/>
  <div>🌽</div>
  <b>Hackathon Data Analysis</b>
  <br/>
  <br/>
  <br/>
</h1>

Sistema de análise de dados agrícolas inteligente para a **AgroSolutions**,
cooperativa que moderniza a agricultura através de IoT e análise preditiva.
A aplicação processa dados de sensores em tempo real (umidade do solo, temperatura,
precipitação), executa regras de negócio configuráveis e gera alertas automáticos
para otimização da produção agrícola. Sistema baseado em Domain-Driven Design (DDD),
ASP.NET Core 8, CQRS com MediatR, banco híbrido (SQL Server + MongoDB) e mensageria
com RabbitMQ.

</div>

> \[!NOTE]
>
> Este projeto visa oferecer uma aplicação robusta, escalável e segura. O desenvolvimento deste projeto é baseado exclusivamente nas suas necessidades guiadas pelo curso de pós graduação Fiap.

<div align="center">

![.NET Core](https://img.shields.io/badge/.NET%20Core-8.0-512BD4?style=flat&logo=dotnet&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?style=flat&logo=microsoft-sql-server&logoColor=white)
![MongoDB](https://img.shields.io/badge/MongoDB-47A248?style=flat&logo=mongodb&logoColor=white)
![RabbitMQ](https://img.shields.io/badge/RabbitMQ-FF6600?style=flat&logo=rabbitmq&logoColor=white)
![New Relic](https://img.shields.io/badge/New%20Relic-008C99?style=flat&logo=new-relic&logoColor=white)
![MediatR](https://img.shields.io/badge/MediatR-CQRS-9932CC?style=flat)
![xUnit](https://img.shields.io/badge/xUnit-512BD4?style=flat&logo=.net&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=flat&logo=docker&logoColor=white)
![JWT](https://img.shields.io/badge/JWT-000000?style=flat&logo=jsonwebtokens&logoColor=white)
![DDD](https://img.shields.io/badge/DDD-Domain--Driven%20Design-FF6B6B?style=flat)

</div>

<details>

<summary>
  <b>Table of contents</b>
</summary>

#### TOC

- [📦 Começando](#-começando)
- [⚙️ Configuração](#️-configuração)
- [🖱️ Primeiro acesso](#️-primeiro-acesso)
- [🚧 Construindo e publicando](#-construindo-e-publicando)
- [🌾 Funcionalidades](#-funcionalidades)
- [✨ Características](#-características)
- [🚀 Recursos](#-recursos)

####

</details>

## 📦 Começando

### 📋 Pré-requisitos

- [**.NET 8 SDK**](https://dotnet.microsoft.com/download/dotnet/8.0) - Framework de desenvolvimento
- **SQL Server** - Banco de dados relacional (SQL Server Express, LocalDB ou Docker)
- **MongoDB** - Banco NoSQL para dados de sensores
- **RabbitMQ** - Sistema de mensageria para processamento assíncrono
- **Serviços Externos** - APIs de autenticação e talhões

### 🚀 Instalação

Comece clonando o repositório:

```bash
git clone https://github.com/andersonsrocha/hackathon-dataanalysis.git
```

Acesse o diretório do projeto:

```bash
cd hackathon-dataanalysis
```

Restaure os pacotes NuGet:

```bash
dotnet restore
```

Aplique as migrações do banco de dados:

```bash
dotnet ef database update -p src/HackathonDataAnalysis.Data -s src/HackathonDataAnalysis.Api
```

Inicie a aplicação:

```bash
dotnet run --project src/HackathonDataAnalysis.Api
```

Acesse a documentação da API: [https://localhost:5268/scalar](https://localhost:5268/scalar)

<br/>

## ⚙️ Configuração

Configure o arquivo `appsettings.json` com as seguintes configurações:

```json
{
  "ConnectionStrings": {
    "SqlConnection": "Data Source=localhost;Database=hackathon;Persist Security Info=True;User ID=sa;Password=Admin@123;TrustServerCertificate=True;",
    "MongoConnection": "mongodb://localhost:27017"
  },
  "Jwt": {
    "Issuer": "https://localhost:7054/",
    "Audience": "https://localhost:7054/",
    "Key": "10ae33a4dcf64e86ad582bd30c19b998"
  },
  "RabbitMQ": {
    "HostName": "localhost",
    "UserName": "admin",
    "Password": "admin123"
  },
  "Services": {
    "AuthService": {
      "BaseUrl": "http://localhost:5296/api/auth"
    },
    "PlotService": {
      "BaseUrl": "http://localhost:5129/api/plots"
    },
    "NewRelic": {
      "BaseUrl": "https://insights-collector.newrelic.com/v1/accounts",
      "AccountId": "7110116",
      "ApiKey": "e20ffdce07272085d33407e1b5408156FFFFNRAL"
    }
  }
}
```

**Certifique-se de que:**

1. O SQL Server está executando na conexão configurada
2. O MongoDB está disponível em localhost:27017
3. O RabbitMQ está executando com as credenciais configuradas
4. Os serviços externos de autenticação e plots estão disponíveis

<br/>

## 🖱️ Primeiro acesso

### 🔑 Autenticação

Para usar a API, você precisa de um token JWT do serviço de autenticação externo:

**Etapas:**

1. Certifique-se de que o serviço de autenticação esteja executando em `http://localhost:5296/api/auth`
2. Obtenha um token JWT através do endpoint de login
3. Inclua o token no header: `Authorization: Bearer <token>`
4. Apenas usuários com perfil "Admin" podem acessar os endpoints

### 📈 Testando a API

**Exemplo de criação de leitura de sensor:**

```bash
curl -X POST https://localhost:5001/api/readings \
  -H "Authorization: Bearer <token>" \
  -H "Content-Type: application/json" \
  -d '{
    "plotId": "550e8400-e29b-41d4-a716-446655440000",
    "soilMoisture": 45.5,
    "temperature": 25.3,
    "precipitation": 12.7
  }'
```

**Exemplo de criação de regra de alerta:**

```bash
curl -X POST https://localhost:5001/api/rules \
  -H "Authorization: Bearer <token>" \
  -H "Content-Type: application/json" \
  -d '{
    "plotId": "550e8400-e29b-41d4-a716-446655440000",
    "name": "Alerta Umidade Baixa",
    "sensorType": "SoilMoisture",
    "operator": "LessThan",
    "threshold": 30.0,
    "durationMinutes": 60,
    "status": "Critical",
    "message": "Umidade do solo abaixo do crítico"
  }'
```

## 🚧 Construindo e publicando

### 🔨 Build Local

Para construir a aplicação:

```bash
dotnet build
```

Para publicar a aplicação:

```bash
dotnet publish -c Release -o ./publish
```

### 🐳 Docker

Executar usando Docker:

```bash
docker build -t hackathon-dataanalysis .
docker run -p 8080:8080 hackathon-dataanalysis
```

### ☸️ Kubernetes

Os manifestos Kubernetes estão na pasta `manifests/`:

```bash
kubectl apply -f manifests/
```

## ✨ Características

- [x] ~~Arquitetura Domain-Driven Design (DDD) com Clean Architecture~~
- [x] ~~CQRS pattern com MediatR~~
- [x] ~~Coleta e processamento de dados de sensores IoT~~
- [x] ~~Sistema inteligente de regras de negócio~~
- [x] ~~Alertas automáticos baseados em thresholds~~
- [x] ~~Banco de dados híbrido (SQL Server + MongoDB)~~
- [x] ~~Mensageria assíncrona com RabbitMQ~~
- [x] ~~Integração com New Relic APM~~
- [x] ~~Autenticação JWT com serviços externos~~
- [x] ~~API RESTful com documentação Scalar~~
- [x] ~~Middleware para correlação de requisições~~
- [x] ~~Tratamento global de exceções~~
- [x] ~~Logs estruturados com Serilog~~
- [x] ~~Containerização com Docker~~
- [x] ~~Deploy em Kubernetes~~
- [x] ~~Testes unitários com xUnit~~

<br/>

## 🚀 Recursos

- 🎨 **.NET 8 SDK**: Framework moderno e multiplataforma da Microsoft que oferece alta performance, suporte nativo para contêineres, APIs mínimas e recursos avançados de desenvolvimento. Inclui melhorias significativas em performance, garbage collection otimizado e suporte completo para desenvolvimento de aplicações web robustas e escaláveis.
- 🗄️ **SQL Server**: Sistema de gerenciamento de banco de dados relacional utilizado para armazenar informações das propriedades, talhões e cultivos. Configurado com Entity Framework Core para mapeamento objeto-relacional.
- 🧪 **xUnit**: Framework de testes unitários para .NET que fornece uma base sólida para testes automatizados, com suporte para testes parametrizados, fixtures e execução paralela.
- 🐳 **Docker**: Containerização da aplicação para garantir consistência entre ambientes de desenvolvimento, teste e produção, facilitando deploy e escalabilidade.
- 🔐 **JWT Authentication**: Sistema de autenticação baseado em tokens seguros e stateless, integrado com serviço externo de usuários.
- 🏗️ **Domain-Driven Design (DDD)**: Arquitetura que foca no domínio do negócio, promovendo código mais organizado, manutenível e alinhado com as regras de negócio agrícola.

<br/>

## 📄 Endpoints da API

### 📡 Readings Controller

- `GET /api/readings/{id}` - Buscar leitura por ID
- `GET /api/readings` - Listar todas as leituras
- `GET /api/readings/plot/{plotId}` - Leituras de um talhão
- `GET /api/readings/plot/{plotId}/since/{date}` - Leituras por período
- `POST /api/readings` - Criar nova leitura de sensor
- `PUT /api/readings` - Atualizar leitura existente

### ⚙️ Rules Controller

- `GET /api/rules/{id}` - Buscar regra por ID
- `GET /api/rules` - Listar todas as regras
- `POST /api/rules` - Criar nova regra de negócio
- `PUT /api/rules` - Atualizar regra existente

> 🔒 **Autenticação**: Todos os endpoints requerem token JWT com perfil "Admin"

<br/>

Copyright © 2026.
