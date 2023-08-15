
export const fetchGet = async (url) => {
    var response = await fetch(url, {
        method: 'GET'
    });

    var result = await response.json();

    return result;
}

export const fetchPost = async (url, body) => {
    var response = await fetch(url, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            "Content-type": "application/json",
        },
        body: JSON.stringify(body)
    }).then(result => result)

    return response;
}
