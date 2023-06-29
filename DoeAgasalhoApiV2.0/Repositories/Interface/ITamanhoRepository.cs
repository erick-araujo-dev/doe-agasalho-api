using DoeAgasalhoApiV2._0.Models.Entities;

namespace DoeAgasalhoApiV2._0.Services.Interface
{
    public interface ITamanhoRepository
    {
        TamanhoModel Add(string sizeName);

        TamanhoModel GetById(int id);

        TamanhoModel GetByName(string name);

        List<TamanhoModel> GetSizesByFilter(int? type, string characteristic);

        void Update(TamanhoModel model);
    }
}