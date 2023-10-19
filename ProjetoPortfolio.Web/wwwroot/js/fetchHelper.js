
export const fetchGet = async (url) => {
    try {
        const response = await fetch(url, {
            method: 'GET'
        });
        const result = await response.json();
        return result;

    } catch (error) {
        const err = "Error: " + error;
        return err
    }
}

export const fetchPost = async (url, body) => {
    try {
        var response = await fetch(url, {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                "Content-type": "application/json",
            },
            body: JSON.stringify(body)
        }).then(result => result)
        return response;
    } catch (error) {
        const err = "Error: " + error;
        return err
    }
}
