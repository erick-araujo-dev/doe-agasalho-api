using DoeAgasalhoApiV2._0.Exceptions;
using DoeAgasalhoApiV2._0.Models.Entities;
using DoeAgasalhoApiV2._0.Repositories.Interface;
using DoeAgasalhoApiV2._0.Services.Interface;

namespace DoeAgasalhoApiV2._0.Services
{
    public class TamanhoService : ITamanhoService
    {
        private readonly ITamanhoRepository _tamanhoRepository;

        public TamanhoService(ITamanhoRepository tamanhoRepository)
        {
            _tamanhoRepository = tamanhoRepository;
        }

        public TamanhoModel CreateNewSize(string size)
        {
            var newSize = _tamanhoRepository.Add(size);
            return newSize;
        }
        public List<TamanhoModel> GetSizesByFilter(int? type, string characteristic)
        {
            return _tamanhoRepository.GetSizesByFilter(type, characteristic);
        }

        public TamanhoModel GetById(int id)
        {
            var size = _tamanhoRepository.GetById(id);
            _ = size ?? throw new NotFoundException("Tamanhp do produto nao encontrado");
            return size;
        }
    }
}
