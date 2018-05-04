var servicePath = "http://localhost:54353/api";
var webUrl = "http://localhost:32625";



function ValidateControl(obj) {
    try {
        if (obj.val().length == 0 || obj.val() == "null" || obj.val() == "0" || obj.val() == "") {
            obj.css("border", "2px solid red");
            obj.focus();
            return false;
        }
        else
            obj.css("border", "");
    } catch (e) {
        alert(e.Message);
    }
}


function ValidateControlForLength(obj) {
    try {
        if (obj.val().length < 2 || obj.val().length >= 50) {
            obj.css("border", "2px solid red");
            obj.focus();
            alert("");
            return false;
        }
        else
            obj.css("border", "");
    } catch (e) {
        alert(e.Message);
    }
}


function ValidateControlWithZeroValue(obj) {
    try {
        if (obj.val() == "-1" || obj.val() == "null") {
            obj.css("border", "2px solid red");
            obj.focus();
            return false;
        }
        else
            obj.css("border", "");
    } catch (e) {
        alert(e.Message);
    }
}


function ValidateControlForLetters(control) {
    try {
        var pattern = new RegExp("^-*[0-9,\.]+$");
        if (pattern.test($(control).val()) == false) {
            //ErrorMsgTimeOut('Only Letters Are Allowed');
            alert('Only Letters Are Allowed');
            obj.css("border", "2px solid red");
            $(control).focus();
            return false;
        }
    } catch (e) {
        alert(e.Message);
    }
}

function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode != 44 && charCode > 32 && (charCode < 48 || charCode > 57)) {
        return false;
    } else {
        return true;
    }
}


function ValidateControlForEmail(control) {
    var emailRegex = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    if (emailRegex.test($(control).val()) == false) {
        console.log(emailRegex);
        alert("Please Input valid Email");
        obj.css("border", "2px solid red");
        $(control).focus();
        return false;
    }
}

function ValidateControlForDigit(control) {
    try {
        var pattern = new RegExp("[0-9]");
        if (pattern.test($(control).val()) == false) {
            //ErrorMsgTimeOut('Only Digits Are Allowed');
            alert("Only Digits Are Allowed");
            obj.css("border", "2px solid red");
            $(control).focus();
            return false;
        }
    } catch (e) {
        alert(e.Message)
    }
}

function ErrorMsgTimeOut(Msg) {
    try {
        $("div.error").fadeIn();
        $("div.error").html(Msg)
        setTimeout(function () { $('.error').fadeOut('slow'); }, 5000);
    } catch (e) {
        alert(e.Message)
    }

}





