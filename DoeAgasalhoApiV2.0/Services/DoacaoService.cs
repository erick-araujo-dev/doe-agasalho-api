using DoeAgasalhoApiV2._0.Exceptions;
using DoeAgasalhoApiV2._0.Models.CustomModels;
using DoeAgasalhoApiV2._0.Models.Entities;
using DoeAgasalhoApiV2._0.Repositories;
using DoeAgasalhoApiV2._0.Repositories.Interface;
using DoeAgasalhoApiV2._0.Repository.Interface;
using DoeAgasalhoApiV2._0.Services.Interface;
using System.Runtime.Intrinsics.Arm;

namespace DoeAgasalhoApiV2._0.Services
{
    public class DoacaoService : IDoacaoService
    {
        private readonly IDoacaoRepository _doacaoRepository;
        private readonly IProdutoRepository _produtoRepository;
        private readonly IUtilsService _utilsService;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPontoColetaRepository _pontoColetaRepository;
        
        public DoacaoService (IDoacaoRepository doacaoRepository, IProdutoRepository produtoRepository,IUtilsService utilsService, IUsuarioRepository usuarioRepository, IPontoColetaRepository pontoColetaRepository)
        {
            _doacaoRepository = doacaoRepository;
            _produtoRepository = produtoRepository;
            _utilsService = utilsService;
            _usuarioRepository = usuarioRepository;
            _pontoColetaRepository = pontoColetaRepository;
        }
        public async Task<DoacaoModel> GetDoacao(int id)
        {
            var doacao = _doacaoRepository.GetDoacao(id);
            _ = doacao ?? throw new NotFoundException("Doação não encontrada.");

            return await doacao;
        }

        //Faz entrada ou saida de um produto
        public async Task<DoacaoModel> DoacaoProduto(DoacaoEntradaSaidaModel model, string tipoMovimento)
        {
            var userAuth = _utilsService.GetUserIdFromToken();

            var product = _produtoRepository.GetById(model.ProdutoId); //verifica se o produto existe
            _ = product ?? throw new NotFoundException("Produto não encontrado.");

            _utilsService.VerifyProductAssociation(model.ProdutoId); //Se o usuario nao estiver associado ao ponto coleta do produto, sera lancado um 404

            if (tipoMovimento == "entrada" && product.Ativo == "0") //verifica se o produto esta ativo, se nao, deixara fazer apenas saidas
            {
                throw new UnauthorizedAccessException("Erro ao fazer entrada, esse produto encontra-se inativo.");
            }

            if (model.Quantidade <= 0) //Verifica se digitou uma quantidade valida
            {
                throw new ArgumentException("Quantidade inválida, ecolha um valor maior que 0.");
            }

            if(tipoMovimento == "saida" && product.Estoque < model.Quantidade)//verifica se a quantidasde eh menor ou igual ao estoque
            {
                throw new ArgumentException("Não há produtos o suficiente em estoque.");
            }

            // Chamar o método Add do DoacaoRepository para salvar a nova doação
            var doacao =  await _doacaoRepository.Add(model.ProdutoId, model.Quantidade, userAuth, tipoMovimento);

            return doacao;
        }
    }
}
