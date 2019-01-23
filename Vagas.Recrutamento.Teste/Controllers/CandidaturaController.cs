using System;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Vagas.Recrutamento.Teste.Controllers
{
    [Route("v1/candidaturas")]
    [ApiController]
    public class CandidaturaController : BaseController
    {
        #region Requests

        [HttpPost]
        public ActionResult<dynamic> Post([FromForm] string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Parametro inválido");

            var jsonParametro = (JObject)JsonConvert.DeserializeObject(value);

            // VALIDAR JSON E ATRIBUIR
            var id_pessoa = Convert.ToInt32(jsonParametro["id_pessoa"]);

            var id_vaga = Convert.ToInt32(jsonParametro["id_vaga"]);

            var pessoaItem = this.CarregarPessoaItem(id_pessoa);

            var vagaItem = this.CarregarVagaItem(id_vaga);

            var candidaturaItem = pessoaItem.Clone<Core.Entidade.Candidatura.CandidaturaItem>();

            candidaturaItem.PessoaId = id_pessoa;

            candidaturaItem.VagaId = id_vaga;

            candidaturaItem = this.SalvarCandidaturaItem(candidaturaItem);

            return new
            {
                candidaturaItem,
                pessoaItem,
                vagaItem
            };
        }

        #endregion

        #region Métodos Privados

        private Core.Entidade.Candidatura.CandidaturaItem SalvarCandidaturaItem(Core.Entidade.Candidatura.CandidaturaItem candidaturaItem)
        {
            var candidaturaNegocio = new Core.Negocio.Candidatura.CandidaturaItem(Program._candidaturaLista, Program._vagaLista, Program._pessoaLista);

            candidaturaItem = candidaturaNegocio.SalvarItem(candidaturaItem);

            return candidaturaItem;
        }

        private Core.Entidade.Pessoa.PessoaItem CarregarPessoaItem(int pessoaId)
        {
            var pessoaNegocio = new Core.Negocio.Pessoa.PessoaItem(Program._pessoaLista);

            var pessoaItem = pessoaNegocio.CarregarItem(pessoaId);

            return pessoaItem;
        }

        private Core.Entidade.Vaga.VagaItem CarregarVagaItem(int vagaId)
        {
            var vagaNegocio = new Core.Negocio.Vaga.VagaItem(Program._vagaLista);

            var vagaItem = vagaNegocio.CarregarItem(vagaId);

            return vagaItem;
        }

        #endregion
    }
}