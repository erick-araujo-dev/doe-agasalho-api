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
        private readonly IUtilsService _utilsService;

        public PontoColetaService(IPontoColetaRepository pontoColetaRepository, IEnderecoService enderecoService, IUtilsService utilsService)
        {
            _pontoColetaRepository = pontoColetaRepository;
            _enderecoService = enderecoService;
            _utilsService = utilsService;
        }

        private void _ValidateCollectPointName(string name)
        {
            var user = _pontoColetaRepository.GetByName(name);

            if (user != null)
            {
                throw new BusinessOperationException("O nome fornecido já está sendo utilizado por outra unidade. Por favor, utilize outro nome.");
            }

            if (name.Length > 50)
            {
                throw new ArgumentException("O nome deve ter no máximo 50 caracteres.");
            }
        }

        //Obter todos pontos de 
        public List<PontoColetaCreateModel> GetAllCollectPoint() => _pontoColetaRepository.GetAll();

        //Obter pontos de coleta ativos
        public List<PontoColetaCreateModel> GetActivateCollectPoint() => _pontoColetaRepository.GetByActiveStatus(true);

        //Obter pontos de coletas desativados
        public List<PontoColetaCreateModel> GetInactiveCollectPoint() => _pontoColetaRepository.GetByActiveStatus(false);

        public bool IsActiveCollectPoint(UsuarioModel user)
        {
            if (user.PontoColetaId == null && user.Tipo != "admin")
            {
                throw new BusinessOperationException("Falha ao fazer login, usuário não está associado a nenhum ponto de coleta.");
            }

            var collectPointId = user.PontoColetaId;

            if (collectPointId.HasValue)
            {
                var idPonto = collectPointId.Value;
                var collectPoint = _pontoColetaRepository.GetById(idPonto);

                if (collectPoint != null)
                {
                    return collectPoint.Ativo == "1";
                }
            }

            return false;
        }

        public PontoColetaModel CreateCollectPoint(PontoColetaCreateModel novoPontoColeta)
        {
            _ValidateCollectPointName(novoPontoColeta.NomePonto);

            _enderecoService.ValidateAddress(
                novoPontoColeta.Numero,
                novoPontoColeta.Logradouro,
                novoPontoColeta.Complemento,
                novoPontoColeta.Bairro,
                novoPontoColeta.Cidade,
                novoPontoColeta.Estado,
                novoPontoColeta.Cep
                );

            //Abrevia e valida o estado
            novoPontoColeta.Estado = _enderecoService.AbbreviateState(novoPontoColeta.Estado);
            _enderecoService.ValidateStateName(novoPontoColeta.Estado);

            //formata o cep
            novoPontoColeta.Cep = _enderecoService.FormatZipCode(novoPontoColeta.Cep);

            return _pontoColetaRepository.Add(novoPontoColeta);
        }

        //Update username Ponto de Coleta 
        public PontoColetaModel UpdateUsername(int id, UpdateUsernameModel pontoColeta)
        {
            var collectPoint = _pontoColetaRepository.GetById(id);
            _ = collectPoint ?? throw new NotFoundException("Ponto de Coleta não encontrado.");


            _ValidateCollectPointName(pontoColeta.Nome);

            collectPoint.NomePonto = pontoColeta.Nome;

            _pontoColetaRepository.Update(collectPoint);
            return collectPoint;
        }

        //Ativar usuario

        public void ActivateCollectPoint(int id)
        {
            var collectPoint = _pontoColetaRepository.GetById(id);
            _ = collectPoint ?? throw new NotFoundException("Ponto de Coleta não encontrado.");

            _utilsService.ValidateActive(collectPoint.Ativo);

            if (collectPoint.Ativo == "1")
            {
                throw new InvalidOperationException("Ponto de coleta já está ativo.");
            }

            _pontoColetaRepository.ActivateCollectPoint(id);
        }


        //Desativar Ponto de coleta
        public void DeactivateCollectPoint(int id)
        {
            var collectPoint = _pontoColetaRepository.GetById(id);

            _ = collectPoint ?? throw new NotFoundException("Ponto de Coleta não encontrado.");

            _utilsService.ValidateActive(collectPoint.Ativo);

            if (collectPoint.Ativo == "0")
            {
                throw new InvalidOperationException("Ponto de coleta já está inativo.");
            }

            _pontoColetaRepository.DeactivateCollectPoint(id);
        }
    }
}