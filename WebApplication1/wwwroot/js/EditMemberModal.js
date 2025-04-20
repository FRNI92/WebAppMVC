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

        //editModal.querySelector('input[name="FormModel.Id"]').value = button.dataset.id;
        //editModal.querySelector('input[name="FormModel.FirstName"]').value = button.dataset.firstname;
        //editModal.querySelector('input[name="FormModel.LastName"]').value = button.dataset.lastname;
        //editModal.querySelector('input[name="FormModel.JobTitle"]').value = button.dataset.role;
        //editModal.querySelector('input[name="FormModel.Email"]').value = button.dataset.email;
        //editModal.querySelector('input[name="FormModel.Phone"]').value = button.dataset.phone;
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