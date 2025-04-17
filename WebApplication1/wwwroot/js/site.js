//document represents the whole html document. gives access to all element on the page for modification
//addEventListener is a method  that is used to listen for an achtion on an element
// DOMContentLoaded will wait for the whole page to finish loading before coninuing

document.addEventListener('DOMContentLoaded', () => {
    // Hjälpfunktion för att stänga alla dropdowns utom en
    function closeAllDropdowns(except = null) {
        document.querySelectorAll('.dropdown, .edith-dropdown, #notification-dropdown, #account-dropdown')
            .forEach(dropdown => {
                if (dropdown !== except) {
                    dropdown.classList.remove('show');
                }
            });
    }

    // Nytt: Hindra att klick inne i dropdown stänger den
    document.querySelectorAll('.dropdown, .edith-dropdown, #notification-dropdown, #account-dropdown')
        .forEach(dropdown => {
            dropdown.addEventListener('click', event => {
                event.stopPropagation();
            });
        });

    // Allmän dropdown-öppnare via data-target
    document.querySelectorAll('[data-type="dropdown"]').forEach(button => {
        button.addEventListener("click", (event) => {
            event.stopPropagation(); // Stoppa klick från att bubbla upp
            const targetSelector = button.getAttribute("data-target");
            const dropdown = document.querySelector(targetSelector);
            if (!dropdown) return;

            const isOpen = dropdown.classList.contains("show");
            closeAllDropdowns(); // Stäng andra dropdowns
            if (!isOpen) {
                dropdown.classList.add("show"); // Visa om den inte redan är öppen
            } else {
                dropdown.classList.remove("show"); // Stäng om den var öppen
            }
        });
    });

    // Klick utanför dropdowns stänger allt
    document.addEventListener("click", () => {
        closeAllDropdowns();
    });

    // Bell-notification: ta bort notifikation utan att stänga dropdown
    document.querySelectorAll('.btn-close').forEach(button => {
        button.addEventListener('click', (event) => {
            event.stopPropagation();
            button.closest('.notification')?.remove();
        });
    });

    // Modal: Öppna/stäng med data-type="modal"/"close"
    document.querySelectorAll('[data-type="modal"]').forEach(button => {
        button.addEventListener("click", () => {
            const target = document.querySelector(button.dataset.target);
            target?.classList.add("modal-show");
        });
    });

    document.querySelectorAll('[data-type="close"]').forEach(button => {
        button.addEventListener("click", () => {
            const target = document.querySelector(button.dataset.target);
            target?.classList.remove("modal-show");
        });
    });


//delete button in edit,add member, delete drop down
document.querySelectorAll('.delete-project').forEach(button => {
    button.addEventListener('click', async (e) => {
        e.preventDefault();
        const id = button.getAttribute('data-id');
        })
    });
    // to handle the color of clock depending on the time
    document.querySelectorAll(".badge[data-enddate]").forEach(badge => {
        const endDateStr = badge.dataset.enddate;
        const span = badge.querySelector(".javascript-badge-text");
        console.log("1")
        if (!endDateStr || !span) return;
        console.log("2")
        const endDate = new Date(endDateStr);
        const today = new Date();
        today.setHours(0, 0, 0, 0); // time is set to zeo

        const diffDays = Math.ceil((endDate - today) / (1000 * 60 * 60 * 24));
        console.log("3")
        // remove all the old classes
        badge.classList.remove("badge-red", "badge-danger", "badge-warning", "badge-normal");
        console.log("4")
        // Sätt ny text och färg
        if (diffDays < 0) {
            span.textContent = "Past deadline";
            badge.classList.add("badge-danger");
            console.log("5")
        } else if (diffDays === 0) {
            span.textContent = "Ends today";
            badge.classList.add("badge-red");
            console.log("6")
        } else if (diffDays <= 3) {
            span.textContent = `${diffDays} days left`;
            badge.classList.add("badge-warning");
            console.log("7")
        } else {
            span.textContent = `${Math.floor(diffDays / 7)} weeks left`;
            badge.classList.add("badge-normal");
        }
    });





    // project card. dropdown edit button
    document.querySelectorAll('.open-edit-modal').forEach(button => {
        button.addEventListener('click', e => {

            const image = button.dataset.image;
            if (image) {
                // sätt hidden-fältet
                document.querySelector('#add-edit-project-modal input[name="FormModel.Image"]').value = image;

                // uppdatera preview
                const preview = document.getElementById('image-preview-edit');
                const previewContainer = document.getElementById('image-preview-icon-container-edit');
                const icon = document.getElementById('image-preview-icon-edit');

                preview.src = `/uploads/${image}`;
                preview.classList.remove('hide');
                previewContainer.classList.add('selected');
                icon.classList.replace('fa-camera', 'fa-pen-to-square');
            }


            // Grunder
            const name = button.dataset.name;
            const clientId = button.dataset.client;
            const statusId = button.dataset.status;
            const description = button.dataset.description;
            const budget = button.dataset.budget;
            const start = button.dataset.start;
            const end = button.dataset.end;
            const members = button.dataset.members?.split(',') || [];


            // ID
            const id = button.dataset.id;
            const idInput = document.querySelector('#add-edit-project-modal input[name="FormModel.Id"]');
            if (idInput) idInput.value = id;

            // Project Name
            document.querySelector('#add-edit-project-modal input[name="FormModel.ProjectName"]').value = name;

            // Client
            const clientSelect = document.querySelector('#add-edit-project-modal input[name="FormModel.ClientId"]');
            const clientWrapper = clientSelect.closest('.form-select');
            const clientOption = clientWrapper.querySelector(`.form-select-option[data-value="${clientId}"]`);
            clientSelect.value = clientId;
            clientWrapper.querySelector('.form-select-text').textContent = clientOption?.textContent ?? 'Choose a client';

            // Status
            const statusSelect = document.querySelector('#add-edit-project-modal input[name="FormModel.StatusId"]');
            const statusWrapper = statusSelect.closest('.form-select');
            const statusOption = statusWrapper.querySelector(`.form-select-option[data-value="${statusId}"]`);
            statusSelect.value = statusId;
            statusWrapper.querySelector('.form-select-text').textContent = statusOption?.textContent ?? 'Choose a status';

            // Member (för enkelhet just nu enstaka)
            const memberSelect = document.querySelector('#add-edit-project-modal input[name="FormModel.MemberIds"]');
            const memberWrapper = memberSelect.closest('.form-select');
            if (members.length > 0) {
                memberSelect.value = members[0];
                const firstMember = memberWrapper.querySelector(`.form-select-option[data-value="${members[0]}"]`);
                memberWrapper.querySelector('.form-select-text').textContent = firstMember?.textContent ?? 'Choose a member';
            }

            // Övriga fält
            document.querySelector('#add-edit-project-modal textarea[name="FormModel.Description"]').value = description;
            document.querySelector('#add-edit-project-modal input[name="FormModel.Budget"]').value = budget;
            document.querySelector('#add-edit-project-modal input[name="FormModel.StartDate"]').value = start;
            document.querySelector('#add-edit-project-modal input[name="FormModel.EndDate"]').value = end;
        });
    });



});





    //open modal

    //close modal
    //const closeButtons = document.querySelectorAll('[data-close="true"]')
    //closeButtons.forEach(button => {
    //    button.addEventListener('click', () => {
    //        const modal = button.closest('.modal')
    //        if (modal) {
    //            modal.style.display = 'none'

    //            //clear formdata
    //            modal.querySelectorAll('form').forEach(form => {
    //                form.reset()

    //                const imagePreview = form.querySelector('.image-preview')
    //                if (imagePreview)
    //                    imagePreview.src = ''
    //                const imagePreviewer = form.querySelector('.image-previewer')
    //                if (imagePreviewer)
    //                    imagePreviewer.classList.remove('selected')
    //            })
    //        }
    //    })
    //})
    //close modal
    //const previewSize = 150


    // handle image-previewer
    //document.querySelectorAll('.image-previewer').forEach(previewer => {
    //    const fileInput = previewer.querySelector('input[type="file"]');
    //    const imagePreview = previewer.querySelector('.image-preview');

    //    previewer.addEventListener('click', () => fileInput.click());

    //    fileInput.addEventListener('change', ({ target: { files } }) => {
    //        const file = files[0]
    //        if (file)
    //            processImage(file, imagePreview, previewer, previewSize)

    //    })    

        // handle submit forms
//        const forms = document.querySelectorAll('form')
//        forms.forEach(form => {
//            form.addEventListener('submit', async (e) => {
//                e.preventDefault()

//                clearErrorMessages(form)
//                const formData = new FormData(form)

//                try {
//                    const res = await fetch(form.action, {
//                        method: 'post',
//                        body: formData
//                    })

//                    if (res.ok) {
//                        const modal = form.closest('.modal')
//                        if (modal)
//                            modal.style.display = 'none';

//                        window.location.reload()
//                    }

//                    else if (res.status === 400) {
//                        const data = await res.json()

//                        if (data.errors) {
//                            Object.keys(data.errors).forEach(key => {

//                                let input = form.querySelector(`[name="${key}"]`)
//                                if (input) {
//                                    input.classList.add('input-validation-error')
//                                }

//                                let span = form.querySelector(`[data-valmsg-for="${key}"]`)
//                            if (span) {
//                                span.innerText = data.errors[key].join('\n');
//                                span.classList.add('field-validation-error')
//                            }

//                        })
//        }
//                    }
//                }
//                catch {
//    console.log('error submitting form')
//}
//            })
//        })


//    });
// handle submit forms
//function clearErrorMessages(form) {
//    form.querySelectorAll('[data-val="true"]').forEach(input => {
//        input.classList.remove('input-validation-error')
//    })

//    form.querySelectorAll('[data-valmsg-for]').forEach(span => {
//        span.innerText = ''
//        span.classList.remove('field-validation-error')
//    })
//}


//function addErrorMessages(key, errorMessage) {

//}



// image preview
//async function loadImage(file) {
//    return new Promise((resolve, reject) => {
//        const reader = new FileReader()

//        reader.onerror = () => reject(new Error("Failed to load file."))
//        reader.onload = (e) => {
//            const img = new Image()
//            img.onerror = () => reject(new Error("failed to load image"))
//            img.onload = () => resolve(img)
//            img.src = e.target.result
//        }
//        reader.readAsDataURL(file)
//    })
//}


//async function processImage(file, imagePreview, previewer, previewSize = 150) {
//    try {
//        const img = await loadImage(file)
//        const canvas = document.createElement('canvas')
//        canvas.width = previewSize
//        canvas.height = previewSize

//        const ctx = canvas.getContext('2d')
//        ctx.drawImage(img, 0, 0, previewSize, previewSize)
//        imagePreview.src = canvas.toDataURL('image/jpeg')
//        previewer.classList.add('selected')
//    }
//    catch (error) {
//        console.error('failed on image-processing:')
//    }
//}

//})