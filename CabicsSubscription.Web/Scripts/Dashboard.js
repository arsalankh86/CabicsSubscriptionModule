
$(document).ready(function () {
    var account = { "Email": "abcds@sadsa.com", "Name": "dfdfdsfff", "ClientId": "1" };

    $.ajax({
        type: "POST",
        url: servicePath +"/Account/GetAccountToken",
        data: JSON.stringify(account),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {


            window.location.href = webUrl + "/Home/ViewPlan?data="+data;


        },
        failure: function (errMsg) {
            alert(errMsg);
        }
    });
});
