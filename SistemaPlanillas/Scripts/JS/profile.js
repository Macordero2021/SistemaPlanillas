/**
 * Function to send the update request to the server using AJAX.
 * @param {string} fieldToUpdate - The field to be updated.
 * @param {string} fieldValue - The new value for the field.
*/
function sendUpdateRequest(fieldToUpdate, fieldValue) {
    // Get the user ID from the hidden field
    const userId = document.getElementById('userId').value;

    // Create the formData object with the field and value to update
    const formData = {
        userId: userId,
        fieldName: fieldToUpdate,
        value: fieldValue.trim()
    };

    // AJAX call using fetch
    fetch('/Role/UpdateUserInfo', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'X-Requested-With': 'XMLHttpRequest'
        },
        body: JSON.stringify(formData)
    })
    .then(response => response.json())
        .then(result => {
        // If the update was successful, show a success message and refresh the page
        if (result.success) {
            Swal.fire({
                icon: 'success',
                title: result.message,
                showConfirmButton: false,
                timer: 2000,
                timerProgressBar: true,
            }).then(() => {
                window.location.reload();
            });
        } else
        {
            // Show the corresponding error message based on the received result
            switch (result.error) {
                case 'User not found':
                case 'Invalid field':
                case 'Error updating user':
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: result.error,
                        timer: 2000,
                        timerProgressBar: true,
                    });
                    break;
                case 'No changes made':
                    Swal.fire({
                        icon: 'warning',
                        title: 'Oops...',
                        text: 'Your are putting your current information',
                        timer: 2000,
                        timerProgressBar: true,
                    });
                    break;
                case 'Email already exists':
                    Swal.fire({
                        icon: 'warning',
                        title: 'Oops...',
                        text: 'Email already exists.',
                        timer: 2000,
                        timerProgressBar: true,
                    });
                    break;
                default:
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'An error occurred while processing the request.',
                        timer: 2000,
                        timerProgressBar: true,
                    });
            }
        }
    })
    .catch(error => {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'An error occurred while processing the request.',
            timer: 2000,
            timerProgressBar: true,
        });
    });
}

document.addEventListener('DOMContentLoaded', function () {
    let openButton = null; // Variable to keep track of the currently open "Update" button

    // Get all "Update" buttons
    const updateButtons = document.querySelectorAll('button[type="button"]');

    updateButtons.forEach(button => {
        // Add a click event to each button
        button.addEventListener('click', function () {
            // If there is already an open "Update" button, close it before continuing
            if (openButton && openButton !== button) {
                closeUpdateForm(openButton);
            }

            // Get the parent container of the button
            const inputContainer = button.closest('.input-container');

            // Get the input and "Cancel" button inside the container
            const input = inputContainer.querySelector('input');
            const cancelButton = inputContainer.querySelector('button[type="reset"]:last-of-type');

            // If the button has the text "Update"
            if (button.textContent === 'Update') {
                // Show the input and "Cancel" button
                input.style.display = 'inline-block';
                cancelButton.style.display = 'inline-block';

                // Change the button text to "Save"
                button.textContent = 'Save';

                // Add the class "button-save" to the button
                button.classList.add('button-save');

                // Save the currently open "Update" button
                openButton = button;
            } else {
                // If the button has the text "Save" and the input is not empty
                if (button.textContent === 'Save' && input.value.trim() !== '') {
                    // Get the field to update from the custom attribute
                    const fieldToUpdate = input.getAttribute('data-field');
                    const fieldValue = input.value.trim();

                    //Ajax to sent the data to the controller
                    sendUpdateRequest(fieldToUpdate, fieldValue);

                    closeUpdateForm(button);
                } else {
                    // If the button has the text "Save" but the input is empty, show a warning SweetAlert
                    Swal.fire({
                        icon: 'warning',
                        title: 'Oops...',
                        text: 'You cannot leave this space in blank',
                        timer: 1500,
                        timerProgressBar: true,
                    });
                }
            }
        });

        // Add a click event to the "Cancel" button
        button.nextElementSibling.addEventListener('click', function () {
            closeUpdateForm(button);
        });
    });

    // Function to close the update form
    function closeUpdateForm(button) {
        // Get the parent container of the button
        const inputContainer = button.closest('.input-container');

        // Get the input and "Cancel" button inside the container
        const input = inputContainer.querySelector('input');
        const cancelButton = inputContainer.querySelector('button[type="reset"]:last-of-type');

        input.style.display = 'none';
        cancelButton.style.display = 'none';
        button.textContent = 'Update';
        button.classList.remove('button-save');

        // Clear the openButton variable
        openButton = null;
    }
});