using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPICasino.DTOs;
using WebAPICasino.Entidades;

namespace WebAPICasino.Controllers{
    [ApiController]
    [Route("api/rifas/{rifaId:int}/premios")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PremiosController: ControllerBase{
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper; 
        public PremiosController(ApplicationDbContext context, IMapper mapper){
            this.context = context; 
            this.mapper = mapper;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<List<PremioDTO>>> Get (int rifaId){
            var existeRifa = await context.Rifas.AnyAsync(rifa => rifa.Id == rifaId);
            if(!existeRifa){
                return NotFound("No existe la rifa "+rifaId.ToString());
            }
            var premios = await context.Premios.Where(premio => premio.RifaId == rifaId).ToListAsync();
            return mapper.Map<List<PremioDTO>>(premios);
        }
        [HttpPost]
        public async Task<ActionResult> Post(int rifaId, PremioCreacionDTO premioCreacionDTO){
            var rifa = await context.Rifas.FirstOrDefaultAsync(r => r.Id == rifaId);
            if(rifa == null){
                return NotFound("No existe una rifa con ID "+rifaId.ToString());
            }
            var premio = mapper.Map<Premio>(premioCreacionDTO);
            premio.RifaId = rifaId;
            context.Add(premio);
            await context.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Delete(int id){
            var existe = await context.Premios.AnyAsync(p => p.Id == id);
            if (!existe){
                return BadRequest("No existe un premio con ID: "+id.ToString());
            }
            context.Remove(new Premio() {Id = id});
            await context.SaveChangesAsync();
            return Ok();
        }
    }
    
}

/*
            boleto.RifaId = rifaId;
            context.Add(boleto);

            rifa.BoletosDisponibles.Remove(boletoCreacionDTO.Numero);
            context.Update(rifa);
            await context.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete]
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
*/