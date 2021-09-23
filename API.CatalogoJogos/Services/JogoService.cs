using API.CatalogoJogos.InputModel;
using API.CatalogoJogos.Repositories;
using API.CatalogoJogos.ViewModel;
using API.CatalogoJogos.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.CatalogoJogos.Entities;

namespace API.CatalogoJogos.Services
{
    public class JogoService : IJogoService
    {
        private readonly IJogoRepository _jogoRepository;

        public JogoService(IJogoRepository jogoRepository)
        {
            _jogoRepository = jogoRepository;
        }
        /// <summary>
        /// Busca os Jogos 
        /// </summary>
        /// <remarks>
        /// Não é possível retornar todos os jogos em uma unica requisição (sem paginação)
        /// </remarks>
        /// <param name="pagina">Indica qual página está sendo consultada.</param>
        /// <param name="quantidade">Indica a quantidade de registros por página. </param>
        /// <param name="quantidade"></param>
        /// <returns>Uma lista de jogos localizados</returns>
        public async Task<List<JogoViewModel>> Obter(int pagina, int quantidade)
        {
            var jogos = await _jogoRepository.Obter(pagina, quantidade);

            return jogos.Select(jogo => new JogoViewModel
            {
                Id = jogo.Id,
                Nome = jogo.Nome,
                Produtora = jogo.Produtora,
                Preco = jogo.Preco
            }).ToList();
        }
        /// <summary>
        /// Busca um determinado jogo procurasndo pelo seu Id
        /// </summary>
        /// <param name="idJogo">Id do jogo buscado</param>
        /// <returns>Os dados do jogo localizado</returns>
        public async Task<JogoViewModel> Obter(Guid id)
        {
            var jogo = await _jogoRepository.Obter(id);

            if (jogo == null)
                return null;

            return new JogoViewModel
            {
                Id = jogo.Id,
                Nome = jogo.Nome,
                Produtora = jogo.Produtora,
                Preco = jogo.Preco
            };
        }
        /// <summary>
        /// Inserir os dados de um jogo no catálogo
        /// </summary>
        /// <param name="jogoInputModel">Dados do jogo que será inserido</param>
        /// <returns>Os dados do jogo inserido</returns>
        public async Task<JogoViewModel> Inserir(JogoInputModel jogo)
        {
            var entidadeJogo = await _jogoRepository.Obter(jogo.Nome , jogo.Produtora);

            if (entidadeJogo.Count > 0)
                throw new JogoJaCadastradoException();

            var jogoInsert = new Jogo
            {
                Id = Guid.NewGuid(),
                Nome = jogo.Nome,
                Produtora = jogo.Produtora,
                Preco = jogo.Preco
            };

            await _jogoRepository.Inserir(jogoInsert);

            return new JogoViewModel
            {
                Id = jogoInsert.Id,
                Nome = jogo.Nome,
                Produtora = jogo.Produtora,
                Preco = jogo.Preco
            };
        }
        /// <summary>
        /// Atualizar todos os dados de um jogo no catálogo
        /// </summary>
        /// /// <param name="idJogo">Id do jogo a ser atualizado</param>
        /// <param name="jogoInputModel">Novos dados para atualizar o jogo indicado</param>
        public async Task Atualizar(Guid id, JogoInputModel jogo)
        {
            var entidadeJogo = await _jogoRepository.Obter(id);

            if (entidadeJogo == null)
                throw new JogoNaoCadastradoException();

            entidadeJogo.Nome = jogo.Nome;
            entidadeJogo.Produtora = jogo.Produtora;
            entidadeJogo.Preco = jogo.Preco;

            await _jogoRepository.Atualizar(entidadeJogo);
        }
        /// <summary>
        /// Atualizar o preço de um determinado jogo
        /// </summary>
        /// /// <param name="idJogo">Id do jogo a ser atualizado</param>
        /// <param name="preco">Novo preço do jogo</param>
        public async Task Atualizar(Guid id, double preco)
        {
            var entidadeJogo = await _jogoRepository.Obter(id);

            if (entidadeJogo == null)
                throw new JogoNaoCadastradoException();

            entidadeJogo.Preco = preco;

            await _jogoRepository.Atualizar(entidadeJogo);
        }
        /// <summary>
        /// Excluir um jogo específico
        /// </summary>
        /// /// <param name="id">Id do jogo a ser excluído</param>
        public async Task Remover(Guid id)
        {
            var jogo = await _jogoRepository.Obter(id);

            if (jogo == null)
                throw new JogoNaoCadastradoException();

            await _jogoRepository.Remover(id);
        }

        public void Dispose()
        {
            _jogoRepository?.Dispose();
        }
    }
}
