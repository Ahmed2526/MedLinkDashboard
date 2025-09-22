namespace MedLinkDashboard.MiddleWares
{
    public class BlockTheFrench
    {
        private readonly RequestDelegate _next;

        public BlockTheFrench(RequestDelegate next)
        {
            _next = next;
        }

        //Needs implementation
        public async Task InvokeAsync(HttpContext context)
        {
            var data = context;


            await _next.Invoke(context);

        }

    }
}
