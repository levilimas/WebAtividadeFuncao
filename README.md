# Sistema de Clientes e Beneficiários

Implementação de sistema de cadastro e alteração de clientes com gerenciamento de beneficiários, desenvolvido como teste técnico utilizando ASP.NET MVC (.NET Framework) com SQL Server.

---

## Visão Geral

O sistema permite:

- Cadastro de clientes
- Alteração de clientes
- Inclusão, edição e exclusão de beneficiários via modal
- Validação de CPF (cliente e beneficiário)
- Prevenção de duplicidade de CPF
- Persistência transacional entre Cliente e Beneficiários
- Listagem paginada e ordenada de clientes

O foco da implementação foi garantir:

- Integridade de dados
- Separação clara de responsabilidades
- Validação tanto no frontend quanto no backend
- Código legível e de fácil manutenção

---

## Arquitetura

O projeto está estruturado em camadas:

### 1. Camada Web (FI.WebAtividadeEntrevista)

Responsável por:

- Controllers
- Views
- Manipulação de interface com jQuery e Bootstrap
- Comunicação AJAX

### 2. Camada de Negócio (FI.AtividadeEntrevista.BLL)

Responsável por:

- Regras de negócio
- Validações adicionais
- Coordenação de operações entre entidades

### 3. Camada de Dados (FI.AtividadeEntrevista.DML)

Responsável por:

- Acesso ao banco de dados
- Execução de comandos SQL
- Mapeamento de entidades

---

## Decisões Técnicas

### 1. Persistência Transacional

Foi utilizado `TransactionScope` nas operações de inclusão e alteração para garantir consistência entre:

- Cliente
- Beneficiários

Em caso de falha na gravação de qualquer parte, toda a transação é revertida.

---

### 2. Estratégia de Atualização de Beneficiários

Na alteração do cliente, foi adotada a estratégia de:

- Exclusão dos beneficiários existentes
- Regravação da nova lista enviada

Motivos:

- Simplificação da lógica
- Redução de inconsistência
- Evita controle manual de estados (inserido, alterado, removido)

---

### 3. Validação de CPF

Implementada validação completa de CPF utilizando algoritmo de dígitos verificadores:

- Aplicada no frontend (JavaScript)
- Reforçada no backend (C#)

Evita inconsistências e manipulações indevidas.

---

### 4. Prevenção de Duplicidade

Regras implementadas:

- Cliente não pode ter CPF duplicado
- Um cliente não pode possuir dois beneficiários com o mesmo CPF

Validação feita no backend para garantir integridade.

---

### 5. Não Versionamento de Banco (.mdf)

---

## Banco de Dados (.mdf) — Como executar

Este repositório inclui o banco de dados em formato **SQL Server LocalDB (.mdf)**, conforme exigido no teste, para permitir que o avaliador execute o projeto localmente sem necessidade de scripts adicionais.

### Pré-requisitos
- Visual Studio (recomendado)
- SQL Server Express LocalDB instalado (normalmente já vem com Visual Studio)
- (Opcional) SQL Server Management Studio (SSMS) para anexar manualmente

---

## Opção 1 (Recomendada): anexar automaticamente pelo Visual Studio

1. Abra a solution no Visual Studio
2. No menu: **View > SQL Server Object Explorer**
3. Expanda:  
   **(localdb)\MSSQLLocalDB**
4. Clique com o botão direito em **Databases** > **Add New Database...**
5. Selecione o arquivo `.mdf` que está no repositório (pasta do projeto/banco)
6. Confirme e aguarde o banco aparecer na lista de Databases

Após isso, o banco estará anexado e pronto para uso.

---

## Opção 2: anexar manualmente pelo SSMS

1. Abra o **SQL Server Management Studio**
2. Em **Server name**, use:  
   `(localdb)\MSSQLLocalDB`
3. Conecte
4. Clique com o botão direito em **Databases** > **Attach...**
5. Clique em **Add...**
6. Selecione o arquivo `.mdf` do repositório
7. Confirme

---

## Connection String

No arquivo `Web.config`, ajuste a connection string para apontar para o LocalDB e para o banco anexado:

### Exemplo (por nome do catálogo)
```xml
<connectionStrings>
  <add name="FI_TESTE"
       connectionString="Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=FI_TESTE;Integrated Security=True;"
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

### Alternativa (por caminho do MDF)

#### Caso o banco esteja sendo carregado por caminho físico, utilize:

```xml
<connectionStrings>
  <add name="FI_TESTE"
       connectionString="Data Source=(localdb)\MSSQLLocalDB;
                         AttachDbFilename=|DataDirectory|\FI_TESTE.mdf;
                         Integrated Security=True;
                         MultipleActiveResultSets=True;"
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

## Testes Realizados

### Testes manuais cobrindo:

- Inclusão de cliente com múltiplos beneficiários
- Alteração com inclusão e exclusão de beneficiários
- Persistência correta após alteração
  
  [af2b50ff-c5e4-4f70-acf1-375169c2bfb2.webm](https://github.com/user-attachments/assets/2cabd0de-035e-4c9e-8433-15b064cf57b4)


- Validação de CPF inválido e Tentativa de CPF duplicado

  [cd026423-9ee6-4d8b-95e3-b7d26a43272e.webm](https://github.com/user-attachments/assets/b18ac476-d802-4b19-b9c3-5e8e5a0a726a)

  
