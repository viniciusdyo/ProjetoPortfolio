// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var mainContent = document.querySelector('#mainContent');

var pathName = window.location.pathname;

mainContent.addEventListener('change', () => {
    mainContent.classList.add('animacao-pagina')
})