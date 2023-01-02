using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace web.api.Utility.Filter
{
    public class MyExceptonFilter : IAsyncExceptionFilter
    {
        private readonly IWebHostEnvironment _webHostEnv;

        public MyExceptonFilter(IWebHostEnvironment webHostEnv) { _webHostEnv = webHostEnv; }
        public Task OnExceptionAsync(ExceptionContext context)
        {
            var resp = new ObjectResult(new { code = 500, msg = _webHostEnv.IsDevelopment() ? context.Exception.StackTrace : "error ocurred" });
            context.Result = resp;
            //context.ExceptionHandled = true;
            return Task.CompletedTask;
        }
    }
}
