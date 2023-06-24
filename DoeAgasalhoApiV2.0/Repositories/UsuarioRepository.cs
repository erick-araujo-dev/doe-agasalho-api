using DoeAgasalhoApiV2._0.Context;
using DoeAgasalhoApiV2._0.Entities;
using DoeAgasalhoApiV2._0.Models;
using DoeAgasalhoApiV2._0.Repository.Interface;

namespace DoeAgasalhoApiV2._0.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly DbDoeagasalhov2Context _context;

        public UsuarioRepository(DbDoeagasalhov2Context context)
        {
            _context = context;
        }

        public Usuario GetByEmail(string email)
        {
            return _context.Usuarios.FirstOrDefault(u => u.Email == email);
        }

        public List<Usuario> GetAll()
        {
            return _context.Usuarios.ToList();
        }


        public List<Usuario> GetByActiveStatus(bool ativo)
        {
            string ativoString = ativo ? "1" : "0";
            return _context.Usuarios.Where(u => u.Ativo == ativoString).ToList();
        }

        public Usuario GetById(int id)
        {
            return _context.Usuarios.Find(id);
        }

        public Usuario GetByUserName(string name)
        {
            return _context.Usuarios.FirstOrDefault(u => u.Nome == name);
        }

        public Usuario GetByEmailAndPassword(string email, string password)
        {
            return _context.Usuarios.FirstOrDefault(u => u.Email == email && u.Senha == password);
        }
                

        public Usuario Add(UsuarioCreateModel usuario)
        {
            var novoUsuario = new Usuario
            {
                Nome = usuario.Nome,
                Email = usuario.Email,
                Senha = usuario.Senha,
                Tipo = usuario.Tipo,
                Ativo = "1",
                PontoColetaId = usuario.PontoColetaId
            };

            _context.Usuarios.Add(novoUsuario);
            _context.SaveChanges();

            return novoUsuario;
        }

        public void Update(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            _context.SaveChanges();
        }

        public void ActivateUser(int userId)
        {
            var user = _context.Usuarios.Find(userId);
            if (user != null)
            {
                user.Ativo = "1";
                _context.SaveChanges();
            }
        }

        public void DeactivateUser(int userId)
        {
            var user = _context.Usuarios.Find(userId);
            if (user != null)
            {
                user.Ativo = "0";
                _context.SaveChanges();
            }
        }
    }
}
