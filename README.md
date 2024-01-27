# Database Connection Library

Esta biblioteca fornece uma série de classes para facilitar a execução de operações em um banco de dados usando ADO.NET.

## Funcionalidades Principais

### `DbActions` - Classe principal para execução de ações no banco de dados

#### Método `Execute<T>` - Executa uma consulta no banco de dados e retorna os resultados formatados como uma lista de objetos do tipo especificado.

```csharp
    Execute<T>(string sql, Dictionary<string, object>? bindParams, DbConnection connection);
```
