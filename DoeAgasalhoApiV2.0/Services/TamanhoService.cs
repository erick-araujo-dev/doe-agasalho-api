using DoeAgasalhoApiV2._0.Models.Entities;
using DoeAgasalhoApiV2._0.Services.Interface;
using System.Drawing;

namespace DoeAgasalhoApiV2._0.Services
{
    public class TamanhoService : ITamanhoService
    {
        private readonly ITamanhoRepository _tamanhoRepository;

        public TamanhoService(ITamanhoRepository tamanhoRepository)
        {
            tamanhoRepository = _tamanhoRepository;
        }

        public TamanhoModel? CreateNewSize(string tamanho)
        {
            throw new NotImplementedException();
        }
        public List<TamanhoModel> GetSizesByFilter(int? type, string characteristic)
        {
            return _tamanhoRepository.GetSizesByFilter(type, characteristic);
        }
    }
}
