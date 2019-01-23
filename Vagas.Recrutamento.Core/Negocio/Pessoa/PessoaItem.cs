using System;
using System.Collections.Generic;

namespace Vagas.Recrutamento.Core.Negocio.Pessoa
{
    public class PessoaItem : _BaseItem
    {
        #region Propriedades 

        private Interface.Pessoa.IPessoaItem _persistenciaPessoaItem { get; set; }

        #endregion

        #region Construtores 

        public PessoaItem()
            : this(new Persistencia.Pessoa.PessoaItem())
        { }

        public PessoaItem(Interface.Pessoa.IPessoaItem persistenciaPessoaItem)
        {
            this._persistenciaPessoaItem = persistenciaPessoaItem;
        }

        #endregion

        #region Métodos Públicos 

        public List<Entidade.Pessoa.PessoaItem> CarregarLista()
        {
            return _persistenciaPessoaItem.CarregarLista();
        }

        public Entidade.Pessoa.PessoaItem CarregarItem(int pessoaId)
        {
            if (pessoaId.Equals(0))
                throw new ArgumentException("Parâmetro inválido");

            return _persistenciaPessoaItem.CarregarItem(pessoaId);
        }

        public Entidade.Pessoa.PessoaItem InserirItem(Entidade.Pessoa.PessoaItem pessoaItem)
        {
            this.ValidarParametroItem(pessoaItem);

            return _persistenciaPessoaItem.InserirItem(pessoaItem);
        }

        public Entidade.Pessoa.PessoaItem AtualizarItem(Entidade.Pessoa.PessoaItem pessoaItem)
        {
            this.ValidarParametroItem(pessoaItem);

            return _persistenciaPessoaItem.AtualizarItem(pessoaItem);
        }

        public Entidade.Pessoa.PessoaItem ExcluirItem(Entidade.Pessoa.PessoaItem pessoaItem)
        {
            this.ValidarParametroItem(pessoaItem);

            return _persistenciaPessoaItem.ExcluirItem(pessoaItem);
        }

        public Entidade.Pessoa.PessoaItem SalvarItem(Entidade.Pessoa.PessoaItem pessoaItem)
        {
            if (pessoaItem.Id.Equals(0))
                pessoaItem = this.InserirItem(pessoaItem);
            else
                pessoaItem = this.AtualizarItem(pessoaItem);

            return pessoaItem;
        }

        #endregion

        #region Métodos Privados

        private void ValidarParametroItem(Entidade.Pessoa.PessoaItem pessoaItem)
        {
            if (pessoaItem == null)
                throw new ArgumentException("Parâmetro inválido");

            if (string.IsNullOrEmpty(pessoaItem.Localizacao))
                throw new ArgumentException("Parâmetro Localizacao inválido");

            if (pessoaItem.Nivel.Equals(0))
                throw new ArgumentException("Parâmetro Nivel inválido");

            if (string.IsNullOrEmpty(pessoaItem.Nome))
                throw new ArgumentException("Parâmetro Nome inválido");

            if (string.IsNullOrEmpty(pessoaItem.Profissao))
                throw new ArgumentException("Parâmetro Profissao inválido");
        }

        #endregion
    }
}
