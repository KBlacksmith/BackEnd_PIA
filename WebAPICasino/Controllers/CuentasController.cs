using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebAPICasino.DTOs;
using WebAPICasino.Servicios;

namespace WebAPICasino.Controllers{
    [ApiController]
    [Route("api/cuentas")]
    public class CuentasController: ControllerBase{
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IDataProtector dataProtector;
        private readonly HashService hashService;
        public CuentasController(UserManager<IdentityUser> userManager, 
        IConfiguration configuration, SignInManager<IdentityUser> signInManager, 
        IDataProtectionProvider dataProtectionProvider, HashService hashService){
            this.userManager = userManager;
            this.configuration = configuration;
            this.signInManager = signInManager;
            this.hashService = hashService;
            this.dataProtector =  dataProtectionProvider.CreateProtector("mi_valor_de_encriptación");
        }
        /*[HttpGet("hash/{textoPlano}")]
        public ActionResult RealizarHash(string textoPlano){
            var resultado1 = hashService.Hash(textoPlano);
            var resultado2 = hashService.Hash(textoPlano);
            return Ok(new {
                textoPlano = textoPlano, 
                Hash1 = resultado1, 
                Hash2 = resultado2
            });
        }*/
        /*[HttpGet("encriptar")]
        public ActionResult Encriptar(){
            var textoPlano = "Kenneth Rodriguez";
            var textoCifrado = dataProtector.Protect(textoPlano);
            var textoDesencriptado = dataProtector.Unprotect(textoCifrado);
            return Ok(new {
                textoPlano = textoPlano, 
                textoCifrado = textoCifrado, 
                textoDesencriptado = textoDesencriptado
            });
        }*/
        [HttpPost("registrar")]
        public async Task<ActionResult<RespuestaAutenticacion>> Registrar(CredencialesUsuario credencialesUsuario){
            var usuario = new IdentityUser {UserName = credencialesUsuario.Email, 
            Email = credencialesUsuario.Email};
            var resultado = await userManager.CreateAsync(usuario, credencialesUsuario.Password);
            if(resultado.Succeeded){
                return await ConstruirToken(credencialesUsuario);
            }
            else{
                return BadRequest(resultado.Errors);
            }
        }
        [HttpPost("login")]
        public async Task<ActionResult<RespuestaAutenticacion>> Login(CredencialesUsuario credencialesUsuario){
            var resultado = await signInManager.PasswordSignInAsync(credencialesUsuario.Email, credencialesUsuario.Password, 
            isPersistent: false, lockoutOnFailure: false);
            if (resultado.Succeeded){
                return await ConstruirToken(credencialesUsuario);
            }
            else{
                return BadRequest("Login incorrecto");
            }
        }
        [HttpGet("renovarToken")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<RespuestaAutenticacion>> Renovar(){
            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;

            var credencialesUsuario = new CredencialesUsuario(){
                Email = email
            }; 
            return await ConstruirToken(credencialesUsuario);
        }
        private async Task<RespuestaAutenticacion> ConstruirToken(CredencialesUsuario credencialesUsuario){
            var claims = new List<Claim>{
                new Claim("email", credencialesUsuario.Email)
            };
            var usuario = await userManager.FindByEmailAsync(credencialesUsuario.Email);
            var claimsDB = await userManager.GetClaimsAsync(usuario);

            claims.AddRange(claimsDB);

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["llavejwt"]));
            var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);
            var expiracion = DateTime.UtcNow.AddDays(1);
            var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims, 
            expires: expiracion, signingCredentials: creds);
            return new RespuestaAutenticacion(){
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                Expiracion = expiracion
            };
        }
        [HttpPost("HacerAdmin")]
        public async Task<ActionResult> HacerAdmin(EditarAdminDTO editarAdminDTO){
            var usuario = await userManager.FindByEmailAsync(editarAdminDTO.Email);
            await userManager.AddClaimAsync(usuario, new Claim("esAdmin", "1"));
            return NoContent();
        }
        [HttpPost("RemoverAdmin")]
        public async Task<ActionResult> RemoverAdmin(EditarAdminDTO editarAdminDTO){
            var usuario = await userManager.FindByEmailAsync(editarAdminDTO.Email);
            await userManager.RemoveClaimAsync(usuario, new Claim("esAdmin", "1"));
            return NoContent();
        }
    }
}