using System;
using System.Collections.Generic;
using System.Linq;

namespace Vagas.Recrutamento.Core.Persistencia.Vaga
{
    public class VagaItem : _BaseItem, Interface.Vaga.IVagaItem
    {
        #region Propriedades 

        private string _connectionString { get; set; }

        private Dictionary<int, Entidade.Vaga.VagaItem> _vagaLista { get; set; }

        #endregion 

        #region Construtores 

        public VagaItem()
            : this("")
        { }

        public VagaItem(string connectionString)
        {
            this._connectionString = connectionString;

            this._vagaLista = new Dictionary<int, Entidade.Vaga.VagaItem>();
        }

        #endregion

        #region Métodos Públicos 

        public List<Entidade.Vaga.VagaItem> CarregarLista()
        {
            var lista = this._vagaLista
                .Select(x => x.Value)
                .ToList();

            return lista;

            //ADAPTAÇÃO PARA USO DE SQL SERVER (ou outra base, porém é necessário adaptar o sql em alguns pontos);
            var databaseItem = new Vagas.Database.DatabaseItem();

            var sql = this.PrepararSelecaoSql();

            var dicionario = this.ObterDicionarioSelecaoSql();

            return base.CarregarLista<Entidade.Vaga.VagaItem>(databaseItem, sql, dicionario);
        }

        public Entidade.Vaga.VagaItem CarregarItem(int vagaId)
        {
            var vagaItem = null as Entidade.Vaga.VagaItem;

            this._vagaLista.TryGetValue(vagaId, out vagaItem);

            return vagaItem;

            //ADAPTAÇÃO PARA USO DE SQL SERVER (ou outra base, porém é necessário adaptar o sql em alguns pontos);
            var databaseItem = new Vagas.Database.DatabaseItem();

            var sql = this.PrepararSelecaoSql(vagaId);

            var dicionario = this.ObterDicionarioSelecaoSql();

            var retorno = base.CarregarItem<Entidade.Vaga.VagaItem>(databaseItem, sql, dicionario);

            return retorno;
        }

        public Entidade.Vaga.VagaItem InserirItem(Entidade.Vaga.VagaItem vagaItem)
        {
            var vagaId = this._vagaLista.Count + 1;

            vagaItem.Id = vagaId;

            vagaItem.DataInclusao = DateTime.Now;

            var sucesso = this._vagaLista.TryAdd(vagaId, vagaItem);

            if (!sucesso)
                throw new InvalidOperationException("Não foi possível inserir devido a problemas técnicos");

            return vagaItem;

            //ADAPTAÇÃO PARA USO DE SQL SERVER (ou outra base, porém é necessário adaptar o sql em alguns pontos);
            var databaseItem = new Vagas.Database.DatabaseItem();

            var dicionario = this.ObterDicionarioSelecaoSql();

            var sql = this.PrepararInsercaoSql(vagaItem);

            sql += this.ObterUltimoItemInseridoSql();

            return base.CarregarItem<Entidade.Vaga.VagaItem>(databaseItem, sql, dicionario);
        }

        public Entidade.Vaga.VagaItem AtualizarItem(Entidade.Vaga.VagaItem vagaItem)
        {
            var sucesso = this._vagaLista[vagaItem.Id] = vagaItem;

            return vagaItem;

            //ADAPTAÇÃO PARA USO DE SQL SERVER (ou outra base, porém é necessário adaptar o sql em alguns pontos);
            var databaseItem = new Vagas.Database.DatabaseItem();

            var sql = this.PrepararAtualizacaoSql(vagaItem);

            sql += this.PrepararSelecaoSql(vagaItem.Id);

            var dicionario = this.ObterDicionarioSelecaoSql();

            return base.CarregarItem<Entidade.Vaga.VagaItem>(databaseItem, sql, dicionario);
        }

        public Entidade.Vaga.VagaItem ExcluirItem(Entidade.Vaga.VagaItem vagaItem)
        {
            var sucesso = this._vagaLista.Remove(vagaItem.Id);

            return vagaItem;

            //ADAPTAÇÃO PARA USO DE SQL SERVER (ou outra base, porém é necessário adaptar o sql em alguns pontos);
            var databaseItem = new Vagas.Database.DatabaseItem();

            var sql = this.PrepararExclusaoSql(vagaItem);

            var dicionario = this.ObterDicionarioSelecaoSql();

            return base.CarregarItem<Entidade.Vaga.VagaItem>(databaseItem, sql, dicionario);
        }

        #endregion 

        #region Métodos Privados 

        private Dictionary<string, string> ObterDicionarioSelecaoSql()
        {
            var dicionario = new Dictionary<string, string>();

            dicionario.Add("Id", "VAGA_ID");
            dicionario.Add("DataInclusao", "DATA_INCLUSAO");
            dicionario.Add("DataAlteracao", "DATA_ALTERACAO");
            dicionario.Add("Empresa", "EMPRESA");
            dicionario.Add("Titulo", "TITULO");
            dicionario.Add("Descricao", "DESCRICAO");
            dicionario.Add("Localizacao", "LOCALIZACAO");
            dicionario.Add("Nivel", "NIVEL");

            return dicionario;
        }

        private string PrepararSelecaoSql()
        {
            var sql = "";

            sql += "SELECT \n";
            sql += "    A.VAGA_ID,\n";
            sql += "    A.DATA_INCLUSAO,\n";
            sql += "    A.DATA_ALTERACAO,\n";
            sql += "    A.EMPRESA,\n";
            sql += "    A.TITULO,\n";
            sql += "    A.DESCRICAO,\n";
            sql += "    A.LOCALIZACAO,\n";
            sql += "    A.NIVEL\n";
            sql += "FROM \n";
            sql += "    VAGA_TB A\n";

            return sql;
        }

        private string PrepararSelecaoSql(int? vagaId)
        {
            var sql = "";

            if (vagaId.HasValue)
                sql += "A.VAGA_ID = " + vagaId.Value + "\n";

            if (!string.IsNullOrEmpty(sql))
            {
                sql = sql.Substring(0, sql.Length - 1);

                sql = sql.Replace("\n", "\nAND ");

                sql = " WHERE\n\t" + sql;
            }

            sql = this.PrepararSelecaoSql() + " " + sql;

            return sql;
        }

        private string PrepararInsercaoSql(Entidade.Vaga.VagaItem vagaItem)
        {
            var sql = string.Empty;

            sql += "INSERT INTO VAGA_TB(\n";
            sql += "    EMPRESA,\n";

            sql += "    TITULO,\n";

            sql += "    DESCRICAO,\n";

            sql += "    LOCALIZACAO,\n";

            sql += "    NIVEL,\n";

            sql = sql.Substring(0, sql.Length - 2) + "\n";

            sql += ") VALUES (\n";
            sql += "    '" + vagaItem.Empresa.Replace("'", "''") + "',\n";

            sql += "    '" + vagaItem.Titulo.Replace("'", "''") + "',\n";

            sql += "    '" + vagaItem.Descricao.Replace("'", "''") + "',\n";

            sql += "    '" + vagaItem.Localizacao.Replace("'", "''") + "',\n";

            sql += "    " + vagaItem.Nivel.ToString() + ",\n";

            sql = sql.Substring(0, sql.Length - 2) + "\n";

            sql += ");\n";

            return sql;
        }

        private string PrepararAtualizacaoSql(Entidade.Vaga.VagaItem vagaItem)
        {
            var sql = string.Empty;

            sql += "UPDATE \n";
            sql += "    A\n";
            sql += "SET\n";
            sql += "    A.DATA_ALTERACAO = CURRENT_TIMESTAMP,\n";

            sql += "    A.EMPRESA = '" + vagaItem.Empresa.Replace("'", "''") + "',\n";

            sql += "    A.TITULO = '" + vagaItem.Titulo.Replace("'", "''") + "',\n";

            sql += "    A.DESCRICAO = '" + vagaItem.Descricao.Replace("'", "''") + "',\n";

            sql += "    A.LOCALIZACAO = '" + vagaItem.Localizacao.Replace("'", "''") + "',\n";

            sql += "    A.NIVEL = " + vagaItem.Nivel.ToString() + ",\n";

            sql = sql.Substring(0, sql.Length - 2) + "\n";

            sql += "FROM\n";
            sql += "    VAGA_TB A\n";
            sql += "WHERE\n";
            sql += "    A.VAGA_ID = " + vagaItem.Id + "\n";
            return sql;
        }

        private string PrepararExclusaoSql(Entidade.Vaga.VagaItem vagaItem)
        {
            var sql = string.Empty;

            sql += "DELETE \n";
            sql += "    A\n";
            sql += "FROM\n";
            sql += "    VAGA_TB A\n";
            sql += "WHERE\n";
            sql += "    A.VAGA_ID = " + vagaItem.Id + "\n";
            return sql;
        }

        #endregion

        #region Métodos Específicos do Banco

        private string ObterUltimoItemInseridoSql()
        {
            var sql = this.PrepararSelecaoSql();

            sql += "WHERE \n";

            sql += "    A.VAGA_ID = SCOPE_IDENTITY()\n";

            return sql;
        }

        #endregion
    }
}
