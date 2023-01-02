using System.Reflection;

namespace Middleware.Example;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;
    private readonly IWebHostEnvironment _webHostEnv;

    public LoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory, IWebHostEnvironment webHostEnv)
    {
        _next = next;
        _logger = loggerFactory.CreateLogger<LoggingMiddleware>();
        _webHostEnv = webHostEnv;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var RemoteIP = context.Connection.RemoteIpAddress;
        var path = context.Request.Path;
        var host = context.Request.Host.ToString();
        var scheme = context.Request.Scheme;
        var User = context.Items["User"];
        var User2 = context.User;
        var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;  

        _logger.LogInformation($"{User}:{RemoteIP} - {scheme}://{host}{path}");

        var pathValue = context.Request.Path.Value ?? "";
        var file = _webHostEnv.WebRootFileProvider.GetFileInfo(pathValue);
        if (file.Exists)
        {
            using var stream = file.CreateReadStream();
            using var ms = new MemoryStream();
            stream.CopyTo(ms);
            
             await context.Response.Body.WriteAsync(ms.ToArray());
        }


        try
        { 
            await _next(context);
        }
        catch {
            _logger.LogInformation("Error ocurred!");
        } 
      
    }
}