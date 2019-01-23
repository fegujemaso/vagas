using System.Collections.Generic;
using System.Linq;

namespace Vagas.Recrutamento.Core.Negocio.Localidade
{
    public class LocalidadeItem : _BaseItem
    {

        #region Métodos Públicos 

        public List<Entidade.Localidade.LocalidadeItem> CarregarLista()
        {
            var localidadeLista = new List<Entidade.Localidade.LocalidadeItem>();

            localidadeLista.Add(new Entidade.Localidade.LocalidadeItem { Origem = "A", Destino = "B", Valor = 5 });
            localidadeLista.Add(new Entidade.Localidade.LocalidadeItem { Origem = "B", Destino = "C", Valor = 7 });
            localidadeLista.Add(new Entidade.Localidade.LocalidadeItem { Origem = "B", Destino = "D", Valor = 3 });
            localidadeLista.Add(new Entidade.Localidade.LocalidadeItem { Origem = "C", Destino = "E", Valor = 4 });
            localidadeLista.Add(new Entidade.Localidade.LocalidadeItem { Origem = "D", Destino = "E", Valor = 10 });
            localidadeLista.Add(new Entidade.Localidade.LocalidadeItem { Origem = "D", Destino = "F", Valor = 8 });

            return localidadeLista;
        }

        public int CalcularCaminhoValor(string localidadeOrigem, string localidadeDestino)
        {
            return this.CalcularCaminhoValor(null, localidadeOrigem, localidadeDestino);
        }

        public int CalcularCaminhoValor(List<Entidade.Localidade.LocalidadeItem> localidadeAtualLista, string localidadeOrigem, string localidadeDestino)
        {
            if (localidadeAtualLista == null)
                localidadeAtualLista = this.CarregarLista();

            var localidadeLista = localidadeAtualLista // mantendo referências
                .ToList();

            var localidadeLinqLista = localidadeLista
                .Where(x => x.Origem.Equals(localidadeOrigem) || x.Destino.Equals(localidadeOrigem))
                .OrderBy(x => x.Origem)
                .ToList();

            var valorMenor = 0;

            for (int i = 0; i < localidadeLinqLista.Count; i++)
            {
                var localidadeLinqItem = localidadeLinqLista[i];

                if (localidadeLinqItem.Destino.Equals(localidadeDestino) || localidadeLinqItem.Origem.Equals(localidadeDestino))
                    return localidadeLinqItem.Valor;

                localidadeLista.Remove(localidadeLinqItem);

                var valorCalculado = 0;

                if (localidadeLinqItem.Origem.Equals(localidadeOrigem))
                    valorCalculado = this.CalcularCaminhoValor(localidadeLista, localidadeLinqItem.Destino, localidadeDestino);
                else
                    valorCalculado = this.CalcularCaminhoValor(localidadeLista, localidadeLinqItem.Origem, localidadeDestino);

                if (valorCalculado > 0)
                    valorCalculado += localidadeLinqItem.Valor;

                if (valorCalculado > 0 && (valorMenor > valorCalculado || valorMenor.Equals(0)))
                    valorMenor = valorCalculado;
            }

            return valorMenor;
        }

        #endregion
    }
}
