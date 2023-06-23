using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;
using MVCApp2.Enums;


namespace MVCApp2.Models
{
    public class LoginModel
    {
        public string? Login { get; set; }

        public string? Senha { get; set; }

    }
}
