function showSuccessAlert(title, message) {
    Swal.fire({
        icon: "success",
        title: title,
        text: message,
        timer: 2000,
        timerProgressBar: true,
    });
}

function showErrorAlert(title, message) {
    Swal.fire({
        icon: "error",
        title: title,
        text: message,
        timer: 2000,
        timerProgressBar: true,
    });
}