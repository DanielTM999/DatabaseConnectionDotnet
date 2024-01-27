using DatabaseConnection.conn.exceptions;
using DatabaseConnection.core;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConnection.conn
{
    public class DbActions : ConnectionMaker, IDbAction
    {
        public List<T> Execute<T>(string sql, Dictionary<string, object>? bindParams, DbConnection connection) where T : new()
        {
            List<Dictionary<string, object?>> results = new List<Dictionary<string, object?>>();
            isEntity(typeof(T));
            results = queryRes(sql, bindParams, connection);
            return Format<T>(results);
        }
        public List<Dictionary<string, object?>> ExecuteNoFormat(string sql, Dictionary<string, object>? bindParams, DbConnection connection)
        {
            return queryRes(sql, bindParams, connection);
        }
        public bool ExecuteNoResponse(string sql, Dictionary<string, object>? bindParams, DbConnection connection)
        {
            return NoReturn(sql, bindParams, connection);
        }
        
        private List<Dictionary<string, object?>> queryRes(string sql, Dictionary<string, object>? bindParams, DbConnection connection)
        {
            List<Dictionary<string, object?>> results = new List<Dictionary<string, object?>>();
            OpenIfNecessary(connection);
            using (DbCommand command = connection.CreateCommand())
            {
                command.CommandText = sql;
                command.CommandType = CommandType.Text;

                if (bindParams != null)
                {
                    foreach (var param in bindParams)
                    {
                        DbParameter parameter = command.CreateParameter();
                        parameter.ParameterName = param.Key;
                        parameter.Value = param.Value;
                        command.Parameters.Add(parameter);
                    }
                }

                using (DbDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Dictionary<string, object?> row = new Dictionary<string, object?>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string nomeColuna = reader.GetName(i);
                            object? valorColuna = reader[i];
                            row.Add(nomeColuna, valorColuna);
                        }
                        results.Add(row);
                    }
                }
            }
            return results;
        }

        private bool NoReturn(string sql, Dictionary<string, object>? bindParams, DbConnection connection) {
            List<Dictionary<string, object?>> results = new List<Dictionary<string, object?>>();
            OpenIfNecessary(connection);
            using (DbCommand command = connection.CreateCommand())
            {
                command.CommandText = sql;
                command.CommandType = CommandType.Text;

                if (bindParams != null)
                {
                    foreach (var param in bindParams)
                    {
                        DbParameter parameter = command.CreateParameter();
                        parameter.ParameterName = param.Key;
                        parameter.Value = param.Value;
                        command.Parameters.Add(parameter);
                    }
                }
                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }
        
        private void isEntity(Type t)
        {
            var atribute = t.GetCustomAttribute(typeof(EntityModel));
            if(atribute == null)
            {
                throw new ModelException($"Classe '{t.Name}' não é um modelo");
            }
        }

        private void OpenIfNecessary(DbConnection connection)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
        }

        private List<T> Format<T>(List<Dictionary<string, object?>> result) where T : new()
        {
            List<T> formatedRes = new List<T>();
            Type mainEntity = typeof(T);

            for (int i = 0; i < result.Count; i++)
            {
                T instance = new T();
                var props = mainEntity.GetProperties().ToList<PropertyInfo>();
                props.ForEach(p =>
                {
                    string key = atributeName(p);
                    if (isTerminal(p))
                    {
                        if (result[i].TryGetValue(key, out var value))
                        {
                            p.SetValue(instance, value);
                        }
                    }else if (p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                    {
                        Type listType = p.PropertyType.GetGenericArguments()[0];
                        if (result[i].TryGetValue(key, out var value))
                        {
                            IList lista = getList(value, key, result, listType);
                            p.SetValue(instance, lista);
                        }
                    }
                    else
                    {
                        if (!p.PropertyType.Equals(typeof(T)))
                        {
                            object value = formatClass(p.PropertyType, result[i], result);
                            p.SetValue(instance, value);
                        }
                    }
                });
                formatedRes.Add(instance);
            }

            return formatedRes;
        }

        private object formatClass(Type classe, Dictionary<string, object?> res, List<Dictionary<string, object?>> result)
        {
            object instance = Activator.CreateInstance(classe);
            var props = classe.GetProperties().ToList<PropertyInfo>();
            props.ForEach(p =>
            {
                string key = p.Name.ToLower();
                if (isTerminal(p))
                {
                    if (res.TryGetValue(key, out var value))
                    {
                        p.SetValue(instance, value);
                    }
                }
                else if (p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    Type listType = p.PropertyType.GetGenericArguments()[0];
                    if (res.TryGetValue(key, out var value))
                    {
                        IList lista = getList(value, key, result, listType);
                        p.SetValue(instance, lista);
                    }
                }
                else
                {
                    if (!p.PropertyType.Equals(classe))
                    {
                        object value = formatClass(p.PropertyType, res, result);
                        p.SetValue(instance, value);
                    }
                }
            });

            return instance;
        }

        private IList getList(object val, string propName ,List<Dictionary<string, object?>> result, Type listType)
        {
            IList newList = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(listType));
            Parallel.ForEach(result, item =>
            {
                if(item[propName] != null && item[propName].Equals(val))
                {
                    newList.Add(formatClass(listType, item, result));
                }
            });

            return newList;
        }

        private bool isTerminal(PropertyInfo prop)
        {
            return prop.PropertyType.IsPrimitive || prop.PropertyType == typeof(string);
        }

        private string atributeName(PropertyInfo p)
        {
            var atribute = p.GetCustomAttribute(typeof(PropName)) as PropName;
            if(atribute == null)
            {
                return p.Name.ToLower();
            }
            else
            {
                return atribute.Name;
            }
        }

    }
}
