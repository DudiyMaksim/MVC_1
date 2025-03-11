function postRequest(url, data) {
    fetch(url, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(data)
    })
        .then((response) => {
            if (response.ok) {
                window.location.reload();
            }
        })
        .catch((error) => console.error(error));
}

function addToCart(id) {
    postRequest("/Cart/AddToCart", { productId: id });
}

function removeFromCart(id) {
    postRequest("/Cart/RemoveFromCart", { productId: id });
}

function RemoveFromCartintoCart(id) {
    postRequest("/Cart/RemoveFromCartintoCart", { productId: id })
}