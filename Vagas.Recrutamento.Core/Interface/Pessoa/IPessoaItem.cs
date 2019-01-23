using System.Collections.Generic;

namespace Vagas.Recrutamento.Core.Interface.Pessoa 
{ 
    public interface IPessoaItem
    { 
        List<Entidade.Pessoa.PessoaItem> CarregarLista(); 

        Entidade.Pessoa.PessoaItem CarregarItem(int pessoaId);

        Entidade.Pessoa.PessoaItem InserirItem(Entidade.Pessoa.PessoaItem pessoaItem); 

        Entidade.Pessoa.PessoaItem AtualizarItem(Entidade.Pessoa.PessoaItem pessoaItem); 

        Entidade.Pessoa.PessoaItem ExcluirItem(Entidade.Pessoa.PessoaItem pessoaItem); 
    } 
} 
