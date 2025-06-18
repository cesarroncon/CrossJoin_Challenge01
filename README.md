# CRM System em C# (.NET 7)

Este projeto é um sistema básico de CRM (Customer Relationship Management) desenvolvido em C# com .NET 7. Ele organiza a gestão de **companies**, **leads**, **proposals** e **products**, com funcionalidades completas para criação, atualização, associação e finalização dessas entidades.

---

## Estrutura do Projeto

O projeto está organizado em vários files que representam as principais entidades do sistema:

- **Companies:** Gestão de empresas (criação, atualização).
- **Leads:** Gestão de leads, que podem ser convertidos em propostas.
- **Proposals:** Propostas comerciais ligadas às leads e às companies.
- **Products:** Produtos que podem ser adicionados às propostas ou diretamente ao sistema.
- **DataRepository:**  Guarda as entidades em listas
- **Rules:** Regras que podem ser definidas em runtime.

---

## Funcionamento Principal

O ponto de entrada do programa é o arquivo `Program.cs`, que chama o terminal de interação. O usuário escolhe a ação desejada através de um **switch**, podendo executar:

- Criar ou atualizar uma **company**
- Criar ou atualizar uma **lead**
- Criar ou atualizar um **product**
- Converter uma **lead** num **proposal**
- Atualizar um **proposal**
- Adicionar **products** a um **proposal**
- Finalizar um **proposal** se todos os dados estiverem preenchidos

---

## Funcionalidades Importantes

### Produtos e Dependências

- Produtos com dependências ou dependentes têm que ter o mesmo tipo, para isso sempre que se atualiza o tipo de um produto, é preciso atualizar todos os correspondentes.
- Para tratar de dependências, há uma função **recursiva** que garante que todos os produtos dependentes sejam corretamente processados.
- Essa função é utilizada tanto na atualização de produtos quanto na adição de produtos às propostas.

### Validações

- O sistema inclui validações dinâmicas, com o uso de `setRequired`.
- Validações garantem integridade dos dados e consistência das operações realizadas.

---

## Testes

Este repositório inclui um conjunto fechado de testes automatizados que cobrem:

- Testes de todas as classes
- Verificação da integridade dos dados e referências
- Testes das funções recursivas para dependências de produtos
- Validações dinâmicas

---

## Tecnologias

- Linguagem: C#
- Framework: .NET 7
- Teste: xUnit
- IDE: Visual Studio Code

---

## Como usar

No terminal:
1. Clone o repositório para um direterio pessoal.
   "git clone https://github.com/cesarroncon/CrossJoin_Challenge01.git"
4. Entre na pasta "Challenge".
    "cd .\CrossJoin_Challenge01\Challenge\"
6. Build.
   "dotnet build"
7. Test
   "dotnet test"
8. Run
   "dotnet run"

---
## Testes

- Dentro do ficheiro **testclass.cs** está explicado detalhadamente os testes realizados, em cometários. 
- Os testes abordam  **rule validation and entity evolution**


**Autor:** César Roncon

