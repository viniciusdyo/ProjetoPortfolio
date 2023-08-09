﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using ProjetoPortfolio.Web.Models;
using ProjetoPortfolio.Web.Models.ViewModels;

namespace ProjetoPortfolio.Web.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class AdminConteudoController : Controller
    {
        private readonly string ENDPOINT = "https://localhost:44318/api";
        private readonly HttpClient _httpClient = null;
        public AdminConteudoController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(ENDPOINT);
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var responseTask = await _httpClient.GetAsync($"{ENDPOINT}/Conteudo/Conteudos");
                if (responseTask.IsSuccessStatusCode)
                {
                    var result = responseTask.Content;
                    var readResult = await result.ReadAsStringAsync();
                    var conteudos = JsonConvert.DeserializeObject<List<ConteudoModel>>(readResult);


                    if (conteudos == null || conteudos.Count == 0)
                        throw new Exception("Nenhum conteudo encontrado.");

                    List<ConteudoViewModel> conteudoView = new List<ConteudoViewModel>();

                    var categoriasResponse = await _httpClient.GetAsync($"{ENDPOINT}/CategoriaConteudo/Categorias");
                    if (categoriasResponse.IsSuccessStatusCode)
                    {
                        var readCategoriasResult = await categoriasResponse.Content.ReadAsStringAsync();

                        var categorias = JsonConvert.DeserializeObject<List<CategoriaConteudoModel>>(readCategoriasResult);

                        if (categorias == null || categorias.Count == 0)
                            throw new Exception("Categorias não encontradas");

                        ViewBag.Categorias = categorias;

                        foreach (var item in conteudos)
                        {
                            CategoriaConteudoModel categoria = item.CategoriaConteudoModel;

                            var i = new ConteudoViewModel()
                            {
                                Conteudo = new ConteudoModel()
                                {
                                    Id = item.Id,
                                    Nome = item.Nome,
                                    Titulo = item.Titulo,
                                    Conteudo = item.Conteudo
                                },
                                CategoriaConteudo = categoria,
                                Categorias = categorias
                            };
                            conteudoView.Add(i);
                        }
                    }




                    if (conteudoView == null || conteudoView.Count == 0)
                        throw new Exception("Nenhum conteudo encontrado.");

                    return View(conteudoView);
                }
                throw new Exception("Nenhum conteudo encontrado.");
            }
            catch (Exception e)
            {
                ViewData["Erro"] = e;
                return View();
            }
        }

        public async Task<IActionResult> Adicionar(ConteudoViewModel conteudoView)
        {
            try
            {
                if (conteudoView.Conteudo == null)
                    throw new Exception("Conteúdo inválido");

                ConteudoModel conteudoModel = new ConteudoModel()
                {
                    Id = Guid.Empty,
                    Nome = conteudoView.Conteudo.Nome,
                    Titulo = conteudoView.Conteudo.Titulo,
                    Conteudo = conteudoView.Conteudo.Conteudo,
                    CategoriaConteudoId = conteudoView.Conteudo.CategoriaConteudoId,
                    CategoriaConteudoModel = new CategoriaConteudoModel()
                    {
                        CategoriaConteudoId = conteudoView.Conteudo.CategoriaConteudoId,
                        Descricao = string.Empty,
                        Nome = string.Empty
                    },
                };

                if (conteudoModel == null)
                    throw new Exception("Conteúdo inválido");

                var responseTask = await _httpClient.PostAsJsonAsync($"{ENDPOINT}/Conteudo/CadastrarConteudo", conteudoModel);

                if (responseTask.IsSuccessStatusCode)
                {
                    RedirectToAction("Index");
                }
                throw new Exception("Erro no servidor");
            }
            catch (Exception e)
            {

                return RedirectToAction("Index", e);
            }
        }

        public async Task<IActionResult> Editar(ConteudoViewModel conteudoView)
        {
            try
            {
                
                if (conteudoView.Conteudo == null) throw new Exception("Conteúdo inválido");
                ConteudoModel conteudoModel = new ConteudoModel()
                {
                    Id = conteudoView.Conteudo.Id,
                    Nome = conteudoView.Conteudo.Nome,
                    Titulo = conteudoView.Conteudo.Titulo,
                    Conteudo = conteudoView.Conteudo.Conteudo,
                    CategoriaConteudoId = conteudoView.Conteudo.CategoriaConteudoId,
                    CategoriaConteudoModel = new CategoriaConteudoModel()
                    {
                        CategoriaConteudoId = conteudoView.Conteudo.CategoriaConteudoId,
                        Descricao = string.Empty,
                        Nome = string.Empty
                    },
                };
                var conteudoResponse = await _httpClient.PutAsJsonAsync($"{ENDPOINT}/Conteudo/Editar", conteudoModel);

                if (conteudoResponse.IsSuccessStatusCode)
                    RedirectToAction("Index");


                throw new Exception("Erro no servidor");
            }
            catch (Exception e)
            {

                return RedirectToAction("Index", e);
            }
        }
    }
}
