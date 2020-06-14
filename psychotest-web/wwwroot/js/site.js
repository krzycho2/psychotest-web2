// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function check_survey_enable() {
    if ( 
        document.querySelector('input[title=q2]:checked') != null &&
        document.querySelector('input[title=q3]:checked') != null &&
        document.querySelector('input[title=q4]:checked') != null &&
        document.querySelector('input[title=q5]:checked') != null &&
        document.querySelector('input[title=q6]:checked') != null &&
        document.querySelector('input[title=q7]:checked') != null &&
        document.querySelector('input[title=q8]').value != "" &&
        document.querySelector('input[title=q9]:checked') != null) {

        document.getElementById("submit_survey").disabled = false
    }
}



function validate(evt) {
    var theEvent = evt || window.event;

    // Handle paste
    if (theEvent.type === 'paste') {
        key = event.clipboardData.getData('text/plain');
    } else {
        // Handle key press
        var key = theEvent.keyCode || theEvent.which;
        key = String.fromCharCode(key);
    }
    var regex = /[0-9]|\./;
    
    if (!regex.test(key)) {
        theEvent.returnValue = false;
        if (theEvent.preventDefault) theEvent.preventDefault();
    }

}




function quiz_check_enable() {
    console.log("elo")
    var inputs = document.querySelectorAll("input[type=text]")
    for (index = 0; index < inputs.length; index++) {
        if (inputs[index].value == "") {
            document.getElementById("submit_quiz").disabled = true
            return
        }
            
    }
    document.getElementById("submit_quiz").disabled = false
}

function quiz_validate_enable(evt) {
    validate(evt);
    quiz_check_enable();
}