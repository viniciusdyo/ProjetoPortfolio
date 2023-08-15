import { fetchGet } from './fetchHelper';

const adminConteudo = () => {

    const listarConteudos = async () => {
        const conteudoViewModel = await fetchGet('AdminConteudo/ListarConteudoAdmin');
        const conteudos = conteudoViewModel.conteudos;
        const categorias = conteudoViewModel.categorias;
        const conteudoRoot = document.querySelector('#conteudos');
        const main = document.querySelector('main');
        console.log(conteudos)

        conteudos.forEach(i => {
            const card = `<div class="col-4">
            <div class="card bg-dark-darker text-white border-primary">
                <h5 class="card-header border-primary text-primary"> ${i.titulo}</h5>
                <div class="card-body">
                    <h5 class="card-title"><b>Nome:</b> ${i.nome}</h5>
                    <p class="card-text"><b>Conteúdo:</b> ${i.conteudo}</p>
                    <div class="d-flex align-content-center">
                        <p class="card-text me-2">
                            <b>Categoria:</b>
                        </p> 
                        <a class="text-decoration-none dropdown-toggle" data-bs-toggle="dropdown" aria-expanded="false">
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
                <div class="card-body border-top border-primary">
                    <div id="${i.nomeNormalizado}-ativos">
                        <h4>Ativos</h4>

                    </div>
                </div>
                <div id="card-footer-${i.nomeNormalizado}" class="card-footer border-primary">
                    <a id="btn-editar-${i.nomeNormalizado}" data-bs-toggle="modal" data-bs-target="#editar-${i.nomeNormalizado}-modal" class="btn btn-primary text-white">Editar</a>
                   
                </div>
            </div>
        </div>`;

            conteudoRoot.innerHTML += card;
            i.ativosConteudo.forEach(e => {
                var ativoNome = document.createElement('p');
                ativoNome.innerHTML = `<b>Nome:</b> ${e.nome}`
                var ativoTipo = document.createElement('p');
                ativoTipo.innerHTML = `<b>Tipo:</b> ${e.tipoAtivo}`
                var ativoDescricao = document.createElement('p');
                ativoDescricao.innerHTML = `<b>Descricao:</b> ${e.descricao}`
                var ativosDiv = conteudoRoot.querySelector(`#${i.nomeNormalizado}-ativos`);
                ativosDiv.appendChild(ativoNome);
                ativosDiv.appendChild(ativoDescricao);
                ativosDiv.appendChild(ativoTipo);
            })

        });

        conteudos.forEach(c => {
            var modal = criarModalEditar(c);
            main.innerHTML += modal;
            var modalForm = document.querySelector(`#${c.nomeNormalizado}-form`);
            var formSelect = document.querySelector(`#${c.nomeNormalizado}-select`);
            var divInputAtivos = modalForm.querySelector(`#${c.nomeNormalizado}-inputs-ativos`);


            categorias.forEach(o => {
                var optSelect = document.createElement('option');

                optSelect.value = `${o.categoriaId}`;
                optSelect.innerText = `${o.nome}`
                if (c.categoriaId == o.categoriaId) {
                    optSelect.selected = true;
                }
                formSelect.appendChild(optSelect)
            });

            c.ativosConteudo.forEach(e => {
                var ativos = `
                <div id="${e.nome}-div" class="form-ativo col-12">
                        <a class="btn btn-danger btn-remover-ativo text-white my-3">Remover</a>
                        <div class="col-12 mb-3">
                            <div class="form-floating">
                                <input type="text"  placeholder="Nome Ativo" class="form-control text-white" name="nome" value="${e.nome}" />
                                <label class="control-label text-white">Nome Ativo</label>
                                <span class="text-danger"></span>
                            </div>
                        </div>
                        <div class="col-12 mb-3">
                            <div class="form-floating">
                                <input type="text"  placeholder="Descrição" class="form-control text-white" name="descricao" value="${e.descricao}" />
                                <label class="control-label text-white">Descrição</label>
                                <span class="text-danger"></span>
                            </div>
                        </div>
                        <div class="col-12 mb-3">
                            <div class="form-floating">
                                <input type="text"  placeholder="Valor" class="form-control text-white" name="nome" value="${e.valor}" />
                                <label class="control-label text-white">Valor</label>
                                <span class="text-danger"></span>
                            </div>
                        </div>
                        <div class="col-12 mb-3">
                            <select class="form-select mb-3" name="tipoAtivo">
                                <option value="1">Link</option>
                                <option value="2">Imagem</option>
                            </select>
                        </div>
                    </div>
                    `;


                divInputAtivos.innerHTML += ativos;

            })
            c.ativosConteudo.forEach(a => {
                adicionarAtivo()
                removerAtivo()
            })


            modalForm.addEventListener('submit', e => {
                e.preventDefault()
                editarSubmit(modalForm)
            });
        });
    }

    const removerAtivo = () => {
        var btnRemover = document.querySelectorAll('.btn-remover-ativo');
        btnRemover.forEach(b => {
            b.addEventListener('click', e => {
                console.log(b.closest('.form-ativo').remove())
            })
        })
    }

    const adicionarAtivo = () => {
        var btnAdicionar = document.querySelectorAll('.btn-adicionar-ativo');

        btnAdicionar.forEach(b => {
            var ativo = `
                <div class="form-ativo col-12">
                        <a class="btn btn-danger btn-remover-ativo text-white my-3">Remover</a>
                        <div class="col-12 mb-3">
                            <div class="form-floating">
                                <input type="text"  placeholder="Nome Ativo" class="form-control text-white" name="nome" />
                                <label class="control-label text-white">Nome Ativo</label>
                                <span class="text-danger"></span>
                            </div>
                        </div>
                        <div class="col-12 mb-3">
                            <div class="form-floating">
                                <input type="text"  placeholder="Descrição" class="form-control text-white" name="descricao" />
                                <label class="control-label text-white">Descrição</label>
                                <span class="text-danger"></span>
                            </div>
                        </div>
                        <div class="col-12 mb-3">
                            <div class="form-floating">
                                <input type="text"  placeholder="Valor" class="form-control text-white" name="nome" />
                                <label class="control-label text-white">Valor</label>
                                <span class="text-danger"></span>
                            </div>
                        </div>
                        <div class="col-12 mb-3">
                            <select class="form-select mb-3" name="tipoAtivo">
                                <option value="1">Link</option>
                                <option value="2">Imagem</option>
                            </select>
                        </div>
                    </div>
                    `;
            var btnParent = b.parentElement;
            var formBtn = btnParent.parentElement;
            var divAtivos = formBtn.querySelector('.ativo-div-inputs')
            b.addEventListener('click', e => {
                e.preventDefault()
                divAtivos.innerHTML += ativo;
                removerAtivo()
            })

        });
    }

    const editarSubmit = (form) => {
        var formData = new FormData(form);
        console.log(formData)
        for (const [k, v] of formData) {
            console.log(k, v)
        }
    }


    const criarModalEditar = (conteudo) => {
        var modal = `<div class="modal-editar modal fade" id="editar-${conteudo.nomeNormalizado}-modal" tabindex="-1" aria-labelledby="editar-${conteudo.nomeNormalizado}-modal" aria-hidden="true">
        <div class="modal-dialog">
            <div id="editar-root" class="modal-content">
                <div class="modal-header">
                    <h1 class="modal-title fs-5">Editar ${conteudo.titulo}</h1>
                    <button type="button" class="btn-fechar" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <form id="${conteudo.nomeNormalizado}-form" data-bs-theme="portfolio-dark" class="row row-cols-1 g-3" autocomplete="off">
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
                            <select id="${conteudo.nomeNormalizado}-select" class="form-select mb-3" name="categoria">
                                
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
                    <button type="submit" form="${conteudo.nomeNormalizado}-form" class="btn btn-success text-white">Salvar Alterações</button>
                </div>
            </div>
        </div>
    </div>`


        return modal;
    }

    const init = () => {
        window.addEventListener('DOMContentLoaded', (e) => {
            e.preventDefault()
            listarConteudos()
        });
    }

    init()
}

adminConteudo()

