using System.ComponentModel.DataAnnotations;
using WebAPICasino.Validaciones;

namespace WebAPICasino.Entidades{
    public class Rifa{//}: IValidatableObject{
        public int Id {get; set; }
        [Required(ErrorMessage = "El nombre de la rifa es requerido")]
        [StringLength(maximumLength:40, MinimumLength= 1, ErrorMessage ="El nombre de la rifa debe contener al menos 1 caracter y máximo 40")]
        [PrimeraLetraMayuscula]
        public string Nombre {get; set; }
        public List<int> BoletosDisponibles {get; set; }
        public List<Premio> Premios {get; set; }
        //public List<BoletoDeLoteria> Boletos {get; set; }
        //public List<ParticipanteRifa> ParticipantesRifas {get; set; }
    }
 }