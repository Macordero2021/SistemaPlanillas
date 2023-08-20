// Función para establecer el valor seleccionado en el almacenamiento de sesión
function setSelectedValue() {
    var selectedValue = $("#monthSelected").val();
    sessionStorage.setItem("selectedMonth", selectedValue);
}

// Función para restaurar el valor seleccionado desde el almacenamiento de sesión
function restoreSelectedValue() {
    var selectedValue = sessionStorage.getItem("selectedMonth");
    if (selectedValue !== null) {
        $("#monthSelected").val(selectedValue);
    }
}

// Enviar el formulario al cambiar el valor seleccionado
function submitForm() {
    setSelectedValue();
    document.getElementById("filterForm").submit();
}

// Borrar el valor seleccionado al cargar la página
$(document).ready(function () {
    // Si hay un valor en el almacenamiento de sesión, restablecer el valor en el select
    restoreSelectedValue();

    // Manejar el envío del formulario cuando el valor seleccionado cambia
    $("#monthSelected").change(function () {
        submitForm();
    });

    // Restablecer el valor seleccionado al cargar la página
    $(window).on("beforeunload", function () {
        sessionStorage.removeItem("selectedMonth");
    });
});