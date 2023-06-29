using DoeAgasalhoApiV2._0.Models.Custom_Models;
using DoeAgasalhoApiV2._0.Models.CustomModels;
using DoeAgasalhoApiV2._0.Models.Entities;

namespace DoeAgasalhoApiV2._0.Services.Interface
{
    public interface IProdutoService
    {
        ProdutoModel CreateProduct(ProdutoCreateModel newProductCreate);

        List<ProdutoModel> GetProdutosByPontoColeta();

        ProdutoModel UpdateProduct(int id, UpdateProdutoModel product);
    }
}