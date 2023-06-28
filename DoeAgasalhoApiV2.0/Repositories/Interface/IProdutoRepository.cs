using DoeAgasalhoApiV2._0.Models.CustomModels;
using DoeAgasalhoApiV2._0.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DoeAgasalhoApiV2._0.Repositories.Interface
{
    public interface IProdutoRepository
    {
        List<ProdutoModel> GetAll();

        List<ProdutoModel> GetProductActive();

        List<ProdutoModel> GetProductInactive();

        ProdutoModel GetSingleByFilter(Func<ProdutoModel, bool> filter);

        List<ProdutoModel> GetAllByFilter(Func<ProdutoModel, bool> filter);

        List<ProdutoModel> GetFilteredProducts(int? tipoId, int? tamanhoId, string genero, string caracteristica);

        ProdutoModel Add(ProdutoModel product);

        void Update(ProdutoModel product);

        void ActivateProduct(int productId);

        void DeactivateProduct(int productId);

        List<string> GetCharacteristicsByFilter(int tipoId, int tamanhoId);
    }
}
