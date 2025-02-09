# ProdutosMVC-API

Este projeto foi desenvolvido em **.NET 8** e utiliza o **PostgreSQL** como banco de dados, além de diversas bibliotecas para auxiliar na criação, validação, migração e testes da aplicação.

---

## Requisitos para Rodar o Projeto

- **PostgreSQL**
- **.NET SDK** (incluindo o **.NET 8**)
- **Visual Studio**
- **Docker Desktop** (para execução via Docker)

---

## Tecnologias Utilizadas

- FluentMigrator.Runner.Postgres  
- FluentValidation.AspNetCore  
- Microsoft.EntityFrameworkCore.Design  
- Microsoft.EntityFrameworkCore.InMemory  
- Microsoft.EntityFrameworkCore.Tools  
- Microsoft.Extensions.DependencyInjection  
- Microsoft.NET.Build.Containers  
- Microsoft.NET.Test.Sdk  
- Microsoft.VisualStudio.Web.CodeGeneration.Design  
- Moq  
- Npgsql.EntityFrameworkCore.PostgreSQL  
- xunit  
- xunit.runner.visualstudio  

---

## Passo a Passo para Executar o Projeto

### Executando Localmente

1. **Pré-requisitos**:
   - Certifique-se de que o **PostgreSQL** está instalado e configurado em sua máquina.
   - Abra o projeto no **Visual Studio**.
   - Garanta que o **.NET SDK** (incluindo o **.NET 8**) esteja instalado.

2. **Configuração do Banco de Dados**:
   - A aplicação está configurada para se conectar a um banco de dados PostgreSQL utilizando a porta padrão `5432`.
   - **Observações**:
     - Se o banco de dados (padrão: `postgres`) já existir, o projeto criará as tabelas necessárias automaticamente.
     - Se o banco de dados não existir, a aplicação criará o banco e as tabelas automaticamente.
     - Caso o seu banco de dados esteja em outro servidor ou utilize uma porta diferente de `5432`, será necessário ajustar a string de conexão na configuração do projeto.

3. **Execução**:
   - Compile e execute o projeto utilizando o **Visual Studio** ou, via terminal, com o comando:
     ```bash
     dotnet run
     ```
   - A aplicação iniciará e, conforme a configuração, realizará a criação do banco de dados e das tabelas, se necessário.

---

### Executando com Docker Compose

Para simplificar a criação e configuração do ambiente, este projeto oferece suporte para execução via Docker Compose. Com essa ferramenta, você pode levantar containers para o PostgreSQL e para a aplicação de forma rápida e integrada, sem a necessidade de instalações ou configurações locais adicionais.

#### O que o Docker Compose Faz?

- **Instalação do PostgreSQL**:  
  - Cria uma instância local com a **última versão** do PostgreSQL.
  - **Atenção**: O armazenamento é **não persistente**. Ou seja, quando o container parar ou for removido, todos os dados serão apagados. Essa abordagem é ideal para ambientes de desenvolvimento e testes.

- **Configuração do Ambiente do Banco de Dados**:  
  São definidas variáveis de ambiente para configurar o acesso ao banco:
  - `POSTGRES_DB`: Nome do banco de dados (padrão: `postgres`).
  - `POSTGRES_USER`: Usuário de acesso (padrão: `postgres`).
  - `POSTGRES_PASSWORD`: Senha de acesso (padrão: `root`).
  - `PGDATA`: Diretório dentro do container onde os dados serão armazenados.

- **Container da Aplicação**:  
  - Utiliza a imagem pré-construída do projeto.
  - Expõe a aplicação na porta **8082** (mapeando para a porta interna **8080** do container).

- **Rede Compartilhada**:  
  - Ambos os containers (o do PostgreSQL e o da aplicação) são conectados à mesma rede Docker, denominada `mvc-network`. Essa configuração facilita a comunicação entre eles.

#### Exemplo de Arquivo `docker-compose.yml`:

```yaml
version: '3'

services:
  
  produtos-db:
    container_name: produtos-db
    image: postgres:latest
    environment:
      POSTGRES_DB: postgres
      POSTGRES_USER: postgres
      POSTGRES_PASSWORD: root
      PGDATA: /data/postgres
    networks:
      - mvc-network
    ports:
      - "5432:5432"
  
  produtos-mvc:
    container_name: produtos-mvc
    image: pablo1212/produtosmvc:latest
    depends_on:
      - produtos-db
    ports:
      - "8082:8080"
    networks:
      - mvc-network
      
networks:
  mvc-network:
    driver: bridge
