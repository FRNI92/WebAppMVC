// Dropdown select logik
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

    // need to define form before I cant useer form.appenchild realinput
    // had a placeholder for index [0] that needed to be written over before adding to database
    const form = document.querySelector("#AddProjectForm");
    options.forEach(option => {
        option.addEventListener('click', () => {
            setValue(option.dataset.value, option.textContent);
            select.classList.remove('open');

            if (hiddenInput.name === "FormModel.MemberIds[0]") {
                // Ta bort gamla MemberIds
                document.querySelectorAll("input[name^='FormModel.MemberIds']").forEach(e => e.remove());

                const realInput = document.createElement("input");
                realInput.type = "hidden";
                realInput.name = "FormModel.MemberIds[0]";
                realInput.value = option.dataset.value;
                realInput.setAttribute("data-val", "true");
                realInput.setAttribute("data-val-required", "Please choose a member.");
                form.appendChild(realInput);

                // Validera nya inputen
                setTimeout(() => validateField(realInput), 0);
            } else {
                // Validera client hidden input
                setTimeout(() => validateField(hiddenInput), 0);
            }
        });
    });

    document.addEventListener('click', e => {
        if (!select.contains(e.target)) {
            select.classList.remove('open');
        }
    });
});

    function validateField(field) {
        const errorSpan = form.querySelector(`span[data-valmsg-for='${field.name}']`);
        if (!errorSpan) return true;

        let errorMessage = "";
        const value = field.value;

        if (field.name === "FormModel.ClientId") {
            if (value === "0") {
                errorMessage = "Please choose a client.";
            }
        }

        if (field.name === "FormModel.MemberIds") {
            if (!value || value === "0") {
                errorMessage = "Please choose a member.";
            }
        }

        if (field.hasAttribute("data-val-required") && value.trim() === "") {
            errorMessage = field.getAttribute("data-val-required");
        }

        if (field.name === "FormModel.ClientId" || field.name === "FormModel.MemberIds") {
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


        //validate part
        function validateField(field) {
            const errorSpan = form.querySelector(`span[data-valmsg-for='${field.name}']`);
            if (!errorSpan) return true;

            let errorMessage = "";
            const value = field.value;


            //image checker
            if (field.name === "FormModel.ImageFile") {
                if (!field.value) {
                    errorMessage = "Please upload an image.";
                }
                console.log("Budget value:", value);
            }

            if (field.name === "FormModel.ClientId" && value === "0") {
                errorMessage = "Please choose a client.";
            }

            if (field.name === "FormModel.MemberIds[0]" && (!value || value === "0")) {
                errorMessage = "Please choose a member.";
            }

            if (field.hasAttribute("data-val-required") && value.trim() === "") {
                errorMessage = field.getAttribute("data-val-required");
            }


            //check the cliend and the member. client is a list. member is a intlist
            if (
                field.name === "FormModel.ClientId" ||
                field.name === "FormModel.MemberIds[0]"
            ) {
                const selectWrapper = field.closest(".form-select");
                if (errorMessage) {
                    selectWrapper?.classList.add("input-validation-error");
                } else {
                    selectWrapper?.classList.remove("input-validation-error");
                }
            }

            //check the budget
            if (field.name === "FormModel.Budget") {
                if (value.trim() === "" || isNaN(value) || parseFloat(value) <= 0) {
                    errorMessage = "Please enter a valid budget.";
                }
            }
            //console.log("Budget value:", value);

            //control the classes
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