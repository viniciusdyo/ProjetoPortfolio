import { fetchGet } from "./fetchHelper";

const adminProjeto = () => {
    const mainContent = document.querySelector("#mainContent");
    const projetoRoot = mainContent.querySelector("#projetoRoot");

    async function projetoInit() {
        const listaProjetos = await listarProjetos();
    }

    async function listarProjetos() {
        const response = await fetchGet("AdminProjeto/Listar");

        if (response != null)
            console.log(response);
    }

    function init() {
        projetoInit()
    };
    init();
};

adminProjeto();