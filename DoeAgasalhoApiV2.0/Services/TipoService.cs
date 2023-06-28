using DoeAgasalhoApiV2._0.Models.Entities;
using DoeAgasalhoApiV2._0.Repositories.Interface;
using DoeAgasalhoApiV2._0.Services.Interface;

namespace DoeAgasalhoApiV2._0.Services
{
    public class TipoService : ITipoService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly ITipoRepository _tipoRepository;

        public TipoService (IProdutoRepository produtoRepository, ITipoRepository tipoRepository)
        {
            _produtoRepository = produtoRepository;
            _tipoRepository = tipoRepository;
        }
        public TipoModel? CreateNewType(string type)
        {
            var typeName = _tipoRepository.GetByName(type);
            if (typeName != null)
            {
                throw new InvalidOperationException("Tipo já cadastrado.");
            }
            //Parei aqui 27/06
            return null;
         }

        public List<string> GetCharacteristicsByFilter(int TipoId, int TamanhoId)
        {
            return _produtoRepository.GetCharacteristicsByFilter(TipoId, TamanhoId);
        }
    }
}
