import { fetchGet } from './fetchHelper';

const adminConteudo = () => {

    const listarConteudos = async () => {
        var conteudos = await fetchGet('AdminConteudo/Listar');
        var conteudoRoot = document.querySelector('#conteudos');
        conteudoRoot.classList.add('animacao-pagina');

        for (var i of conteudos) {
            console.log(i)
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
                <div class="card-footer border-primary">
                    <a class="btn btn-secondary text-white" data-bs-toggle="modal" data-bs-target="#${i.nomeNormalizado}-modal">Editar</a>
                    <a class="btn btn-danger text-white">Excluir</a>
                </div>
            </div>
        </div>`;

            conteudoRoot.innerHTML += card;
        }
    }





    const init = () => {
        window.addEventListener('DOMContentLoaded', () => {
            listarConteudos()
        });
    }

    init()
}

adminConteudo()

