const editModal = document.querySelector('#add-edit-member-modal');

document.querySelectorAll('.open-edit-member-modal').forEach(button => {
    button.addEventListener('click', () => {
        editModal.classList.add('modal-show');



        const image = button.dataset.image;
        if (image) {
            editModal.querySelector('input[name="FormModel.Image"]').value = image;

            const preview = editModal.querySelector('#image-preview');
            const previewContainer = editModal.querySelector('#image-preview-icon-container');
            const icon = editModal.querySelector('#image-preview-icon');

            preview.src = `/uploads/${image}`;
            preview.classList.remove('hide');
            previewContainer.classList.add('selected');
            icon.classList.replace('fa-camera', 'fa-pen-to-square');
        }
    });
});

editModal.querySelector('#upload-trigger-edit')?.addEventListener('click', () => {
    editModal.querySelector('#image-upload')?.click();
});

editModal.querySelector('#image-upload')?.addEventListener('change', function (e) {
    const file = e.target.files[0];
    if (file && file.type.startsWith('image/')) {
        const reader = new FileReader();
        reader.onload = (e) => {
            editModal.querySelector('#image-preview').src = e.target.result;
            editModal.querySelector('#image-preview').classList.remove('hide');
            editModal.querySelector('#image-preview-icon-container').classList.add('selected');
            editModal.querySelector('#image-preview-icon').classList.replace('fa-camera', 'fa-pen-to-square');
        };
        reader.readAsDataURL(file);
    }
});


//Member card: dropdown edit button
document.querySelectorAll('.open-edit-member-modal').forEach(button => {
    button.addEventListener('click', e => {
        const id = button.dataset.id;
        const firstName = button.dataset.firstname;
        const lastName = button.dataset.lastname;
        const jobTitle = button.dataset.jobtitle;
        const email = button.dataset.email;
        const phone = button.dataset.phone;
        const city = button.dataset.city;
        const streetName = button.dataset.streetname;
        const streetNumber = button.dataset.streetnumber;
        const postalCode = button.dataset.postalcode;
        const dateOfBirth = button.dataset.dateofbirth;


        //for debugging
        console.log(firstName, lastName, email, phone, jobTitle, city, streetName, dateOfBirth);


        const modal = document.querySelector('#add-edit-member-modal');
        modal?.classList.add('modal-show');

        modal.querySelector('input[name="FormModel.Id"]').value = id;
        modal.querySelector('input[name="FormModel.FirstName"]').value = firstName;
        modal.querySelector('input[name="FormModel.LastName"]').value = lastName;
        modal.querySelector('input[name="FormModel.Email"]').value = email;
        modal.querySelector('input[name="FormModel.Phone"]').value = phone;
        modal.querySelector('input[name="FormModel.JobTitle"]').value = jobTitle;
        modal.querySelector('input[name="FormModel.Address.City"]').value = city;
        modal.querySelector('input[name="FormModel.Address.StreetName"]').value = streetName;
        modal.querySelector('input[name="FormModel.Address.StreetNumber"]').value = streetNumber;
        modal.querySelector('input[name="FormModel.Address.PostalCode"]').value = postalCode;
        modal.querySelector('input[name="FormModel.DateOfBirth"]').value = dateOfBirth;





        //get the correct dropdown via connectappuserid
        const select = modal.querySelector('select[name="FormModel.ConnectedAppUserId"]');

        //set the value to nothing
        select.value = "";
        //console.log('Select after reset:', select.value);
        // see if there is a value choosen earlier and set the select dropdown to that value
        const linkedId = button.dataset.connectedappuserid;
        if (linkedId) {
            select.value = linkedId;
            //console.log('Select is set to linkedId:', select.value);
        }



        const editForm = document.querySelector('#add-edit-member-modal form');

        if (editForm) {
            const firstName = editForm.querySelector('input[name="FormModel.FirstName"]');
            const lastName = editForm.querySelector('input[name="FormModel.LastName"]');
            const email = editForm.querySelector('input[name="FormModel.Email"]');

            function validateField(field, type) {
                const span = editForm.querySelector(`span[data-valmsg-for='${field.name}']`);
                if (!span) {
                    console.log(`No span found for ${field.name}`);
                    return true;
                }

                let ok = true;
                let message = '';

                if (type === "text" && !field.value.trim()) {
                    ok = false;
                    message = `${field.previousElementSibling?.textContent} is required.`;
                }

                if (type === "email") {
                    const val = field.value.trim();
                    if (!val) {
                        ok = false;
                        message = "Email is required.";
                    } else if (!/^[^@\s]+@[^@\s]+\.[^@\s]+$/.test(val)) {
                        ok = false;
                        message = "x@x.x";
                    }
                }

                if (!ok) {
                    console.log(`Validation failed for ${field.name}: ${message}`);
                    field.classList.add("input-validation-error");
                    span.textContent = message;
                    span.classList.add("field-validation-error");
                    span.classList.remove("field-validation-valid");
                } else {
                    console.log(`No error for ${field.name}`);
                    field.classList.remove("input-validation-error");
                    span.textContent = "";
                    span.classList.remove("field-validation-error");
                    span.classList.add("field-validation-valid");
                }

                return ok;
            }

            // realtime
            [firstName, lastName, email].forEach(field => {
                field.addEventListener("input", () => {
                    const type = field === email ? "email" : "text";
                    validateField(field, type);
                });
            });

            // on submit
            editForm.addEventListener("submit", e => {
                let valid = true;
                valid &= validateField(firstName, "text");
                valid &= validateField(lastName, "text");
                valid &= validateField(email, "email");

                if (!valid) {
                    console.log("Form has errors. Submission cancelled.");
                    e.preventDefault();
                }
            });
        }
    });
});

