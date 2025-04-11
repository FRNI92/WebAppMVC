document.addEventListener("DOMContentLoaded", () => {
    const form = document.querySelector(".signin-form");
    if (!form) return;

    const inputGroups = form.querySelectorAll(".input-group");

    inputGroups.forEach(group => {
        const input = group.querySelector("input");
        const span = group.querySelector("span");

        input.addEventListener("input", () => {
            const value = input.value.trim();
            let errorMessage = "";

            if (input.hasAttribute("data-val-required") && value === "") {
                errorMessage = input.getAttribute("data-val-required");
            }

            if (input.name === "Password" && value !== "") {
                const pattern = /^(?=.*[A-Za-z])(?=.*\d).{8,}$/;
                if (!pattern.test(value))
                    errorMessage = "Weakness!";
            }

            if (errorMessage) {
                input.classList.add("input-validation-error");
                span.classList.add("field-validation-error");
                span.classList.remove("field-validation-valid");
                span.textContent = errorMessage;
            } else {
                input.classList.remove("input-validation-error");
                span.classList.remove("field-validation-error");
                span.classList.add("field-validation-valid");
                span.textContent = "";
            }
        });
    });
});