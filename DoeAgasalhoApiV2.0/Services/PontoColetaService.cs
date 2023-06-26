using DoeAgasalhoApiV2._0.Repositories.Interface;
using DoeAgasalhoApiV2._0.Models.CustomModels;
using DoeAgasalhoApiV2._0.Services.Interface;
using DoeAgasalhoApiV2._0.Models.Entities;
using DoeAgasalhoApiV2._0.Exceptions;

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

        private void NameAlreadyUsed(string name)
        {
            var user = _pontoColetaRepository.GetByName(name);

            if (user != null)
            {
                throw new BusinessOperationException("O nome fornecido já está sendo utilizado por outra unidade. Por favor, utilize outro nome.");
            }
        }

        public PontoColeta CreateCollectPoint(NovoPontoColetaModel novoPontoColeta)
        {
            NameAlreadyUsed(novoPontoColeta.NomePonto);

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