using DoeAgasalhoApiV2._0.Exceptions;
using DoeAgasalhoApiV2._0.Models.Entities;
using DoeAgasalhoApiV2._0.Repositories.Interface;
using DoeAgasalhoApiV2._0.Services.Interface;

namespace DoeAgasalhoApiV2._0.Services
{
    public class TipoService : ITipoService
    {
        private readonly ITipoRepository _tipoRepository;

        public TipoService (ITipoRepository tipoRepository)
        {
            _tipoRepository = tipoRepository;
        }
        public TipoModel CreateNewType(string type)
        {
            var newType = _tipoRepository.Add(type);
            return newType;
        }

        public TipoModel GetById(int id)
        {
            var type = _tipoRepository.GetById(id); 
            _ = type ?? throw new NotFoundException("Tipo do produto nao encontrado");
            return type;
        }
    }
}
