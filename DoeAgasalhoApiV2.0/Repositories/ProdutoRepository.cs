using DoeAgasalhoApiV2._0.Data.Context;
using DoeAgasalhoApiV2._0.Models.Custom_Models;
using DoeAgasalhoApiV2._0.Models.Entities;
using DoeAgasalhoApiV2._0.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace DoeAgasalhoApiV2._0.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly DbDoeagasalhov2Context _context;

        public ProdutoRepository(DbDoeagasalhov2Context context)
        {
            _context = context;
        }

        //ativa um produto
        public void ActivateProduct(int productId)
        {
            var product = _context.Produtos.FirstOrDefault(p => p.Id == productId);
            if (product != null)
            {
                product.Ativo = "1";
                _context.SaveChanges();
            }
        }

        //Desativa um produto
        public void DeactivateProduct(int productId)
        {
            var product = _context.Produtos.FirstOrDefault(p => p.Id == productId);
            if (product != null)
            {
                product.Ativo = "0";
                _context.SaveChanges();
            }
        }

        //adiciona um novo produto, um novo relacionamento n n, e um novo registro na tabela de Doacoes
        public ProdutoModel Add(ProdutoModel product, int collectPointId, int userId)
        {
            //Cria um novo produto
            var newProduct = new ProdutoModel
            {
                TipoId = product.TipoId,
                TamanhoId = product.TamanhoId,
                Genero = product.Genero,
                Caracteristica = product.Caracteristica,
                Ativo = "1",
                Estoque = 0
            };

            _context.Produtos.Add(newProduct);
            _context.SaveChanges();

            //Cria a relacao do produto com o ponto de coleta
            var productPoint = new PontoProdutoModel
            {
                ProdutoId = newProduct.Id,
                PontoColetaId = collectPointId,
            };

            _context.PontoProdutos.Add(productPoint);
            _context.SaveChanges();

            //cria um registro de doacao na tabela de doacoes
            var DonationRegister = new DoacaoModel
            {
                ProdutoId = newProduct.Id,
                TipoMovimento = "cadastro",
                DataMovimento = DateTime.Now,
                Quantidade = 0,
                UsuarioId = userId
            };

            _context.Doacoes.Add(DonationRegister);
            _context.SaveChanges(); 

            return newProduct;
        }

        //faz um filtro retornando o primeiro produto encontrado, pode buscar por id, categoria, tamanho etc 
        public ProdutoModel GetSingleByFilter(Func<ProdutoModel, bool> filter)
        {
            return _context.Produtos.FirstOrDefault(filter);
        }

        //faz um filtro retornando o uma lista de produtos encontrado, pode buscar por id, categoria, tamanho etc 

        public List<ProdutoModel> GetAllByFilter(Func<ProdutoModel, bool> filter)
        {
            return _context.Produtos.Where(filter).ToList();
        }

        //filtro personalizado para usar na busca de produto, tem todas opcoes, porém só fará a busca, para os filstros selecionados
       

        public IQueryable<ProdutoModel> GetAll()
        {
            return _context.Produtos.
                Include(p => p.PontoProdutos);
        }



        //filtragem para usar na exibicao dos valores do select, quando marcar um tipo, retornara as caracteristicas para aquele tipo especifico
        public List<string> GetCharacteristicsByFilter(int tipoId, int tamanhoId)
        {
            var characteristics = _context.Produtos
                .Where(p => p.TipoId == tipoId && p.TamanhoId == tamanhoId)
                .Select(p => p.Caracteristica)
                .Distinct()
                .ToList();

            return characteristics;
        }

        //Retorna os produtos de acordo com o ponto de coleta
        public List<ProdutoViewModel> GetProdutosByPontoColeta(int pontoColetaId, string ativo = null)
        {
            var query = _context.PontoProdutos
                .Where(pp => pp.PontoColetaId == pontoColetaId)
                .Include(pp => pp.Produto)
                .Select(pp => new ProdutoViewModel
                {
                    Id = pp.Produto.Id,
                    Tipo = pp.Produto.Tipo.Nome,
                    Ativo = pp.Produto.Ativo,
                    Caracteristica = pp.Produto.Caracteristica,
                    Tamanho = pp.Produto.Tamanho.Nome,
                    Genero = pp.Produto.Genero,
                    Estoque = pp.Produto.Estoque
                });

            if(ativo == "1")
            {
                query = query.Where(pp => pp.Ativo == "1");
            } 
            else if(ativo == "0")
            {
                query = query.Where(pp => pp.Ativo == "0");
            }

            var produtos = query.ToList();

            return produtos;
        }

        public ProdutoModel GetById(int produtoId)
        {
            var produto = _context.Produtos
                .Include(p => p.Tipo)
                .Include(p => p.Tamanho)
                .FirstOrDefault(p => p.Id == produtoId);

            return produto;
        }

        //atualiza um produto
        public void Update(ProdutoModel product)
        {
            _context.Produtos.Update(product);
            _context.SaveChanges();
        }
    }
}
