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
   > **Observações**:
     > Se o banco de dados (padrão: `postgres`) já existir, o projeto criará as tabelas necessárias automaticamente.
     > Se o banco de dados não existir, a aplicação criará o banco e as tabelas automaticamente.
     > Caso o seu banco de dados esteja em outro servidor ou utilize uma porta diferente de `5432`, será necessário ajustar a string de conexão na configuração do projeto.

3. **Execução**:
   - Compile e execute o projeto utilizando o **Visual Studio** ou, via terminal, com o comando:
     ```bash
     dotnet run
     ```
   - A aplicação iniciará e, conforme a configuração, realizará a criação do banco de dados e das tabelas, se necessário (As migrações irão ser executadas automaticas configuração localizada em `Program.cs` na region `Aplicar migrações no banco de dados`).

---

### Executando com Docker Compose

Para simplificar a criação e configuração do ambiente, este projeto oferece suporte para execução via Docker Compose. Com essa ferramenta, você pode levantar containers para o PostgreSQL e para a aplicação de forma rápida e integrada, sem a necessidade de instalações ou configurações locais adicionais.

3. **Execução**:
   -  Abra o projeto e execute o terminal ou navegue via terminal na pasta raiz do projeto e execute o comando:
     ```bash
     docker compose up
     ```

#### O que o Docker Compose Faz?

- **Instalação do PostgreSQL**:  
  - Cria uma instância local com a **última versão** do PostgreSQL.
  - **Atenção**: O armazenamento é **não persistente**. Ou seja, quando o container parar ou for removido, todos os dados serão apagados.

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
 ```
---
## Testes integrados 

Os testes de serviço estão localizados na pasta `teste` (ou no diretório `ProdutosMvc.Tests.Services`, conforme a estrutura do projeto) e foram implementados utilizando as seguintes bibliotecas:

- **XUnit**: Framework para criação e execução dos testes.
- **XUnitVisualStudioRunner**: Runner para execução dos testes no Visual Studio.
- **Moq**: Biblioteca para criação de mocks, permitindo simular dependências e comportamentos.

### Como Executar os testes via Terminal
1. Abra o terminal na raiz do projeto.
2. Execute o comando:
   ```bash
   dotnet test

### Como Executar os testes usando o **XUnitVisualStudioRunner**
1. Abra o projeto.
2. Clique com o botão direito e qualquer uma das classes de testes e logo sem seguida clique na opção `Executar testes`:

1. **Repositories - Localizados no arquivo `ProdutosRepositoryTests.cs`**:
## Testes da Services
O arquivo `ProdutoServiceTests.cs` contém testes para o serviço que gerencia os produtos. Abaixo, um resumo dos testes implementados:

- **GetAllAsync_DeveRetornarTodosOsProdutos**  
  **Objetivo**: Verificar se o método `GetAllAsync` retorna todos os produtos.  
  **Como funciona**:
  - Cria um mock do repositório (`IProdutosRepository`) que retorna uma lista contendo dois produtos.
  - Chama o método `GetAllAsync` do `ProdutoService`.
  - Verifica se o resultado não é nulo, se a quantidade de produtos é igual a 2 e se o método do repositório foi chamado exatamente uma vez.

- **GetByIdAsync_ProdutoExiste_DeveRetornarProduto**  
  **Objetivo**: Validar que o método `GetByIdAsync` retorna o produto correto quando ele existe.  
  **Como funciona**:
  - Configura o mock para retornar um produto específico ao chamar `GetByIdAsync` com o ID 1.
  - Chama o método `GetByIdAsync` do `ProdutoService` com o ID 1.
  - Verifica se o resultado não é nulo, se o produto retornado possui o ID correto e se o método do repositório foi chamado exatamente uma vez.

- **PostAsync_DeveChamarMetodoDoRepositorio**  
  **Objetivo**: Garantir que, ao adicionar um novo produto, o método `PostAsync` do repositório seja chamado.  
  **Como funciona**:
  - Cria uma instância de `Produto` representando um novo produto.
  - Chama o método `PostAsync` do `ProdutoService`.
  - Verifica se o método `PostAsync` do repositório foi invocado exatamente uma vez.

- **PutAsync_DeveChamarMetodoDoRepositorio**  
  **Objetivo**: Confirmar que a atualização de um produto invoca o método `PutAsync` do repositório.  
  **Como funciona**:
  - Cria uma instância de `Produto` com ID e informações atualizadas.
  - Chama o método `PutAsync` do `ProdutoService`.
  - Verifica se o método `PutAsync` do repositório foi invocado exatamente uma vez.

- **DeleteAsync_DeveChamarMetodoDoRepositorio**  
  **Objetivo**: Assegurar que a exclusão de um produto aciona o método `DeleteAsync` do repositório.  
  **Como funciona**:
  - Define um ID (por exemplo, 1) e chama o método `DeleteAsync` do `ProdutoService`.
  - Verifica se o método `DeleteAsync` do repositório foi invocado exatamente uma vez.

2. **Repositories - Localizados no arquivo `ProdutosRepositoryTests.cs`**:
## Testes de Repositório

Os testes do repositório estão localizados na pasta `teste` (ou `ProdutosMvc.Tests.Repositories`, conforme a estrutura do projeto) e foram implementados utilizando as seguintes bibliotecas:

- **XUnit**: Framework para criação e execução dos testes.
- **XUnitVisualStudioRunner**: Runner para execução dos testes no Visual Studio.
- **Moq**: (utilizado nos testes de service, embora aqui o foco seja o uso do EF Core InMemory).
- **Microsoft.EntityFrameworkCore.InMemory**: Para simular um banco de dados real utilizando uma instância em memória.

### Detalhamento dos Testes em `ProdutosRepositoryTests.cs`

O arquivo `ProdutosRepositoryTests.cs` contém testes para o repositório que gerencia os produtos. Abaixo, um resumo dos testes implementados:

- **GetAllAsync_DeveRetornarTodosOsProdutos**  
  **Objetivo**: Verificar se o método `GetAllAsync` retorna todos os produtos presentes no banco.  
  **Como funciona**:
  - Inicialmente, são adicionados dois produtos ao banco de dados em memória.
  - Após salvar as alterações, o método `GetAllAsync` do repositório é chamado.
  - O teste valida se o retorno não é nulo e se a contagem de produtos é igual a 2.

- **GetByIdAsync_ProdutoExiste_DeveRetornarProduto**  
  **Objetivo**: Validar que o método `GetByIdAsync` retorna o produto correto quando este existe.  
  **Como funciona**:
  - Um produto é adicionado ao banco de dados.
  - O método `GetByIdAsync` é chamado com o ID do produto recém-adicionado.
  - O teste confirma que o produto retornado não é nulo e que os valores (como ID e Nome) correspondem ao esperado.

- **PostAsync_DeveAdicionarProdutoNoBanco**  
  **Objetivo**: Garantir que, ao adicionar um novo produto, o método `PostAsync` insere o produto corretamente no banco.  
  **Como funciona**:
  - É criada uma instância de `Produto` representando um novo produto.
  - O método `PostAsync` do repositório é chamado para adicionar esse produto.
  - Após a execução, é realizada uma consulta direta ao contexto para verificar se o produto foi inserido e se seus dados estão corretos.

- **PutAsync_DeveAtualizarProduto**  
  **Objetivo**: Confirmar que a atualização de um produto através do método `PutAsync` reflete as alterações no banco de dados.  
  **Como funciona**:
  - Um produto é adicionado inicialmente ao banco.
  - Em seguida, os dados do produto (como Nome e Preço) são modificados.
  - O método `PutAsync` é chamado para atualizar o produto.
  - O teste verifica se as alterações foram persistidas, consultando o banco de dados em memória.

- **DeleteAsync_DeveRemoverProdutoDoBanco**  
  **Objetivo**: Assegurar que o método `DeleteAsync` remove o produto do banco de dados corretamente.  
  **Como funciona**:
  - Um produto é adicionado ao banco.
  - O método `DeleteAsync` é chamado com o ID do produto.
  - Após a remoção, o teste confirma que o produto não pode ser encontrado no banco, validando a exclusão.

- **ValidateExistProdutoNameAsync_DeveRetornarTrueSeProdutoExiste**  
  **Objetivo**: Verificar se o método `ValidateExistProdutoNameAsync` retorna `true` quando um produto com o nome informado existe no banco.  
  **Como funciona**:
  - Um produto é adicionado ao banco com um nome específico.
  - O método é chamado passando esse nome e o teste valida que o retorno é `true`.

- **ValidateExistProdutoNameAsync_DeveRetornarFalseSeProdutoNaoExiste**  
  **Objetivo**: Confirmar que o método `ValidateExistProdutoNameAsync` retorna `false` quando não há nenhum produto com o nome informado.  
  **Como funciona**:
  - O método é chamado com um nome que não foi cadastrado no banco.
  - O teste valida que o retorno é `false`.

> **Observação**:  
> Os testes de repostories implementa a interfface `IDisposable` para garantir que o banco de dados em memória seja limpo após cada execução de teste.

