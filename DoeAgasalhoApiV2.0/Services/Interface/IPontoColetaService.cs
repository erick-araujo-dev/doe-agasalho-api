using DoeAgasalhoApiV2._0.Models.CustomModels;
using DoeAgasalhoApiV2._0.Models.Entities;

namespace DoeAgasalhoApiV2._0.Services
{
    public interface IPontoColetaService
    {
        PontoColeta CreateCollectPoint(NovoPontoColetaModel novoPontoColeta);
    }
}
