using Microsoft.AspNetCore.Mvc;
using WebAPICasino.Entidades;
using Microsoft.EntityFrameworkCore;
using WebAPICasino.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebAPICasino.Controllers{
    [ApiController]
    [Route("api/participantes")]
    public class ParticipantesController: ControllerBase{
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        public ParticipantesController(ApplicationDbContext context, IMapper mapper){
            this.context = context;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<List<ParticipanteDTO>>> Get(){
            var participantes =  await context.Participantes.ToListAsync();
            return mapper.Map<List<ParticipanteDTO>>(participantes);
        }
        
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ParticipanteDTO>> Get(int id){
            var p =  await context.Participantes.FirstOrDefaultAsync(x => x.Id == id);
            if (p == null){
                return NotFound("No se encontró un participante con ID "+id.ToString());
            }
            return mapper.Map<ParticipanteDTO>(p);
        }

        [HttpPost]
        public async Task<ActionResult> Post(ParticipanteCreacionDTO participanteCreacionDTO){
            var existeParticipante = await context.Participantes.AnyAsync(p => p.Nombre == participanteCreacionDTO.Nombre);
            
            if(existeParticipante){
                return BadRequest("Ya existe un participante con el nombre "+participanteCreacionDTO.Nombre);
            }
            var correoEnUso = await context.Participantes.AnyAsync(p => p.Email == participanteCreacionDTO.Email);
            if(correoEnUso){
                return BadRequest("El email '"+participanteCreacionDTO.Email+"' ya se encuentra en uso por otro participante");
            }

            var participante = mapper.Map<Participante>(participanteCreacionDTO);
            participante.Boletos = new List<BoletoDeLoteria>();
            
            context.Add(participante);
            await context.SaveChangesAsync();
            return Ok();
        }
        [HttpPut("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<ActionResult> Put(Participante participante, int id){
            if (participante.Id != id){
                return BadRequest("El ID del participante no corresponde con el ID de la URL");
            }
            var existe = await context.Participantes.AnyAsync(p => p.Id == id);
            if (!existe){
                return BadRequest("No existe el participante con ID: "+id.ToString());
            }
            context.Update(participante);
            await context.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Delete(int id){
            var existe = await context.Participantes.AnyAsync(p => p.Id == id);
            if (!existe){
                return BadRequest("No existe el participante con ID: "+id.ToString());
            }
            context.Remove(new Participante() {Id = id});
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
