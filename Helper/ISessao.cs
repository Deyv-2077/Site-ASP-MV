using MVCApp2.Models;

namespace MVCApp2.Helper
{
    public interface ISessao
    {

        void CriarSessaoDoUsuario(UserModel2 usuarioModel);
        void RemoverSessaoDoUsuario();
        UserModel2 BuscarSessaoDoUsuario();


    }
}
