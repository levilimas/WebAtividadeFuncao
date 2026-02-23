# Sistema de Clientes e Beneficiários

Projeto desenvolvido como teste técnico.

## 📌 Funcionalidades

- Cadastro de clientes
- Validação de CPF (cliente e beneficiário)
- Inclusão, edição e exclusão de beneficiários via modal
- Impede CPF duplicado para cliente
- Persistência em banco SQL Server
- Transação para garantir consistência entre Cliente e Beneficiários

## 🛠 Tecnologias

- ASP.NET MVC (.NET Framework)
- jQuery
- Bootstrap
- SQL Server
- TransactionScope

## 📂 Estrutura

- `FI.AtividadeEntrevista.BLL`
- `FI.AtividadeEntrevista.DML`
- `FI.WebAtividadeEntrevista`

## ▶️ Como executar

1. Restaurar pacotes NuGet
2. Configurar string de conexão no `Web.config`
3. Executar o script de criação do banco
4. Rodar o projeto

## 🔎 Regras implementadas

- CPF validado via algoritmo de dígitos verificadores
- Beneficiário só é persistido ao salvar cliente
- Não permite duplicidade de CPF por cliente
- Operações realizadas dentro de TransactionScope

## 🧪 Testes manuais realizados

- Inclusão de cliente com beneficiários
- Alteração com regravação de beneficiários
- Exclusão de beneficiários
- Validação de CPF inválido
- Tentativa de duplicidade

## 👨‍💻 Autor

Levi Lima
