using DoeAgasalhoApiV2._0.Models.CustomModels;
using DoeAgasalhoApiV2._0.Models.Entities;

namespace DoeAgasalhoApiV2._0.Services.Interface
{
    public interface IDoacaoService
    {
        Task<DoacaoModel> DoacaoProduto(DoacaoEntradaSaidaModel model, string tipoMovimento);

        Task<DoacaoModel> GetDoacao(int id);
    }
}