using System.ComponentModel.DataAnnotations;
using WebAPICasino.Validaciones;

namespace WebAPICasino.Entidades{
    public class Participante{
        public int Id {get; set;}
        [Required(ErrorMessage ="El nombre de la rifa es requerido")]
        [StringLength(maximumLength:100, MinimumLength =1, ErrorMessage ="El nombre de la rifa debe contener al menos 1 caracter y máximo 40")]
        [PrimeraLetraMayuscula]
        public string Nombre {get; set; }
        [Required(ErrorMessage ="La dirección de correo electrónico es requerida")]
        [EmailAddress(ErrorMessage ="Se requiere una dirección de correo electrónico válida")]
        public string Email {get; set; }
        public List<BoletoDeLoteria> Boletos {get; set; }
    }
}
