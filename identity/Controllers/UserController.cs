using identity.classes;
using identity.classes.Postgresql;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using NpgsqlTypes;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace identity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> LoginAsync(string user, string pwd, [FromServices] JwtSettings jwtSettings, [FromServices] IDbPostgresql dbPostgresql, [FromServices] IDbPostgresql dbPostgresql2)
        {
            string userInDB = "";
            string pwdInDB = "";

            //var pgSettings = optionPgSettings.Value;
            //await using var con = new NpgsqlConnection(pgSettings.ConnectionString) ;
            //await con.OpenAsync();

            //await using(var cmd = con.CreateCommand())
            //{
            //    cmd.CommandText = "select * from users where userid = :pUser";
            //    cmd.Parameters.Add("pUser", NpgsqlDbType.Varchar).Value = user;
            //    using (var dr = await cmd.ExecuteReaderAsync())
            //    {
            //        if (await dr.ReadAsync())
            //        {
            //            userInDB = dr["userid"].ToString();
            //            pwdInDB = dr["pwd"].ToString();
            //        }
            //    }
            //}
            var sql = "select * from users where userid = :pUser";
            await dbPostgresql.Prepare(sql);
            dbPostgresql.AddParameter("pUser", NpgsqlDbType.Varchar, user);
            var dr = await dbPostgresql.ExecuteReaderAsync();
            if (await dr.ReadAsync())
            {
                userInDB = dr["userid"].ToString();
                pwdInDB = dr["pwd"].ToString();
            }


   
            await dbPostgresql2.Prepare(sql);
            dbPostgresql2.AddParameter("pUser", NpgsqlDbType.Varchar, user);
            var dr2 = await dbPostgresql2.ExecuteReaderAsync();
            if (await dr2.ReadAsync())
            {
                userInDB = dr2["userid"].ToString();
                pwdInDB = dr2["pwd"].ToString();
            }

            if (user == userInDB && pwd == pwdInDB)
            {
                var jwtTokenHandler = new JwtSecurityTokenHandler();
                var jwtTokenKey = Encoding.UTF8.GetBytes(jwtSettings.Key!);
                var jwtTokenDescriptor = new SecurityTokenDescriptor
                {
                    Issuer = jwtSettings.Issuer,
                    Audience = jwtSettings.Audience,
                    Subject = new ClaimsIdentity(new Claim[]{
                        new Claim(ClaimTypes.Name,user)
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(jwtSettings.ExpiredInMin),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(jwtTokenKey), SecurityAlgorithms.HmacSha256Signature)
                };

                var securityToken = jwtTokenHandler.CreateToken(jwtTokenDescriptor);
                var token = jwtTokenHandler.WriteToken(securityToken);

                return Ok(new
                {
                    success = true,
                    token
                });
            }
            else
            {
                return new BadRequestResult();
            }

        }

        [HttpPost]
        [Authorize]
        public IActionResult Test(TestRequest request)
        {
            return Ok($"Hello world: {request.User}");
        }

        [HttpPut]
        [Authorize]
        public IActionResult Test2([FromForm] string name)
        {
            return Ok($"Hello world: {name}");
        }
    }

    public class TestRequest
    {
        public string User { get; set; }

    }
}