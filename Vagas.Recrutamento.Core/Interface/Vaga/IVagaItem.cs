using System.Collections.Generic;

namespace Vagas.Recrutamento.Core.Interface.Vaga 
{ 
    public interface IVagaItem
    { 
        List<Entidade.Vaga.VagaItem> CarregarLista(); 

        Entidade.Vaga.VagaItem CarregarItem(int vagaId);

        Entidade.Vaga.VagaItem InserirItem(Entidade.Vaga.VagaItem vagaItem); 

        Entidade.Vaga.VagaItem AtualizarItem(Entidade.Vaga.VagaItem vagaItem); 

        Entidade.Vaga.VagaItem ExcluirItem(Entidade.Vaga.VagaItem vagaItem); 
    } 
} 
