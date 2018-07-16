
$(document).ready(function () {

    var email = GetParameterValues('email');
    var firstname = GetParameterValues('firstname');
    var lastname = GetParameterValues('lastname');
    var cabicsSystemId = GetParameterValues('id');

    function GetParameterValues(param) {
        var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < url.length; i++) {
            var urlparam = url[i].split('=');
            if (urlparam[0] == param) {
                return urlparam[1];
            }
        }
    }  

    if ((email != undefined && firstname != undefined && email != "" && firstname != "")) {
        var account = { "Email": email, "Name": firstname + lastname, "ClientId": "1", "CabicsSystemId": cabicsSystemId };

        $.ajax({
            type: "POST",
            url: servicePath + "/Account/GetAccountToken",
            data: JSON.stringify(account),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                localStorage.setItem("accounttoken", data);
                window.location.href = webUrl + "/Home/Dashboard?data=" + data;
            },
            failure: function (errMsg) {
                alert(errMsg);
            }
        });
    }
});
