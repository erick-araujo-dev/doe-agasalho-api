using DoeAgasalhoApiV2._0.Exceptions;
using DoeAgasalhoApiV2._0.Models.Custom_Models;
using DoeAgasalhoApiV2._0.Models.CustomModels;
using DoeAgasalhoApiV2._0.Models.Entities;
using DoeAgasalhoApiV2._0.Repositories.Interface;
using DoeAgasalhoApiV2._0.Services.Interface;
using Microsoft.Build.Evaluation;
using Microsoft.EntityFrameworkCore;
using System.Reflection.PortableExecutable;

namespace DoeAgasalhoApiV2._0.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly ITipoRepository _tipoRepository;
        private readonly ITamanhoRepository _tamanhoRepository;
        private readonly ITipoService _tipoService;
        private readonly ITamanhoService _tamanhoService;

        public ProdutoService 
            (
            IProdutoRepository produtoRepository,
            ITipoRepository tipoRepository,
            ITamanhoRepository tamanhoRepository,
            ITipoService tipoService,
            ITamanhoService tamanhoService
            )
        {
            _produtoRepository = produtoRepository;
            _tipoRepository = tipoRepository;
            _tamanhoRepository = tamanhoRepository;
            _tipoService = tipoService;
            _tamanhoService = tamanhoService;
        }   

        public ProdutoModel CreateProduct(ProdutoCreateModel newProductCreate)
        {
            var existingType = _GetOrCreateTipo(newProductCreate.Tipo);
            var existingSize = _GetOrCreateTamanho(newProductCreate.Tamanho);

            var newProduct = new ProdutoModel
            {
                TipoId = existingType.Id,
                TamanhoId = existingSize.Id,
                Caracteristica = newProductCreate.Caracteristica,
                Genero = newProductCreate.Genero
            };

            return _produtoRepository.Add(newProduct);
        }

        public ProdutoModel UpdateProduct(int id, UpdateProductModel product)
        {
            var existingProduct = _produtoRepository.GetSingleByFilter(p => p.Id == id);

            _ = existingProduct ?? throw new NotFoundException("Produto não encontrado."); //Verifica se eh nulo, se sim, lanca uma exception

            var existingType = _GetOrCreateTipo(product.Tipo);
            existingProduct.TipoId = existingType.Id != existingProduct.TipoId ? existingType.Id : existingProduct.TipoId;

            var existingSize = _GetOrCreateTamanho(product.Tamanho);
            existingProduct.TamanhoId = existingSize.Id != existingProduct.TamanhoId ? existingSize.Id : existingProduct.TamanhoId;

            existingProduct.Caracteristica = product.Caracteristica;
            existingProduct.Genero = product.Genero;

            _produtoRepository.Update(existingProduct);
            return existingProduct;
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


