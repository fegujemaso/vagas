using System;
using System.Collections.Generic;
using System.Linq;

namespace Vagas.Recrutamento.Core.Persistencia
{
    public class _BaseItem
    {
        
        public _BaseItem()
        { }

        protected List<T> ParseDataTable<T>(System.Data.DataTable dataTable, Dictionary<string, string> dicionario) where T : new()
        {
            var lista = new List<T>();

            if (dataTable != null)
                foreach (var dataRow in dataTable.Rows)
                {
                    var item = this.ParseDataRow<T>((System.Data.DataRow)dataRow, dicionario);

                    lista.Add(item);
                }

            return lista;
        }

        protected T ParseDataRow<T>(System.Data.DataRow dataRow, Dictionary<string, string> dicionario) where T : new()
        {
            var entidade = new T();

            var propriedadeLista = entidade.GetType().GetProperties();

            foreach (var propriedadeItem in propriedadeLista)
            {
                var dicionarioItem = dicionario
                    .Where(x => x.Key.Equals(propriedadeItem.Name))
                    .FirstOrDefault();

                if (propriedadeItem != null && propriedadeItem.CanWrite)
                {
                    var propriedadeNome = string.Empty;

                    if (dicionarioItem.Value == null)
                        propriedadeNome = propriedadeItem.Name;
                    else
                        propriedadeNome = dicionarioItem.Value;

                    if (!dataRow.Table.Columns.Contains(propriedadeNome))
                        continue;

                    if (dataRow.IsNull(propriedadeNome) && !propriedadeItem.PropertyType.Name.Equals("String"))
                        continue;

                    var propriedadeValor = Convert.ChangeType(dataRow[propriedadeNome], propriedadeItem.PropertyType);

                    if (propriedadeItem.PropertyType.Name.Equals("String") && string.IsNullOrEmpty((string)propriedadeValor))
                        propriedadeValor = string.Empty;

                    propriedadeItem.SetValue(entidade, propriedadeValor, null);
                }
            }

            return entidade;
        }

        protected List<T> CarregarLista<T>(Vagas.Database.Interface.IDatabase database, string sql, Dictionary<string, string> dicionario) where T : new()
        {
            var dataTable = database.ExecutarRetornandoDataTable(sql);

            return this.ParseDataTable<T>(dataTable, dicionario);
        }

        protected T CarregarItem<T>(Vagas.Database.Interface.IDatabase database, string sql, Dictionary<string, string> dicionario) where T : new()
        {
            var dataTable = database.ExecutarRetornandoDataTable(sql);

            var lista = this.ParseDataTable<T>(dataTable, dicionario);

            return lista.FirstOrDefault();
        }

        protected System.Data.DataTable CarregarDataTable(Vagas.Database.Interface.IDatabase database, string sql)
        {
            return database.ExecutarRetornandoDataTable(sql);
        }

        protected string PrepararSqlClob(string textoOriginal)
        {
            var sqlClob = "";

            if (textoOriginal.Length >= 4000)
            {
                var tamanhoTotal = textoOriginal.Length;

                var tamanhoCopiado = 0;

                while (tamanhoTotal > tamanhoCopiado)
                {
                    var sqlParcial = string.Empty;

                    if ((tamanhoTotal - tamanhoCopiado) >= 4000)
                        sqlParcial = textoOriginal.Substring(tamanhoCopiado, 4000);
                    else
                        sqlParcial = textoOriginal.Substring(tamanhoCopiado);

                    sqlClob += "TO_CLOB('" + sqlParcial.Replace("'", "''") + "') || ";

                    tamanhoCopiado += sqlParcial.Length;
                }

                sqlClob = sqlClob.Substring(0, sqlClob.Length - 3) + " \n";
            }
            else if (string.IsNullOrEmpty(textoOriginal))
            {
                sqlClob += "    NULL \n";
            }
            else
            {
                sqlClob += "    TO_CLOB('" + textoOriginal.Replace("'", "''") + "') \n";
            }

            return sqlClob;
        }

    }
}
