using System.ComponentModel.DataAnnotations;

namespace WebAPICasino.DTOs{
    public class PremioDTO{
        public int Id {get; set; }
        public string Nombre {get; set; }
        public float Valor {get; set; }
        public int Ganador {get; set; }
    }
}
