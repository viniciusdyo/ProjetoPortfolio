import { fetchGet, fetchPost } from "./fetchHelper";

const projetos = () => {
    const mainContent = document.querySelector('#mainContent');
    const projetosRoot = mainContent.querySelector('#projetosRoot');

    function init() {
        projetosInit();
    }

    async function projetosInit() {
        const response = await fetchGet('Projeto/Listar');
        console.log(response)
        if (response.results != null) {
            const projetos = response.results;
            projetos.forEach(projeto => {
                var card = criaCardProjeto(projeto);
                projetosRoot.appendChild(card)
            });
        } else {
            var div = document.createElement('div');
            var divNaoEncontrado = document.createElement('div');
            var h1 = document.createElement('h1');
            var p = document.createElement('p');
            var icone = document.createElement('i');
            var divText = document.createElement('div');

            icone.classList.add('text-primary', 'display-1', 'bi', 'bi-cone-striped', 'me-3',);

            div.classList.add('text-white', 'py-5', 'px-3', 'w-100', 'bg-principal-escura', 'rounded-2')
            divNaoEncontrado.classList.add('d-flex');
            h1.classList.add('px-2', 'mb-3');
            p.classList.add('px-2', 'm-0');
            divText.classList.add('d-flex', 'flex-column');
            p.innerHTML = `Os projetos estão em construção, logo serão adicionados aqui. Acesse a página <a href="/Home/Privacy">Sobre Mim</a> para mais informações.`;
            h1.innerText = `Área em contrução`;
            divText.appendChild(h1)
            divText.appendChild(p)
            divNaoEncontrado.appendChild(icone)
            divNaoEncontrado.appendChild(divText)
            div.appendChild(divNaoEncontrado);
            projetosRoot.appendChild(div);
        }
    }

    function criaCardProjeto(projeto) {
        var div = document.createElement('div');
        div.classList.add('col');
        var status = ''
        var statusCor = ''
        switch (parseInt(projeto.status)) {
            case 1:
                status = 'No ar';
                statusCor = 'text-success'
                break;
            case 2:
                status = 'Fora do Ar';
                statusCor = 'text-danger'
                break;
            case 3:
                statusCor = 'text-warning'
                status = 'Em manutenção';
                break;
            case 4:
                statusCor = 'text-white'
                status = 'No Github';
                break;
            default:
                status = 'Indefinido';
        }
        var card = `<div class="card mb-3 border-primary" data-bs-theme="portfolio-dark">
                        <div class="row g-0 ">
                            <div class="col-md-5">
                                <img src="${projeto.urlImagem}" class="img-fluid rounded-start" alt="...">
                            </div>
                            <div class="col-md-7 d-flex flex-column">
                                <div class="card-body border-primary d-flex flex-column">
                                    <h5 class="card-title text-white">${projeto.titulo}</h5>
                                    <p class="card-text text-white">${projeto.descricao}</p>
                                    <p class="card-text ${statusCor} align-self-end mt-auto"><i>${status}</i></p>
                                </div>
                                <div class="card-footer d-flex justify-content-end align-self-end  w-100 border-primary">
                                    <a id="btn-detalhes-${projeto.tituloNormalizado}" class="btn btn-success text-white mx-2">Detalhes</a>
                                    <a href="${projeto.urlRedirecionar}" id="btn-acessar-${projeto.tituloNormalizado}" class="btn btn-primary text-white mx-2">Acessar</a>
                                </div>
                            </div>
                        </div>
                    </div>`

        div.innerHTML = card;
        return div;
    }

    init();
}

projetos();