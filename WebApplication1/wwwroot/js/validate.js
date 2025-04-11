//document get access to whole html page.
//.addEventListener is a function that takes 3 arguments
//1 argument is DOMContectLoaded. that wait for the page to finish before starting the script
//2 argument is a function. the function looks in the document for the first <form> and saves it in const form
//if form is empty, return/exis script
//else continue. select all forms that have "input[data-val='true'"
//save them in const fields
//foreach all the items and call each item field and runt it with arrow function
//check all fields and listen for the input and then rund that through a validateField function(send the item)

document.addEventListener("DOMContentLoaded", function () {
    const form = document.querySelector("#signUpForm");
    console.log("1")
    if (!form) return
        
    const fields = form.querySelectorAll("input[data-val='true']")

    console.log("2")
    fields.forEach(field => {
        field.addEventListener("input", function () {
            validateField(field)
        })
    })

    console.log("3")
    //this is the function to validate every field in fields.
    //document searches whole html
    //.queryselector will select the <span> belonging to the field
    //<span> with this attribute [data-valmsg-for for field.name] makes it so the queryselector can handle different names
    function validateField(field) {
        let errorSpan = document.querySelector(`span[data-valmsg-for='${field.name}']`)

        //if no errorSpan. exit js
        if (!errorSpan) return;
        console.log("4")
        //set errormessage to empty
        let errorMessage = ""
        //trims what was put in the input field. removes spaces
        let value = field.value.trim()

        //if the field has the attrubute data-val-required(the field is required) and the value is emptys. continue
        if (field.hasAttribute("data-val-required") && value === "")
            //save the data-val-required in errormessage
            errorMessage = field.getAttribute("data-val-required")

        console.log("5")
            // EMAIL
            //if the field has data-val-regex and value is not empty. continue
        if (field.hasAttribute("data-val-regex") && value !== "") {
            //gets the regex from data annotations in the viewmodel
            //if it does not pass the test, save data-val-regex in errorMessage
            let pattern = new RegExp(field.getAttribute("data-val-regex-pattern"))
            if (!pattern.test(value))
                errorMessage = field.getAttribute("data-val-regex")

        }

        // PASSWORD
        //if field.name is not the same as in html it will still use the dataannotations errors
        if (field.name === "Password" && value !== "") {
            let pattern = /^(?=.*[A-Za-z])(?=.*\d).{8,}$/;
            if (!pattern.test(value))
                errorMessage = "min 6 characters a number"
        }


        console.log("6")

        if (errorMessage) {
            //if errorMessage is true. add this class
            field.classList.add("input-validation-error")

            //remove this class from errorSpan
            errorSpan.classList.remove("field-validation-valid")
            //add this class
            errorSpan.classList.add("field-validation-error")
            //add this error message from annotation that is saved in errorMessage, in to errorSpan.textContent
            errorSpan.textContent = errorMessage
        } else {
            field.classList.remove("input-validation-error")
            errorSpan.classList.remove("field-validation-error")
            errorSpan.classList.add("field-validation-valid")
            errorSpan.textContent = ""
            // do NOT!!! spell classList with small L :P
        }
    }
})