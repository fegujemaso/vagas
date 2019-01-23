using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Vagas.Recrutamento.Teste.Controllers
{
    [Route("v1/vagas")]
    [ApiController]
    public class VagaController : BaseController
    {
        #region Requests

        [HttpGet]
        [Route("{id}/candidaturas/ranking")]
        public ActionResult<List<Core.Entidade.Candidatura.CandidaturaItem>> Get(int vagaId)
        {
            if (vagaId.Equals(0))
                throw new ArgumentException("Parametro inválido");

            var candidaturaLista = this.CarregarVagaRankingListaPorVagaId(vagaId);

            return candidaturaLista;
        }

        [HttpPost]
        public ActionResult<Core.Entidade.Vaga.VagaItem> Post([FromForm] string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Parametro inválido");

            var vagaItem = this.ProcessarJsonParametro<Core.Entidade.Vaga.VagaItem>(value);

            vagaItem = this.SalvarVagaItem(vagaItem);

            return vagaItem;
        }

        #endregion

        #region Métodos Privados

        private Core.Entidade.Vaga.VagaItem SalvarVagaItem(Core.Entidade.Vaga.VagaItem vagaItem)
        {
            var vagaNegocio = new Core.Negocio.Vaga.VagaItem(Program._vagaLista);

            vagaItem = vagaNegocio.SalvarItem(vagaItem);

            return vagaItem;
        }

        private List<Core.Entidade.Candidatura.CandidaturaItem> CarregarVagaRankingListaPorVagaId(int vagaId)
        {
            var vagaNegocio = new Core.Negocio.Vaga.VagaItem(Program._vagaLista, Program._candidaturaLista, Program._pessoaLista);

            var candidaturaLista = vagaNegocio.CarregarRankingListaPorVagaId(vagaId);

            return candidaturaLista;
        }
        
        #endregion
    }

}