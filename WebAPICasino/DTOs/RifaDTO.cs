namespace WebAPICasino.DTOs{
    public class RifaDTO{
        public int Id {get; set; }
        public string Nombre {get; set; }
        public List<int> BoletosDisponibles {get; set; }
        //public List<BoletoDTO> Boletos {get; set; }
    }
}