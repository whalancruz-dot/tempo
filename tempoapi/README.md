🚀 Guia de Inicialização - TempoApi

Este guia contém o passo a passo necessário para configurar o banco de dados e colocar a aplicação em execução.


📋 Pré-requisitos

Antes de começar, certifique-se de ter instalado:

    .NET SDK (Versão 8.0 ou superior recomendada)

    SQL Server ou o banco de dados configurado no seu appsettings.json.

    VS Code (opcional, mas recomendado).


🛠️ Passo a Passo

1. Configuração do Banco de Dados

A aplicação depende de uma tabela específica para gerenciar os dados.

    Vá até a pasta ScriptSQL na raiz do projeto.

    Execute o script de criação de tabela no banco de dados master (ou no banco especificado em sua Connection String no arquivo appsettings.json).    


2. Compilação do Projeto

Com o banco configurado, abra o terminal na pasta raiz do projeto e prepare os binários:
CMD: dotnet build



3. Execução da API

Você tem duas formas principais de subir o servidor:

Opção A: Pelo VS Code (Recomendado para Debug)

    No menu lateral esquerdo, clique no ícone Run and Debug (ou pressione Ctrl+Shift+D).

    No topo do menu, selecione a configuração ".NET Core Launch (web)".

    Clique no botão Play (verde) ou pressione F5.

Opção B: Pelo Terminal

Execute o comando abaixo para subir a aplicação rapidamente:  
CMD: dotnet run 
Acesse http://localhost:5185/swagger 