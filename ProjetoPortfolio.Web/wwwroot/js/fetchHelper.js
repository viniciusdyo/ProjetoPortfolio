
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
        body: JSON.stringify(body)
    })

    var result = await response.json();

    return result;
}
