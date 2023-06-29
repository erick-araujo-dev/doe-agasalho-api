using DoeAgasalhoApiV2._0.Exceptions;
using DoeAgasalhoApiV2._0.Models.Custom_Models;
using DoeAgasalhoApiV2._0.Models.CustomModels;
using DoeAgasalhoApiV2._0.Models.Entities;
using DoeAgasalhoApiV2._0.Repositories.Interface;
using DoeAgasalhoApiV2._0.Repository.Interface;
using DoeAgasalhoApiV2._0.Services.Interface;

namespace DoeAgasalhoApiV2._0.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly ITipoRepository _tipoRepository;
        private readonly ITamanhoRepository _tamanhoRepository;
        private readonly IPontoColetaRepository _pontoColetaRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ITipoService _tipoService;
        private readonly ITamanhoService _tamanhoService;
        private readonly IUtilsService _utilsService;

        public ProdutoService
            (
            IProdutoRepository produtoRepository,
            ITipoRepository tipoRepository,
            ITamanhoRepository tamanhoRepository,
            IPontoColetaRepository pontoColetaRepository,
            IUsuarioRepository usuarioRepository,
            ITipoService tipoService,
            ITamanhoService tamanhoService,
            IUtilsService utilsService
            )
        {
            _produtoRepository = produtoRepository;
            _tipoRepository = tipoRepository;
            _tamanhoRepository = tamanhoRepository;
            _pontoColetaRepository = pontoColetaRepository;
            _usuarioRepository = usuarioRepository;
            _tipoService = tipoService;
            _tamanhoService = tamanhoService;
            _utilsService = utilsService;
        }

        //criar um novo produto
        public ProdutoModel CreateProduct(ProdutoCreateModel newProductCreate)
        {
            //Recupera o Id do user auth
            var userId = _utilsService.GetUserIdFromToken();
            var existingUser = _usuarioRepository.GetById(userId);
            if (existingUser == null || existingUser.Ativo == "0" || existingUser.Tipo == "admin")
            {
                throw new UnauthorizedAccessException("Você não tem permissão para adicionar um novo produto.");
            }
            
            //Recupera o ponto de coleta do user auth
            var collectPointId = _utilsService.GetPontoColetaIdFromToken();
            var existingCollectPoint = _pontoColetaRepository.GetById(collectPointId);
            if (existingCollectPoint == null || existingCollectPoint.Ativo == "0")
            {
                throw new UnauthorizedAccessException("Houve um erro ao vincular o produto ao ponto de coleta.");
            }
            //Valida caracteristica e genero
            _utilsService.ValidateStringField(newProductCreate.Caracteristica, "caracteristica", 50, false);
            _ValidateGender(newProductCreate.Genero);

            //Valida os dados, e cria um novo tipo e tamanho se necessario
            _utilsService.ValidateStringField(newProductCreate.Tipo, "tipo", 20, false);
            var existingType = _GetOrCreateTipo(newProductCreate.Tipo);

            _utilsService.ValidateStringField(newProductCreate.Tamanho, "tamanho", 20, true);
            var existingSize = _GetOrCreateTamanho(newProductCreate.Tamanho);

            var product = new ProdutoModel
            {
                TipoId = existingType.Id,
                TamanhoId = existingSize.Id,
                Caracteristica = newProductCreate.Caracteristica,
                Genero = newProductCreate.Genero,
            };

            var newProduct = _produtoRepository.Add(product, collectPointId, userId);
            return newProduct;
        }

        //Obter todos produtos
        public List<ProdutoModel> GetProdutosByPontoColeta()
        {
            var collectPointId = _utilsService.GetPontoColetaIdFromToken();
            var existingCollectPoint = _pontoColetaRepository.GetById(collectPointId);
            if (existingCollectPoint == null || existingCollectPoint.Ativo == "0")
            {
                throw new UnauthorizedAccessException("Ocorreu um erro ao tentar acessar o estoque do ponto de coleta");
            }

            return _produtoRepository.GetProdutosByPontoColetaId(existingCollectPoint.Id);
        }

        public ProdutoModel UpdateProduct(int id, UpdateProdutoModel product)
        {
            var existingProduct = _produtoRepository.GetSingleByFilter(p => p.Id == id);

            _ = existingProduct ?? throw new NotFoundException("Produto não encontrado."); //Verifica se eh nulo, se sim, lanca uma exception

            //Valida os dados recebidos para atualizacao
            _utilsService.ValidateStringField(product.Caracteristica, "caracteristica", 50, false);
            _ValidateGender(product.Genero);
            _utilsService.ValidateStringField(product.Tamanho, "tamanho", 20, true);
            _utilsService.ValidateStringField(product.Tipo, "tipo", 20, false);

            //Se forem validos prossegue para o update
            existingProduct.Caracteristica = product.Caracteristica;
            existingProduct.Genero = product.Genero;

            var existingType = _GetOrCreateTipo(product.Tipo);
            existingProduct.TipoId = existingType.Id;

            var existingSize = _GetOrCreateTamanho(product.Tamanho);
            existingProduct.TamanhoId = existingSize.Id;

            _produtoRepository.Update(existingProduct);
            return existingProduct;
        }

        
        //Valida genero
        private void _ValidateGender(string gender)
        {
            if (!"MFU".Contains(gender.ToUpper()) || gender.Length > 1)
            {
                throw new ArgumentException("Digite apenas a inicial, M para Masculino, F para Feminino, U para Unissex");
            }
        }

        //Faz a busca e caso nao encontre cria um novo tipo
        private TipoModel _GetOrCreateTipo(string tipoName)
        {
            var existingType = _tipoRepository.GetByName(tipoName);

            if (existingType == null)
            {
                existingType = _tipoService.CreateNewType(tipoName);
            }

            return existingType;
        }
        //Faz a busca e caso nao encontre cria um novo tamanho
        private TamanhoModel _GetOrCreateTamanho(string tamanhoName)
        {
            var existingSize = _tamanhoRepository.GetByName(tamanhoName);

            if (existingSize == null)
            {
                existingSize = _tamanhoService.CreateNewSize(tamanhoName);
            }

            return existingSize;
        }

        public List<ProdutoModel> GetFilteredProducts(int? tipoId, int? tamanhoId, string genero, string caracteristica)
        {
            return _produtoRepository.GetFilteredProducts(tipoId, tamanhoId, genero, caracteristica);
        }


    }
}


