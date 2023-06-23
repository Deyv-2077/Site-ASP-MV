using System.ComponentModel.DataAnnotations;
using System.Net;

namespace MVCApp2.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Display(Name = "Nome")]
        public string? Name { get; set; }
        public string Address { get; set; }
        public decimal Valor { get; set; }
        public int Quantidade { get; set; }


    }
}
