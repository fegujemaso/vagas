using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Vagas.Recrutamento.Teste
{
    public class Program
    {
        public static Core.Persistencia.Vaga.VagaItem _vagaLista { get; set; }

        public static Core.Persistencia.Pessoa.PessoaItem _pessoaLista { get; set; }

        public static Core.Persistencia.Candidatura.CandidaturaItem _candidaturaLista { get; set; }

        public static void Main(string[] args)
        {
            _vagaLista = new Core.Persistencia.Vaga.VagaItem();

            _pessoaLista = new Core.Persistencia.Pessoa.PessoaItem();

            _candidaturaLista = new Core.Persistencia.Candidatura.CandidaturaItem();

            CreateWebHostBuilder(args)
                .Build()
                .Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseContentRoot(Directory.GetCurrentDirectory());
    }
}
