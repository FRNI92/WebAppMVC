document.querySelectorAll('#add-edit-project-modal .form-select').forEach(select => {
    const trigger = select.querySelector('.form-select-trigger');
    const triggerText = trigger.querySelector('.form-select-text');
    const options = select.querySelectorAll('.form-select-option');
    const hiddenInput = select.querySelector('input[type="hidden"]');
    const placeholder = select.dataset.placeholder || "Choose";
    //const selectedContainer = select.querySelector('#selected-members-container-edit'); // För att hålla koll på valda medlemmar

    // Funktion för att sätta värdet och texten för triggern
    const setValue = (value = "", text = placeholder) => {
        triggerText.textContent = text;
        hiddenInput.value = value;
        select.classList.toggle('has-placeholder', !value);
    };

    // Initiera med standardvärde (kan vara tomt eller ett valt värde)
    setValue();

    // Öppna dropdown när man klickar på triggern
    trigger.addEventListener('click', e => {
        e.stopPropagation();
        document.querySelectorAll('.form-select.open')
            .forEach(el => el !== select && el.classList.remove('open'));
        select.classList.toggle('open');
    });

    // När ett alternativ väljs
    options.forEach(option => {
        option.addEventListener('click', () => {
            // Kontrollera om dropdownen är för medlemmar och behandla det
            if (select.id === "member-select") {
                const value = option.dataset.value;
                const name = option.textContent.trim();
                console.log("right before adding class to selected members")
                // Uppdatera valda medlemmar
                if (!hiddenInput.value.includes(value)) {
                    hiddenInput.value += value + ",";
                    triggerText.textContent = `${name} selected`;
                    option.classList.add("selected-option");
                    console.log("adding selected option class")
                }
                else {
                    hiddenInput.value = hiddenInput.value.replace(value + ",", "");
                    triggerText.textContent = "Choose a member";
                    option.classList.remove("selected-option"); // <-- Ta bort klass om avmarkerad
                }
            }
            else {
                // Uppdatera andra dropdowns
                setValue(option.dataset.value, option.textContent);
            }
            select.classList.remove('open'); // Stänger dropdownen
        });
    });

    // Stäng dropdown om man klickar utanför
    document.addEventListener('click', e => {
        if (!select.contains(e.target)) {
            select.classList.remove('open');
        }
    });
});




// EDITH IMAGE PREVIEW. 
document.getElementById('upload-trigger-edit')?.addEventListener('click', () => {
    document.getElementById('image-upload-edit')?.click();
});

document.getElementById('image-upload-edit')?.addEventListener('change', function (e) {
    const file = e.target.files[0];
    if (file && file.type.startsWith('image/')) {
        const reader = new FileReader();
        reader.onload = (e) => {
            document.getElementById('image-preview-edit').src = e.target.result;
            document.getElementById('image-preview-edit').classList.remove('hide');
            document.getElementById('image-preview-icon-container-edit').classList.add('selected');
            document.getElementById('image-preview-icon-edit').classList.replace('fa-camera', 'fa-pen-to-square');
        };
        reader.readAsDataURL(file);
    }
});





const editForm = document.getElementById('EditProjectForm');
if (editForm) {
    // plocka ut de fält vi vill validera
    const nameInput = editForm.querySelector('input[name="FormModel.ProjectName"]');
    const descInput = editForm.querySelector('textarea[name="FormModel.Description"]');
    const budgetInput = editForm.querySelector('input[name="FormModel.Budget"]');

    // helper som sätter fel-span
    function validateField(field, message) {
        const span = editForm.querySelector(`span[data-valmsg-for='${field.name}']`);
        if (!span) return true;
        let ok = true;
        let err = '';
        if (field === nameInput || field === descInput) {
            if (!field.value.trim()) {
                ok = false;
                err = field === nameInput
                    ? 'Please enter a project name.'
                    : 'Please enter a description.';
            }
        }
        if (field === budgetInput) {
            const v = parseFloat(field.value);
            if (isNaN(v) || v <= 0) {
                ok = false;
                err = 'Please enter a valid budget.';
            }
        }
        if (!ok) {
            field.classList.add('input-validation-error');
            span.textContent = err;
            span.classList.add('field-validation-error');
            span.classList.remove('field-validation-valid');
        } else {
            field.classList.remove('input-validation-error');
            span.textContent = '';
            span.classList.remove('field-validation-error');
            span.classList.add('field-validation-valid');
        }
        return ok;
    }

    // validera på input
    [nameInput, descInput, budgetInput].forEach(f => {
        f.addEventListener('input', () => validateField(f));
    });

    // validera på submit
    editForm.addEventListener('submit', function (e) {
        let ok = true;
        [nameInput, descInput, budgetInput].forEach(f => {
            if (!validateField(f)) ok = false;
        });
        if (!ok) e.preventDefault();
    });
}











