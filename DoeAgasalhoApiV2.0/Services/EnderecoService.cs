using DoeAgasalhoApiV2._0.Services.Interface;
using System.Text;

namespace DoeAgasalhoApiV2._0.Services
{
    public class EnderecoService : IEnderecoService
    {
        public void ValidateAddress(int numero, string logradouro, string bairro, string cidade, string estado, string cep)
        {
            if (numero < 0 || string.IsNullOrWhiteSpace(logradouro) || string.IsNullOrWhiteSpace(bairro)
                || string.IsNullOrWhiteSpace(cidade) || string.IsNullOrWhiteSpace(estado)
                || string.IsNullOrWhiteSpace(cep))
            {
                throw new ArgumentException("Todos os campos do endereço, exceto o complemento, devem ser preenchidos corretamente.");
            }

            // Abreviar se necessário
            estado = AbreviarEstado(estado);

            //Formata o cep
            FormatarCep(cep);

            // Valida se a sigla é válida
            if (!ValidarSiglaEstado(estado))
            {
                throw new ArgumentException("Digite um Estado válido.");
            }
        }

        private string AbreviarEstado(string estado)
        {
            switch (estado.ToUpperInvariant().Normalize(NormalizationForm.FormD))
            {
                case "ACRE":
                    return "AC";
                case "ALAGOAS":
                    return "AL";
                case "AMAPA":
                    return "AP";
                case "AMAZONAS":
                    return "AM";
                case "BAHIA":
                    return "BA";
                case "CEARA":
                    return "CE";
                case "DISTRITO FEDERAL":
                    return "DF";
                case "ESPIRITO SANTO":
                    return "ES";
                case "GOIAS":
                    return "GO";
                case "MARANHAO":
                    return "MA";
                case "MATO GROSSO":
                    return "MT";
                case "MATO GROSSO DO SUL":
                    return "MS";
                case "MINAS GERAIS":
                    return "MG";
                case "PARA":
                    return "PA";
                case "PARAIBA":
                    return "PB";
                case "PARANA":
                    return "PR";
                case "PERNAMBUCO":
                    return "PE";
                case "PIAUI":
                    return "PI";
                case "RIO DE JANEIRO":
                    return "RJ";
                case "RIO GRANDE DO NORTE":
                    return "RN";
                case "RIO GRANDE DO SUL":
                    return "RS";
                case "RONDONIA":
                    return "RO";
                case "RORAIMA":
                    return "RR";
                case "SANTA CATARINA":
                    return "SC";
                case "SAO PAULO":
                    return "SP";
                case "SERGIPE":
                    return "SE";
                case "TOCANTINS":
                    return "TO";
                default:
                    return estado;
            }
        }

        private bool ValidarSiglaEstado(string estado)
        {
            var siglasValidas = new List<string> { "AC", "AL", "AP", "AM", "BA", "CE", "DF", "ES", "GO", "MA", "MT", "MS", "MG", "PA", "PB", "PR", "PE", "PI", "RJ", "RN", "RS", "RO", "RR", "SC", "SP", "SE", "TO" };
            return siglasValidas.Contains(estado.ToUpper());
        }

        private string FormatarCep(string cep)
        {
            if (!string.IsNullOrWhiteSpace(cep) && cep.Length == 8)
            {
                cep = cep.Insert(5, "-");
            }

            return cep;
        }

    }
}
