using DoeAgasalhoApiV2._0.Services.Interface;

namespace DoeAgasalhoApiV2._0.Services
{
    public class UtilsService : IUtilsService
    {
        //Validacao Ativo
        public void ValidateActive(string ativo)
        {
            if (ativo != "1" && ativo != "0")
            {
                throw new ArgumentException("O valor do campo 'ativo' é inválido. Deve ser '1' ou '0'.");
            }
        }
    }
    
}
