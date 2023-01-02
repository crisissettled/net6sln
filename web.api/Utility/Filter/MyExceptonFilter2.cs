using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace web.api.Utility.Filter
{
    public class MyExceptonFilter2 : IAsyncExceptionFilter
    {
        private readonly IWebHostEnvironment _webHostEnv;

        public MyExceptonFilter2(IWebHostEnvironment webHostEnv) { _webHostEnv = webHostEnv; }
        public Task OnExceptionAsync(ExceptionContext context)
        {           
            var resp = new ObjectResult(new { code = 500, msg = _webHostEnv.IsDevelopment() ? context.Exception.StackTrace : "error ocurred" });
            context.Result = resp;
       
            return Task.CompletedTask;
        }
    }
}
