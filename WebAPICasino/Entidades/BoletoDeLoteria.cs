using System.ComponentModel.DataAnnotations;

namespace WebAPICasino.Entidades{
    public class BoletoDeLoteria{
        public int Id {get; set; }
        [Required]
        public int Numero {get; set; }
        [Required]
        public int RifaId {get; set; }
        [Required]
        public int ParticipanteId {get; set; }
        public Participante Participante {get; set; }
        public bool Ganador {get; set; }
    }
}
