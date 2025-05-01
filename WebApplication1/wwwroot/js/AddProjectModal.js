// I was making changes to edit modal and the members I chose kept showing up here aswell.
// so I listen for the addproject modal clear the innehtml in members container. and remove the highlightclass
// after that I add choose, so the select is not empty
document.querySelector('[data-type="modal"][data-target="#add-project-modal"]')?.addEventListener("click", () => {
    const selectedContainer = document.getElementById("selected-members-container");
    selectedContainer.innerHTML = "";

    document.querySelectorAll('#add-project-modal .form-select-option.selected-option')
        .forEach(option => option.classList.remove('selected-option'));

    const memberTriggerText = document.querySelector('#add-project-modal #member-select .form-select-text');
    if (memberTriggerText) memberTriggerText.textContent = "Choose";
});


document.querySelectorAll('#add-project-modal .form-select').forEach(select => {
    const trigger = select.querySelector('.form-select-trigger');
    const triggerText = trigger.querySelector('.form-select-text');
    const options = select.querySelectorAll('.form-select-option');
    const hiddenInput = select.querySelector('input[type="hidden"]');
    const placeholder = select.dataset.placeholder || "Choose";

    //const form = document.querySelector("#AddProjectForm");

    const setValue = (value = "0", text = placeholder) => {
        triggerText.textContent = text;
        if (hiddenInput) {
            hiddenInput.value = value;
        }
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
            select.classList.remove('open');

            if (select.id === "member-select") {
                const selectedContainer = document.getElementById("selected-members-container");

                const index = selectedContainer.querySelectorAll('input').length;
                const alreadySelected = Array.from(selectedContainer.querySelectorAll('input'))
                    .some(input => input.value === option.dataset.value);



                if (alreadySelected) {
                    // Ta bort input
                    const inputToRemove = Array.from(selectedContainer.querySelectorAll('input'))
                        .find(input => input.value === option.dataset.value);
                    if (inputToRemove) {
                        selectedContainer.removeChild(inputToRemove);
                    }


                    option.classList.remove("selected-option"); // Ta bort highlight

                    // Uppdatera text
                    const selectedCount = selectedContainer.querySelectorAll('input').length;
                    triggerText.textContent = selectedCount > 0
                        ? `${selectedCount} member${selectedCount > 1 ? "s" : ""} selected`
                        : "Choose";

                    validateMembers();
                    return;
                }


                const newInput = document.createElement("input");
                newInput.type = "hidden";
                newInput.name = `FormModel.MemberIds[${index}]`;
                newInput.value = option.dataset.value;
                selectedContainer.appendChild(newInput);
                validateMembers();// to remove error when you select a member
                option.classList.add("selected-option");// style the member you have chosen
                // Uppdatera trigger text
                const selectedCount = selectedContainer.querySelectorAll('input').length;
                triggerText.textContent = `${selectedCount} member${selectedCount > 1 ? "s" : ""} selected`;

                // Validera direkt
                setTimeout(() => validateField(newInput), 0);
            }
            else {
                // För client dropdown
                const triggerText = select.querySelector('.form-select-text');
                const hiddenInput = select.querySelector('input[type="hidden"]');
                const placeholder = select.dataset.placeholder || "Choose";

                triggerText.textContent = option.textContent;
                if (hiddenInput) {
                    hiddenInput.value = option.dataset.value;
                    setTimeout(() => validateField(hiddenInput), 0);
                }
            }

            //added this to access the validate in real time
            triggerText.textContent = option.textContent;
            if (hiddenInput) {
                hiddenInput.value = option.dataset.value;
                setTimeout(() => {
                    validateClientId(hiddenInput);
                }, 0);
            }
        });
    });

    document.addEventListener('click', e => {
        if (!select.contains(e.target)) {
            select.classList.remove('open');
        }
    });
});



// validates on submit
document.querySelector('[data-target="#add-project-modal"]')?.addEventListener("click", () => {
        const form = document.querySelector("#AddProjectForm");
        if (!form) return;

        form.addEventListener("submit", function (e) {
            let hasErrors = false;

            const projectName = form.querySelector('input[name="FormModel.ProjectName"]');
            const description = form.querySelector('textarea[name="FormModel.Description"]');
            const startDate = form.querySelector('input[name="FormModel.StartDate"]');
            const endDate = form.querySelector('input[name="FormModel.EndDate"]');
            const clientId = form.querySelector('input[name="FormModel.ClientId"]');
            const budget = form.querySelector('input[name="FormModel.Budget"]');
            const imageFile = form.querySelector('input[name="FormModel.ImageFile"]');
            const selectedMembers = document.querySelectorAll("#selected-members-container input");

            if (!projectName.value.trim()) {
                console.log("Project Name is empty on sumbmit.");
                showError(projectName, "Project Name is required.");
                hasErrors = true;
            }
            if (!description.value.trim()) {
                showError(description, "Description is required.");
                console.log("Description is empty on sumbmit.");
                hasErrors = true;
            }
            if (!startDate.value.trim()) {
                showError(startDate, "Start Date is required.");
                hasErrors = true;
            }
            if (!endDate.value.trim()) {
                showError(endDate, "End Date is required.");
                hasErrors = true;
            }
            if (clientId.value === "0") {
                showError(clientId, "Please choose a client.");
                console.log("client not chosen on submit.");
                hasErrors = true;
            }
            if (selectedMembers.length === 0) {
                const memberSelect = document.getElementById("member-select");

                memberSelect.classList.add("input-validation-error");
                const errorSpan = memberSelect.querySelector('span[data-valmsg-for="FormModel.MemberIds"]');
                if (errorSpan) {
                    errorSpan.textContent = "Please choose at least one member.";
                    errorSpan.classList.add("field-validation-error");
                    errorSpan.classList.remove("field-validation-valid");
                }
                hasErrors = true;
            }
            if (!budget.value.trim() || isNaN(budget.value) || parseFloat(budget.value) <= 0) {
                showError(budget, "Please enter a valid budget.");
                hasErrors = true;
            }
            if (!imageFile.value.trim()) {
                console.log("Image is empty on sumbmit.");
                showError(imageFile, "Please upload an image.");
                hasErrors = true;
            }

            if (hasErrors) {
                e.preventDefault();
            }
        });
    });

    function showError(field, message) {
        const form = document.querySelector("#AddProjectForm");
        const errorSpan = form.querySelector(`span[data-valmsg-for='${field.name}']`);
        if (errorSpan) {
            errorSpan.textContent = message;
            errorSpan.classList.add("field-validation-error");
            errorSpan.classList.remove("field-validation-valid");
        }
        field.classList.add("input-validation-error");
}





// realtime validate. listen for projectname and description
document.addEventListener('DOMContentLoaded', () => {
    const projectName = document.querySelector('input[name="FormModel.ProjectName"]');  // Hitta Project Name fältet
    const description = document.querySelector('textarea[name="FormModel.Description"]'); // dont use input, use textarea
    const budget = document.querySelector('input[name="FormModel.Budget"]'); // find budget
    const imageFile = document.querySelector('input[name="FormModel.ImageFile"]');// find image


    //this was not working correctly. try like I did with the others. 
    // tried eventlistener and input
    const clientId = document.querySelector('input[name="FormModel.ClientId"]');
    clientId?.addEventListener("change", () => validateClientId(clientId));
    console.log(`ClientId value: ${clientId.value}`);
    // Lägg till event listener för realtidsvalidering
    projectName.addEventListener('input', (e) => {
        validateProjectName(e.target);  // Anropa valideringsfunktionen vid varje input
    });
    description.addEventListener('input', (e) => {// can use input and textarea here to listen.
        validateDescription(e.target);
    });
    budget.addEventListener('input', (e) => {
        validateBudget(e.target);
    });
    if (imageFile) {
        imageFile.addEventListener('change', (e) => {
            validateImageFile(e.target); // Call validateImageFile when file is selected
        });
    } else {
        console.log("Image file input not found.");
    }
});

// Funktion för att validera Project Name
function validateProjectName(field) {
    const form = document.querySelector("#AddProjectForm");
    console.log("realtime val projectname")
    const errorSpan = form.querySelector(`span[data-valmsg-for='${field.name}']`);
    let errorMessage = "";

    // trim input and remove space
    const value = field.value.trim();

    // see it its empty
    if (!value) {
        errorMessage = "Project Name is required.";  // errormessage
    }

    if (errorMessage) {
        field.classList.add("input-validation-error");  // add errorclass
        if (errorSpan) {
            errorSpan.textContent = errorMessage;  // set message
            errorSpan.classList.add("field-validation-error");
            errorSpan.classList.remove("field-validation-valid");
        }
    } else {
        field.classList.remove("input-validation-error");  // remove class
        if (errorSpan) {
            errorSpan.textContent = "";  // clear message. 
            errorSpan.classList.remove("field-validation-error");
            errorSpan.classList.add("field-validation-valid");
        }
    }
}


//real time for description
function validateDescription(field) {
    const form = document.querySelector("#AddProjectForm");
    console.log("realtime val description")
    const errorSpan = form.querySelector(`span[data-valmsg-for='${field.name}']`);
    let errorMessage = "";

    const value = field.value.trim();

    if (!value) {
        errorMessage = "Description is required.";  // errormessage
    }

    if (errorMessage) {
        field.classList.add("input-validation-error");  // adding the the class
        if (errorSpan) {
            errorSpan.textContent = errorMessage;  // setting the message
            errorSpan.classList.add("field-validation-error");
            errorSpan.classList.remove("field-validation-valid");
        }
    } else {
        field.classList.remove("input-validation-error");  // removes class
        if (errorSpan) {
            errorSpan.textContent = "";  // clear the message
            errorSpan.classList.remove("field-validation-error");
            errorSpan.classList.add("field-validation-valid");
        }
    }
}

//realtime for budget
function validateBudget(field) {
    const form = document.querySelector("#AddProjectForm");
    console.log("realtime val budget")
    const errorSpan = form.querySelector(`span[data-valmsg-for='${field.name}']`);
    let errorMessage = "";

    const value = field.value.trim();

    if (!value) {
        errorMessage = "Budget is required.";  // Felmeddelande om tomt fält
    }
    // Kontrollera om värdet inte är ett tal eller om det är mindre än eller lika med 0
    else if (isNaN(value) || parseFloat(value) <= 0) {
        errorMessage = "Use Numbers. cant use 0 either!"; 
    }

    if (errorMessage) {
        field.classList.add("input-validation-error");  // adding the the class
        if (errorSpan) {
            errorSpan.textContent = errorMessage;  // setting the message
            errorSpan.classList.add("field-validation-error");//adding class on span 
            errorSpan.classList.remove("field-validation-valid");// removing class
        }
    } else {
        field.classList.remove("input-validation-error");  // removes class
        if (errorSpan) {
            errorSpan.textContent = "";  // clear the message
            errorSpan.classList.remove("field-validation-error");
            errorSpan.classList.add("field-validation-valid");
        }
    }
}

function validateImageFile(field) {
    console.log("image realtime")
    const form = document.querySelector("#AddProjectForm");
    const errorSpan = form.querySelector(`span[data-valmsg-for='${field.name}']`);
    let errorMessage = "";

    const file = field.files[0];  // gets the first image chosen

    // Om ingen fil är vald
    if (!file) {
        console.log("image realtime")
        errorMessage = "Image is required."; 
    }

    if (errorMessage) {
        field.classList.add("input-validation-error");
        if (errorSpan) {
            errorSpan.textContent = errorMessage;
            errorSpan.classList.add("field-validation-error");
            errorSpan.classList.remove("field-validation-valid");
        }
    } else {
        field.classList.remove("input-validation-error");
        if (errorSpan) {
            errorSpan.textContent = "";
            errorSpan.classList.remove("field-validation-error");
            errorSpan.classList.add("field-validation-valid");
        }
    }
}


//this is the latest one. build like the others to handle realtime
// if I submit I get error. if I make choice it should remove error
// cant see console log on line 347
// added domcontentloaded, removed domconent and only use the main one
function validateClientId(field) {
    console.log("real time client")
    const form = document.querySelector("#AddProjectForm");
    const errorSpan = form.querySelector(`span[data-valmsg-for='${field.name}']`);
    const value = field.value;

    if (value === "0") {
        field.classList.add("input-validation-error");
        if (errorSpan) {
            errorSpan.textContent = "Please choose a client.";
            errorSpan.classList.add("field-validation-error");
            errorSpan.classList.remove("field-validation-valid");
        }
    } else {
        field.classList.remove("input-validation-error");
        if (errorSpan) {
            errorSpan.textContent = "";
            errorSpan.classList.remove("field-validation-error");
            errorSpan.classList.add("field-validation-valid");
        }
    }
}




// real time fo members
document.addEventListener('DOMContentLoaded', () => {
    const memberSelect = document.querySelector('#member-select'); // find drop down for members
    const selectedMembersContainer = document.getElementById("selected-members-container"); //to keep track of chosen member

   
    memberSelect.addEventListener('change', () => { // listen for change in dropdown
        validateMembers(); // call the validate function
    });
});

function validateMembers() {
    console.log("real time member")
    const selectedMembersContainer = document.getElementById("selected-members-container");
    const memberSelect = document.querySelector('#member-select');
    const errorSpan = memberSelect.querySelector('span[data-valmsg-for="FormModel.MemberIds"]');
    let errorMessage = "";

    // see if some member is chosen
    const memberInputs = selectedMembersContainer.querySelectorAll('input');
    if (memberInputs.length === 0) {
        errorMessage = "Please choose at least one member."; // message if none chosen
    }

    if (errorMessage) {
        memberSelect.classList.add("input-validation-error");
        if (errorSpan) {
            errorSpan.textContent = errorMessage;
            errorSpan.classList.add("field-validation-error");
            errorSpan.classList.remove("field-validation-valid");
        }
    } else {
        memberSelect.classList.remove("input-validation-error");
        if (errorSpan) {
            errorSpan.textContent = "";
            errorSpan.classList.remove("field-validation-error");
            errorSpan.classList.add("field-validation-valid");
        }
    }
}



// IMAGE PREVIEW (Visar bild när användaren väljer en bildfil)
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



