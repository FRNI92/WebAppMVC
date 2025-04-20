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

                // update preview
                const preview = document.getElementById('image-preview-edit');
                const previewContainer = document.getElementById('image-preview-icon-container-edit');
                const icon = document.getElementById('image-preview-icon-edit');

                preview.src = `/uploads/${image}`;
                preview.classList.remove('hide');
                previewContainer.classList.add('selected');
                icon.classList.replace('fa-camera', 'fa-pen-to-square');
            }


            // basic
            const name = button.dataset.name;
            const clientId = button.dataset.client;
            const statusId = button.dataset.status;
            const description = button.dataset.description;
            const budget = button.dataset.budget;
            const start = button.dataset.start;
            const end = button.dataset.end;
            const members = button.dataset.members?.split(',') || [];


            // id
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

            // Member (need rewrite when adding more than 1 member)
            const memberSelect = document.querySelector('#add-edit-project-modal input[name="FormModel.MemberIds"]');
            const memberWrapper = memberSelect.closest('.form-select');
            if (members.length > 0) {
                memberSelect.value = members[0];
                const firstMember = memberWrapper.querySelector(`.form-select-option[data-value="${members[0]}"]`);
                memberWrapper.querySelector('.form-select-text').textContent = firstMember?.textContent ?? 'Choose a member';
            }

            // the rest of basic fields
            document.querySelector('#add-edit-project-modal textarea[name="FormModel.Description"]').value = description;
            document.querySelector('#add-edit-project-modal input[name="FormModel.Budget"]').value = budget;
            document.querySelector('#add-edit-project-modal input[name="FormModel.StartDate"]').value = start;
            document.querySelector('#add-edit-project-modal input[name="FormModel.EndDate"]').value = end;
        });
    });




    //Members minimenu

     //Member card: dropdown edit button
    document.querySelectorAll('.open-edit-member-modal').forEach(button => {
        button.addEventListener('click', e => {
            const id = button.dataset.id;
            const firstName = button.dataset.firstname;
            const lastName = button.dataset.lastname;
            const role = button.dataset.role;
            const email = button.dataset.email;
            const phone = button.dataset.phone;

            const modal = document.querySelector('#add-edit-member-modal');
            modal?.classList.add('modal-show');

            modal.querySelector('input[name="FormModel.Id"]').value = id;
            modal.querySelector('input[name="FormModel.FirstName"]').value = firstName;
            modal.querySelector('input[name="FormModel.LastName"]').value = lastName;
            modal.querySelector('input[name="FormModel.JobTitle"]').value = role;
            modal.querySelector('input[name="FormModel.Phone"]').value = phone;
            modal.querySelector('input[name="FormModel.Email"]').value = email;

        });
    });

});

