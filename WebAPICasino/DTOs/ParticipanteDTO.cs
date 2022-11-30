using WebAPICasino.Entidades;

namespace WebAPICasino.DTOs{
    public class ParticipanteDTO{
        public int Id {get; set;}
        public string Nombre {get; set; }
        public string Email {get; set; }
        public List<BoletoDeLoteria> Boletos {get; set; }
        
        /*public int RifaId {get; set; }
        public Rifa Rifa {get; set;}*/
    }
}
