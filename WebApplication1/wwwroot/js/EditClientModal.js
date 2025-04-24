console.log("adding data to client edit 1");
// 🔁 Återanvänd befintlig modalhantering men med data från knappen
document.querySelectorAll('.open-edit-client-modal').forEach(button => {
    button.addEventListener("click", (e) => {
        e.stopPropagation();
        const targetSelector = button.getAttribute("data-target"); // t.ex. "#add-edit-client-modal"
        const modal = document.querySelector(targetSelector);
        if (!modal) return;

        // Stäng alla andra modaler först
        document.querySelectorAll('.modal.modal-show').forEach(m => m.classList.remove('modal-show'));

        // Fyll i data om det finns
        modal.querySelector('input[name="Id"]').value = button.dataset.id;
        modal.querySelector('input[name="ClientName"]').value = button.dataset.name;

        modal.classList.add("modal-show");
    });
});

//find open edt client modal. foreach all buttons, listen for click.
//find add edit client modal and add show
// check data sent from page right before. in editlink. its id and name
// set the value and set the name inside to input field

