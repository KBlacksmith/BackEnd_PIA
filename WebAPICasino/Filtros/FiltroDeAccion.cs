using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPICasino.Filtros{
    public class FiltroDeAccion: IActionFilter{
        private readonly ILogger<FiltroDeAccion> logger;

        public FiltroDeAccion(ILogger<FiltroDeAccion> logger){
            this.logger = logger;
        }
        public void OnActionExecuting(ActionExecutingContext context){
            logger.LogInformation("Antes de ejecutar la acción");
        }
        public void OnActionExecuted(ActionExecutedContext context){
            logger.LogInformation("Después de ejecutar la acción");
        }

    }
}