using Microsoft.AspNetCore.Http;
using MVCApp2.Models;
using Newtonsoft.Json;

namespace MVCApp2.Helper
{
    public class Sessao : ISessao
    {
        private readonly IHttpContextAccessor _httpContext;

        public Sessao(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;

        }
        public UserModel2 BuscarSessaoDoUsuario()
        {
            string sessaoUsuario = _httpContext.HttpContext.Session.GetString("sessaoUsuarioLogado");
            if (string.IsNullOrEmpty(sessaoUsuario)) return null;

            return JsonConvert.DeserializeObject<UserModel2>(sessaoUsuario);
        }

        public void CriarSessaoDoUsuario(UserModel2 usuarioModel)
        {
            string valor = JsonConvert.SerializeObject(usuarioModel);

            _httpContext.HttpContext.Session.SetString("sessaoUsuarioLogado", valor);
        }

        public void RemoverSessaoDoUsuario()
        {
            _httpContext.HttpContext.Session.Remove("sessaoUsuarioLogado");
        }
    }
}

