using DoeAgasalhoApiV2._0.Repositories.Interface;
using DoeAgasalhoApiV2._0.Models.CustomModels;
using DoeAgasalhoApiV2._0.Services.Interface;
using DoeAgasalhoApiV2._0.Models.Entities;

namespace DoeAgasalhoApiV2._0.Services
{
    public class PontoColetaService : IPontoColetaService
    {
        private readonly IPontoColetaRepository _pontoColetaRepository;
        private readonly IEnderecoService _enderecoService;

        public PontoColetaService(IPontoColetaRepository pontoColetaRepository, IEnderecoService enderecoService)
        {
            _pontoColetaRepository = pontoColetaRepository;
            _enderecoService = enderecoService;
        }

        //Implenar metodo pra verificar se nome ja esta em uso

        public PontoColeta CreateCollectPoint(NovoPontoColetaModel novoPontoColeta)
        {

            _enderecoService.ValidateAddress(
                novoPontoColeta.Numero,
                novoPontoColeta.Logradouro,
                novoPontoColeta.Bairro,
                novoPontoColeta.Cidade,
                novoPontoColeta.Estado,
                novoPontoColeta.Cep
                );
            return _pontoColetaRepository.Add(novoPontoColeta);
        }

    }
}
