using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjetoPortfolio.API.Data;
using ProjetoPortfolio.API.Models;
using ProjetoPortfolio.API.Models.DTOs;
using ProjetoPortfolio.API.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjetoPortfolio.API.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly PortfolioDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public TokenService(IConfiguration configuration, TokenValidationParameters tokenValidationParameters, PortfolioDbContext context, UserManager<IdentityUser> userManager)
        {
            _configuration= configuration;
            _tokenValidationParameters = tokenValidationParameters;
            _context = context;
            _userManager = userManager;
        }
        public async Task<AutenticacaoResult> GenerateToken(IdentityUser user) 
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(_configuration.GetSection("JwtConfigs:Key").Value);

            var expireDate = DateTime.UtcNow.AddHours(1);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("Nome", user.UserName)
                }),
                Expires = expireDate,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);

            var jwtToken = jwtTokenHandler.WriteToken(token);

            var refreshToken = new RefreshToken()
            {
                JwtId = token.Id,
                Token = GerarStringAleatoria(32),
                AdicionadoData = DateTime.UtcNow,
                TempoExpiracao = DateTime.UtcNow.AddHours(6),
                Revogado = false,
                Usado = false,
                UsuarioId = user.Id
            };

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            return new AutenticacaoResult()
            {
                Token= jwtToken,
                ExpireDate= expireDate,
                RefreshToken = refreshToken.Token,
                Result = true
            };
        }

        public async Task<AutenticacaoResult> VerificaEGeraToken(TokenRequest request)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var tokenEmVerificacao = jwtTokenHandler.ValidateToken(request.Token, _tokenValidationParameters, out var tokenValidado);
                if(tokenValidado is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

                    if (result == false)
                        throw new Exception("Erro ao verificar token");
                }

                var utcDataExpiracao = long.Parse(tokenEmVerificacao.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
                var dataExpiracao = UnixTimeStampToDateTime(utcDataExpiracao);

                if(dataExpiracao < DateTime.UtcNow)
                {
                    return new AutenticacaoResult()
                    {
                        Result = false,
                        Errors = new List<string>() { "Token expirado" }
                    }; ;
                }

                var tokenPersistido = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == request.RefreshToken);

                if(tokenPersistido == null)
                {
                    return new AutenticacaoResult()
                    {
                        Result = false,
                        Errors = new List<string>()
                        {
                            "Tokens inválidos"
                        }
                    };
                }

                if (tokenPersistido.Usado)
                {
                    return new AutenticacaoResult()
                    {
                        Result = false,
                        Errors = new List<string>()
                        {
                            "Tokens inválidos"
                        }
                    };
                }

                if (tokenPersistido.Revogado)
                {
                    return new AutenticacaoResult()
                    {
                        Result = false,
                        Errors = new List<string>()
                        {
                            "Tokens inválidos"
                        }
                    };
                }

                var jti = tokenEmVerificacao.Claims.FirstOrDefault(t => t.Type == JwtRegisteredClaimNames.Jti)?.Value;

                if(jti == null || tokenPersistido.JwtId != jti)
                {
                    return new AutenticacaoResult()
                    {
                        Result = false,
                        Errors = new List<string>()
                        {
                            "Tokens inválidos"
                        }
                    };
                }

                if(tokenPersistido.TempoExpiracao < DateTime.UtcNow)
                {
                    return new AutenticacaoResult()
                    {
                        Result = false,
                        Errors = new List<string>()
                        {
                            "Tokens expirados"
                        }
                    };
                }

                tokenPersistido.Usado = true;
                _context.RefreshTokens.Update(tokenPersistido);
                await _context.SaveChangesAsync();

                var dbUser = await _userManager.FindByIdAsync(tokenPersistido.UsuarioId);
                var tokenResult = await GenerateToken(dbUser);
                return tokenResult;
            }
            catch (Exception e) 
            {

                return new AutenticacaoResult()
                {
                    Result = false,
                    Errors = new List<string>()
                    {
                        e.Message
                    }
                };
            }
        }

        private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
                var dateTimeVal = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                dateTimeVal = dateTimeVal.AddSeconds(unixTimeStamp).ToUniversalTime();


                return dateTimeVal;
        }

        private string GerarStringAleatoria(int tamanho)
        {
            var aleatorio = new Random();
            var caracteres = "ABCDEFGHIJKLMNOPQRSTUVWYXZ123456789abcdefghijklmnopqrstuvwxyz_";

            return new string(Enumerable.Repeat(caracteres, tamanho).Select(c => c[aleatorio.Next(c.Length)]).ToArray());
        }
    }
}
