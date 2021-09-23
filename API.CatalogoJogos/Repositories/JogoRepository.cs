using API.CatalogoJogos.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.CatalogoJogos.Repositories
{
    public class JogoRepository : IJogoRepository
    {
        /// <summary>
        /// Cria um dicionário de Jogos Iniciais
        /// </summary>
        private static Dictionary<Guid, Jogo> jogos = new Dictionary<Guid, Jogo>()
        {
            {Guid.Parse("0ca314a5-9282-45d8-92c3-2985f2a9fd04"), new Jogo{ Id = Guid.Parse("0ca314a5-9282-45d8-92c3-2985f2a9fd04"), Nome = "Fifa 21", Produtora = "EA", Preco = 200} },
            {Guid.Parse("eb909ced-1862-4789-8641-1bba36c23db3"), new Jogo{ Id = Guid.Parse("eb909ced-1862-4789-8641-1bba36c23db3"), Nome = "Fifa 20", Produtora = "EA", Preco = 190} },
            {Guid.Parse("5e99c84a-108b-4dfa-ab7e-d8c55957a7ec"), new Jogo{ Id = Guid.Parse("5e99c84a-108b-4dfa-ab7e-d8c55957a7ec"), Nome = "Fifa 19", Produtora = "EA", Preco = 180} },
            {Guid.Parse("da033439-f352-4539-879f-515759312d53"), new Jogo{ Id = Guid.Parse("da033439-f352-4539-879f-515759312d53"), Nome = "Fifa 18", Produtora = "EA", Preco = 170} },
            {Guid.Parse("92576bd2-388e-4f5d-96c1-8bfda6c5a268"), new Jogo{ Id = Guid.Parse("92576bd2-388e-4f5d-96c1-8bfda6c5a268"), Nome = "Street Fighter V", Produtora = "Capcom", Preco = 80} },
            {Guid.Parse("c3c9b5da-6a45-4de1-b28b-491cbf83b589"), new Jogo{ Id = Guid.Parse("c3c9b5da-6a45-4de1-b28b-491cbf83b589"), Nome = "Grand Theft Auto V", Produtora = "Rockstar", Preco = 190} }
        };

        /// <summary>
        /// Obtém lista de Jogos encontrados
        /// </summary>
        /// <param name="pagina">Indica qual página está sendo consultada. </param>
        /// <param name="quantidade">Indica a quantidade de registros por página.</param>
        /// <returns>Uma lista de jogos localizados</returns>
        public Task<List<Jogo>> Obter(int pagina, int quantidade)
        {
            return Task.FromResult(jogos.Values.Skip((pagina - 1) * quantidade).Take(quantidade).ToList());
        }

        /// <summary>
        /// Busca um determinado jogo procurando pelo seu Id
        /// </summary>
        /// <param name="id">Id do jogo buscado</param>
        /// <returns>Os dados do jogo localizado</returns>
        public Task<Jogo> Obter(Guid id)
        {
            if (!jogos.ContainsKey(id))
                return Task.FromResult<Jogo>(null);

            return Task.FromResult(jogos[id]);
        }

        /// <summary>
        /// Obtém o jogo buscando pelo seu nome e produtora usando Lambda
        /// </summary>
        /// <param name="nome">Nome do Jogo</param>
        /// <param name="produtora">nome da produtora do jogo</param>
        /// <returns></returns>
        public Task<List<Jogo>> Obter(string nome, string produtora)
        {
            return Task.FromResult(jogos.Values.Where(jogo => jogo.Nome.Equals(nome) && jogo.Produtora.Equals(produtora)).ToList());
        }

        /// <summary>
        /// Obtém o jogo buscando pelo seu nome e produtora sem a utilização de Lambda
        /// </summary>
        /// <param name="nome">Nome do Jogo</param>
        /// <param name="produtora">nome da produtora do jogo</param>
        /// <returns></returns>
        public Task<List<Jogo>> ObterSemLambda(string nome, string produtora)
        {
            var retorno = new List<Jogo>();

            foreach (var jogo in jogos.Values)
            {
                if (jogo.Nome.Equals(nome) && jogo.Produtora.Equals(produtora))
                    retorno.Add(jogo);
            }

            return Task.FromResult(retorno);
        }

        /// <summary>
        /// Inserir os dados de um jogo no catálogo
        /// </summary>
        /// <param name="jogo">Dados do jogo que será inserido</param>
        /// <returns>Os dados do jogo inserido</returns>
        public Task Inserir(Jogo jogo)
        {
            jogos.Add(jogo.Id, jogo);
            return Task.CompletedTask;
        }

        public Task Atualizar(Jogo jogo)
        {
            jogos[jogo.Id] = jogo;
            return Task.CompletedTask;
        }

        public Task Remover(Guid id)
        {
            jogos.Remove(id);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            //Fechar conexão com o banco
        }
    }
}
