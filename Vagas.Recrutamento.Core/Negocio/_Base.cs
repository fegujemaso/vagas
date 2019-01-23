using System;
using System.Collections.Generic;
using System.Linq;

namespace Vagas.Recrutamento.Core.Negocio
{
    public class _BaseItem : IDisposable, ICloneable
    {
        public bool ValidarDivergenciaItem<T>(T entidadeOrigem, T entidadeDestino)
        {
            return this.ValidarDivergenciaItem(entidadeOrigem, entidadeDestino, new List<string>());
        }

        public bool ValidarDivergenciaItem<T>(T entidadeOrigem, T entidadeDestino, List<string> excecao)
        {
            var propriedadeOrigemLista = entidadeOrigem.GetType().GetProperties();

            var propriedadeDestinoLista = entidadeDestino.GetType().GetProperties();

            foreach (var propriedadeOrigemItem in propriedadeOrigemLista)
            {
                if (excecao.Contains(propriedadeOrigemItem.Name))
                    continue;

                var propriedadeDestinoItem = propriedadeDestinoLista.Where(x => x.Name.Equals(propriedadeOrigemItem.Name)).FirstOrDefault();

                var propriedadeOrigemValor = propriedadeOrigemItem.GetValue(entidadeOrigem, null);

                propriedadeOrigemValor = Convert.ChangeType(propriedadeOrigemValor, propriedadeOrigemItem.PropertyType);

                if (propriedadeOrigemItem.PropertyType.Name.ToLower().Equals("string") && string.IsNullOrEmpty((string)propriedadeOrigemValor))
                    propriedadeOrigemValor = "";

                var propriedadeDestinoValor = propriedadeDestinoItem.GetValue(entidadeDestino, null);

                propriedadeDestinoValor = Convert.ChangeType(propriedadeDestinoValor, propriedadeDestinoItem.PropertyType);

                if (propriedadeDestinoItem.PropertyType.Name.ToLower().Equals("string") && string.IsNullOrEmpty((string)propriedadeDestinoValor))
                    propriedadeDestinoValor = "";

                if (!propriedadeOrigemValor.Equals(propriedadeDestinoValor))
                    return true;
            }

            return false;
        }

        public void Dispose()
        { }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
