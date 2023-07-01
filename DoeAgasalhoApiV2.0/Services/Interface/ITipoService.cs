using DoeAgasalhoApiV2._0.Models.Entities;

namespace DoeAgasalhoApiV2._0.Services.Interface
{
    public interface ITipoService
    {
        TipoModel CreateNewType(string type);

        TipoModel GetById(int id);
    }
}