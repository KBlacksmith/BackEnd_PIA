using System.ComponentModel.DataAnnotations;
using WebAPICasino.Validaciones;

namespace WebAPICasino.DTOs{
    public class PremioCreacionDTO{
        [Required]
        [StringLength(maximumLength: 100)]
        public string Nombre {get; set; }
        [Required]
        [Range(0, float.MaxValue)]
        public float Valor {get; set; }
    }
}
