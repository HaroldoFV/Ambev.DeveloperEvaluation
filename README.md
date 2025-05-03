# Developer Evaluation Project


## Tecnologias Utilizadas

- **.NET 8**: Framework de desenvolvimento para a criação de aplicações web e APIs.
- **PostgreSQL**: Sistema gerenciador de banco de dados relacional.
- **Docker**: Plataforma de conteinerização para facilitar a implantação e execução do ambiente de desenvolvimento e
  produção.
- **RabbitMQ**: Sistema de mensageria para comunicação assíncrona entre serviços.


## Informações do Banco de Dados
- Database: developer_evaluation
- User: developer
- Tables:
    - Users
    - Sales
    - SaleItems
    - __EFMigrationsHistory

## Estrutura do Projeto
- `/backend` - Backend API
    - `/src/sql` - Database initialization scripts
  
## Como Executar

O projeto pode ser executado utilizando Docker e Docker Compose, garantindo assim, que o ambiente de desenvolvimento
seja fácil de configurar e replicar. Siga as instruções abaixo para configurar o ambiente:

### Pré-requisitos

- Docker
- Docker Compose

### Instruções de Execução

1. Clone o repositório do projeto para sua máquina local.

2. Navegue até a pasta raiz do projeto, onde está localizado o arquivo `docker-compose.yml`.

3. Execute o seguinte comando para construir e iniciar os contêineres do serviço e do banco de dados:

   ```bash
   docker-compose up --build

4. Após a inicialização dos containers, a aplicação estará acessível através do navegador ou cliente de API no seguinte
   endereço:

    ```
    http://localhost:8080/swagger/index.html
    ```
### Testando Endpoints da API

O projeto inclui arquivos `.http` que podem ser usados para testar os endpoints da API diretamente do seu IDE (como Rider ou Visual Studio Code). Estes arquivos estão localizados em:
- `src/Ambev.DeveloperEvaluation.WebApi` - Endpoints relacionados a usuários e vendas

Para usar estes arquivos:
1. Abra-os no seu IDE
2. Clique no link "Send Request" acima de cada requisição
3. Visualize a resposta diretamente no IDE

### ⚠️ Observação sobre arquivos sensíveis
Os arquivos appsettings.json e .env foram incluídos no versionamento exclusivamente por se tratar de um teste técnico.
Em ambientes reais de produção, segredos e configurações sensíveis nunca devem ser versionados — o ideal é utilizar variáveis de ambiente ou ferramentas como Azure Key Vault, AWS Secrets Manager ou o dotnet user-secrets durante o desenvolvimento local.


### Implementações Pendentes

### 1. Modelagem de Entidades
- Entidade `Categoria`
- Entidade `Produto`
- Entidade `Cliente`
- Relacionamentos entre estas entidades e as já existentes

### 2. Eventos na Entidade User
- Implementação dos eventos de domínio para a entidade User
- Tratamento dos eventos relacionados ao ciclo de vida do usuário

### 3. Padrões de API
Na camada `src/Ambev.DeveloperEvaluation.WebApi`:
- Refatorar para seguir o padrão existente no projeto
- Remover uso direto dos commands/results da camada de application

### 4. Testes Pendentes

#### Testes de Integração
- Testes de integração com banco de dados
- Testes de integração entre camadas
- Testes de fluxos completos de negócio
- Testes de persistência

#### Testes End-to-End
- Testes dos endpoints da API
- Testes de fluxos completos via API
- Testes de cenários de erro
- Testes de performance
