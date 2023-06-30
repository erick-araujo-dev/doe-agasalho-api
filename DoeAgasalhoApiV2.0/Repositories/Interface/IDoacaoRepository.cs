using DoeAgasalhoApiV2._0.Models.Entities;

namespace DoeAgasalhoApiV2._0.Repositories.Interface
{
    public interface IDoacaoRepository
    {
        Task<DoacaoModel> Add(int produtoId, int quantidade, int userId, string tipoMovimento);

        Task<DoacaoModel> GetDoacao(int id);
    }
}