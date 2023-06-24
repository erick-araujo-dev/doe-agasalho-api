using DoeAgasalhoApiV2._0.Models;
using DoeAgasalhoApiV2._0.Entities;
using DoeAgasalhoApiV2._0.Repositories.Interface;

namespace DoeAgasalhoApiV2._0.Services
{
    public class PontoColetaService : IPontoColetaService
    {
        private readonly IPontoColetaRepository _pontoColetaRepository;

        public PontoColetaService(IPontoColetaRepository pontoColetaRepository)
        {
            _pontoColetaRepository = pontoColetaRepository;
        }

        public PontoColeta CreateCollectPoint(NovoPontoColetaModel novoPontoColeta)
        {

            ValidateAddress(
                novoPontoColeta.Numero,
                novoPontoColeta.Logradouro,
                novoPontoColeta.Bairro,
                novoPontoColeta.Cidade,
                novoPontoColeta.Estado,
                novoPontoColeta.Cep
                );
            return _pontoColetaRepository.Add(novoPontoColeta);
        }


        private void ValidateAddress(int numero, string logradouro, string bairro, string cidade, string estado, string cep)
        {
            if (numero < 0 || string.IsNullOrWhiteSpace(logradouro) || string.IsNullOrWhiteSpace(bairro)
                || string.IsNullOrWhiteSpace(cidade) || string.IsNullOrWhiteSpace(estado)
                || string.IsNullOrWhiteSpace(cep))
            {
                throw new ArgumentException("Todos os campos do endereço, exceto o complemento, devem ser preenchidos corretamente.");
            }

            // Abreviar se necessário
            estado = AbreviarEstado(estado);

            // Valida se a sigla é válida
            if (!ValidarSiglaEstado(estado))
            {
                throw new ArgumentException("Sigla de estado inválida.");
            }

            // Valida se o nome está correto
            if (!ValidarNomeEstado(estado))
            {
                throw new ArgumentException("Nome de estado inválido.");
            }
        }

        private string AbreviarEstado(string estado)
        {
            // Exemplo de implementação simples:
            switch (estado.ToUpper())
            {
                case "SÃO PAULO":
                    return "SP";
                case "RIO DE JANEIRO":
                    return "RJ";
                // Adicione mais casos conforme necessário
                default:
                    return estado;
            }
        }

        private bool ValidarSiglaEstado(string estado)
        {
            // Implemente a lógica para validar se a sigla do estado é válida
            // Verifique em uma lista de siglas de estados aceitas

            // Exemplo de implementação simples:
            var siglasValidas = new List<string> { "SP", "RJ", "MG", "RS" };
            return siglasValidas.Contains(estado.ToUpper());
        }

        private bool ValidarNomeEstado(string estado)
        {
            // Implemente a lógica para validar se o nome do estado está correto
            // Verifique em uma lista de nomes de estados aceitos

            // Exemplo de implementação simples:
            var nomesValidos = new List<string> { "SÃO PAULO", "RIO DE JANEIRO", "MINAS GERAIS", "RIO GRANDE DO SUL" };
            return nomesValidos.Contains(estado.ToUpper());
        }



    }
}
