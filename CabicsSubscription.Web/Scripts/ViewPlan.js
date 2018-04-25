$(document).ready(function () {


    var account = GetParameterValues('data');

    localStorage.setItem("accounttoken", account);

    function GetParameterValues(param) {
        var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < url.length; i++) {
            var urlparam = url[i].split('=');
            if (urlparam[0] == param) {
                return urlparam[1];
            }
        }
    }  

    FetchPlanForView();

    function FetchPlanForView(account) {

        $.ajax({
            type: "Get",
            url: servicePath + "/Plan/GetAllPlan",
            //data: JSON.stringify(account),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                console.log(data);
                var html = '<table><tr><td>Plan Name</td><td>Plan Code</td><td>Description</td><td>Amount</td><td>Credit</td></tr>'

                $.each(data, function (index, value) {
                    console.log(value);
                    html += '<tr><td>' + value.Name + '</td><td>' + value.PlanCode + '</td><td></td><td></td><td></td></tr></html>';

                });
                console.log(html);

                $("dvplan").innerhtml(html);

               // window.location.href = webUrl + "/Home/Index?data=" + data;


            },
            failure: function (errMsg) {
                alert(errMsg);
            }
        });

    }

});
