📦 **OrdersApp** é uma solução fullstack para gerenciamento de pedidos, construída com backend .NET e frontend Angular. O projeto demonstra uma arquitetura bem estruturada, com separação clara de responsabilidades e uso de boas práticas em ambas as camadas.

## Backend (.NET 10)

O backend implementa uma API REST com arquitetura em camadas:

- **Controller** — Expõe endpoints HTTP e retorna respostas padronizadas.
- **Service** — Concentra a lógica de negócios da aplicação.
- **Repository** — Abstrai o acesso aos dados, isolando a infraestrutura.

A persistência utiliza **SQL Server** rodando em container Docker. Parte da lógica foi implementada diretamente no banco:

- **Stored Procedures** — Executam filtros complexos com melhor performance.
- **Triggers** — Calculam automaticamente o valor total dos pedidos.

O projeto inclui **DTOs** para transferência de dados e **Models** para as entidades de domínio.

Os **testes unitários** são executados automaticamente durante o deploy do container. A **documentação interativa** (Swagger UI) fica disponível em `http://localhost:5000/swagger/index.html` após a inicialização.

![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=csharp&logoColor=white)
![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![Docker](https://img.shields.io/badge/docker-%230db7ed.svg?style=for-the-badge&logo=docker&logoColor=white)
![MicrosoftSQLServer](https://img.shields.io/badge/Microsoft%20SQL%20Server-CC2927?style=for-the-badge&logo=microsoft%20sql%20server&logoColor=white)
![Swagger](https://img.shields.io/badge/-Swagger-%23Clojure?style=for-the-badge&logo=swagger&logoColor=white)

## Frontend (Angular 16+)

A interface foi desenvolvida com **Angular** e **Material Design**, priorizando uma experiência de usuário fluida:

- **Filtros Encadeados** — O usuário seleciona Marca, depois Modelo (baseado na marca) e finalmente Ano (baseado no modelo), com atualização dinâmica das opções.
- **Exportação** — Permite baixar a listagem de pedidos em formato PDF ou Excel.
- **Componentes Modulares** — Estrutura componentizada que facilita manutenção e reutilização.

![Angular](https://img.shields.io/badge/angular-%23DD0031.svg?style=for-the-badge&logo=angular&logoColor=white)
![TypeScript](https://img.shields.io/badge/typescript-%23007ACC.svg?style=for-the-badge&logo=typescript&logoColor=white)
![Material Design](https://img.shields.io/badge/material%20design-757575?style=for-the-badge&logo=material%20design&logoColor=white)
![SASS](https://img.shields.io/badge/SASS-hotpink.svg?style=for-the-badge&logo=SASS&logoColor=white)

## 🛠️ Instalação e Execução

A aplicação utiliza **Docker Compose** para o backend e banco de dados. O frontend é executado localmente com Node.js.

### 1️⃣ Subir Backend e Banco de Dados

A partir do diretório `OrdersAppBackend`:

```bash
docker-compose up --build
```

Este comando inicializa:
- **SQL Server** (porta 1433)
- **Backend API** (porta 5000)

O banco de dados executa automaticamente o script de inicialização (`init.sql`) na primeira execução.

### 2️⃣ Executar o Frontend

Em outro terminal, a partir do diretório raiz:

```bash
cd orders-app-frontend
npm install
npm start
```

A aplicação estará disponível em `http://localhost:4200`.

## ⚖️ Licença

Este projeto é distribuído sob a **Licença MIT**. Você é livre para utilizar e modificar o código. O único requisito é dar o devido crédito.
