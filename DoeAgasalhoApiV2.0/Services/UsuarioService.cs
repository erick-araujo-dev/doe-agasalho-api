using DoeAgasalhoApiV2._0.Entities;
using DoeAgasalhoApiV2._0.Exceptions;
using DoeAgasalhoApiV2._0.Models;
using DoeAgasalhoApiV2._0.Repositories.Interface;
using DoeAgasalhoApiV2._0.Repository.Interface;
using DoeAgasalhoApiV2._0.Services.Interface;
using System.Text.RegularExpressions;

namespace DoeAgasalhoApiV2._0.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPontoColetaRepository _pontoColetaRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository, IPontoColetaRepository pontoColetaRepository)
        {
            _usuarioRepository = usuarioRepository;
            _pontoColetaRepository = pontoColetaRepository;
        }

        public bool IsActiveUser(Usuario user)
        {
            return user?.Ativo == "1";
        }

        private bool IsEmailAlreadyExists(string email)
        {
            var user = _usuarioRepository.GetByEmail(email);
            return user != null;
        }


        //Validacao tipo de dados 

        //Validacao nome
        private void ValidateUsername(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("O nome não pode ser vazio.");
            }

            // Verificar se o nome contém apenas letras e espaços
            if (!Regex.IsMatch(name, @"^[A-Za-z\s]+$"))
            {
                throw new ArgumentException("O nome contém caracteres inválidos.");
            }
        }

        //Validacao email 
        private void ValidateEmail(string email)
        {
            // Expressao regular para validar o formato do email
            string emailRegexPattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

            if (!Regex.IsMatch(email, emailRegexPattern))
            {
                throw new ArgumentException("O email fornecido é inválido.");
            }
        }

        //Valida senha
        private void ValidatePassword(string password)
        {
            // Verificar o comprimento mínimo de oito caracteres
            if (password.Length < 8)
            {
                throw new ArgumentException("A senha deve conter pelo menos oito caracteres.");
            }

            if (!password.Any(char.IsDigit))
            {
                throw new ArgumentException("A senha deve conter pelo menos um número.");
            }

            if (!password.Any(char.IsSymbol) && !password.Any(char.IsPunctuation))
            {
                throw new ArgumentException("A senha deve conter pelo menos um caractere especial.");
            }

            if (!password.Any(char.IsUpper))
            {
                throw new ArgumentException("A senha deve conter pelo menos uma letra maiúscula.");
            }

            if (!password.Any(char.IsLower))
            {
                throw new ArgumentException("A senha deve conter pelo menos uma letra minúscula.");
            }
        }

        //Valida tipo de usuario
        private void ValidateUserType(string userType)
        {
            if (userType != "admin" && userType != "normal")
            {
                throw new ArgumentException("Tipo de usuário inválido. Esse campo só aceita os valores admin/normal.");
            }
        }

        //Valida ponto de coleta 
        private void ValidateCollectPoint(ChangeCollectPointModel changePointModel)
        {
            /*Verifica se eh admin, se for só aceita valor null, se for normal não aceita valor null e faz 
            uma busca na tabela de ponto de coleta pra ver se o existe o ponto de coleta*/
            
            if (changePointModel.Tipo == "admin" && changePointModel.PontoColetaId.HasValue)
            {
                throw new ArgumentException("Para usuários administradores, o ponto de coleta deve ser nulo.");
            }

            if (changePointModel.Tipo == "normal" && !changePointModel.PontoColetaId.HasValue)
            {
                throw new ArgumentException("O ponto de coleta não pode ser nulo para usuários normais.");
            }

            if (changePointModel.PontoColetaId.HasValue)
            {
                var pontoColeta = _pontoColetaRepository.GetById(changePointModel.PontoColetaId.Value);
                if (pontoColeta == null)
                {
                    throw new NotFoundException("Ponto de coleta não encontrado.");
                }
            }
        }

        private void ValidateCollectPoint(UsuarioCreateModel createUserModel)
        { 
            if (createUserModel.Tipo == "admin" && createUserModel.PontoColetaId.HasValue)
            {
                throw new ArgumentException("Para usuários administradores, o ponto de coleta deve ser nulo.");
            }

            if (createUserModel.Tipo == "normal" && !createUserModel.PontoColetaId.HasValue)
            {
                throw new ArgumentException("O ponto de coleta não pode ser nulo para usuários normais.");
            }

            if (createUserModel.PontoColetaId.HasValue)
            {
                var pontoColeta = _pontoColetaRepository.GetById(createUserModel.PontoColetaId.Value);
                if (pontoColeta == null)
                {
                    throw new NotFoundException("Ponto de coleta não encontrado.");
                }
            }
        }

        //Validacao Ativo
        private void ValidateActive(string ativo)
        {
            if (ativo != "1" && ativo != "0")
            {
                throw new ArgumentException("O valor do campo 'ativo' é inválido. Deve ser '1' ou '0'.");
            }
        }

        //Obter todos usuarios
        public List<Usuario> GetAllUsers()
        {
            return _usuarioRepository.GetAll();
        }

        //Obter usuarios ativos
        public List<Usuario> GetActiveUsers()
        {
            return _usuarioRepository.GetByActiveStatus(true);
        }

        //Obter usuarios desativados
        public List<Usuario> GetInactiveUsers()
        {
            return _usuarioRepository.GetByActiveStatus(false);
        }

        // Add novo usuario
        public Usuario CreateUser(UsuarioCreateModel user)

        {
            if (IsEmailAlreadyExists(user.Email))
            {
                throw new BusinessOperationException("O email fornecido já está sendo utilizado por outro usuário. Por favor, utilize outro email.");
            }

            ValidateUsername(user.Nome);
            ValidateEmail(user.Email);
            ValidatePassword(user.Senha);
            ValidateUserType(user.Tipo);
            ValidateCollectPoint(user);

            return _usuarioRepository.Add(user);
        }


        //Update username
        public Usuario UpdateUsername(int id, int requestingUserId, UpdateUsernameModel user)
        {
            var existingUser = _usuarioRepository.GetById(id);

            if (existingUser == null)
            {
                throw new NotFoundException("Usuário não encontrado.");
            }

            if (requestingUserId != existingUser.Id)
            {
                throw new UnauthorizedAccessException("Você não tem permissão para atualizar este usuário.");
            }

            ValidateUsername(user.Nome);

            existingUser.Nome = user.Nome;

            _usuarioRepository.Update(existingUser);
            return existingUser;
        }

        //Alterar ponto de coleta
        public Usuario ChangeCollectPoint(int id, ChangeCollectPointModel user)
        {
            var existingUser = _usuarioRepository.GetById(id);

            if(existingUser.Tipo == "admin")
            {
                throw new ArgumentException("Usuarios tipo admin não podem estar associados a pontos de coletas");
            }

            if (existingUser.Tipo == "normal" && user.PontoColetaId == null)
            {
                throw new ArgumentException("Voce deve escolher um valor valido.");
            }

            if (existingUser.PontoColetaId == user.PontoColetaId)
            {
                throw new InvalidOperationException("O usuário já esta cadastrado nesse ponto.");
            }

            ValidateCollectPoint(user);

            existingUser.PontoColetaId = user.PontoColetaId;

            _usuarioRepository.Update(existingUser);
            return existingUser;
        }



        //Alteracao de senha
        public Usuario ChangePassword(int id, ChangePasswordModel user, int requestingUserId)
        {

            if (id != requestingUserId)
            {
                throw new UnauthorizedAccessException("Você não tem autorizacao para alterar a senha de outro usuario.");
            }

            var existingUser = _usuarioRepository.GetById(id);

            if (existingUser == null)
            {
                throw new NotFoundException("Usuário não encontrado.");
            }

            // Verificar se a senha atual fornecida corresponde à senha do usuário
            if (!VerifyPassword(existingUser, user.SenhaAtual))
            {
                throw new InvalidOperationException("A senha atual está incorreta.");
            }

            // Validar a nova senha
            ValidatePassword(user.NovaSenha);

            // Atualizar a senha do usuário
            existingUser.Senha = user.NovaSenha;

            _usuarioRepository.Update(existingUser);
            return existingUser;
        }

        private bool VerifyPassword(Usuario user, string password)
        {
            return user.Senha == password;
        }



        //Ativar usuario

        public void ActivateUser(int id)
        {
            var user = _usuarioRepository.GetById(id);

            if (user == null)
            {
                throw new NotFoundException("Usuário não encontrado.");
            }

            if (user.Tipo == "admin")
            {
                throw new UnauthorizedAccessException("Admin não tem permissão para ativar outros admin.");
            }

            ValidateActive(user.Ativo);

            if (user.Ativo == "1")
            {
                throw new InvalidOperationException("O usuário já está ativo.");
            }

            _usuarioRepository.ActivateUser(id);
        }


        //Desativar usuario
        public void DeactivateUser(int id)
        {
            var user = _usuarioRepository.GetById(id);

            if (user == null)
            {
                throw new NotFoundException("Usuário não encontrado.");
            }

            if (user.Tipo == "admin")
            {
                throw new UnauthorizedAccessException("Admin não tem permissão para ativar outros admin.");
            }

            ValidateActive(user.Ativo);

            if (user.Ativo == "0")
            {
                throw new InvalidOperationException("O usuário já está desativado.");
            }

            _usuarioRepository.DeactivateUser(id);
        }

        
    }
}
