using System;

namespace Vagas.Recrutamento.Core.Entidade.Vaga 
{ 
    public class VagaItem : _BaseItem 
    { 
        public DateTime DataInclusao { get; set; } 

        public DateTime DataAlteracao { get; set; } 

        public string Empresa { get; set; } 

        public string Titulo { get; set; } 

        public string Descricao { get; set; } 

        public string Localizacao { get; set; } 

        public int Nivel { get; set; } 
    } 
} 
