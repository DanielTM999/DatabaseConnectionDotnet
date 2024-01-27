# Database Connection Library

Esta biblioteca fornece uma série de classes para facilitar a execução de operações em um banco de dados usando ADO.NET.

## Funcionalidades Principais

### `DbActions` - Classe principal para execução de ações no banco de dados


```csharp

    IDbAction actions = new DbActions();

```

### `ConnectionMaker` -  Classe principal para execução de criação de conxão no no banco de dados

```csharp

    DbConnection con = action.GenerateConnection(ConnectionFactory => ConnectionFactory
        .TypeDb(DbTypes.SeuTypoDeBANCO) //SqlServer,PostgreSql,Mysql
        .Host("SeuHost")
        .Dbname("SeuBanco")
        .User("SeuUsuario")
        .Password("SuaSenha")
    );

```

#### Método `Execute<T>` - Executa uma consulta no banco de dados e retorna os resultados formatados como uma lista de objetos do tipo especificado.

```csharp

    List<T> Execute<T>(string sql, Dictionary<string, object>? bindParams, DbConnection connection);

```
#### Método `ExecuteNoResponse` - Executa uma consulta no banco de dados para dados que não tem retorno e retorna True se a consulta foi bem sucedida e false caso falhe.

```csharp

    bool ExecuteNoResponse(string sql, Dictionary<string, object>? bindParams, DbConnection connection);

```
