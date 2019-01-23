using System.Linq;
using System.Collections.Generic;
using System;

namespace Vagas.Recrutamento.Core.Negocio.Vaga
{
    public class VagaItem : _BaseItem
    {
        #region Propriedades 

        private Interface.Vaga.IVagaItem _persistenciaVagaItem { get; set; }

        private Interface.Candidatura.ICandidaturaItem _persistenciaCandidaturaItem { get; set; }

        private Interface.Pessoa.IPessoaItem _persistenciaPessoaItem { get; set; }

        #endregion 

        #region Construtores 

        public VagaItem()
            : this(new Persistencia.Vaga.VagaItem(), null, null)
        { }

        public VagaItem(Interface.Vaga.IVagaItem persistenciaVagaItem)
            : this(persistenciaVagaItem, null, null)
        { }

        public VagaItem(Interface.Vaga.IVagaItem persistenciaVagaItem, Interface.Candidatura.ICandidaturaItem persistenciaCandidaturaItem, Interface.Pessoa.IPessoaItem persistenciaPessoaItem)
        {
            this._persistenciaVagaItem = persistenciaVagaItem;

            this._persistenciaCandidaturaItem = persistenciaCandidaturaItem;

            this._persistenciaPessoaItem = persistenciaPessoaItem;
        }

        #endregion

        #region Métodos Públicos 

        public List<Entidade.Vaga.VagaItem> CarregarLista()
        {
            return _persistenciaVagaItem.CarregarLista();
        }

        public Entidade.Vaga.VagaItem CarregarItem(int vagaId)
        {
            if (vagaId.Equals(0))
                throw new ArgumentException("Parâmetro inválido");

            return _persistenciaVagaItem.CarregarItem(vagaId);
        }

        public Entidade.Vaga.VagaItem InserirItem(Entidade.Vaga.VagaItem vagaItem)
        {
            this.ValidarParametroItem(vagaItem);

            return _persistenciaVagaItem.InserirItem(vagaItem);
        }

        public Entidade.Vaga.VagaItem AtualizarItem(Entidade.Vaga.VagaItem vagaItem)
        {
            this.ValidarParametroItem(vagaItem);

            return _persistenciaVagaItem.AtualizarItem(vagaItem);
        }

        public Entidade.Vaga.VagaItem ExcluirItem(Entidade.Vaga.VagaItem vagaItem)
        {
            this.ValidarParametroItem(vagaItem);

            return _persistenciaVagaItem.ExcluirItem(vagaItem);
        }

        public Entidade.Vaga.VagaItem SalvarItem(Entidade.Vaga.VagaItem vagaItem)
        {
            // validar parametro com exception

            if (vagaItem.Id.Equals(0))
                vagaItem = this.InserirItem(vagaItem);
            else
                vagaItem = this.AtualizarItem(vagaItem);

            return vagaItem;
        }

        public List<Entidade.Candidatura.CandidaturaItem> CarregarRankingListaPorVagaId(int vagaId)
        {
            if (vagaId.Equals(0))
                throw new ArgumentException("Parâmetro inválido");

            var candidaturaLista = _persistenciaCandidaturaItem.CarregarListaPorVagaId(vagaId);

            candidaturaLista = candidaturaLista
                ?.OrderByDescending(x => x.Score)
                ?.ToList();

            return candidaturaLista;
        }

        #endregion

        #region Métodos Privados

        private void ValidarParametroItem(Entidade.Vaga.VagaItem vagaItem)
        {
            if (vagaItem == null)
                throw new ArgumentException("Parâmetro inválido");

            if (string.IsNullOrEmpty(vagaItem.Localizacao))
                throw new ArgumentException("Parâmetro Localizacao inválido");

            if (vagaItem.Nivel.Equals(0))
                throw new ArgumentException("Parâmetro Nivel inválido");

            if (string.IsNullOrEmpty(vagaItem.Empresa))
                throw new ArgumentException("Parâmetro Empresa inválido");

            if (string.IsNullOrEmpty(vagaItem.Descricao))
                throw new ArgumentException("Parâmetro Descricao inválido");

            if (string.IsNullOrEmpty(vagaItem.Titulo))
                throw new ArgumentException("Parâmetro Titulo inválido");
        }

        #endregion
    }
}
