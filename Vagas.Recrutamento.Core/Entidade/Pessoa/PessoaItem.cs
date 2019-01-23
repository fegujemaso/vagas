using System;

namespace Vagas.Recrutamento.Core.Entidade.Pessoa 
{ 
    public class PessoaItem : _BaseItem 
    { 
        public DateTime DataInclusao { get; set; } 

        public DateTime DataAlteracao { get; set; } 

        public string Nome { get; set; } 

        public string Profissao { get; set; } 

        public string Localizacao { get; set; } 

        public int Nivel { get; set; } 
    } 
} 
