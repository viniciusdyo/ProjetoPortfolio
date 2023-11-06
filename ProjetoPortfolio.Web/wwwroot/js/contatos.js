import { fetchPost } from "./fetchHelper";
const contatos = () => {
    const formEmail = document.querySelector('#form-email');
    const init = () => {
        const formEmailBtn = formEmail.querySelector('button');
        formEmailBtn.addEventListener('click', (e) => {
            e.preventDefault();
            enviarEmail();
        })
    };

    const enviarEmail = async () => {
        const email = getFormData();
        criaPlaceholder();
        //const fetchData = await fetchPost('Contatos/EnviarEmail', email);
        //recarregaPagina(fetchData);
    }

    const recarregaPagina = (data) => {
        const status = data.status;
        const url = data.url;
        switch (status) {
            case 200:
                window.location.reload(true)
                break;
            default:
                alert('Erro ao enviar e-mail')
                break;
        }
    }

    const criaPlaceholder = () => {

        formEmail.querySelectorAll('div').forEach(d => {
            d.classList.add('contatos-fade-out');
        });

        formEmail.innerHTML = '';

        const divHeader = document.createElement('div');
        divHeader.classList.add('col-12')

        const header = document.createElement('h2');
        header.innerText = 'Obrigado por entrar em contato !';
        header.classList.add('text-white');


        const hr = document.createElement('hr');
        divHeader.appendChild(header);
        divHeader.appendChild(hr);

        const divPlaceHolder = document.createElement('div');
        divPlaceHolder.classList.add('placeholder-glow', 'col-12', 'contatos-animacao');
        divPlaceHolder.appendChild(divHeader);


        var assunto = document.createElement('span');
        assunto.classList.add('placeholder', 'col-12', 'py-4', 'my-3');
        divPlaceHolder.appendChild(assunto);

        var remetente = document.createElement('span');
        remetente.classList.add('placeholder', 'col-12', 'py-4', 'my-3');
        divPlaceHolder.appendChild(remetente);

        var mensagem = document.createElement('span');
        mensagem.classList.add('placeholder', 'col-12', 'py-4', 'my-3');
        divPlaceHolder.appendChild(mensagem);

        var btn = document.createElement('span');
        btn.classList.add('placeholder', 'col-2', 'py-4', 'my-3');
        divPlaceHolder.appendChild(btn);
        divPlaceHolder.classList.add('contatos-fade-in')
        formEmail.appendChild(divPlaceHolder);
    }

    const limpaForm = () => {
        const inputs = formEmail.querySelectorAll('input');
        inputs.forEach(i => {
            i.value = ''
        })
        const textarea = formEmail.querySelector('textarea');
        textarea.value = ''
    }
    const getFormData = () => {
        const formData = new FormData(formEmail);
        const obj = {};
        formData.forEach((value, key) => {
            obj[key] = value
        });

        return obj;
    }

    init();
};

contatos();