using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyBankIdentity.DTO;
using Newtonsoft.Json;

namespace MyBankIdentity.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JwtMiddlewareOptions _options;


        public JwtMiddleware(RequestDelegate next, IOptions<JwtMiddlewareOptions> options)
        {
            _next = next;
            _options = options.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            bool isUserAttached = true;
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(' ').Last();

            if (token != null)
            {
                isUserAttached = AttachUserToContext(context, token);
            }

            if (isUserAttached)
            {
                await _next(context);
            }
            else
            {
                return;
            }
        }

        private bool AttachUserToContext(HttpContext context, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_options.Secret);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    //ClockSkew = TimeSpan.Zero
                };

                SecurityToken validatedToken;

                tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                var user = JsonConvert.DeserializeObject<UserDTO>(jwtToken.Claims.First(x => x.Type == "user").Value);


                ModuleDTO module = user.Role.Modules.FirstOrDefault(m => m.Name.ToLower() == _options.ModuleName.ToLower());

                if (module == null)
                {
                    context.Response.StatusCode = 403;
                    context.Response.WriteAsync("You do not have an access to this module");
                    return false;
                }


                user.Role.Modules.Clear();
                user.Role.Modules.Add(module);

                //if (!IsModuleAllowed(user))
                //{
                //    context.Response.StatusCode = 403;
                //    context.Response.WriteAsync("You do not have an access to this module");
                //    return false;
                //}




                // attach user to context on successful jwt validation
                context.Items["User"] = JsonConvert.SerializeObject(user);

            }
            catch (SecurityTokenExpiredException ex)
            {
                //context.Response.Clear();
                context.Response.StatusCode = 401;
                context.Response.WriteAsync("JWT has expired");
                return false;
            }

            catch (Exception ex)
            {
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
            }
            return true;
        }

        private bool IsModuleAllowed(UserDTO user)
        {
            var exist = user.Role.Modules.Any(m => m.Name.ToLower() == _options.ModuleName.ToLower());
            //var exist = user.Role.Modules.Any(m => m.Name.ToLower() == _jwtConfig.Module.ToLower());
            if (exist)
            {
                return true;
            }
            return false;
        }

    }

}
