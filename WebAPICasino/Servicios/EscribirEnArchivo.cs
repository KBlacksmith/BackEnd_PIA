using System.Threading;
using System.Threading.Tasks;

namespace WebAPICasino.Servicios{
    public class EscribirEnArchivo: IHostedService{
        private readonly IWebHostEnvironment env;
        private readonly string nombreArchivo = "Archivo 1.txt";
        //private readonly ApplicationDbContext dbContext;
        private Timer timer;
        public EscribirEnArchivo(IWebHostEnvironment env){
            this.env = env;
        }
        public Task StartAsync(CancellationToken cancellationToken){
            timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromHours(1));
            Escribir("Proceso Iniciado");
            return Task.CompletedTask;
        }
        public Task StopAsync(CancellationToken cancellationToken){
            Escribir("Proceso Finalizado");
            return Task.CompletedTask;
        }
        private void DoWork(object state){
            Escribir("Proceso en ejecución: "+DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));
            /*var boletos = dbContext.Boletos.ToList();
            boletos.ForEach(boleto =>{
                Escribir(boleto.ToString());
            });*/
        }
        private void Escribir(string mensaje){
            var ruta = $@"{env.ContentRootPath}/wwwroot/{nombreArchivo}";
            using (StreamWriter writer = new StreamWriter(ruta, append: true)){
                writer.WriteLine(mensaje);
            }
        }
    }
}