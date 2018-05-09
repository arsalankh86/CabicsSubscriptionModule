
$(document).ready(function () {
    var account = { "Email": "admin@vs.com", "Name": "VSAdmin", "ClientId": "1" };

    $.ajax({
        type: "POST",
        url: servicePath +"/Account/GetAccountToken",
        data: JSON.stringify(account),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {


            window.location.href = webUrl + "/Admin/ViewPlan?data="+data;


        },
        failure: function (errMsg) {
            alert(errMsg);
        }
    });
});
