using Microsoft.AspNetCore.Mvc;

namespace Middleware.Example;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;

    }

    public async Task Invoke(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if(token == null){ 
            
            context.Items["User"] = "Unknown User";

            //await context.Response.WriteAsync("un authorized");
            //return;
        
        }    
        else { context.Items["User"] = "Authenticated User"; }

        await _next(context);
    }    
}