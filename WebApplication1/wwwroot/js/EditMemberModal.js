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

    });
});