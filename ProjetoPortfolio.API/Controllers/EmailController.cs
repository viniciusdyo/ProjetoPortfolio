﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjetoPortfolio.API.Models;
using ProjetoPortfolio.API.Models.DTOs;
using ProjetoPortfolio.API.Models.DTOs.Response;
using ProjetoPortfolio.API.Repositories.Interfaces;
using ProjetoPortfolio.API.Services.Interfaces;

namespace ProjetoPortfolio.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailConfigRepository _emailConfigRepository;
        private readonly IEmailService _emailService;

        public EmailController(IEmailConfigRepository emailConfigRepository, IEmailService emailService)
        {
            _emailConfigRepository = emailConfigRepository;
            _emailService = emailService;
        }

        [HttpPost("Cadastrar")]
        public async Task<IActionResult> Cadastrar(EmailConfigDto emailConfigDto)
        {
            try
            {
                if (emailConfigDto == null)
                    throw new ArgumentNullException("Conteúdo inválido ou nulo.");

                PorfolioResponse<EmailConfigModel> response = await _emailConfigRepository.Cadastrar(emailConfigDto);

                if (response == null)
                    throw new ArgumentNullException("response");

                if (response.Errors.Any())
                    throw new Exception(response.Errors.First());

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("BuscarPorId")]

        public async Task<IActionResult> BuscarPorId(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentOutOfRangeException("id");
                if (id == null)
                    throw new ArgumentNullException("id");

                PorfolioResponse<EmailConfigDto> response = await _emailConfigRepository.BuscarPorId(id);

                if (response == null)
                    throw new ArgumentNullException("response");

                if (response.Errors.Any())
                    throw new Exception(response.Errors.First());


                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Deletar")]

        public async Task<IActionResult> Remover(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentOutOfRangeException("id");

                PorfolioResponse<EmailConfigModel> response = await _emailConfigRepository.Remover(id);

                if (response == null)
                    throw new ArgumentNullException("Response");

                if (response.Errors.Any())
                    throw new Exception(response.Errors.First());

                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Editar")]

        public async Task<IActionResult> Editar(int id, EmailConfigDto emailConfigDto)
        {
            try
            {
                if (emailConfigDto == null)
                    new ArgumentNullException("Dto");

                if (emailConfigDto.Id <= 0)
                    emailConfigDto.Id = id;

                PorfolioResponse<EmailConfigModel> response = await _emailConfigRepository.Editar(id, emailConfigDto);

                if (response == null)
                    throw new ArgumentNullException("Response");

                if(response.Errors.Any())
                    throw new Exception(response.Errors.First());

                return Ok(response);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost("EnviarEmail")]

        public async Task<IActionResult> EnviarEmail(string assunto, string html, string de = null)
        {
            try
            {
                if (assunto == null)
                    throw new ArgumentNullException("Assunto");
                if (html == null)
                    throw new ArgumentNullException("Html");
                if (de == null)
                    throw new ArgumentNullException("De");

                PorfolioResponse<EmailConfigDto> response = await _emailConfigRepository.BuscarPorId(3);

                if (response == null)
                    throw new ArgumentNullException("response");

                if (response.Errors.Any())
                    throw new Exception(response.Errors.First());

                if(response.Results == null || !response.Results.Any())
                    throw new ArgumentNullException("results");

                EmailConfigDto emailConfig = response.Results.FirstOrDefault();

                if(emailConfig == null)
                    throw new ArgumentNullException("emailConfig");
                    
                _emailService.Enviar(emailConfig,assunto, html, de);

                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
