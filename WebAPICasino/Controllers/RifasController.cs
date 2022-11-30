using Microsoft.AspNetCore.Mvc;
using WebAPICasino.Entidades;
using Microsoft.EntityFrameworkCore;
using WebAPICasino.DTOs;
using AutoMapper;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebAPICasino.Controllers{
    [ApiController]
    [Route("api/rifas")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
    public class RifasController: ControllerBase{
        private readonly ApplicationDbContext context;
        private readonly ILogger<RifasController> logger;
        private readonly IMapper mapper;
        public RifasController(ApplicationDbContext context, 
        ILogger<RifasController> logger, IMapper mapper){
            this.context = context;
            this.logger = logger;
            this.mapper = mapper;
        }
        [HttpGet]
        [AllowAnonymous]
        [ResponseCache(Duration = 10)]
        public async Task<ActionResult<List<RifaDTOLista>>> Get(){
            //logger.LogInformation("Log: \nObteniendo listado de las rifas activas");
            var rifas = await context.Rifas.ToListAsync();
            //mapper.Map<List<RifaDTOLista>>(rifas);
            return mapper.Map<List<RifaDTOLista>>(rifas);
            //return await context.Rifas.ToListAsync();
        }
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<RifaDTO>> Get(int id){
            var rifa = await context.Rifas.Include(r => r.Premios).FirstOrDefaultAsync(x => x.Id == id);
            if (rifa == null){
                logger.LogWarning("No se encontró la rifa con ID "+id.ToString());
                return NotFound("No se encontró la rifa con ID "+id.ToString());
            }
            return mapper.Map<RifaDTO>(rifa);
        }
        [HttpGet("{nombre}")]
        [AllowAnonymous]
        public async Task<ActionResult<RifaDTO>> Get(string nombre){
            //var rifas = await context.Rifas.Where(x => x.Nombre.ToLower().Contains(nombre.ToLower())).ToListAsync();
            var rifa = await context.Rifas.FirstOrDefaultAsync( x=> x.Nombre.ToLower().Contains(nombre.ToLower()));
            if(rifa == null){
                return NotFound("No se encontró una rifa de nombre: "+nombre);
            }

            return mapper.Map<RifaDTO>(rifa);
        }
        [HttpGet("{id:int}/ganador")]
        public async Task<ActionResult<Object>> Ganador(int id){
            var boletosVendidos = new List<int>();
            var rifa = await context.Rifas.FirstOrDefaultAsync(r => r.Id == id);
            if(rifa == null){
                return NotFound("La rifa con Id "+id.ToString()+" no existe");
            }
            int i = 1; 
            while(i <= 54){
                var disponible = rifa.BoletosDisponibles.Any(num => num == i);
                if(!disponible){
                    boletosVendidos.Add(i);
                }
                i++;
            }
            var rnd = new Random();
            Console.WriteLine("Vendidos");
            Console.WriteLine(boletosVendidos.Count());
            if(boletosVendidos.Count() == 0){
                return BadRequest("No se han vendido boletos");
            }
            int g = rnd.Next(0, boletosVendidos.Count()-1);
            Console.WriteLine("test");
            Console.WriteLine(g);
            var ganador = boletosVendidos.ElementAt(g);
            //int ganador = boletosVendidos[];
            var p_ganador = await context.Boletos.FirstOrDefaultAsync(b => b.RifaId == id && b.Numero == ganador);
            var premios = await context.Premios.Where(p => p.RifaId == id && p.Ganador == 0).ToListAsync();
            Console.WriteLine(premios);
            Console.WriteLine(premios.Count());
            
            if(p_ganador == null || premios.Count() == 0){
                return NotFound("No hay ganador esta ronda");
            }
            var premio = await context.Premios.FirstOrDefaultAsync(p => p.RifaId == id && p.Ganador == 0);
            if(premio == null){
                return NotFound("No hay premio disponible");
            }
            premio.Ganador = ganador;
            context.Update(premio);
            await context.SaveChangesAsync();
            return new{
                p_ganador, 
                premio
            };
        }
        [HttpPost]
        public async Task<ActionResult> Post(RifaCreacionDTO rifaCreacionDTO){
            var existeRifa = await context.Rifas.AnyAsync(x => x.Nombre == rifaCreacionDTO.Nombre);
            if(existeRifa){
                return BadRequest("Ya existe una rifa con el nombre \""+rifaCreacionDTO.Nombre+"\"");
            }
            /*var numeros_disponibles = new List<int> {};
            for(int i = 1; i <= 54; i++){
                numeros_disponibles.Add(i);
            }
            rifa.NumerosDisponibles = numeros_disponibles;*/
            var rifa = mapper.Map<Rifa>(rifaCreacionDTO);
            rifa.BoletosDisponibles = new List<int>();
            for(int i = 1; i <= 54; i++){
                rifa.BoletosDisponibles.Add(i);
            }
            context.Add(rifa);
            await context.SaveChangesAsync();
            return Ok();
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Rifa rifa, int id){
            if(rifa.Id != id){
                logger.LogWarning("El ID de la rifa no coincide con el ID de la URL");
                return BadRequest("El ID de la rifa no coincide con el ID de la URL");
            }
            var existe = await context.Rifas.AnyAsync(x => x.Id == id);
            if (!existe){
                return NotFound("No se encontró la rifa con el ID #"+id.ToString());
            }
            context.Update(rifa);
            await context.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id){
            var existe = await context.Rifas.AnyAsync(rifa => rifa.Id == id);
            if (!existe){
                return NotFound("No se encontró la rifa con el ID #"+id.ToString());
            }
            context.Remove(new Rifa() {Id = id});
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}