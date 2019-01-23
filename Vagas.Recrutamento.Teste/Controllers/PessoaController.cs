using System;
using Microsoft.AspNetCore.Mvc;

namespace Vagas.Recrutamento.Teste.Controllers
{
    [Route("v1/pessoas")]
    [ApiController]
    public class PessoaController : BaseController
    {
        #region Requests

        [HttpPost]
        public ActionResult<Core.Entidade.Pessoa.PessoaItem> Post([FromForm] string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Parametro inválido");

            var pessoaItem = this.ProcessarJsonParametro<Core.Entidade.Pessoa.PessoaItem>(value);

            pessoaItem = this.SalvarPessoaItem(pessoaItem);

            return pessoaItem;
        }

        #endregion

        #region Métodos Privados

        private Core.Entidade.Pessoa.PessoaItem SalvarPessoaItem(Core.Entidade.Pessoa.PessoaItem pessoaItem)
        {
            var pessoaNegocio = new Core.Negocio.Pessoa.PessoaItem(Program._pessoaLista);

            pessoaItem = pessoaNegocio.SalvarItem(pessoaItem);

            return pessoaItem;
        }

        #endregion
    }
}