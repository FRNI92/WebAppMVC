// Form select
document.querySelectorAll('#add-member-modal .form-select').forEach(select => {
    const trigger = select.querySelector('.form-select-trigger');
    const triggerText = trigger.querySelector('.form-select-text');
    const options = select.querySelectorAll('.form-select-option');
    const hiddenInput = select.querySelector('input[type="hidden"]');
    const placeholder = select.dataset.placeholder || "Choose";

    const setValue = (value = "", text = placeholder) => {
        triggerText.textContent = text;
        hiddenInput.value = value;
        select.classList.toggle('has-placeholder', !value);
    };

    setValue();

    trigger.addEventListener('click', e => {
        e.stopPropagation();
        document.querySelectorAll('.form-select.open')
            .forEach(el => el !== select && el.classList.remove('open'));
        select.classList.toggle('open');
    });
    options.forEach(option => {
        option.addEventListener('click', () => {
            setValue(option.dataset.value, option.textContent);
            select.classList.remove('open');

            // Extra kod för MemberId
            const inputName = hiddenInput.getAttribute('name');
            if (inputName === "FormModel.MemberIds") {
                hiddenInput.value = option.dataset.value;
            }
        });
    });

    document.addEventListener('click', e => {
        if (!select.contains(e.target)) {
            select.classList.remove('open');
        }
    });
});


// EDITH IMAGE PREVIEW. 
document.getElementById('upload-trigger')?.addEventListener('click', () => {
    document.getElementById('image-upload')?.click();
});

document.getElementById('image-upload')?.addEventListener('change', function (e) {
    const file = e.target.files[0];
    if (file && file.type.startsWith('image/')) {
        const reader = new FileReader();
        reader.onload = (e) => {
            document.getElementById('image-preview').src = e.target.result;
            document.getElementById('image-preview').classList.remove('hide');
            document.getElementById('image-preview-icon-container').classList.add('selected');
            document.getElementById('image-preview-icon').classList.replace('fa-camera', 'fa-pen-to-square');
        };
        reader.readAsDataURL(file);
    }
});
//$(document).ready(function () {
//    // Kontrollera om vi ska visa modalen efter form submission
//    var showModal = '@TempData["ShowAddMemberModal"]';
//    if (showModal === "true") {
//        // Öppna modalen
//        document.getElementById('add-member-modal').style.display = 'block';  // Öppna modalen
//    }

//    // Event för att stänga modalen (kanske knappen med 'data-type="close"')
//    document.querySelector('.btn-close').addEventListener('click', function () {
//        document.getElementById('add-member-modal').style.display = 'none'; // Stäng modalen
//    });
//});

document.addEventListener('DOMContentLoaded', function () {
    const form = document.querySelector("#add-member-form");

    if (!form) return;

    // Validera fälten på input
    const fields = form.querySelectorAll("input[data-val='true'], textarea[data-val='true']");

    fields.forEach(field => {
        field.addEventListener("input", () => validateField(field));
    });

    form.addEventListener("submit", function (e) {
        let hasErrors = false;

        // Validera alla fält
        fields.forEach(field => {
            const valid = validateField(field);
            if (!valid) hasErrors = true;
        });

        // Förhindra att formuläret skickas om det finns fel
        if (hasErrors) {
            e.preventDefault();
        }
    });

    // Validering av ett specifikt fält
    function validateField(field) {
        const errorSpan = form.querySelector(`span[data-valmsg-for='${field.name}']`);
        if (!errorSpan) return true;

        let errorMessage = "";
        const value = field.value.trim();

        if (field.name === "FormModel.FirstName" && !value) {
            errorMessage = "First Name is required.";
        } else if (field.name === "FormModel.LastName" && !value) {
            errorMessage = "Last Name is required.";
        } else if (field.name === "FormModel.Email" && !value) {
            errorMessage = "Email is required.";
        } else if (field.name === "FormModel.Phone" && !value) {
            errorMessage = "Phone is required.";
        } else if (field.name === "FormModel.DateOfBirth" && !value) {
            errorMessage = "Date of Birth is required.";
        } else if (field.name === "FormModel.Address.StreetName" && !value) {
            errorMessage = "Street Name is required.";
        } else if (field.name === "FormModel.Address.StreetNumber" && !value) {
            errorMessage = "Street Number is required.";
        } else if (field.name === "FormModel.Address.PostalCode" && !value) {
            errorMessage = "Postal Code is required.";
        } else if (field.name === "FormModel.Address.City" && !value) {
            errorMessage = "City is required.";
        }

        // Kontrollera om fältet är tomt eller ogiltigt
        if (errorMessage) {
            field.classList.add("input-validation-error");
            errorSpan.textContent = errorMessage;
            errorSpan.classList.add("field-validation-error");
            errorSpan.classList.remove("field-validation-valid");
            return false;
        } else {
            field.classList.remove("input-validation-error");
            errorSpan.textContent = "";
            errorSpan.classList.remove("field-validation-error");
            errorSpan.classList.add("field-validation-valid");
            return true;
        }
    }
});