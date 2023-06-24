using DoeAgasalhoApiV2._0.Entities;
using DoeAgasalhoApiV2._0.Models;

namespace DoeAgasalhoApiV2._0.Repositories.Interface
{
    public interface IPontoColetaRepository
    {
        PontoColeta GetById(int id);

        List<NovoPontoColetaModel> GetAll();
        
        PontoColeta Add(NovoPontoColetaModel novoPontoColeta);

        void Update(PontoColeta pontoColeta);

        void ActivateCollectPoint(int id);

        void DeactivateCollectPoint(int id);

    }
}
