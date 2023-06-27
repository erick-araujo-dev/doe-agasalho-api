using DoeAgasalhoApiV2._0.Models.CustomModels;
using DoeAgasalhoApiV2._0.Models.Entities;

namespace DoeAgasalhoApiV2._0.Repositories.Interface
{
    public interface IPontoColetaRepository
    {
        PontoColetaModel GetById(int id);

        PontoColetaModel GetByName(string name);

        List<NovoPontoColetaModel> GetByActiveStatus(bool ativo);

        List<NovoPontoColetaModel> GetAll();

        PontoColetaModel Add(NovoPontoColetaModel novoPontoColeta);

        void Update(PontoColetaModel pontoColeta);

        void ActivateCollectPoint(int id);

        void DeactivateCollectPoint(int id);

    }
}
