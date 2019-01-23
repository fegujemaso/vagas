using System;
using System.Collections.Generic;
using System.Linq;

namespace Vagas.Recrutamento.Core.Persistencia.Pessoa
{
    public class PessoaItem : _BaseItem, Interface.Pessoa.IPessoaItem
    { 
        #region Propriedades 

        private string _connectionString { get; set; }

        private Dictionary<int, Entidade.Pessoa.PessoaItem> _pessoaLista { get; set; }

        #endregion 

        #region Construtores 

        public PessoaItem() 
            : this("") 
        { } 

        public PessoaItem(string connectionString) 
        { 
            this._connectionString = connectionString;

            this._pessoaLista = new Dictionary<int, Entidade.Pessoa.PessoaItem>();
        }

        #endregion

        #region Métodos Públicos 

        public List<Entidade.Pessoa.PessoaItem> CarregarLista()
        {
            var lista = this._pessoaLista
                .Select(x => x.Value)
                .ToList();

            return lista;

            //ADAPTAÇÃO PARA USO DE SQL SERVER (ou outra base, porém é necessário adaptar o sql em alguns pontos);
            var databaseItem = new Vagas.Database.DatabaseItem();

            var sql = this.PrepararSelecaoSql();

            var dicionario = this.ObterDicionarioSelecaoSql();

            return base.CarregarLista<Entidade.Pessoa.PessoaItem>(databaseItem, sql, dicionario);
        }

        public Entidade.Pessoa.PessoaItem CarregarItem(int pessoaId)
        {
            var pessoaItem = null as Entidade.Pessoa.PessoaItem;

            this._pessoaLista.TryGetValue(pessoaId, out pessoaItem);

            return pessoaItem;

            //ADAPTAÇÃO PARA USO DE SQL SERVER (ou outra base, porém é necessário adaptar o sql em alguns pontos);
            var databaseItem = new Vagas.Database.DatabaseItem();

            var sql = this.PrepararSelecaoSql(pessoaId);

            var dicionario = this.ObterDicionarioSelecaoSql();

            var retorno = base.CarregarItem<Entidade.Pessoa.PessoaItem>(databaseItem, sql, dicionario);

            return retorno;
        }

        public Entidade.Pessoa.PessoaItem InserirItem(Entidade.Pessoa.PessoaItem pessoaItem)
        {
            var pessoaId = this._pessoaLista.Count + 1;

            pessoaItem.Id = pessoaId;

            var sucesso = this._pessoaLista.TryAdd(pessoaId, pessoaItem);

            if (!sucesso)
                throw new InvalidOperationException("Não foi possível inserir devido a problemas técnicos");

            return pessoaItem;

            //ADAPTAÇÃO PARA USO DE SQL SERVER (ou outra base, porém é necessário adaptar o sql em alguns pontos);
            var databaseItem = new Vagas.Database.DatabaseItem();

            var dicionario = this.ObterDicionarioSelecaoSql();

            var sql = this.PrepararInsercaoSql(pessoaItem);

            sql += this.ObterUltimoItemInseridoSql();

            return base.CarregarItem<Entidade.Pessoa.PessoaItem>(databaseItem, sql, dicionario);
        }

        public Entidade.Pessoa.PessoaItem AtualizarItem(Entidade.Pessoa.PessoaItem pessoaItem)
        {
            var sucesso = this._pessoaLista[pessoaItem.Id] = pessoaItem;

            return pessoaItem;

            //ADAPTAÇÃO PARA USO DE SQL SERVER (ou outra base, porém é necessário adaptar o sql em alguns pontos);
            var databaseItem = new Vagas.Database.DatabaseItem();

            var sql = this.PrepararAtualizacaoSql(pessoaItem);

            sql += this.PrepararSelecaoSql(pessoaItem.Id);

            var dicionario = this.ObterDicionarioSelecaoSql();

            return base.CarregarItem<Entidade.Pessoa.PessoaItem>(databaseItem, sql, dicionario);
        }

        public Entidade.Pessoa.PessoaItem ExcluirItem(Entidade.Pessoa.PessoaItem pessoaItem)
        {
            var sucesso = this._pessoaLista.Remove(pessoaItem.Id);

            return pessoaItem;

            //ADAPTAÇÃO PARA USO DE SQL SERVER (ou outra base, porém é necessário adaptar o sql em alguns pontos);
            var databaseItem = new Vagas.Database.DatabaseItem();

            var sql = this.PrepararExclusaoSql(pessoaItem);

            var dicionario = this.ObterDicionarioSelecaoSql();

            return base.CarregarItem<Entidade.Pessoa.PessoaItem>(databaseItem, sql, dicionario);
        }

        #endregion

        #region Métodos Privados 

        private Dictionary<string, string> ObterDicionarioSelecaoSql()
        { 
            var dicionario = new Dictionary<string, string>(); 

            dicionario.Add("Id", "PESSOA_ID"); 
            dicionario.Add("DataInclusao", "DATA_INCLUSAO"); 
            dicionario.Add("DataAlteracao", "DATA_ALTERACAO"); 
            dicionario.Add("Nome", "NOME"); 
            dicionario.Add("Profissao", "PROFISSAO"); 
            dicionario.Add("Localizacao", "LOCALIZACAO"); 
            dicionario.Add("Nivel", "NIVEL"); 

            return dicionario; 
        } 

        private string PrepararSelecaoSql()
        { 
            var sql = ""; 

            sql += "SELECT \n";
            sql += "    A.PESSOA_ID,\n";
            sql += "    A.DATA_INCLUSAO,\n";
            sql += "    A.DATA_ALTERACAO,\n";
            sql += "    A.NOME,\n";
            sql += "    A.PROFISSAO,\n";
            sql += "    A.LOCALIZACAO,\n";
            sql += "    A.NIVEL\n";
            sql += "FROM \n";
            sql += "    PESSOA_TB A\n";

            return sql; 
        } 

        private string PrepararSelecaoSql(int? pessoaId)
		{ 
			var sql = ""; 

			if (pessoaId.HasValue)
				sql += "A.PESSOA_ID = " + pessoaId.Value + "\n";

            if (!string.IsNullOrEmpty(sql))
            {
                sql = sql.Substring(0, sql.Length - 1);

                sql = sql.Replace("\n", "\nAND "); 

                sql = " WHERE\n\t" + sql; 
            } 

            sql = this.PrepararSelecaoSql() + " " + sql;

            return sql; 
        } 

        private string PrepararInsercaoSql(Entidade.Pessoa.PessoaItem pessoaItem) 
        { 
            var sql = string.Empty; 

            sql += "INSERT INTO PESSOA_TB(\n";
			sql += "    NOME,\n";

			sql += "    PROFISSAO,\n";

			sql += "    LOCALIZACAO,\n";

			sql += "    NIVEL,\n";

			sql = sql.Substring(0, sql.Length - 2) + "\n";

			sql += ") VALUES (\n";
			    sql += "    '" + pessoaItem.Nome.Replace("'", "''") + "',\n";

			    sql += "    '" + pessoaItem.Profissao.Replace("'", "''") + "',\n";

			    sql += "    '" + pessoaItem.Localizacao.Replace("'", "''") + "',\n";

			sql += "    " + pessoaItem.Nivel.ToString() + ",\n";

			sql = sql.Substring(0, sql.Length - 2) + "\n";

            sql += ");\n";

            return sql; 
        } 

        private string PrepararAtualizacaoSql(Entidade.Pessoa.PessoaItem pessoaItem) 
        { 
            var sql = string.Empty; 

            sql += "UPDATE \n";
            sql += "    A\n";
            sql += "SET\n";
			sql += "    A.DATA_ALTERACAO = CURRENT_TIMESTAMP,\n";

			sql += "    A.NOME = '" + pessoaItem.Nome.Replace("'", "''") + "',\n";

			sql += "    A.PROFISSAO = '" + pessoaItem.Profissao.Replace("'", "''") + "',\n";

			sql += "    A.LOCALIZACAO = '" + pessoaItem.Localizacao.Replace("'", "''") + "',\n";

			sql += "    A.NIVEL = " + pessoaItem.Nivel.ToString() + ",\n"; 

			sql = sql.Substring(0, sql.Length - 2) + "\n";

            sql += "FROM\n";
            sql += "    PESSOA_TB A\n";
            sql += "WHERE\n";
            sql += "    A.PESSOA_ID = " + pessoaItem.Id + "\n";
            return sql; 
        } 

        private string PrepararExclusaoSql(Entidade.Pessoa.PessoaItem pessoaItem) 
        { 
            var sql = string.Empty; 

            sql += "DELETE \n";
            sql += "    A\n";
            sql += "FROM\n";
            sql += "    PESSOA_TB A\n";
            sql += "WHERE\n";
            sql += "    A.PESSOA_ID = " + pessoaItem.Id + "\n";
            return sql; 
        } 

        #endregion 
    
		#region Métodos Específicos do Banco

		private string ObterUltimoItemInseridoSql()
		{
			var sql = this.PrepararSelecaoSql();

			sql += "WHERE \n";

			sql += "    A.PESSOA_ID = SCOPE_IDENTITY()\n";

			return sql;
		}

		#endregion
	}
}
