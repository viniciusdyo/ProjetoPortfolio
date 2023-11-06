// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.



var pathName = window.location.pathname;



const site = () => {
    const init = () => {
        const mainContent = document.querySelector('#mainContent');
        mainContent.addEventListener('change', () => {
            mainContent.classList.add('animacao-pagina')
        });
    };
    init();
}
site();