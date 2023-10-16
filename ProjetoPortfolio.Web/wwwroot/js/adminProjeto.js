import { fetchGet, fetchPost } from "./fetchHelper";
import * as bootstrap from 'bootstrap';

const adminProjeto = () => {
    const mainContent = document.querySelector("#mainContent");
    const projetoRoot = mainContent.querySelector("#projetos-root");
    const btnAdicionarProjeto = mainContent.querySelector('#btn-adicionar-projeto');

    async function projetoInit() {
        listarProjetos();
    }

    async function listarProjetos() {
        btnAdicionarProjeto.onclick = function () {
            adicionarProjeto();
        }
        const response = await fetchGet("AdminProjeto/Listar");

        if (response == null || response == undefined) {
            projetoRoot.innerHTML = `<h1>Nenhum conteúdo encontrado</h1>`
        } else {
            projetoRoot.innerHTML = '';
            const projetos = response.projetos;

            projetos.forEach(projeto => {
                var card = criaCardProjeto(projeto);
                projetoRoot.appendChild(card);
                var btnEditar = card.querySelector(`#btn-editar-${projeto.tituloNormalizado}`);
                var btnExcluir = card.querySelector(`#btn-excluir-${projeto.tituloNormalizado}`);
                btnExcluir.onclick = function () {
                    excluirProjeto(projeto);
                };
                btnEditar.onclick = function () { ativaModalEditar(projeto) };
                console.log(projeto)
            });
        }

    }
    async function adicionarProjeto() {
        var modal = criaModalAdicionar();
        mainContent.appendChild(modal);

        const elementoModal = mainContent.querySelector('#modal-adicionar-projeto');

        const modalAdicionar = new bootstrap.Modal(elementoModal);

        modalAdicionar.show();

        const btnAdicionarProjeto = elementoModal.querySelector('.btn-adicionar');
        const formAdicionar = elementoModal.querySelector('form');

        btnAdicionarProjeto.onclick = async () => {
            const formData = new FormData(formAdicionar);
            const select = formAdicionar.querySelector('select');
            formData.append(select.name, select.value);
            var obj = {}
            formData.forEach((value, key) => {
                obj[key] = value
            });
            console.log(obj);
            var response = await salvarProjeto(formData, 'adicionar');
            if (!response || response == null || response == undefined)
                return;
            modalAdicionar.hide();
            projetoInit();
            location.reload()
        };
        elementoModal.addEventListener('hidden.bs.modal', (e) => {
            modalAdicionar.dispose();
            elementoModal.remove();
        });
    }

    function criaModalAdicionar() {
        const modal = document.createElement('div');
        modal.classList.add('modal', 'fade');
        modal.id = `modal-adicionar-projeto`;
        modal.tabIndex = -1;
        modal.ariaLabel = `modal-adicionar-projetoLabel`;
        modal.ariaHidden = true;
        modal.innerHTML = `<div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <h1 class="modal-title fs-5" id="modal-adicionar-projetoLabel">Adicionar novo projeto</h1>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Fechar"></button>
            </div>
            <div class="modal-body">
                <form class="row row-cols-1 g-3" data-bs-theme="portfolio-dark">

                    <div class="col">
                        <div class="form-floating">
                            <input type="text" class="form-control" id="editar-titulo" name="titulo" value="">
                            <label for="editar-titulo" class="col-form-label text-white">Título</label>
                        </div>
                    </div>

                    <div class="col">
                        <div class="form-floating">
                            <textarea class="form-control" id="editar-descricao" name="descricao"></textarea>
                            <label for="editar-descricao" class="col-form-label text-white">Descrição</label>
                        </div>
                    </div>

                    <div class="col">
                        <div class="form-floating">
                            <select id="editar-status" class="form-select" aria-label="Status do projeto" name="status" required>
                                <option class="px-1 py-2" selected disabled>Selecione um status</option>
                                <option class="px-1 py-2" value="1">No Ar</option>
                                <option class="px-1 py-2" value="2">Desativado</option>
                                <option class="px-1 py-2" value="3">Em manutenção</option>
                                <option class="px-1 py-2" value="4">No GitHub</option>
                            </select>
                            <label for="editar-status" class="col-form-label text-white">Status</label>
                        </div>
                    </div>

                    <div class="col">
                        <div class="form-floating">
                            <input type="text" class="form-control" id="editar-url-imagem" name="urlImagem" value="">
                            <label for="editar-url-imagem" class="col-form-label text-white">Url da imagem</label>
                        </div>
                    </div>

                    <div class="col">
                        <div class="form-floating">
                            <input type="text" class="form-control" id="editar-url-redirecionar" name="urlRedirecionar" value="">
                            <label for="editar-url-redirecionar" class="col-form-label text-white">Url de Redirecionar</label>
                        </div>
                    </div>

                </form>
            </div>
            <div class="modal-footer">
                <button id="btn-dismiss-modal" type="button" class="btn btn-secondary text-white" data-bs-dismiss="modal">Sair</button>
                <button type="button" class="btn btn-adicionar btn-success text-white">Adicionar</button>
            </div>
        </div>
    </div>`;

        return modal;
    }

    async function excluirProjeto(projeto) {
        console.log('foi excluir ' + projeto.id)
        const modal = criaModalExcluir(projeto);
        mainContent.appendChild(modal);
        const elementoModal = mainContent.querySelector(`#modal-excluir-${projeto.tituloNormalizado}`);
        const btnModalExcluir = elementoModal.querySelector('.btn-excluir');
        const modalExcluir = new bootstrap.Modal(elementoModal);
        modalExcluir.show();

        elementoModal.addEventListener('hidden.bs.modal', (e) => {
            modalExcluir.dispose();
            elementoModal.remove();
        });

        btnModalExcluir.onclick = async function () {
            modalExcluir.hide();
            var response = await fetchPost('AdminProjeto/Excluir', projeto);
            if (response) {
                projetoInit();
                location.reload();
            } else {
                projetoInit();
                location.reload();
            }
        };
    }

    function criaModalExcluir(projeto) {
        const modal = document.createElement('div');
        modal.classList.add('modal', 'fade');
        modal.id = `modal-excluir-${projeto.tituloNormalizado}`;
        modal.tabIndex = -1;
        modal.ariaLabel = `modal-excluir-${projeto.tituloNormalizado}Label`;
        modal.ariaHidden = true;
        modal.innerHTML = `<div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <h1 class="modal-title fs-5" id="modal-excluir-${projeto.tituloNormalizado}Label">${projeto.titulo}</h1>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body">
                Tem certeza que quer excluir o projeto "${projeto.titulo}"?            
            </div>
            <div class="modal-footer">
                <button id="btn-dismiss-modal" type="button" class="btn btn-primary text-white" data-bs-dismiss="modal">Sair</button>
                <button type="button" class="btn btn-excluir btn-danger text-white">Excluir</button>
            </div>
        </div>
        </div>`;
        return modal;
    }
    async function ativaModalEditar(projeto) {
        var modal = criaModalEditar(projeto);
        mainContent.appendChild(modal);


        var elementoModal = mainContent.querySelector(`#modal-editar-${projeto.tituloNormalizado}`)
        const modalEditar = new bootstrap.Modal(elementoModal);
        modalEditar.show()

        var btnSalvar = elementoModal.querySelector('.btn-salvar');
        var formEditar = elementoModal.querySelector('form');
        btnSalvar.onclick = () => {
            const formData = new FormData(formEditar);
            const select = formEditar.querySelector('select');
            formData.append('id', projeto.id);
            formData.append(select.name, select.value);
            formData.append('excluido', false);
            formData.append('tituloNormalizado', "");
            var salvado = salvarProjeto(formData, 'salvar');
            if (salvado) {
                console.log(salvado)
                modalEditar.hide();
                //projetoInit();
                //location.reload();
            }
        }

        elementoModal.addEventListener('hidden.bs.modal', (event) => {
            modalEditar.dispose()
            elementoModal.remove()
        });
    }

    async function salvarProjeto(data, tipo) {
        const obj = {}

        data.forEach((value, key) => {
            obj[key] = value;
        });
        var status = parseInt(obj.status);
        var excluido = eval(obj.excluido);
        if (typeof excluido == typeof true) {
            obj['excluido'] = excluido;
        } else {
            return false
        }
        if (typeof status == typeof 0) {
            obj['status'] = status;

        } else {
            return false;
        }
        console.log(obj)
        switch (tipo) {
            case 'salvar':
                var response = await fetchPost('AdminProjeto/Editar', obj);
                if (response) {
                    return true;
                }

                return false;

            case 'adicionar':
                obj['id'] = '00000000-0000-0000-0000-000000000000';
                obj['tituloNormalizado'] = '';
                console.log(obj);
                var response = await fetchPost('AdminProjeto/Adicionar', obj);
                if (response) {
                    return true;
                }
                return false
            case 'excluir':
                var response = await fetchPost('AdminProjeto/Excluir', obj)
                if (response) {
                    return true;
                }
                return false;

            default: return false;
        }
    }

    function criaModalEditar(projeto) {
        const modal = document.createElement('div');
        modal.classList.add('modal', 'fade');
        modal.id = `modal-editar-${projeto.tituloNormalizado}`;
        modal.tabIndex = -1;
        modal.ariaLabel = `modal-editar-${projeto.tituloNormalizado}Label`;
        modal.ariaHidden = true;
        modal.innerHTML = `<div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h1 class="modal-title fs-5" id="modal-editar-${projeto.tituloNormalizado}Label">${projeto.titulo}</h1>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <form class="row row-cols-1 g-3" data-bs-theme="portfolio-dark">

                        <div class="col">
                            <div class="form-floating">
                                <input type="text" class="form-control" id="editar-titulo" name="titulo" value="${projeto.titulo}">
                                <label for="editar-titulo" class="col-form-label text-white">Título</label>
                            </div>
                        </div>

                        <div class="col">
                            <div class="form-floating">
                                <textarea class="form-control" id="editar-descricao" name="descricao">${projeto.descricao}</textarea>
                                <label for="editar-descricao" class="col-form-label text-white">Descrição</label>
                            </div>
                        </div>

                        <div class="col">
                            <div class="form-floating">
                                <select id="editar-status" class="form-select" aria-label="Status do projeto" name="status" required>
                                    <option class="px-1 py-2" selected disabled>Selecione um status</option>
                                    <option class="px-1 py-2" value="1">No Ar</option>
                                    <option class="px-1 py-2" value="2">Desativado</option>
                                    <option class="px-1 py-2" value="3">Em manutenção</option>
                                    <option class="px-1 py-2" value="4">No GitHub</option>
                                </select>
                                <label for="editar-status" class="col-form-label text-white">Status</label>
                            </div>
                        </div>

                        <div class="col">
                            <div class="form-floating">
                                <input type="text" class="form-control" id="editar-url-imagem" name="urlImagem" value="${projeto.urlImagem}">
                                <label for="editar-url-imagem" class="col-form-label text-white">Url da imagem</label>
                            </div>
                        </div>

                        <div class="col">
                            <div class="form-floating">
                                <input type="text" class="form-control" id="editar-url-redirecionar" name="urlRedirecionar" value="${projeto.urlRedirecionar}">
                                <label for="editar-url-redirecionar" class="col-form-label text-white">Url de Redirecionar</label>
                            </div>
                        </div>

                    </form>
                </div>
                <div class="modal-footer">
                    <button id="btn-dismiss-modal" type="button" class="btn btn-secondary text-white" data-bs-dismiss="modal">Sair</button>
                    <button type="button" class="btn btn-salvar btn-success text-white">Salvar alterações</button>
                </div>
            </div>
        </div>`;

        var select = modal.querySelector('select');
        var options = Array.from(select.options);
        var optionSelect = options.find(item => parseInt(item.value) == parseInt(projeto.status));
        optionSelect.selected = true;
        return modal;
    }

    function criaCardProjeto(projeto) {
        const card = document.createElement('div');
        card.classList.add('card', 'mb-3', 'border-primary')
        var status = '';
        switch (parseInt(projeto.status)) {
            case 1:
                status = "No ar";
                break;
            case 2:
                status = "Desativado";
                break;
            case 3:
                status = "Em manutenção";
                break;
            case 4:
                status = "No Github";
                break;
            default: status = ''
        }
        var cardContent = '';
        card.setAttribute('data-bs-theme', "portfolio-dark")
        cardContent = `
        <div class="row g-0">
        <div class="col-md-4">
          <img src="${projeto.urlImagem}" class="img-fluid rounded-start" alt="...">
        </div>
        <div class="col-md-8">
          <div class="card-body border-primary">
            <h5 class="card-title text-white">${projeto.titulo}</h5>
            <p class="card-text text-white">${projeto.descricao}</p>
            <p class="card-text text-white">Url de Redirecionar: <i>${projeto.urlRedirecionar}</i></p>
            <p class="card-text text-white">Status: <i>${status}</i></p>
          </div>
          <div class="card-footer d-flex justify-content-end border-primary">
            <a id="btn-editar-${projeto.tituloNormalizado}" class="btn btn-secondary text-white mx-2">Editar</a>
            <a id="btn-excluir-${projeto.tituloNormalizado}"class="btn btn-danger text-white">Excluir</a>
          </div>
        </div>
      </div>
        `;

        card.innerHTML = cardContent;

        const divCol = document.createElement('div');
        divCol.classList.add('col');
        divCol.appendChild(card);
        return divCol;
    }

    function init() {
        projetoInit()
    };
    init();
};

adminProjeto();