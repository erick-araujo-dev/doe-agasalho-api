using DoeAgasalhoApiV2._0.Models.CustomModels;
using DoeAgasalhoApiV2._0.Models.Entities;

namespace DoeAgasalhoApiV2._0.Repositories.Interface
{
    public interface IPontoColetaRepository
    {
        PontoColeta GetById(int id);

        PontoColeta GetByName(string name);

        List<NovoPontoColetaModel> GetAll();
        
        PontoColeta Add(NovoPontoColetaModel novoPontoColeta);

        void Update(PontoColeta pontoColeta);

        void ActivateCollectPoint(int id);

        void DeactivateCollectPoint(int id);

    }
}
