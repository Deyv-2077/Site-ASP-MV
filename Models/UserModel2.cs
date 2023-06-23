using MVCApp2.Enums;
using System;

namespace MVCApp2.Models
{
    public class UserModel2
    {

        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Login { get; set; }

        public string? Email { get; set; }

        public PerfilEnum Perfil { get; set; }

        public string? Senha { get; set; }

        public DateTime DataCadastro { get; set; }

        public DateTime DataAtualizacao { get; set; }

        public bool SenhaValida(string senha)
        {
            return Senha == senha;
        }

    }
}
