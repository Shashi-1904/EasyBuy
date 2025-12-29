function increaseQty(button) {
    const input = button.previousElementSibling;
    let value = parseInt(input.value);
    let max = parseInt(input.max);

    if (value < max) {
        input.value = value + 1;
    }
}

function decreaseQty(button) {
    const input = button.nextElementSibling;
    let value = parseInt(input.value);
    let min = parseInt(input.min);

    if (value > min) {
        input.value = value - 1;
    }
}
