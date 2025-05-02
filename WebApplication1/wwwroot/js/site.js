//document represents the whole html document. gives access to all element on the page for modification
//addEventListener is a method  that is used to listen for an achtion on an element
// DOMContentLoaded will wait for the whole page to finish loading before coninuing

document.addEventListener('DOMContentLoaded', () => {
    // Hjälpfunktion för att stänga alla dropdowns utom en
    function closeAllDropdowns(except = null) {
        document.querySelectorAll('.dropdown, .edith-dropdown, #notification-dropdown, #account-dropdown, .client-edith-dropdown')
            .forEach(dropdown => {
                if (dropdown !== except) {
                    dropdown.classList.remove('show');
                }
            });
    }

    // prevent click inside modal to close it
    document.querySelectorAll('.dropdown, .edith-dropdown, #notification-dropdown, #account-dropdown')
        .forEach(dropdown => {
            dropdown.addEventListener('click', event => {
                event.stopPropagation();
            });
        });

    // modal opener
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

    // click outside should close all modals
    document.addEventListener("click", () => {
        closeAllDropdowns();
    });

    // remover notification without closing dropdown
    document.querySelectorAll('.btn-close').forEach(button => {
        button.addEventListener('click', (event) => {
            event.stopPropagation();
            button.closest('.notification')?.remove();
        });
    });



    // open/close modat of type modal
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
            console.log("opening edit modal");
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

            console.log("fetched from database");
            console.log({name, clientId, statusId, description, budget, start, end, members});

            //console.log('this is the members', members);
            //document.querySelectorAll('#member-select .form-select-option.selected-option')
            //    .forEach(option => option.classList.remove('selected-option'));
            //members.forEach(id => {
            //    console.log(`${id}`)
            //    console.log("im giving member select class to the member id that was chosen when creating project")
            //    const option = document.querySelector(`#member-select .form-select-option[data-value="${id}"]`);
            //    if (option) {
            //        option.classList.add("selected-option");
            //    }
            //});



            // so that I cant see what members are already chosen
            //const selectedContainer = document.querySelector('#selected-members-container-edit');
            //if (selectedContainer) {
            //    selectedContainer.innerHTML = "";

                // Ta bort endast selected-option från member-listan i edit
            //    document.squerySelectorAll('#add-edit-project-modal #member-select .form-select-option.selected-option')
            //        .forEach(option => option.classList.remove('selected-option'));

            //    // Lägg till inputs och markera valda
            //    members.forEach((id, index) => {
            //        const option = document.querySelector(`#add-edit-project-modal #member-select .form-select-option[data-value="${id}"]`);
            //        if (option) {
            //            option.classList.add('selected-option');

            //            const input = document.createElement("input");
            //            input.type = "hidden";
            //            input.name = `FormModel.MemberIds[${index}]`;
            //            input.value = id;
            //            selectedContainer.appendChild(input);
            //        }
            //    });

            //    // update the text in dropdonw
            //    const triggerText = document.querySelector('#add-edit-project-modal #member-select .form-select-text');
            //    if (triggerText) {
            //        triggerText.textContent = members.length > 0
            //            ? `${members.length} member${members.length > 1 ? "s" : ""} selected`
            //            : "Choose a member";
            //    }
            //}



            // id
            const id = button.dataset.id;
            const idInput = document.querySelector('#add-edit-project-modal input[name="FormModel.Id"]');
            if (idInput)
            {
                idInput.value = id;
                console.log(`id set to ${id}`);
            } 

            // Project Name
            document.querySelector('#add-edit-project-modal input[name="FormModel.ProjectName"]').value = name;
            console.log(`project name is set to: ${name}`)


            // Client. 
            const clientSelect = document.querySelector('#add-edit-project-modal input[name="FormModel.ClientId"]');
            console.log(`found the cllientselect: ${clientSelect.value}`)
            const clientWrapper = clientSelect.closest('.form-select');
            console.log(`found the clientWrapper:  ${clientWrapper.innerHTML}`)
            const clientOption = clientWrapper.querySelector(`.form-select-option[data-value="${clientId}"]`);
            console.log(`searching fo options with client id:${clientId}`);
            clientSelect.value = clientId;
            console.log(`sets clientselect.value:${clientSelect.value}`);


            const clientTextElement = clientWrapper.querySelector('.form-select-text');
            if (clientTextElement) {
                clientTextElement.textContent = clientOption?.textContent ?? 'Choose a client';
                console.log(`shows clientname: ${clientTextElement.textContent}`);// needs to get the data before I cant consol.log it
            } else {
                console.log("could not find form select for client")
            }


            // Status
            const statusSelect = document.querySelector('#add-edit-project-modal input[name="FormModel.StatusId"]');
            const statusWrapper = statusSelect.closest('.form-select');
            const statusOption = statusWrapper.querySelector(`.form-select-option[data-value="${statusId}"]`);
            statusSelect.value = statusId;
            statusWrapper.querySelector('.form-select-text').textContent = statusOption?.textContent ?? 'Choose a status';

            // Member (need rewrite when adding more than 1 member)
            // Lägg till inputs och markera valda
            const selectedContainer = document.querySelector('#selected-members-container-edit');
            if (selectedContainer) {
                selectedContainer.innerHTML = "";

                document.querySelectorAll('#add-edit-project-modal #member-select .form-select-option.selected-option')
                    .forEach(option => option.classList.remove('selected-option'));

                members.forEach((id, index) => {
                    const option = document.querySelector(`#add-edit-project-modal #member-select .form-select-option[data-value="${id}"]`);
                    if (option) {
                        option.classList.add('selected-option');

                        const input = document.createElement("input");
                        input.type = "hidden";
                        input.name = `FormModel.MemberIds[${index}]`;
                        input.value = id;
                        selectedContainer.appendChild(input);
                    }
                });

                const triggerText = document.querySelector('#add-edit-project-modal #member-select .form-select-text');
                if (triggerText) {
                    triggerText.textContent = members.length > 0
                        ? `${members.length} member${members.length > 1 ? "s" : ""} selected`
                        : "Choose a member";
                }
            }

            // the rest of basic fields
            document.querySelector('#add-edit-project-modal textarea[name="FormModel.Description"]').value = description;
            document.querySelector('#add-edit-project-modal input[name="FormModel.Budget"]').value = budget;
            document.querySelector('#add-edit-project-modal input[name="FormModel.StartDate"]').value = start;
            document.querySelector('#add-edit-project-modal input[name="FormModel.EndDate"]').value = end;
        });
    });
});








//darkmode
const toggle = document.getElementById("mini-menu-dark-mode-toggle");

if (localStorage.getItem("darkMode") === "true") {
    toggle.checked = true;
    document.body.classList.add("dark-theme");
}

toggle.addEventListener("change", async () => {
    const isDark = toggle.checked;
    document.body.classList.toggle("dark-theme", isDark);
    localStorage.setItem("darkMode", isDark);

    await setDarkModeCookie(isDark); 
});


// this part updates message count
document.querySelectorAll('.notifications .btn-close').forEach(button => {
    button.addEventListener('click', event => {
        event.stopPropagation();

        const notification = button.closest('.notification-item');
        if (notification) {
            notification.remove();
            updateNotificationCount(); // <-- UPPDATERA siffran DIREKT
        }
    });
});


//handle the time when messages was added
function updateRelativeTimes() {
    const elements = document.querySelectorAll('.notification .time');
    const now = new Date();

    elements.forEach(el => {
        const created = new Date(el.getAttribute('data-created'));
        const diff = now - created;

        const diffSeconds = Math.floor(diff / 1000);
        const diffMinutes = Math.floor(diffSeconds / 60);
        const diffHours = Math.floor(diffMinutes / 60);
        const diffDays = Math.floor(diffHours / 24);
        const diffWeeks = Math.floor(diffDays / 7);

        let relativeTime = '';

        if (diffMinutes < 1) {
            relativeTime = 'Just now';
        } else if (diffMinutes < 60) {
            relativeTime = `${diffMinutes} min ago`;
        } else if (diffHours < 24) {
            relativeTime = `${diffHours} hours ago`;
        } else if (diffDays < 7) {
            relativeTime = `${diffDays} days ago`;
        } else {
            relativeTime = `${diffWeeks} weeks ago`;
        }

        el.textContent = relativeTime;
    });
}

