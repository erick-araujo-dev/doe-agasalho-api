using DoeAgasalhoApiV2._0.Models.CustomModels;
using DoeAgasalhoApiV2._0.Models.Entities;

namespace DoeAgasalhoApiV2._0.Services
{
    public interface IPontoColetaService
    {
        bool IsActiveCollectPoint(UsuarioModel user);

        List<NovoPontoColetaModel> GetAllCollectPoint();

        List<NovoPontoColetaModel> GetActivateCollectPoint();

        List<NovoPontoColetaModel> GetInactiveCollectPoint();

        PontoColetaModel CreateCollectPoint(NovoPontoColetaModel novoPontoColeta);

        PontoColetaModel UpdateUsername(int id, UpdateUsernameModel pontoColeta);

        void ActivateCollectPoint(int id);

        void DeactivateCollectPoint(int id);
    }
}
