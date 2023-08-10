import Splide from '@splidejs/splide';

const home = {

    splideCarousel() {
        document.addEventListener('DOMContentLoaded', function () {
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



        });
    },

    init() {
        this.splideCarousel();
    }
}
home.init()