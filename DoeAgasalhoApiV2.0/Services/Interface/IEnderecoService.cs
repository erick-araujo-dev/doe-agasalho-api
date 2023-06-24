namespace DoeAgasalhoApiV2._0.Services.Interface
{
    public interface IEnderecoService
    {
        void ValidateAddress(int numero, string logradouro, string bairro, string cidade, string estado, string cep);
    }
}
