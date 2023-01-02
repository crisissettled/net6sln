using Microsoft.AspNetCore.Mvc.Filters;

namespace web.api.Utility.Filter
{
    public class MyActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Console.WriteLine("action start------------->");
            var r =   await next();
            Console.WriteLine("action finish------------->");
            if(r.Exception!= null)
            {

                Console.WriteLine("There is an exception!!!");
            }
            else
            {
                Console.WriteLine("Action success!");
            }
        }
    }
}
