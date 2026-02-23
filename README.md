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

Apesar do teste fornecer um `.mdf`, optou-se por não versionar o arquivo por boas práticas:

- Arquivos físicos de banco são dependentes de ambiente
- Dificultam portabilidade
- Geram conflitos desnecessários

Foi incluído script SQL para recriação do banco.

---

## Banco de Dados

O sistema utiliza SQL Server LocalDB.

### Script de Criação

Execute no SQL Server:

```sql
CREATE DATABASE FI_TESTE;
GO

USE FI_TESTE;
GO

CREATE TABLE CLIENTES (
    ID BIGINT IDENTITY(1,1) PRIMARY KEY,
    NOME VARCHAR(50) NOT NULL,
    SOBRENOME VARCHAR(255) NOT NULL,
    CPF VARCHAR(11) NOT NULL,
    NACIONALIDADE VARCHAR(50) NOT NULL,
    CEP VARCHAR(9) NOT NULL,
    ESTADO VARCHAR(2) NOT NULL,
    CIDADE VARCHAR(50) NOT NULL,
    LOGRADOURO VARCHAR(500) NOT NULL,
    EMAIL VARCHAR(2079) NOT NULL,
    TELEFONE VARCHAR(15) NOT NULL
);

CREATE TABLE BENEFICIARIOS (
    ID BIGINT IDENTITY(1,1) PRIMARY KEY,
    IDCLIENTE BIGINT NOT NULL,
    CPF VARCHAR(11) NOT NULL,
    NOME VARCHAR(100) NOT NULL,
    CONSTRAINT FK_BENEFICIARIO_CLIENTE
        FOREIGN KEY (IDCLIENTE)
        REFERENCES CLIENTES(ID)
        ON DELETE CASCADE
);
