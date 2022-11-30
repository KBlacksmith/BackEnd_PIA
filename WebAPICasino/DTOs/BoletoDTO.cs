namespace WebAPICasino.DTOs{
    public class BoletoDTO{
        public int Id {get; set; }
        public int Numero {get; set; }
        public int RifaId {get; set; }
        public int  ParticipanteId{get; set; }
        public bool Ganador {get; set; }
        
        //public Rifa Rifa {get; set; }
    }
}