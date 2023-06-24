using DoeAgasalhoApiV2._0.Models;
using DoeAgasalhoApiV2._0.Entities;

namespace DoeAgasalhoApiV2._0.Services
{
    public interface IPontoColetaService
    {
        PontoColeta CreateCollectPoint(NovoPontoColetaModel novoPontoColeta);
    }
}
