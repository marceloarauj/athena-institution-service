# Athena Institution Service

Microsserviço escolar da plataforma Athena Students Union, responsável pelo domínio educacional: turmas, aulas, disciplinas, notas e frequência.

## Visão geral

É o núcleo operacional da plataforma. Gerencia toda a estrutura acadêmica de cada instituição — da configuração de grades curriculares ao lançamento de notas com fórmulas de avaliação customizadas. Gera automaticamente o OpenAPI para o portal de documentação.

## Funcionalidades

- **Disciplinas** — criação e gerenciamento com carga horária, créditos e tópicos
- **Turmas (Grades)** — séries/turmas com períodos letivos e matrículas
- **Aulas (Classes)** — agendamento, salas, professores e chamada
- **Notas** — lançamento com suporte a fórmulas NCalc customizadas por instituição
- **Frequência** — registro por aula com cálculo de percentual
- **Professores** — cadastro, disponibilidade e associação de disciplinas
- **Calendário acadêmico** — feriados, recessos e edições de programa
- **Arquivos** — upload de materiais via S3 (LocalStack em desenvolvimento)

## Stack

| Camada | Tecnologia |
|---|---|
| Framework | ASP.NET Core 10 |
| Banco de dados | PostgreSQL (porta 5455) |
| Armazenamento | AWS S3 / LocalStack |
| Avaliação de fórmulas | NCalc |
| ORM | EF Core 10 |
| Arquitetura | Clean Architecture + CQRS (Mediator customizado) |

## Estrutura

```
Institution/                   # Controllers, Program.cs
Institution.Application/       # CQRS, Auth, DTOs
Institution.Domain/            # Entidades do domínio educacional
Institution.Infrastructure/    # EF, repositórios, S3, NCalc
Institution.Tests/             # Testes unitários (xUnit)
```

## Repositórios relacionados

| Serviço | Repositório |
|---|---|
| Frontend | [athena-students-union-front](https://github.com/marceloarauj/athena-students-union-front) |
| Identidade | [athena-identity](https://github.com/marceloarauj/athena-identity) |
| Backend de IA | [athena-union-ai](https://github.com/marceloarauj/athena-union-ai) |
| Documentação | [athena-docs](https://github.com/marceloarauj/athena-docs) |
| Biblioteca compartilhada | [AthenaUnionLibrary](https://github.com/marceloarauj/AthenaUnionLibrary) |
