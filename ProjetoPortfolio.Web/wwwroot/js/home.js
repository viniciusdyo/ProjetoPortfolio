import Splide from '@splidejs/splide';
import { fetchGet, fetchPost } from './fetchHelper';

const home = function () {
    const mainContent = document.querySelector('#mainContent');


    function splideCarousel() {
        var bvCarousel = new Splide('#home-carousel-bemvindo', {
            type: 'loop',
            autoplay: true,
            pagination: false,
            arrows: false,
            fixedWidth: '100vw',
            height: '100%',
            gap: '1em',
            interval: 3000
        });

        var progressBar = bvCarousel.root.querySelector('.bv-carousel-progress-bar');

        bvCarousel.on('mounted', () => {
            progressBar.style.height = String(100 / bvCarousel.length) + '%';
        })



        bvCarousel.on('mounted move', function () {
            var end = bvCarousel.Components.Controller.getEnd() + 1;
            var rate = Math.min((bvCarousel.index + 1) / end, 1) - (1 / bvCarousel.length);
            progressBar.style.top = String(100 * (rate)) + '%';
        });

        bvCarousel.mount();
    };

    async function homeInit() {
        const conteudosHome = await fetchGet('Conteudo/ListaConteudoHome');
        const projetosHome = await fetchGet('Home/ListarProjetos');
        const rootProjetos = document.querySelector("#projetos-home");


        if (projetosHome != null && projetosHome != undefined && typeof projetosHome != "string") {
            if (projetosHome.errors.length < 1) {
                if (projetosHome.results.length > 0) {
                    projetos(rootProjetos, projetosHome.results);
                } else {
                    rootProjetos.innerHTML = '';
                    var div = divMensagem('Em desenvolvimento...')
                    rootProjetos.appendChild(div);
                }
            }
        } else {
            rootProjetos.innerHTML = '';
            var div = divMensagem('Em manutenção...')
            rootProjetos.appendChild(div);
        }

        if (conteudosHome != null && conteudosHome != undefined && typeof conteudosHome != "string") {
            conteudosHome.forEach(conteudo => {
                var conteudoNome = conteudo.nome.toLowerCase();

                switch (conteudoNome) {
                    case 'slide':
                        populaCarousel(conteudo);
                        break;
                    default:
                        return;
                }
            });

            splideCarousel();
        } else {
            console.log('erro no servidor');
        }

        sombraEfeito()
    }

    function divMensagem(mensagem) {
        var div = document.createElement('div');
        var h1 = document.createElement('h1');
        h1.innerText = mensagem;
        h1.classList.add('h1');
        div.classList.add('col-12', 'p-3');
        div.appendChild(h1);
        return div
    }

    function populaCarousel(conteudo) {
        const carouselContent = mainContent.querySelector('#home-carousel-bemvindo');
        const ulCarousel = carouselContent.querySelector('ul');
        const slide = criaSlideCarousel(conteudo.titulo, conteudo.conteudo, conteudo.ativosConteudo);
        ulCarousel.appendChild(slide);
    }

    function criaSlideCarousel(titulo, descricao, ativos) {
        const li = document.createElement('li');
        const div = document.createElement('div');
        const h3 = document.createElement('h3');
        const hr = document.createElement('hr');
        const p = document.createElement('p');
        li.classList.add('splide__slide');
        div.classList.add('col-9', 'col-md-9', 'col-sm-8', 'col-11');
        h3.classList.add('display-5', 'text-white', 'mb-3');
        hr.classList.add('border', 'border-primary', 'border-1', 'opaticy-25', 'mb-3');
        p.classList.add('text-white', 'opacity-75', 'col-9', 'mb-4');

        h3.innerText = titulo;
        p.innerText = descricao;

        div.appendChild(h3);
        div.appendChild(hr);
        div.appendChild(p);

        ativos.forEach(ativo => {
            if (parseInt(ativo.tipoAtivo) == 1) {
                console.log(ativo.valor)
                const a = document.createElement('a');
                a.href = `/${ativo.valor}`
                a.classList.add('btn', 'btn-primary', 'text-white');
                a.innerText = 'Acessar';
                div.appendChild(a);
            }
        })

        li.appendChild(div);
        return li;
    };


    function projetos(root, projetos) {

        root.innerHTML = '';
        console.log(projetos);
        console.log(root);
        projetos.forEach(projeto => {
            console.log(projeto)
            const card = cardProjeto(
                projeto.urlImagem,
                projeto.titulo,
                projeto.urlRedirecionar,
                projeto.descricao,
                projeto.titulo
            );
            root.appendChild(card);
        })
    }
    function cardProjeto(srcImagem, nome, url, descricao, titulo) {
        var divCol = document.createElement('div');
        divCol.classList.add('col', 'card-fix', 'mb-3', 'mb-md-0');

        var card = document.createElement('div');
        card.classList.add('card', 'border-primary', 'sombra-cards', 'sombra-efeito', 'bg-transparent', 'h-100');

        var img = document.createElement('img');
        img.src = srcImagem;
        img.alt = nome;
        img.classList.add('card-img-top')

        var h5 = document.createElement('h5');
        h5.classList.add('card-title');
        h5.innerText = titulo;

        var p = document.createElement('p');
        p.classList.add('card-text', 'text-primary');
        p.innerText = descricao;

        var a = document.createElement('a');
        a.classList.add('col-6', 'btn', 'btn-primary', 'text-white', 'mt-auto');
        a.href = url;
        a.innerText = "Acessar projeto";

        var cardBody = document.createElement('div');
        cardBody.classList.add('card-body', 'bg-dark-darker', 'text-white', 'rounded-bottom-2', 'd-flex', 'flex-column');

        card.appendChild(img);
        cardBody.appendChild(h5);
        cardBody.appendChild(p);
        cardBody.appendChild(a);
        card.appendChild(cardBody);
        divCol.appendChild(card);

        return divCol;
    }

    function sombraEfeito() {
        const sombraElementos = document.querySelectorAll('.sombra-efeito');
        const clamp = (a, m, n) => {
            const max = Math.max(m, n);
            const min = Math.min(m, n);

            return Math.max(min, Math.min(max, a));
        };
        const criaSombra = (elemento, clientX, clientY) => {
            const rect = elemento.getBoundingClientRect();
            const o = Math.min(rect.width, rect.height, 300);

            const mx = clamp(clientX, rect.left - o, rect.right + o);
            const my = clamp(clientY, rect.top - o, rect.bottom + o);
            const tx = rect.right - rect.width / 2;
            const ty = rect.bottom - rect.height / 2;
            const x = (mx - tx) / (rect.right - rect.left + 2 * o);
            const y = (my - ty) / (rect.bottom - rect.top + 2 * o);
            return `${x * o}px ${y * o}px 100px -80px #51ACC5`;
        }

        sombraElementos.forEach(elemento => {

            const height = elemento.clientHeight;
            const width = elemento.clientWidth;
            console.log(height + " height");
            console.log(width + " width");

            elemento.addEventListener('mousemove', (e) => {
                const sombra = criaSombra(elemento, e.clientX, e.clientY);
                elemento.style.boxShadow = sombra;
            });
            elemento.addEventListener('mouseleave', e => {

                elemento.style.boxShadow = `0px 16px 26px 2px rgba(0, 0, 0, 0.45)`;
            })

        });
    }

    function init() {
        homeInit();
    };

    init()
}

home();