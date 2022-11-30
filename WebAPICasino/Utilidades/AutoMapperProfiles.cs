using AutoMapper;
using WebAPICasino.DTOs;
using WebAPICasino.Entidades;

namespace WebAPICasino.Utilidades{
    public class AutoMapperProfiles: Profile{
        public AutoMapperProfiles(){

            //Rifas
            CreateMap<RifaCreacionDTO, Rifa>();
            CreateMap<Rifa, RifaDTOLista>();
            CreateMap<Rifa, RifaDTO>();

            //Participantes
            CreateMap<ParticipanteCreacionDTO, Participante>();
            CreateMap<Participante, ParticipanteDTO>();
            CreateMap<BoletoCreacionDTO, BoletoDeLoteria>();
            CreateMap<BoletoDeLoteria, BoletoDTO>();
            CreateMap<PremioCreacionDTO, Premio>();
            CreateMap<Premio, PremioDTO>();
        }
    }
}