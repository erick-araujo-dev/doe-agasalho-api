using DoeAgasalhoApiV2._0.Models.Custom_Models;
using DoeAgasalhoApiV2._0.Models.CustomModels;
using DoeAgasalhoApiV2._0.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DoeAgasalhoApiV2._0.Repositories.Interface
{
    public interface IProdutoRepository
    {
        ProdutoModel GetSingleByFilter(Func<ProdutoModel, bool> filter);

        List<ProdutoModel> GetAllByFilter(Func<ProdutoModel, bool> filter);

        List<ProdutoModel> GetFilteredProducts(int? tipoId, int? tamanhoId, string genero, string caracteristica);

        void Update(ProdutoModel product);

        void ActivateProduct(int productId);

        void DeactivateProduct(int productId);

        List<string> GetCharacteristicsByFilter(int tipoId, int tamanhoId);

        ProdutoModel Add(ProdutoModel product, int collectPointId, int userId);

        List<ProdutoViewModel> GetProdutosByPontoColeta(int pontoColetaId, string ativo = null);

        ProdutoModel GetById(int produtoId);

        //void AtualizarEstoque(int produtoId, int quantidade, string tipoOperacao);
    }
}
