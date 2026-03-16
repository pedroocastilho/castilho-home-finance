# castilho-home-finance

Sistema de controle de gastos residenciais desenvolvido com .NET 9 e React 18. 
Permite cadastrar pessoas, categorias e transações financeiras, com consulta 
de totais por pessoa e por categoria.

## Stack

- Backend: .NET 9, C#, Entity Framework Core, SQLite
- Frontend: React 18, TypeScript, Tailwind CSS, Vite
- Testes: xUnit

## Arquitetura

O backend segue Clean Architecture dividida em quatro projetos:

```
Domain         — entidades, enums, interfaces de repositório
Application    — serviços de aplicação e DTOs
Infrastructure — implementação dos repositórios com EF Core
WebApi         — controllers e configuração da aplicação
```

A decisão de separar dessa forma vem de um motivo prático: as regras de negócio ficam no Domain, que não depende de nada externo. Isso significa que os testes unitários rodam sem banco de dados, e trocar o provider de persistência não afeta a lógica da aplicação.

As validações ficam dentro das próprias entidades, seguindo DDD. A entidade `Transaction`, por exemplo, verifica no construtor se a pessoa é menor de idade e se a categoria é compatível com o tipo da transação. Não tem validação espalhada em controller ou serviço.

Existe uma `DomainException` própria para separar erros de regra de negócio de erros de infraestrutura. O controller captura e retorna `400 Bad Request` com a mensagem, enquanto outros erros seguem fluxo normal.

## Regras de negócio implementadas

- Menores de 18 anos só podem registrar despesas
- A finalidade da categoria precisa ser compatível com o tipo da transação. Por exemplo, uma categoria marcada como "Receita" não pode ser usada em uma despesa
- Ao excluir uma pessoa, todas as suas transações são removidas em cascata
- Nome de pessoa: máximo 200 caracteres
- Descrição de categoria e transação: máximo 400 caracteres
- Valor da transação deve ser positivo

## Persistência

Optei por SQLite para facilitar o setup local sem depender de um servidor de banco externo. O arquivo `casafinancas.db` é criado automaticamente na primeira execução via `EnsureCreated` e fica salvo em disco — os dados persistem normalmente após reiniciar a aplicação. Em produção, basta trocar a connection string para SQL Server ou PostgreSQL sem alterar nada no restante do código.

## Frontend

O frontend em React consome a API via axios. No formulário de transação, as categorias disponíveis já são filtradas no cliente conforme o tipo selecionado, então o usuário não consegue nem selecionar uma categoria incompatível. Mesmo assim, o backend valida novamente antes de salvar.

## Como rodar

**Backend**

```bash
cd backend/src/CasaFinancas.WebApi
dotnet run
```

A API sobe em `http://localhost:5000`. O Swagger fica disponível em `http://localhost:5000/swagger`.

**Frontend**

```bash
cd frontend
npm install
npm run dev
```

Acesse `http://localhost:5173`.

**Testes**

```bash
cd backend/tests/CasaFinancas.Tests
dotnet test
```

## Estrutura

```
castilho-home-finance/
├── backend/
│   ├── src/
│   │   ├── CasaFinancas.Domain/
│   │   ├── CasaFinancas.Application/
│   │   ├── CasaFinancas.Infrastructure/
│   │   └── CasaFinancas.WebApi/
│   └── tests/
│       └── CasaFinancas.Tests/
└── frontend/
    └── src/
        ├── pages/
        ├── services/
        └── types/
```
