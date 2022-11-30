using System.ComponentModel.DataAnnotations;

namespace WebAPICasino.Entidades{
    public class Premio{
        public int Id {get; set; }
        [Required]
        [StringLength(maximumLength: 100)]
        public string Nombre {get; set; }
        [Required]
        [Range(0, float.MaxValue)]
        public float Valor {get; set; }
        public int Ganador {get; set; }
        public int RifaId {get; set; }
        public Rifa Rifa {get; set; }

    }
}
