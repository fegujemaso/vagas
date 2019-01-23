using System;
using System.Linq;

namespace Vagas.Recrutamento.Core.Entidade
{
    public class _BaseItem : IDisposable, ICloneable
    {
        public int Id { get; set; }

        public void Dispose()
        { }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public T Clone<T>() where T : new()
        {
            var itemOrigem = this.MemberwiseClone();

            var itemDestino = new T();

            var propriedadeOrigemLista = itemOrigem.GetType().GetProperties();

            var propriedadeDestinoLista = itemDestino.GetType().GetProperties();

            foreach (var propriedadeOrigemItem in propriedadeOrigemLista)
            {
                var propriedadeDestinoItem = propriedadeDestinoLista.Where(x => x.Name.Equals(propriedadeOrigemItem.Name)).FirstOrDefault();

                if (propriedadeDestinoItem == null)
                    continue;

                var valor = propriedadeOrigemItem.GetValue(itemOrigem, null);

                propriedadeDestinoItem.SetValue(itemDestino, valor, null);
            }

            return itemDestino;
        }
    }
}
