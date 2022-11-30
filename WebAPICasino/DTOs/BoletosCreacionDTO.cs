using System.ComponentModel.DataAnnotations;

namespace WebAPICasino.DTOs{
    public class BoletoCreacionDTO{
        [Required]
         public int Numero {get; set; }
    }
}