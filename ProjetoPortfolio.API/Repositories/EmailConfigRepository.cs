using ProjetoPortfolio.API.Data;
using ProjetoPortfolio.API.Models;
using ProjetoPortfolio.API.Models.DTOs.Response;
using ProjetoPortfolio.API.Repositories.Interfaces;
using System.Text;
using System.Security.Cryptography;
using ProjetoPortfolio.API.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ProjetoPortfolio.API.Repositories
{
    public class EmailConfigRepository : IEmailConfigRepository
    {
        private readonly PortfolioDbContext _dbContext;
        private readonly static string chave = "3wnwJtNjUdXOy3uKQzfknwDKiEPZr/yqG5St9WnReIo=";

        public EmailConfigRepository(PortfolioDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PorfolioResponse<EmailConfigModel>> Cadastrar(EmailConfigDto emailConfig)
        {
            try
            {
                if (emailConfig == null)
                {
                    throw new Exception("Email inválido ou nulo");
                }

                using (Aes senhaAes = Aes.Create())
                {

                    byte[] senhaEncriptada = Encriptar(emailConfig.Senha);

                    EmailConfigModel emailConfigModel = new()
                    {
                        Email = emailConfig.Email,
                        Nome = emailConfig.Nome,
                        Porta = emailConfig.Porta,
                        Senha = senhaEncriptada,
                        Servidor = emailConfig.Servidor,
                    };
                    await _dbContext.EmailConfig.AddAsync(emailConfigModel);
                    await _dbContext.SaveChangesAsync();

                    PorfolioResponse<EmailConfigModel> response = new()
                    {
                        Results = new List<EmailConfigModel>() { emailConfigModel },
                        Errors = new List<string>() { }
                    };

                    return response;
                }


            }
            catch (Exception ex)
            {
                PorfolioResponse<EmailConfigModel> response = new()
                {
                    Errors = new List<string>() { ex.Message },
                    Results = new List<EmailConfigModel>()
                };

                return response;
            }
        }

        public async Task<PorfolioResponse<EmailConfigDto>> BuscarPorId(int id)
        {
            try
            {
                if (id == null || id < 0)
                    throw new ArgumentNullException("Id");

                EmailConfigModel model = await _dbContext.EmailConfig.FirstOrDefaultAsync(x => x.Id == id);

                if (model == null)
                    throw new ArgumentNullException("Model");


                string senha = null;


                senha = Decriptar(model.Senha);


                PorfolioResponse<EmailConfigDto> response = new()
                {
                    Results = new List<EmailConfigDto>()
                    {
                        new EmailConfigDto()
                        {
                            Id = model.Id,
                            Nome = model.Nome,
                            Servidor = model.Servidor,
                            Email = model.Email,
                            Porta = model.Porta,
                            Senha = senha,
                        },
                    },
                    Errors = new List<string>()
                };

                return response;
            }
            catch (Exception ex)
            {
                return new PorfolioResponse<EmailConfigDto>()
                {
                    Results = new List<EmailConfigDto>(),
                    Errors = new List<string>() { ex.Message }
                };
            }
        }


        public async Task<PorfolioResponse<EmailConfigModel>> Editar(int id, EmailConfigDto emailConfig)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentOutOfRangeException("Id");

                if (emailConfig == null)
                    throw new ArgumentNullException("Model");

                if (emailConfig.Id == 0)
                    emailConfig.Id = id;

                EmailConfigModel configPorId = await _dbContext.EmailConfig.FirstOrDefaultAsync(x => x.Id == id);

                if (configPorId == null)
                    throw new ArgumentNullException("Config por Id");

                if (configPorId.Id <= 0)
                    throw new ArgumentOutOfRangeException("Config Id");

                string senhaDescriptada = Decriptar(configPorId.Senha);
                if (senhaDescriptada == emailConfig.Senha)
                {
                    configPorId.Nome = emailConfig.Nome;
                    configPorId.Servidor = emailConfig.Servidor;
                    configPorId.Porta = emailConfig.Porta;
                    configPorId.Email = emailConfig.Email;
                    
                } else
                {
                    configPorId.Nome = emailConfig.Nome;
                    configPorId.Servidor = emailConfig.Servidor;
                    configPorId.Porta = emailConfig.Porta;
                    configPorId.Email = emailConfig.Email;
                    configPorId.Senha = Encriptar(emailConfig.Senha);
                }

                _dbContext.EmailConfig.Update(configPorId);
                await _dbContext.SaveChangesAsync();

                return new PorfolioResponse<EmailConfigModel>()
                {
                    Results = new List<EmailConfigModel>() { configPorId },
                    Errors = new List<string>()
                };
            }
            catch (Exception ex)
            {
                return new PorfolioResponse<EmailConfigModel> { Results = new List<EmailConfigModel>(), Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<PorfolioResponse<EmailConfigModel>> Remover(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentOutOfRangeException("id");

                EmailConfigModel model = await _dbContext.EmailConfig.FirstOrDefaultAsync(x => x.Id == id);

                if (model == null)
                    throw new ArgumentNullException("Model");

                _dbContext.EmailConfig.Remove(model);
                await _dbContext.SaveChangesAsync();

                return new PorfolioResponse<EmailConfigModel>() { Results = new List<EmailConfigModel>(), Errors = new List<string>() };
            }
            catch (Exception ex)
            {

                return new PorfolioResponse<EmailConfigModel>
                {
                    Results = new List<EmailConfigModel>(),
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public static byte[] Encriptar(string senha)
        {
            try
            {
                if (senha == null || senha.Length <= 0)
                    throw new ArgumentNullException("Senha");

                byte[] encriptado;
                using (Aes aesEncriptar = Aes.Create())
                {
                    using (Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(chave, new byte[16]))
                    {
                        byte[] aesKey = rfc.GetBytes(32);
                        byte[] iv = rfc.GetBytes(16);
                        aesEncriptar.Key = aesKey;
                        aesEncriptar.IV = iv;
                    }

                    ICryptoTransform encriptador = aesEncriptar.CreateEncryptor(aesEncriptar.Key, aesEncriptar.IV);

                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encriptador, CryptoStreamMode.Write))
                        {
                            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                            {
                                swEncrypt.Write(senha);
                            }

                            encriptado = msEncrypt.ToArray();
                        }
                    }
                }

                return encriptado;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public static string Decriptar(byte[] senha)
        {
            try
            {
                if (senha == null)
                    throw new ArgumentNullException("Senha");

                using (Aes aesDecriptar = Aes.Create())
                {
                    using (Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(chave, new byte[16]))
                    {
                        byte[] aesKey = rfc.GetBytes(32);
                        byte[] iv = rfc.GetBytes(16);
                        aesDecriptar.Key = aesKey;
                        aesDecriptar.IV = iv;
                    }

                    ICryptoTransform decriptador = aesDecriptar.CreateDecryptor(aesDecriptar.Key, aesDecriptar.IV);

                    using (MemoryStream msDecrypt = new MemoryStream(senha))
                    {
                        using (CryptoStream csDescrypt = new CryptoStream(msDecrypt, decriptador, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDescrypt))
                            {
                                return srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


    }
}
