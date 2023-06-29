using DoeAgasalhoApiV2._0.Exceptions;
using DoeAgasalhoApiV2._0.Repositories.Interface;
using DoeAgasalhoApiV2._0.Services.Interface;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace DoeAgasalhoApiV2._0.Services
{
    public class UtilsService : IUtilsService
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPontoColetaRepository _pontoColetaRepository;

        public UtilsService(IHttpContextAccessor httpContextAccessor, IPontoColetaRepository pontoColetaRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _pontoColetaRepository = pontoColetaRepository;
        }

        //Validacao Ativo
        public void ValidateActive(string ativo)
        {
            if (ativo != "1" && ativo != "0")
            {
                throw new ArgumentException("O valor do campo 'ativo' é inválido. Deve ser '1' ou '0'.");
            }
        }

        //Valida os dados recebido, recebe o valor, o campo que esta sendo preenchido, o tamanho maximo permitido, e se o campo aceita numeros ou nao
        public void ValidateStringField(string value, string field, int maxLength, bool isNumeric)
        {
            //Verifica se nao eh null ou vazia
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException($"O campo '{field}' não pode estar vazio(a).");
            }

            // Verificar se o nome pode conter numeros e se contém apenas letras numeros hifen e espaços
            if (isNumeric)
            {


                if (!Regex.IsMatch(value, @"^[A-Za-z0-9\s\-]+$"))
                {
                    throw new ArgumentException($"O campo '{field}' contém caracteres inválidos.");
                }
            } else
            {
                // Se nao puder conter numeros, verificaca se so contem letras e espacos

                if (!Regex.IsMatch(value, @"^[\p{L}\s]+$", RegexOptions.IgnoreCase))
                {
                    throw new ArgumentException($"O campo '{field}' contém caracteres inválidos.");
                }
            }

            //Verifica o tamanho
            if (value.Length > maxLength)
            {
                throw new ArgumentException($"O campo '{field}' deve ter no máximo {maxLength} caracteres.");
            }
        }

        //Gera o Id do Ponto de Coleta do usuario autenticado para utilizacao em outros metodos 
        public int GetPontoColetaIdFromToken()
        {
            var pontoColetaIdClaim = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Locality);

            if (!string.IsNullOrEmpty(pontoColetaIdClaim) && int.TryParse(pontoColetaIdClaim, out int pontoColetaId))
            {
                var pontoColeta = _pontoColetaRepository.GetById(pontoColetaId);

                if (pontoColeta == null || pontoColeta.Ativo == "0")
                {
                    throw new NotFoundException("Ponto de coleta não encontrado.");
                }

                return pontoColetaId;

            }

            // A claim PontoColetaId é inválida ou não foi encontrada
            throw new UnauthorizedAccessException("PontoColetaId inválido ou não encontrada.");
        }

        //Obtem do Id do usuario autenticado, se for null gera um Unauthorized
        public int GetUserIdFromToken()
        {
            var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }

            throw new UnauthorizedAccessException("ID do usuário não encontrado no token.");
        }


    }

}
