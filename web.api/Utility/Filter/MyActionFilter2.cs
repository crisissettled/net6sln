using Microsoft.AspNetCore.Mvc.Filters;

namespace web.api.Utility.Filter
{
    public class MyActionFilter2 : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Console.WriteLine("action start2------------->");
            var r =   await next();
            Console.WriteLine("action finish2------------->");
            if(r.Exception!= null)
            {

                Console.WriteLine("There is an exception2!!!");
            }
            else
            {
                Console.WriteLine("Action success2!");
            }
        }
    }
}
