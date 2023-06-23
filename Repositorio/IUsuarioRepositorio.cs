using MVCApp2.Models;

namespace MVCApp2.Repositorio
{
    public interface IUsuarioRepositorio
    {

        UserModel2 BuscarPorLogin(string login);
        List<UserModel2> BuscarTodos();
        UserModel2 BuscarPorID(int id);
        UserModel2 Adicionar(UserModel2 usuario);
        UserModel2 Atualizar(UserModel2 usuario);
        bool Apagar(int id);


    }
}
