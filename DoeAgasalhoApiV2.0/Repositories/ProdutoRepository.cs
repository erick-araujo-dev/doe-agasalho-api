using DoeAgasalhoApiV2._0.Data.Context;
using DoeAgasalhoApiV2._0.Models.Entities;
using DoeAgasalhoApiV2._0.Repositories.Interface;

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
                Quantidade = null,
                UsuarioId = userId
            };

            _context.Doacoes.Add(DonationRegister);
            _context.SaveChanges(); 

            return newProduct;
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

        //obtem todos 
        public List<ProdutoModel> GetAll()
        {
            return _context.Produtos.ToList();
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
        public List<ProdutoModel> GetFilteredProducts(int? tipoId, int? tamanhoId, string genero, string caracteristica)
        {
            var query = _context.Produtos.AsQueryable();

            if (tipoId.HasValue)
            {
                query = query.Where(p => p.TipoId == tipoId.Value);
            }

            if (tamanhoId.HasValue)
            {
                query = query.Where(p => p.TamanhoId == tamanhoId.Value);
            }

            if (!string.IsNullOrEmpty(genero))
            {
                query = query.Where(p => p.Genero == genero);
            }

            if (!string.IsNullOrEmpty(caracteristica))
            {
                query = query.Where(p => p.Caracteristica == caracteristica);
            }

            return query.ToList();
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

        //retorna os produtos ativos
        public List<ProdutoModel> GetProductActive()
        {
            return _context.Produtos.Where(p => p.Ativo == "1").ToList();
        }

        //retorna os produtos inativos
        public List<ProdutoModel> GetProductInactive()
        {
            return _context.Produtos.Where(p => p.Ativo == "0").ToList();
        }

        //Retorna os produtos de acordo com o ponto de coleta
        public List<ProdutoModel> GetProdutosByPontoColetaId(int pontoColetaId)
        {
            var produtos = _context.PontoProdutos
                .Where(pp => pp.PontoColetaId == pontoColetaId)
                .Select(pp => pp.Produto)
                .ToList();

            return produtos;
        }

        //parei aqui, preciso fazer com que retorn o nome do produto e nao o id

        //atualiza um produto
        public void Update(ProdutoModel product)
        {
            _context.Produtos.Update(product);
            _context.SaveChanges();
        }
    }
}
