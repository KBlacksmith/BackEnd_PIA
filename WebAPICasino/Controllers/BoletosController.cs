using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPICasino.DTOs;
using WebAPICasino.Entidades;

namespace WebAPICasino.Controllers{
    [ApiController]
    [Route("api/rifas/{rifaId:int}/boletos")]
    public class BoletosController: ControllerBase{
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        public BoletosController(ApplicationDbContext context, IMapper mapper){
            this.context = context;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<List<BoletoDTO>>> Get(int rifaId){
            var existeRifa = await context.Rifas.AnyAsync(rifa => rifa.Id == rifaId);
            if (!existeRifa){
                return NotFound("No existe la rifa con ID "+rifaId.ToString());
            }
            //var boletoExiste = await context.Boletos.AnyAsync(boleto => boleto.RifaId == rifaId && boleto.Numero == nu)
            var boletos = await context.Boletos.Where(boleto => boleto.RifaId == rifaId).ToListAsync();
            return mapper.Map<List<BoletoDTO>>(boletos);
        }

        [HttpPost]
        [HttpPost("comprarBoleto")]
        public async Task<ActionResult> Post(int rifaId, int participanteId, BoletoCreacionDTO boletoCreacionDTO){
            var rifa = await context.Rifas.FirstOrDefaultAsync(r => r.Id == rifaId);
            if(rifa == null){
                return NotFound("No existe una rifa con ID "+rifaId.ToString());
            }

            var participante = await context.Participantes.FirstOrDefaultAsync(p => p.Id == participanteId); 
            if(participante == null){
                return NotFound("No existe un participante con ID "+participanteId.ToString());
            }

            var boletoDisponible = rifa.BoletosDisponibles.Any(num => num == boletoCreacionDTO.Numero);
            if(!boletoDisponible){
                return BadRequest("El boleto "+boletoCreacionDTO.Numero.ToString()+" no se encuentra disponible en la rifa "+rifaId.ToString());
            }
            var participa = await context.Boletos.FirstOrDefaultAsync(b => b.RifaId == rifaId && b.ParticipanteId == participanteId);
            if(participa != null){
                return BadRequest("El participante "+participanteId.ToString()+" ya participa en la rifa "+rifaId.ToString());
            }
            /*if (participante.Boletos.Any(b => b.RifaId == rifaId)){
                return BadRequest("El participante "+participanteId.ToString()+" ya participa en esta rifa");
            }*/
            var boleto = mapper.Map<BoletoDeLoteria>(boletoCreacionDTO);
            boleto.RifaId = rifaId;
            boleto.ParticipanteId = participanteId;
            context.Add(boleto);

            rifa.BoletosDisponibles.Remove(boletoCreacionDTO.Numero);
            context.Update(rifa);
            await context.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Delete(int rifaId, int numero){
            var boleto = await context.Boletos.FirstOrDefaultAsync(boletoDB => boletoDB.RifaId == rifaId && boletoDB.Numero == numero);
            if(boleto == null){
                return NotFound("No existe el boleto "+numero.ToString()+" en la rifa "+rifaId.ToString());
            }
            var rifa = await context.Rifas.FirstOrDefaultAsync(rifasDB=> rifasDB.Id == rifaId);
            //rifa.Boletos.Remove(boleto);
            rifa.BoletosDisponibles.Add(numero);

            context.Remove(boleto);
            context.Update(rifa);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}