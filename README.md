# NotifyMe 🔔

NotifyMe é um microserviço de notificações simples, elegante e funcional construído com **.NET 9 Web API** e **Minimal APIs**. Ele permite o envio de emails via SMTP e rastreia o histórico de todas as notificações enviadas em um banco de dados SQLite local.

## 🚀 Funcionalidades

-   **Envio de Emails**: API simples para enviar emails usando SMTP (MailKit).
-   **Histórico**: Registra automaticamente todas as tentativas de envio (sucesso ou falha) em um banco de dados SQLite.
-   **Frontend Minimalista**: Uma interface de página única limpa construída com **PicoCSS** para testes instantâneos.
-   **Modo Mock**: Simula o envio de emails para desenvolvimento sem a necessidade de um servidor SMTP real.
-   **Arquitetura Limpa**: Separação de responsabilidades com Models, Data e Services.

## 🛠️ Stack Tecnológica

-   **Backend**: .NET 9 (C#), Minimal APIs
-   **Banco de Dados**: Entity Framework Core + SQLite
-   **Email**: MailKit
-   **Frontend**: HTML5, Vanilla JS, PicoCSS (No-Build)

## 📋 Pré-requisitos

-   [.NET SDK](https://dotnet.microsoft.com/download) (versão mais recente)

## ⚙️ Configuração

A aplicação é configurada através do arquivo `appsettings.json`.

### Configuração SMTP
Para enviar emails reais, configure seu provedor SMTP (ex: Mailersend, SendGrid, Gmail).

```json
"Smtp": {
  "UseMock": false,
  "Host": "smtp.mailersend.net",
  "Port": "587",
  "Username": "seu-usuario",
  "Password": "sua-senha",
  "From": "seu-remetente-verificado@dominio.com"
}
```

### Modo Mock (Desenvolvimento)
Se você não tiver um servidor SMTP pronto, defina `"UseMock": true`. O serviço simulará um atraso de 1 segundo e registrará uma mensagem de sucesso.

```json
"Smtp": {
  "UseMock": true,
  ...
}
```

## 🏃‍♂️ Como Rodar

1.  **Clone o repositório** (ou navegue até a pasta).
2.  **Compile o projeto**:
    ```bash
    dotnet build
    ```
3.  **Execute a aplicação**:
    ```bash
    dotnet run --project NotifyMe/NotifyMe.csproj
    ```
4.  **Acesse o App**:
    Abra seu navegador e vá para a URL mostrada no console (geralmente `https://localhost:7045` ou `http://localhost:5167`).

## 🔌 Endpoints da API

### `POST /api/notify`
Envia uma notificação por email.

**Corpo da Requisição:**
```json
{
  "to": "destinatario@exemplo.com",
  "subject": "Olá",
  "body": "Esta é uma mensagem de teste."
}
```

### `GET /api/history`
Retorna o histórico de emails enviados (ordenado do mais recente para o mais antigo).

**Resposta:**
```json
[
  {
    "id": 1,
    "recipient": "destinatario@exemplo.com",
    "subject": "Olá",
    "sentAt": "2023-10-27T10:00:00Z",
    "success": true,
    "errorMessage": null
  }
]
```

## 📂 Estrutura do Projeto

```
NotifyMe/
├── Data/           # Contexto do DB (EF Core)
├── Models/         # Modelos de Domínio (EmailRequest, EmailLog)
├── Services/       # Lógica de Negócio (EmailService)
├── wwwroot/        # Arquivos Estáticos (Frontend)
├── Program.cs      # Ponto de Entrada do App e Configuração de DI
└── appsettings.json # Configuração
```

## 📄 Licença

Este projeto é para fins de portfólio/educacionais.
