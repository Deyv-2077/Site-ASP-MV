using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MVCApp2.Models;

namespace MVCApp2.Repositorio
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    { 
        
            private readonly DataContext _context;

            public UsuarioRepositorio(DataContext dataContext)
            {
                this._context = dataContext;
            }

            public UserModel2 BuscarPorLogin(string login)
            {
          
            
            throw new NotImplementedException();


            // return _context.Users.FirstOrDefault(x => x.Login.ToUpper() == login.ToUpper());
        }

        public List<UserModel2> BuscarTodos()
            {
                return _context.Users.ToList();
            }

            public UserModel2 BuscarPorID(int id)
            {
            throw new NotImplementedException();

            // return _context.Users.FirstOrDefault(x => x.Id == id);
        }

        public UserModel2 Adicionar(UserModel2 usuario)
            {
                usuario.DataCadastro = DateTime.Now;
                _context.Users.Add(usuario);
                _context.SaveChanges();
                return usuario;
            }

            public UserModel2 Atualizar(UserModel2 usuario)
            {
                // Implemente a lógica para atualizar o usuário no banco de dados
                throw new NotImplementedException();
            }

            public bool Apagar(int id)
            {
                // Implemente a lógica para apagar o usuário do banco de dados
                throw new NotImplementedException();
            }
        

        UserModel2 IUsuarioRepositorio.BuscarPorLogin(string login)
        {
            throw new NotImplementedException();
        }

        UserModel2 IUsuarioRepositorio.BuscarPorID(int id)
        {
            throw new NotImplementedException();
        }
    }
}
