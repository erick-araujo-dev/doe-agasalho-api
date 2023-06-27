using DoeAgasalhoApiV2._0.Models.CustomModels;
using DoeAgasalhoApiV2._0.Models.Entities;

namespace DoeAgasalhoApiV2._0.Repositories.Interface
{
    public interface IProdutoRepository
    {
        List<ProdutoModel> GetAll();

        List<ProdutoModel> GetProductActive();

        List<ProdutoModel> GetProductInactive();

        ProdutoModel GetById(int id);

        ProdutoModel GetByStyle(string caracteristica);

        ProdutoModel Add(ProdutoCreateModel product);

        void Update(ProdutoModel usuario);

        void ActivateProduct(int productId);

        void DeactivateProduct(int productId);

        //parei aqui, 26/06/2023 
    }
}
