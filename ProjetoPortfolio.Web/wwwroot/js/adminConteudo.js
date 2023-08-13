import { fetchGet } from './fetchHelper';
import { Modal } from 'bootstrap'

const adminConteudo = () => {

    const listarConteudos = async () => {
        var conteudos = await fetchGet('AdminConteudo/Listar');
        var conteudoRoot = document.querySelector('#conteudos');
        conteudoRoot.classList.add('animacao-pagina');


        for (var i of conteudos) {
            var card = `<div class="col-4">
            <div class="card bg-dark-darker text-white border-primary">
                <h5 class="card-header border-primary text-primary"> ${i.titulo}</h5>
                <div class="card-body">
                    <h5 class="card-title">${i.nome}</h5>
                    <p class="card-text">${i.conteudo}</p>
                    <div class="d-flex align-content-center">

                        <p class="card-text me-2">
                            Categoria:

                        </p> <a class="text-decoration-none dropdown-toggle" data-bs-toggle="dropdown" aria-expanded="false">
                            ${i.categoriaConteudoModel.nome}
                        </a>
                        <ul class="dropdown-menu dropdown-menu-dark py-3">
                            <li>
                                <p class="px-2 py-0 my-0 text-secondary">Descrição</p>
                                <p class="px-2 py-0 my-0 text-white">${i.categoriaConteudoModel.descricao}</p>
                            </li>
                        </ul>
                    </div>

                </div>
                <div id="card-footer-${i.nomeNormalizado}" class="card-footer border-primary">
                    <a id="btn-editar-${i.nomeNormalizado}" data-bs-toggle="modal" data-bs-target="#editar-${i.nomeNormalizado}-modal" class="btn btn-primary text-white">Editar</a>
                    <a class="btn"></a>
                </div>
            </div>
        </div>`;
            conteudoRoot.innerHTML += card;
        }

        for (var i of conteudos) {
            var main = document.querySelector('main');
            main.innerHTML += modalEditarConteudo(i);
        }

        for (var i of conteudos) {
            var modal = document.querySelector(`#${i.nomeNormalizado}-form`);
            modal.addEventListener('submit', e => {
                e.preventDefault()
                var editarFormData = new FormData(modal);
                console.log('eae')
                editarFormData.forEach(e => {
                    console.log(e)
                })
            });
        }
    }


    var modalEditarConteudo = (conteudo) => {
        var modal = `<div class="modal fade" id="editar-${conteudo.nomeNormalizado}-modal" tabindex="-1" aria-labelledby="editar-${conteudo.nomeNormalizado}-modal" aria-hidden="true">
        <div class="modal-dialog">
            <div id="editar-root" class="modal-content">
                <div class="modal-header">
                    <h1 class="modal-title fs-5">Editar ${conteudo.titulo}</h1>
                    <button type="button" class="btn-fechar" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <form id="${conteudo.nomeNormalizado}-form" asp-area="Admin" asp-controller="AdminConteudo" asp-action="Editar" data-bs-theme="portfolio-dark" class="row row-cols-1 g-3" autocomplete="off">
                        <div class="text-danger"></div>
                        <input value="${conteudo.id}" type="hidden" />
                        <div class="col">
                            <div class="form-floating">
                                <input placeholder="Título" class="form-control text-white" value="${conteudo.titulo}" />
                                <label class="control-label text-white">Título</label>
                                <span class="text-danger"></span>
                            </div>
                        </div>
                        <div class="col">
                            <div class="form-floating">
                                <input placeholder="Nome" class="form-control text-white" value="${conteudo.nome}" />
                                <span asp-validation-for="Conteudo.Nome" class="text-danger"></span>
                                <label class="control-label text-white">Nome</label>
                            </div>
                        </div>
                        <div class="col">
                            <div class="form-floating">
                                <textarea maxlength="2500" placeholder="Conteúdo" class="form-control text-white">${conteudo.conteudo}</textarea>
                                <label class="control-label text-white">Conteúdo</label>
                                <span class="text-danger"></span>
                            </div>
                        </div>
                        <div class="col">
    
                            <select class="form-select form-select-lg mb-3">
    
                                <option value="2" selected>@item.Nome</option>
                                <option value="1">@item.Nome</option>
    
                            </select>
    
                        </div>
                    </form>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary text-white" data-bs-dismiss="modal">Fechar</button>
                    <button type="submit" form="${conteudo.nomeNormalizado}-form" class="btn btn-success text-white">Salvar Alterações</button>
                </div>
            </div>
        </div>
    </div>`

        return modal;
    }


    const init = () => {
        window.addEventListener('DOMContentLoaded', () => {
            listarConteudos()
        });
    }

    init()
}

adminConteudo()

