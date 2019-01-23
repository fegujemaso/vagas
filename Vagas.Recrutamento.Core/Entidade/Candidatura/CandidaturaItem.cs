namespace Vagas.Recrutamento.Core.Entidade.Candidatura
{
    public class CandidaturaItem : Pessoa.PessoaItem 
    { 
        public int VagaId { get; set; } 

        public int PessoaId { get; set; } 

        public int Score { get; set; } 
    } 
} 
