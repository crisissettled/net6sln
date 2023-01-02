using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using web.api.Model;
using web.api.Utility;

namespace web.api.Controllers.v1;

public class TestController : ApiControllerBaseV1
{
    [HttpGet]
    public string Get(string name)
    {
        this.User.FindFirst(ClaimTypes.Name);
        return name;
    }

    [HttpPost]
    public string Post(Person p)
    {
        return $"{p.Name} - {DateTime.Now.ToShortTimeString()}";
    }
}

