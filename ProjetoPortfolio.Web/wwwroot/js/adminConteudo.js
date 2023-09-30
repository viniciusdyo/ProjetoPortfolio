import { fetchGet, fetchPost } from './fetchHelper';


const adminConteudo = function () {

    const conteudosRoot = document.querySelector('#conteudos-root');
    const mainContent = document.querySelector('#mainContent');

    async function conteudosInit() {
        conteudosRoot.innerHTML = ''
        const data = await fetchGet('AdminConteudo/ListarConteudoAdmin');

        if (data != null || data != undefined || data.length > 0) {

            const conteudos = data.conteudos;
            const categorias = data.categorias;
            listarConteudos(conteudos, categorias);
            console.log(data)
        } else {
            console.log('nao encotnrado')
            nenhumEncontrado();
        }
    }

    function nenhumEncontrado() {
        const titulo = document.createElement('h2');
        titulo.textContent = "Nenhum conteúdo encontrado!"
        titulo.classList.add('text-white');
        conteudosRoot.classList.remove('col-md-4');
        conteudosRoot.appendChild(titulo);
    }

    function listarConteudos(conteudos, categorias) {
        if (conteudos.length > 0) {
            conteudos.forEach(conteudo => {
                const card = criaCardLista(conteudo);
                const modalEditar = criaModalEditar(conteudo, categorias);
                const modalExcluir = criaModalExcluirConteudo(conteudo);
                mainContent.appendChild(modalExcluir);
                mainContent.appendChild(modalEditar);
                conteudosRoot.appendChild(card);

                const btnSalvarAlteracoes = modalEditar.querySelector('.btn-salvar');
                btnSalvarAlteracoes.addEventListener('click', function () {
                    salvarConteudo(modalEditar, 'editar');
                });

                const btnExcluir = card.querySelector('.btn-excluir-conteudo');
                btnExcluir.addEventListener('click', function () {
                    criaModalExcluirConteudo(conteudo);
                });

            });
        } else {
            nenhumEncontrado()
        }
        const modalAdicionarConteudo = criaModalAdicionarConteudo(categorias);
        const btnAdicionarConteudo = modalAdicionarConteudo.querySelector('#btn-adicionar-conteudo');
        btnAdicionarConteudo.addEventListener('click', function () {
            salvarConteudo(modalAdicionarConteudo, 'adicionar');
        });
    }

    function criaModalExcluirConteudo(conteudo) {
        var modalContent = `
        <div class="modal-dialog">
          <div class="modal-content">
            <div class="modal-header">
              <h1 class="modal-title fs-5" id="modal-excluir-${conteudo.nomeNormalizado}-label">Excluir ${conteudo.nome}</h1>
              <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body">
                <p  class="text-white">
                    Deseja mesmo excluir este conteúdo?
                </p>
            </div>
            <div class="modal-footer">
              <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Fechar</button>
              <a id="btn-excluir-${conteudo.nomeNormalizado}" class="btn btn-danger text-white btn-remover-conteudo">Excluir</a>
            </div>
          </div>
        </div>
      `;

        var modal = document.createElement('div');
        modal.id = `modal-excluir-${conteudo.nomeNormalizado}`;
        modal.tabIndex = -1;
        modal.ariaLabel = `#modal-excluir-${conteudo.nomeNormalizado}-label`;
        modal.ariaHidden = true;
        modal.classList.add('modal-remover-conteudo', 'modal', 'fade');
        modal.innerHTML = modalContent;
        const btnExcluir = modal.querySelector(`#btn-excluir-${conteudo.nomeNormalizado}`);
        btnExcluir.addEventListener('click', function () {
            excluirConteudo(conteudo.id)
        });

        return modal;
    }

    function criaModalAdicionarConteudo(categorias) {
        const modal = mainContent.querySelector('#modal-adicionar-conteudo');
        const btnAdicionarAtivo = modal.querySelector('#btn-adicionar-conteudo-ativos');
        const divAtivos = modal.querySelector('#ativos-adicionar-conteudo');

        criaSelectCategorias(modal, categorias, null);

        btnAdicionarAtivo.addEventListener('click', function (e) {
            e.preventDefault();
            divAtivos.appendChild(adicionarAtivo(null));
        }, true);

        return modal;
    }


    function criaCardLista(conteudo) {
        const cardHtml = `
            <div class="card bg-dark-darker text-white border-primary">
                <h5 class="card-header border-primary text-primary"> ${conteudo.titulo}</h5>
                <div class="card-body">
                    <h5 class="card-title"><b>Nome:</b> ${conteudo.nome}</h5>
                    <p class="card-text"><b>Conteúdo:</b> ${conteudo.conteudo}</p>
                    <div class="d-flex align-content-center">
                        <p class="card-text me-2">
                            <b>Categoria:</b>
                        </p> 
                        <a class="text-decoration-none dropdown-toggle" data-bs-toggle="dropdown" aria-expanded="false">
                            ${conteudo.categoriaConteudoModel.nome}
                        </a>
                        <ul class="dropdown-menu dropdown-menu-dark py-3">
                            <li>
                                <p class="px-2 py-0 my-0 text-secondary">Descrição</p>
                                <p class="px-2 py-0 my-0 text-white">${conteudo.categoriaConteudoModel.descricao}</p>
                            </li>
                        </ul>                       
                    </div>                    
                </div>
                <div class="card-body ativos-div border-top border-primary">
                    <div id="${conteudo.nomeNormalizado}-ativos">
                        <h4>Ativos</h4>
                    </div>
                </div>
                <div id="card-footer-${conteudo.nomeNormalizado}" class="card-footer border-primary">
                    <a id="btn-editar-${conteudo.nomeNormalizado}" data-bs-toggle="modal" data-bs-target="#modal-editar-${conteudo.nomeNormalizado}" class="btn btn-primary text-white">Editar</a>
                    <a class="btn btn-danger text-white btn-excluir-conteudo" data-bs-toggle="modal" data-bs-target="#modal-excluir-${conteudo.nomeNormalizado}">Excluir</a>
                </div>
            </div>`;

        var card = document.createElement('div');
        card.classList.add('col');
        card.innerHTML = cardHtml;


        var ativos = conteudo.ativosConteudo;
        var ativosDiv = card.querySelector('.ativos-div');
        if (ativos.length == 0 || ativos == null) {
            ativosDiv.remove();
        } else {
            ativos.forEach(ativo => {

                var ativoDescricao = criaAtivosCardDescricao(ativo.nomeAtivo, ativo.tipoAtivo);
                ativosDiv.appendChild(ativoDescricao);
            });
        }

        return card;
    }

    function criaAtivosCardDescricao(nomeAtivo, tipoAtivo) {
        var div = document.createElement('div');
        var nome = document.createElement('p');
        var tipo = document.createElement('p');

        div.classList.add('d-flex', 'text-white', 'justify-content-between');
        nome.classList.add('fw-bold');
        nome.innerText = nomeAtivo;

        switch (tipoAtivo) {
            case 1:
                tipo.innerText = 'Link';
                break;
            case 1:
                tipo.innerText = 'Imagem';
                break;
            default:
                tipo.innerText = 'Não informado';
        }

        div.appendChild(nome);
        div.appendChild(tipo);

        return div;
    }

    function criaModalEditar(conteudo, categorias) {

        var modalContent = `<div class="modal-dialog">
            <div id="editar-root" class="modal-content">
                <div class="modal-header">
                    <h1 id="modal-editar-${conteudo.nomeNormalizado}-label" class="modal-title fs-5">Editar ${conteudo.titulo}</h1>
                    <button type="button" class="btn-fechar" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <form id="editar-conteudo-${conteudo.nomeNormalizado}-form" data-bs-theme="portfolio-dark" class="row row-cols-1 g-3 form-editar-conteudo " autocomplete="off">
                        <div class="text-danger"></div>
                        <input name="id" value="${conteudo.id}" type="hidden" />
                        <div class="col">
                            <div class="form-floating">
                                <input type="text"  placeholder="Título" class="form-control text-white" name="titulo" value="${conteudo.titulo}" />
                                <label class="control-label text-white">Título</label>
                                <span class="text-danger"></span>
                            </div>
                        </div>
                        <div class="col">
                            <div class="form-floating">
                                <input type="text" placeholder="Nome" class="form-control text-white" name="nome" value="${conteudo.nome}" />
                                <span class="text-danger"></span>
                                <label class="control-label text-white">Nome</label>
                            </div>
                        </div>
                        <div class="col">
                            <div class="form-floating">
                                <textarea type="text" maxlength="2500" placeholder="Conteúdo" class="form-control text-white" name="conteudo">${conteudo.conteudo}</textarea>
                                <label class="control-label text-white">Conteúdo</label>
                                <span class="text-danger"></span>
                            </div>
                        </div>
                        <div class="col">
                            <select id="${conteudo.nomeNormalizado}-select" class="form-select mb-3" name="categoriaId">
                                
                            </select>
                        </div>

                        <div class="col">
                            <p class="text-white fw-bold">
                                Ativos
                            </p>
                            <a id="${conteudo.nomeNormalizado}-btn-add-ativo" class="btn btn-success btn-adicionar-ativo text-white">Adicionar ativo</a>
                        </div>

                        <div id="${conteudo.nomeNormalizado}-inputs-ativos" class="ativo-div-inputs" name="ativosConteudo">
                        </div>
                    </form>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary text-white" data-bs-dismiss="modal">Fechar</button>
                    <a class="btn btn-success text-white btn-salvar">Salvar Alterações</a>
                </div>
            </div>
        </div>`;

        var modal = document.createElement('div');

        modal.id = `modal-editar-${conteudo.nomeNormalizado}`;
        modal.tabIndex = -1;
        modal.ariaLabel = `#modal-editar-${conteudo.nomeNormalizado}-label`;
        modal.ariaHidden = true;
        modal.classList.add('modal-editar', 'modal', 'fade');
        modal.innerHTML = modalContent;

        criaSelectCategorias(modal, categorias, conteudo.categoriaId);

        var btnAdicionarAtivo = modal.querySelector(`#${conteudo.nomeNormalizado}-btn-add-ativo`);
        var ativos = conteudo.ativosConteudo;
        var ativosDiv = modal.querySelector(`#${conteudo.nomeNormalizado}-inputs-ativos`);

        if (ativos.length > 0 && ativos != null && ativos != undefined) {
            ativos.forEach(ativo => {
                ativosDiv.appendChild(adicionarAtivo(ativo));
            });
        }

        btnAdicionarAtivo.addEventListener('click', function () {
            ativosDiv.appendChild(adicionarAtivo(null));
        });

        return modal;
    }

    function criaSelectCategorias(modal, categorias, categoriaId) {
        var selectCategorias = modal.querySelector('select');

        if (categorias.length > 0 && categorias != null && categorias != undefined) {
            categorias.forEach(categoria => {
                var option = document.createElement('option');
                option.value = categoria.categoriaId;
                option.innerText = categoria.nome;
                if (categoria.categoriaId == categoriaId && categoriaId != null) {
                    option.selected = true;
                }
                selectCategorias.appendChild(option);
            });
        }

        return;
    }

    function adicionarAtivo(ativoData) {
        var ativo;
        var ativoHtml = ``;

        if (ativoData == null) {
            ativo = document.createElement('div');
            ativoHtml = `<a class="btn btn-danger btn-remover-ativo text-white my-3">Remover</a>
            <input type="hidden" class="form-control text-white" name="ativoId" value="00000000-0000-0000-0000-000000000000" />
            <div class="col-12 mb-3">
                <div class="form-floating">
                    <input type="text"  placeholder="Nome Ativo" class="form-control text-white" name="nomeAtivo" value="" />
                    <label class="control-label text-white">Nome Ativo</label>
                    <span class="text-danger"></span>
                </div>
            </div>
            <div class="col-12 mb-3">
                <div class="form-floating">
                    <input type="text"  placeholder="Descrição" class="form-control text-white" name="descricao" value="" />
                    <label class="control-label text-white">Descrição</label>
                    <span class="text-danger"></span>
                </div>
            </div>
            <div class="col-12 mb-3">
                <div class="form-floating">
                    <input type="text"  placeholder="Valor" class="form-control text-white" name="valor" value="" />
                    <label class="control-label text-white">Valor</label>
                    <span class="text-danger"></span>
                </div>
            </div>
            <div class="col-12 mb-3">
                <select class="form-select mb-3"  name="tipoAtivo">
                    <option value="1">Link</option>
                    <option value="2">Imagem</option>
                </select>
            </div>`;
            ativo.classList.add('form-ativo', 'col-12');
            ativo.innerHTML = ativoHtml;
        } else {
            ativo = document.createElement('div');
            ativoHtml = `<a class="btn btn-danger btn-remover-ativo text-white my-3">Remover</a>
            <input type="hidden" class="form-control text-white" name="ativoId" value="${ativoData.ativoId}" />
            <div class="col-12 mb-3">
                <div class="form-floating">
                    <input type="text"  placeholder="Nome Ativo" class="form-control text-white" name="nomeAtivo" value="${ativoData.nomeAtivo}" />
                    <label class="control-label text-white">Nome Ativo</label>
                    <span class="text-danger"></span>
                </div>
            </div>
            <div class="col-12 mb-3">
                <div class="form-floating">
                    <input type="text"  placeholder="Descrição" class="form-control text-white" name="descricao" value="${ativoData.descricao}" />
                    <label class="control-label text-white">Descrição</label>
                    <span class="text-danger"></span>
                </div>
            </div>
            <div class="col-12 mb-3">
                <div class="form-floating">
                    <input type="text"  placeholder="Valor" class="form-control text-white" name="valor" value="${ativoData.valor}" />
                    <label class="control-label text-white">Valor</label>
                    <span class="text-danger"></span>
                </div>
            </div>
            <div class="col-12 mb-3">
                <select class="form-select mb-3"  name="tipoAtivo">
                    <option value="1">Link</option>
                    <option value="2">Imagem</option>
                </select>
            </div>`;
            ativo.classList.add('form-ativo', 'col-12');
            ativo.innerHTML = ativoHtml;

            var select = ativo.querySelector('select');
            var options = select.querySelectorAll('option');

            options.forEach(opt => {
                if (parseInt(opt.value) == parseInt(ativoData.tipoAtivo)) {
                    opt.selected = true;
                }
            });
        }

        removerAtivo(ativo);
        return ativo;
    }

    function removerAtivo(ativo) {
        var btnRemoverAtivo = ativo.querySelector('.btn-remover-ativo');

        if (btnRemoverAtivo != undefined && btnRemoverAtivo != null) {
            btnRemoverAtivo.addEventListener('click', function () {
                ativo.remove()
            });
        }
    }

    async function salvarConteudo(modal, tipo) {
        const form = modal.querySelector('form');
        const ativos = form.querySelectorAll('.form-ativo');
        const arrAtivos = [];
        const conteudoObj = {}
        let count = 0;

        if (ativos.length > 0 && ativos != null && ativos != undefined) {
            ativos.forEach(ativo => {
                const objAtivo = {}
                var inputs = ativo.querySelectorAll('input');

                if (inputs.length > 0 && inputs != null && inputs != undefined) {
                    inputs.forEach(input => {
                        objAtivo[input.name] = input.value;
                    });
                }

                var selectAtivo = ativo.querySelector('select');

                if (selectAtivo != null && selectAtivo != undefined) {
                    objAtivo[selectAtivo.name] = parseInt(selectAtivo.value)
                }

                arrAtivos[count] = objAtivo;
                count += 1;
            })
        }

        const formData = new FormData(form);

        if (formData != null && formData != undefined) {
            formData.forEach((value, key) => {
                conteudoObj[key] = value
            });
        } else {
            alert('Campos vazios');
            return;
        }

        if (arrAtivos.length > 0) {
            conteudoObj['ativosConteudo'] = arrAtivos;
        }

        delete conteudoObj.ativoId;
        delete conteudoObj.descricao;
        delete conteudoObj.valor;
        delete conteudoObj.nomeAtivo;
        delete conteudoObj.tipoAtivo;

        switch (tipo) {
            case 'editar':
                fetchPost('AdminConteudo/Editar', conteudoObj);
                break;
            case 'adicionar':
                fetchPost('AdminConteudo/Adicionar', conteudoObj);
                break;
            default: alert('Erro ao salvar conteúdo');
        }
        await conteudosInit();
        location.reload();
        return;
    }

    function excluirConteudo(id) {
        if (id != undefined && id != null) {
            fetchPost('AdminConteudo/Excluir', id);
            location.reload()
            return;
        } else {
            alert('Erro ao excluir');
            return;
        }
    }

    function init() {
        conteudosInit()
    }

    init();
}

adminConteudo();