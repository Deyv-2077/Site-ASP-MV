using System.ComponentModel.DataAnnotations;
using System.Net;

namespace MVCApp2.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Display(Name = "Nome")]
        public string? Name { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal Valor { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "A quantidade deve ser um valor não negativo.")]
        public int Quantidade { get; set; }


    }
}
