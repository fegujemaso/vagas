using System.Collections.Generic;

namespace Vagas.Recrutamento.Core.Interface.Candidatura 
{ 
    public interface ICandidaturaItem
    { 
        List<Entidade.Candidatura.CandidaturaItem> CarregarLista(); 

        List<Entidade.Candidatura.CandidaturaItem> CarregarListaPorVagaId(int vagaId); 

        List<Entidade.Candidatura.CandidaturaItem> CarregarListaPorPessoaId(int pessoaId); 

        Entidade.Candidatura.CandidaturaItem CarregarItem(int candidaturaId);

        Entidade.Candidatura.CandidaturaItem InserirItem(Entidade.Candidatura.CandidaturaItem candidaturaItem); 

        Entidade.Candidatura.CandidaturaItem AtualizarItem(Entidade.Candidatura.CandidaturaItem candidaturaItem); 

        Entidade.Candidatura.CandidaturaItem ExcluirItem(Entidade.Candidatura.CandidaturaItem candidaturaItem); 
    } 
} 
