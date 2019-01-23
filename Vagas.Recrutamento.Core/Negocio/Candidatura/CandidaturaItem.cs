using System;
using System.Collections.Generic;

namespace Vagas.Recrutamento.Core.Negocio.Candidatura
{
    public class CandidaturaItem : _BaseItem
    {
        #region Propriedades 

        private Interface.Vaga.IVagaItem _persistenciaVagaItem { get; set; }

        private Interface.Candidatura.ICandidaturaItem _persistenciaCandidaturaItem { get; set; }

        private Interface.Pessoa.IPessoaItem _persistenciaPessoaItem { get; set; }

        #endregion 

        #region Construtores 

        public CandidaturaItem()
            : this(new Persistencia.Candidatura.CandidaturaItem(), null, null)
        { }

        public CandidaturaItem(Interface.Candidatura.ICandidaturaItem persistenciaCandidaturaItem)
            : this(persistenciaCandidaturaItem, null, null)
        { }

        public CandidaturaItem(Interface.Candidatura.ICandidaturaItem persistenciaCandidaturaItem, Interface.Vaga.IVagaItem persistenciaVagaItem, Interface.Pessoa.IPessoaItem persistenciaPessoaItem)
        {
            this._persistenciaCandidaturaItem = persistenciaCandidaturaItem;

            this._persistenciaVagaItem = persistenciaVagaItem;

            this._persistenciaPessoaItem = persistenciaPessoaItem;
        }

        #endregion

        #region Métodos Públicos 

        public List<Entidade.Candidatura.CandidaturaItem> CarregarLista()
        {
            return _persistenciaCandidaturaItem.CarregarLista();
        }

        public List<Entidade.Candidatura.CandidaturaItem> CarregarListaPorVagaId(int vagaId)
        {
            if (vagaId.Equals(0))
                throw new ArgumentException("Parâmetro inválido");

            return _persistenciaCandidaturaItem.CarregarListaPorVagaId(vagaId);
        }

        public List<Entidade.Candidatura.CandidaturaItem> CarregarListaPorPessoaId(int pessoaId)
        {
            if (pessoaId.Equals(0))
                throw new ArgumentException("Parâmetro inválido");

            return _persistenciaCandidaturaItem.CarregarListaPorPessoaId(pessoaId);
        }

        public Entidade.Candidatura.CandidaturaItem CarregarItem(int candidaturaId)
        {
            if (candidaturaId.Equals(0))
                throw new ArgumentException("Parâmetro inválido");

            return _persistenciaCandidaturaItem.CarregarItem(candidaturaId);
        }

        public Entidade.Candidatura.CandidaturaItem InserirItem(Entidade.Candidatura.CandidaturaItem candidaturaItem)
        {
            this.ValidarParametroItem(candidaturaItem);

            if (candidaturaItem.Id > 0) // particularidade
                throw new ArgumentException("Valor ID inválido: objeto já inserido");

            return _persistenciaCandidaturaItem.InserirItem(candidaturaItem);
        }

        public Entidade.Candidatura.CandidaturaItem AtualizarItem(Entidade.Candidatura.CandidaturaItem candidaturaItem)
        {
            this.ValidarParametroItem(candidaturaItem);

            return _persistenciaCandidaturaItem.AtualizarItem(candidaturaItem);
        }

        public Entidade.Candidatura.CandidaturaItem ExcluirItem(Entidade.Candidatura.CandidaturaItem candidaturaItem)
        {
            this.ValidarParametroItem(candidaturaItem);

            return _persistenciaCandidaturaItem.ExcluirItem(candidaturaItem);
        }

        public Entidade.Candidatura.CandidaturaItem SalvarItem(Entidade.Candidatura.CandidaturaItem candidaturaItem)
        {
            candidaturaItem.Score = this.CalcularScore(candidaturaItem);

            if (candidaturaItem.Id.Equals(0))
                candidaturaItem = this.InserirItem(candidaturaItem);
            else
                candidaturaItem = this.AtualizarItem(candidaturaItem);

            return candidaturaItem;
        }

        #endregion

        #region Métodos Privados

        private void ValidarParametroItem(Entidade.Candidatura.CandidaturaItem candidaturaItem)
        {
            if (candidaturaItem == null)
                throw new ArgumentException("Parâmetro inválido");

            if (string.IsNullOrEmpty(candidaturaItem.Localizacao))
                throw new ArgumentException("Valor Localizacao necessário");

            if (candidaturaItem.Nivel.Equals(0))
                throw new ArgumentException("Valor Nivel necessário");

            if (string.IsNullOrEmpty(candidaturaItem.Nome))
                throw new ArgumentException("Valor Nome necessário");

            if (candidaturaItem.PessoaId.Equals(0))
                throw new ArgumentException("Valor PessoaId necessário");

            if (string.IsNullOrEmpty(candidaturaItem.Profissao))
                throw new ArgumentException("Valor Profissao necessário");

            if (candidaturaItem.VagaId.Equals(0))
                throw new ArgumentException("Valor VagaId necessário");
        }

        public int CalcularScore(Entidade.Candidatura.CandidaturaItem candidaturaItem)
        {
            var vagaItem = this.ObterVagaItem(candidaturaItem.VagaId);

            var pessoaItem = this.ObterPessoaItem(candidaturaItem.PessoaId);

            var localidadeVaga = vagaItem.Localizacao;

            var localidadePessoa = pessoaItem.Localizacao;

            var valorMenorCaminho = new Localidade.LocalidadeItem().CalcularCaminhoValor(localidadePessoa, localidadeVaga);

            var valorN = 100 - (25 * (vagaItem.Nivel - pessoaItem.Nivel));

            var valorD = 100 - ((valorMenorCaminho / 5) * 25);

            var score = (valorN + valorD) / 2;

            return score;
        }

        private List<Entidade.Localidade.LocalidadeItem> ObterLocalidadeLista()
        {
            var localidadeNegocio = new Localidade.LocalidadeItem();

            var localidadeLista = localidadeNegocio.CarregarLista();

            return localidadeLista;
        }

        private Entidade.Vaga.VagaItem ObterVagaItem(int vagaId)
        {
            var vagaNegocio = new Vaga.VagaItem(_persistenciaVagaItem);

            var vagaItem = vagaNegocio.CarregarItem(vagaId);

            return vagaItem;
        }

        private Entidade.Pessoa.PessoaItem ObterPessoaItem(int pessoaId)
        {
            var pessoaNegocio = new Pessoa.PessoaItem(_persistenciaPessoaItem);

            var pessoaItem = pessoaNegocio.CarregarItem(pessoaId);

            return pessoaItem;
        }

        #endregion
    }
}
