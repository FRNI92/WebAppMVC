console.log("adding data to client edit 1");
////find open edt client modal. foreach all buttons, listen for click.
//find add edit client modal and add show
// check data sent from page right before. in editlink. its id and name
// set the value and set the name inside to input field

document.querySelectorAll('.open-edit-client-modal').forEach(button => {
    button.addEventListener("click", (e) => {
        e.stopPropagation();
        const targetSelector = button.getAttribute("data-target");
        const modal = document.querySelector(targetSelector);
        if (!modal) return;

        // close other modals
        document.querySelectorAll('.modal.modal-show').forEach(m => m.classList.remove('modal-show'));
        console.log("closing other modals")


        // fill in all the data
        modal.querySelector('input[name="Id"]').value = button.dataset.id;
        modal.querySelector('input[name="ClientName"]').value = button.dataset.name;

        const clientForm = modal.querySelector(`form`)
        const clientNameInput = clientForm.querySelector(`input[name="ClientName"]`);


        clientNameInput.addEventListener("input", () => {
            console.log("realtime validation")
            validateClientForm(clientForm, clientNameInput);
        });
        clientForm.addEventListener("submit", e => {
            if (!validateClientForm(clientForm, clientNameInput)) {
                e.preventDefault();
                console.log("calling the validate client function");
            }
        modal.classList.add("modal-show");
        console.log("add modal show on this class and show the modal");
        });
    });
});


//validate on submit
function validateClientForm(form, clientNameInput) {
    const errorSpan = form.querySelector(`span[data-valmsg-for='ClientName']`);
    let isValid = true;

    console.log("before value check")
    if (!clientNameInput.value.trim()) {

        clientNameInput.classList.add("input-validation-error");
        console.log("adding error on submit")
        if (errorSpan) {
            errorSpan.textContent = "Client Name is required.";
            errorSpan.classList.add("field-validation-error");
            errorSpan.classList.remove("field-validation-valid");
        }
        isValid = false;
    } else {
        clientNameInput.classList.remove("input-validation-error");
        console.log("removing error on submit")
        if (errorSpan) {
            errorSpan.textContent = "";
            errorSpan.classList.remove("field-validation-error");
            errorSpan.classList.add("field-validation-valid");
        }
    }

    return isValid;
}