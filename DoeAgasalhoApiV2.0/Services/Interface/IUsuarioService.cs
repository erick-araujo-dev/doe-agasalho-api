using DoeAgasalhoApiV2._0.Models.CustomModels;
using DoeAgasalhoApiV2._0.Models.Entities;

namespace DoeAgasalhoApiV2._0.Services.Interface
{
    public interface IUsuarioService

    {

        bool IsActiveUser(UsuarioModel user);

        UsuarioModel CreateUser(UsuarioCreateModel usuario);

        UsuarioModel UpdateUsername(int id, int requestingUserId, UpdateUsernameModel usuario);

        UsuarioModel ChangeCollectPoint(int id, ChangeCollectPointModel usuario);

        UsuarioModel ChangePassword(int id, ChangePasswordModel user, int requestingUserId);

        void ActivateUser(int id);

        void DeactivateUser(int id);

        List<UsuarioModel> GetAllUsers();

        List<UsuarioModel> GetActiveUsers();

        List<UsuarioModel> GetInactiveUsers();

    }
}
