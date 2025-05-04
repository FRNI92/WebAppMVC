document.addEventListener("DOMContentLoaded", function () {
    const formAddClient = document.querySelector("#add-client-modal form");
    const clientNameInput = formAddClient.querySelector("[name='ClientName']");
    const errorSpan = formAddClient.querySelector("[data-valmsg-for='ClientName']");
    let hasSubmitted = false; // to track the form and not show error before submit atleast 1 time


    formAddClient.addEventListener("submit", function (e) {
        hasSubmitted = true;

        if (!clientNameInput.value.trim()) {
            e.preventDefault();

            clientNameInput.classList.add("input-validation-error");
            console.log("adding error on submit");

            if (errorSpan) {
                errorSpan.textContent = "Client Name is required.";
                errorSpan.classList.add("field-validation-error");
                errorSpan.classList.remove("field-validation-valid");
            }
        } else {
            clientNameInput.classList.remove("input-validation-error");
            console.log("removing error on submit");

            if (errorSpan) {
                errorSpan.textContent = "";
                errorSpan.classList.remove("field-validation-error");
                errorSpan.classList.add("field-validation-valid");
            }
        }
    });

    clientNameInput.addEventListener("input", function () {
        if (!hasSubmitted) return;

        if (clientNameInput.value.trim()) {
            clientNameInput.classList.remove("input-validation-error");
            console.log("removing error on input");

            if (errorSpan) {
                errorSpan.textContent = "";
                errorSpan.classList.remove("field-validation-error");
                errorSpan.classList.add("field-validation-valid");
            }
        }
    });
});
