using DoeAgasalhoApiV2._0.Entities;
using DoeAgasalhoApiV2._0.Models;

namespace DoeAgasalhoApiV2._0.Services.Interface
{
    public interface IUsuarioService

    {

        bool IsActiveUser(Usuario user);

        Usuario CreateUser(UsuarioCreateModel usuario);

        Usuario UpdateUsername(int id, int requestingUserId, UpdateUsernameModel usuario);

        Usuario ChangeCollectPoint(int id, ChangeCollectPointModel usuario);

        Usuario ChangePassword(int id, ChangePasswordModel user, int requestingUserId);

        void ActivateUser(int id);

        void DeactivateUser(int id);

        List<Usuario> GetAllUsers();

        List<Usuario> GetActiveUsers();

        List<Usuario> GetInactiveUsers();

    }
}
