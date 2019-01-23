using System;
using System.Collections.Generic;
using System.Linq;

namespace Vagas.Recrutamento.Core.Persistencia.Candidatura
{
    public class CandidaturaItem : _BaseItem, Interface.Candidatura.ICandidaturaItem
    { 
        #region Propriedades 

        private string _connectionString { get; set; } 

        private Dictionary<int, Entidade.Candidatura.CandidaturaItem> _candidaturaLista { get; set; }

        #endregion 

        #region Construtores 

        public CandidaturaItem() 
            : this("") 
        { } 

        public CandidaturaItem(string connectionString) 
        { 
            this._connectionString = connectionString;

            this._candidaturaLista = new Dictionary<int, Entidade.Candidatura.CandidaturaItem>();
        } 

        #endregion 

        #region Métodos Públicos 

        public List<Entidade.Candidatura.CandidaturaItem> CarregarLista() 
        {
            var lista = this._candidaturaLista
                .Select(x => x.Value)
                .ToList();

            return lista;

            //ADAPTAÇÃO PARA USO DE SQL SERVER (ou outra base, porém é necessário adaptar o sql em alguns pontos);
            var databaseItem = new Vagas.Database.DatabaseItem(); 

            var sql = this.PrepararSelecaoSql(); 

            var dicionario = this.ObterDicionarioSelecaoSql(); 

            return base.CarregarLista<Entidade.Candidatura.CandidaturaItem>(databaseItem, sql, dicionario); 
        } 

        public List<Entidade.Candidatura.CandidaturaItem> CarregarListaPorVagaId(int vagaId) 
        {
            var lista = this.CarregarLista()
                .Where(x => x.VagaId.Equals(vagaId))
                .ToList();

            return lista;

            //ADAPTAÇÃO PARA USO DE SQL SERVER (ou outra base, porém é necessário adaptar o sql em alguns pontos);
            var databaseItem = new Vagas.Database.DatabaseItem(); 

            var sql = this.PrepararSelecaoSql(null, vagaId, null); 

            var dicionario = this.ObterDicionarioSelecaoSql(); 

            return base.CarregarLista<Entidade.Candidatura.CandidaturaItem>(databaseItem, sql, dicionario); 
        } 

        public List<Entidade.Candidatura.CandidaturaItem> CarregarListaPorPessoaId(int pessoaId) 
        {
            var lista = this.CarregarLista()
                .Where(x => x.PessoaId.Equals(pessoaId))
                .ToList();

            return lista;

            //ADAPTAÇÃO PARA USO DE SQL SERVER (ou outra base, porém é necessário adaptar o sql em alguns pontos);
            var databaseItem = new Vagas.Database.DatabaseItem(); 

            var sql = this.PrepararSelecaoSql(null, null, pessoaId); 

            var dicionario = this.ObterDicionarioSelecaoSql(); 

            return base.CarregarLista<Entidade.Candidatura.CandidaturaItem>(databaseItem, sql, dicionario); 
        } 

        public Entidade.Candidatura.CandidaturaItem CarregarItem(int candidaturaId)
        {
            var candidaturaItem = null as Entidade.Candidatura.CandidaturaItem;

            this._candidaturaLista.TryGetValue(candidaturaId, out candidaturaItem);

            return candidaturaItem;

            //ADAPTAÇÃO PARA USO DE SQL SERVER (ou outra base, porém é necessário adaptar o sql em alguns pontos);
            var databaseItem = new Vagas.Database.DatabaseItem(); 

            var sql = this.PrepararSelecaoSql(candidaturaId, null, null); 

            var dicionario = this.ObterDicionarioSelecaoSql(); 

            var retorno = base.CarregarItem<Entidade.Candidatura.CandidaturaItem>(databaseItem, sql, dicionario); 

            return retorno; 
        }

        public Entidade.Candidatura.CandidaturaItem InserirItem(Entidade.Candidatura.CandidaturaItem candidaturaItem)
        {
            var candidaturaId = this._candidaturaLista.Count + 1;

            candidaturaItem.Id = candidaturaId;

            var sucesso = this._candidaturaLista.TryAdd(candidaturaId, candidaturaItem);

            if (!sucesso)
                throw new InvalidOperationException("Não foi possível inserir devido a problemas técnicos");

            return candidaturaItem;

            //ADAPTAÇÃO PARA USO DE SQL SERVER (ou outra base, porém é necessário adaptar o sql em alguns pontos);
            var databaseItem = new Vagas.Database.DatabaseItem(); 

            var dicionario = this.ObterDicionarioSelecaoSql(); 

            var sql = this.PrepararInsercaoSql(candidaturaItem); 

            sql += this.ObterUltimoItemInseridoSql();

            return base.CarregarItem<Entidade.Candidatura.CandidaturaItem>(databaseItem, sql, dicionario); 
        } 

        public Entidade.Candidatura.CandidaturaItem AtualizarItem(Entidade.Candidatura.CandidaturaItem candidaturaItem)
        {
            var sucesso = this._candidaturaLista[candidaturaItem.Id] = candidaturaItem;

            return candidaturaItem;

            //ADAPTAÇÃO PARA USO DE SQL SERVER (ou outra base, porém é necessário adaptar o sql em alguns pontos);
            var databaseItem = new Vagas.Database.DatabaseItem(); 

            var sql = this.PrepararAtualizacaoSql(candidaturaItem); 

            sql += this.PrepararSelecaoSql(candidaturaItem.Id, null, null);

            var dicionario = this.ObterDicionarioSelecaoSql(); 

            return base.CarregarItem<Entidade.Candidatura.CandidaturaItem>(databaseItem, sql, dicionario); 
        } 

        public Entidade.Candidatura.CandidaturaItem ExcluirItem(Entidade.Candidatura.CandidaturaItem candidaturaItem)
        {
            var sucesso = this._candidaturaLista.Remove(candidaturaItem.Id);

            return candidaturaItem;

            //ADAPTAÇÃO PARA USO DE SQL SERVER (ou outra base, porém é necessário adaptar o sql em alguns pontos);
            var databaseItem = new Vagas.Database.DatabaseItem(); 

            var sql = this.PrepararExclusaoSql(candidaturaItem); 

            var dicionario = this.ObterDicionarioSelecaoSql(); 

            return base.CarregarItem<Entidade.Candidatura.CandidaturaItem>(databaseItem, sql, dicionario); 
        } 

        #endregion 

        #region Métodos Privados 

        private Dictionary<string, string> ObterDicionarioSelecaoSql()
        { 
            var dicionario = new Dictionary<string, string>(); 

            dicionario.Add("Id", "CANDIDATURA_ID"); 
            dicionario.Add("DataInclusao", "DATA_INCLUSAO"); 
            dicionario.Add("VagaId", "VAGA_ID"); 
            dicionario.Add("PessoaId", "PESSOA_ID"); 
            dicionario.Add("Score", "SCORE"); 

            return dicionario; 
        } 

        private string PrepararSelecaoSql()
        { 
            var sql = ""; 

            sql += "SELECT \n";
            sql += "    A.CANDIDATURA_ID,\n";
            sql += "    A.DATA_INCLUSAO,\n";
            sql += "    A.VAGA_ID,\n";
            sql += "    A.PESSOA_ID,\n";
            sql += "    A.SCORE\n";
            sql += "FROM \n";
            sql += "    CANDIDATURA_TB A\n";

            return sql; 
        } 

        private string PrepararSelecaoSql(int? candidaturaId, int? vagaId, int? pessoaId)
		{ 
			var sql = ""; 

			if (candidaturaId.HasValue)
				sql += "A.CANDIDATURA_ID = " + candidaturaId.Value + "\n";

			if (vagaId.HasValue)
				sql += "A.VAGA_ID = " + vagaId.Value + "\n";

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

        private string PrepararInsercaoSql(Entidade.Candidatura.CandidaturaItem candidaturaItem) 
        { 
            var sql = string.Empty; 

            sql += "INSERT INTO CANDIDATURA_TB(\n";
			sql += "    VAGA_ID,\n";

			sql += "    PESSOA_ID,\n";

			sql += "    SCORE,\n";

			sql = sql.Substring(0, sql.Length - 2) + "\n";

			sql += ") VALUES (\n";
			sql += "    " + candidaturaItem.VagaId.ToString() + ",\n";

			sql += "    " + candidaturaItem.PessoaId.ToString() + ",\n";

			sql += "    " + (candidaturaItem.Score > int.MinValue ? candidaturaItem.Score.ToString() : "NULL") + ",\n";

			sql = sql.Substring(0, sql.Length - 2) + "\n";

            sql += ");\n";

            return sql; 
        } 

        private string PrepararAtualizacaoSql(Entidade.Candidatura.CandidaturaItem candidaturaItem) 
        { 
            var sql = string.Empty; 

            sql += "UPDATE \n";
            sql += "    A\n";
            sql += "SET\n";
			sql += "    A.VAGA_ID = " + candidaturaItem.VagaId.ToString() + ",\n"; 

			sql += "    A.PESSOA_ID = " + candidaturaItem.PessoaId.ToString() + ",\n"; 

			sql += "    A.SCORE = " + (candidaturaItem.Score > int.MinValue ? candidaturaItem.Score.ToString() : "NULL") + ",\n"; 

			sql = sql.Substring(0, sql.Length - 2) + "\n";

            sql += "FROM\n";
            sql += "    CANDIDATURA_TB A\n";
            sql += "WHERE\n";
            sql += "    A.CANDIDATURA_ID = " + candidaturaItem.Id + "\n";
            return sql; 
        } 

        private string PrepararExclusaoSql(Entidade.Candidatura.CandidaturaItem candidaturaItem) 
        { 
            var sql = string.Empty; 

            sql += "DELETE \n";
            sql += "    A\n";
            sql += "FROM\n";
            sql += "    CANDIDATURA_TB A\n";
            sql += "WHERE\n";
            sql += "    A.CANDIDATURA_ID = " + candidaturaItem.Id + "\n";
            return sql; 
        } 

        #endregion 
    
		#region Métodos Específicos do Banco

		private string ObterUltimoItemInseridoSql()
		{
			var sql = this.PrepararSelecaoSql();

			sql += "WHERE \n";

			sql += "    A.CANDIDATURA_ID = SCOPE_IDENTITY()\n";

			return sql;
		}

		#endregion
	}
}
