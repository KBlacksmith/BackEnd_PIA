using System.ComponentModel.DataAnnotations;
using WebAPICasino.Validaciones;

namespace WebAPICasino.DTOs{
    public class RifaCreacionDTO{
        [Required(ErrorMessage = "El nombre de la rifa es requerido")]
        [StringLength(maximumLength:40, MinimumLength= 1, ErrorMessage ="El nombre de la rifa debe contener al menos 1 caracter y máximo 40")]
        [PrimeraLetraMayuscula]
        public string Nombre {get; set; }
        //public List<int> ParticipantesIds {get; set; }


        /*public List<int> NumerosDisponibles {get; set; }*/
        /*public List<Participante> Participantes {get; set; }*/
        /*public IEnumerable<ValidationResult> Validate(ValidationContext validationContext){
            if(!string.IsNullOrEmpty(Nombre)){
                var primeraLetra = Nombre[0].ToString();
                if(primeraLetra != primeraLetra.ToUpper()){
                    yield return new ValidationResult("La primera letra debe ser mayúscula", 
                    new string[] {nameof(Nombre)});
                }
            }
        }*/
    }
}