document.querySelector('[data-target="#add-project-modal"]')?.addEventListener("click", () => {
    const form = document.querySelector("#AddProjectForm");
    if (!form) return;

    const fields = form.querySelectorAll("input[data-val='true'], textarea[data-val='true']");

    fields.forEach(field => {
        field.addEventListener("input", () => validateField(field));
    });

    form.addEventListener("submit", function (e) {
        let hasErrors = false;

        fields.forEach(field => {
            const valid = validateField(field);
            if (!valid) hasErrors = true;
        });

        if (hasErrors) {
            e.preventDefault();
        }
    });

    function validateField(field) {
        const errorSpan = form.querySelector(`span[data-valmsg-for='${field.name}']`);
        if (!errorSpan) return true;

        let errorMessage = "";
        const value = field.value;

        if (field.name === "FormModel.ClientId" && value === "0") {
            errorMessage = "Please choose a client.";
        } else if (field.hasAttribute("data-val-required") && value.trim() === "") {
            errorMessage = field.getAttribute("data-val-required");
        }

        if (field.name === "FormModel.ClientId") {
            const selectWrapper = field.closest(".form-select");
            if (errorMessage) {
                selectWrapper.classList.add("input-validation-error");
            } else {
                selectWrapper.classList.remove("input-validation-error");
            }
        }

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

    // Gör så dropdownen kan validera direkt:
    document.querySelectorAll('#add-project-modal .form-select').forEach(select => {
        const trigger = select.querySelector('.form-select-trigger');
        const triggerText = trigger.querySelector('.form-select-text');
        const options = select.querySelectorAll('.form-select-option');
        const hiddenInput = select.querySelector('input[type="hidden"]');
        const placeholder = select.dataset.placeholder || "Choose";

        const setValue = (value = "0", text = placeholder) => {
            triggerText.textContent = text;
            hiddenInput.value = value;
            select.classList.toggle('has-placeholder', value === "0");
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

                setTimeout(() => validateField(hiddenInput), 0);

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