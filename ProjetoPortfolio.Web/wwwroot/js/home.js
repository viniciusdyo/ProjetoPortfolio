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
            gap: '1em',
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
        if (conteudosHome != null && conteudosHome != undefined) {
            conteudosHome.forEach(conteudo => {
                switch (conteudo.nome) {
                    case 'slide':
                        populaCarousel(conteudo);

                        break;
                    default:
                        return;
                }
            });

            splideCarousel();
        } else {
            alert('Erro no servidor');
        }
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
        div.classList.add('col-9');
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

    function init() {
        homeInit();
    };

    init()
}

home();