# SALLIX — Gestão Comercial

SALLIX é um protótipo de gestão comercial com backend em .NET e frontend em Angular.  

## Objetivo
O objetivo do projeto é inserir os conhecimentos de lógica, organização, criatividade e estrutura.

## Pré-requisitos
- .NET SDK 6.0+ — https://dotnet.microsoft.com/download  
- Node.js 16+ e npm — https://nodejs.org/  
- (Opcional) Angular CLI: `npm install -g @angular/cli`  
- Git (opcional)

## Instalação rápida

### Backend (.NET)
1. Abra terminal e vá para a pasta do backend:
   ```bash
   cd d:\Sallix\backend\Sallix
   ```
2. Restaurar dependências:
   ```bash
   dotnet restore
   ```
3. Executar:
   ```bash
   dotnet run
   ```
4. A API será executada em: `https://localhost:7064`. 

### Frontend (Angular)
1. Abra terminal e vá para a pasta do frontend:
   ```bash
   cd d:\Sallix\frontend\app
   ```
2. Instalar dependências:
   ```bash
   npm install
   ```
3. Executar em desenvolvimento:
   ```bash
   ng serve
   # ou
   npm start
   ```
4. Abra no navegador:
   ```
   http://localhost:4200
   ```

## Configuração
- Ajuste a URL da API no frontend (se necessário):  
  Arquivo: `d:\Sallix\frontend\app\src\app\services\api.ts`  
  Exemplo:
  ```ts
  private baseUrl = 'https://localhost:7064/api';
  ```
- O backend espera receber o caminho do arquivo de estoque (geralmente em `Program.cs` ao instanciar `EstoqueService`).

## Exemplo de arquivo de estoque
Altere ou crie um JSON com a propriedade raiz `estoque`. Exemplo (arquivo: `estoque.json`):
```json
{
  "estoque": [
    { "codigoProduto": 1, "descricaoProduto": "Produto A", "estoque": 100 },
    { "codigoProduto": 2, "descricaoProduto": "Produto B", "estoque": 50 }
  ]
}
```
Coloque esse arquivo na pasta apontada pelo backend.

## ✨ Autora

Desenvolvido por **Ruth Novais**  
📧 [ruthcnovais@outlook.com](mailto:ruthcnovais@outlook.com)  
💻 [LinkedIn](https://www.linkedin.com/in/ruthcnovais) • [GitHub](https://github.com/Ruthless-n)

