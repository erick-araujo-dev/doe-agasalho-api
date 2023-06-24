using DoeAgasalhoApiV2._0.Entities;
using DoeAgasalhoApiV2._0.Models;

namespace DoeAgasalhoApiV2._0.Repository.Interface
{
    public interface IUsuarioRepository

    {
        List<Usuario> GetAll();

        Usuario GetByEmail(string email);

        List<Usuario> GetByActiveStatus(bool ativo);

        Usuario GetById(int id);

        Usuario GetByUserName(string name);

        Usuario GetByEmailAndPassword(string email, string password);

        Usuario Add(UsuarioCreateModel usuario);

        void Update(Usuario usuario);

        void ActivateUser(int userId);

        void DeactivateUser(int userId);

    }
}
