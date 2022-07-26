namespace ProjetoAWS.web.Middlewares
{
    public class MiddlewareUsuario
    {
        private readonly RequestDelegate _next;
        public MiddlewareUsuario(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (System.Exception ex)
            {
                context.Response.StatusCode = 400;
            }


        }
    }
}