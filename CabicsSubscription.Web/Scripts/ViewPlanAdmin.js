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

    FetchPlanForView(account);

    function FetchPlanForView(account) {

        $.ajax({
            type: "Get",
            url: servicePath + "/Plan/GetAllPlan",
            //data: JSON.stringify(account),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                console.log(data);
                var html = '<table border="1"><tr><td>Plan Name</td><td>Plan Code</td><td>Description</td><td>Amount</td><td>Credit</td><td>Delete</td><td>Purchase For Cab Office</td></tr>'

                $.each(data, function (index, value) {
                    console.log(value);
                    html += '<tr><td>' + value.Name + '</td><td>' + value.PlanCode + '</td><td>' + value.Description + '</td><td>' + value.CreditPrice + '</td><td>' + value.Credit + '</td><td>'
                    +
                        '<a data-PlanId ="' + value.Id +
                        '" data-AccountId="' + account +
                        '" onclick="DeletePlan(this)">Delete Plan(S)</a>'
                    +
                        '</td ><td>'
                        +
                        '<a data-PlanId ="' + value.Id +
                        '" onclick="PurchseForCabOffice(this)">Purchase Plan(S)</a>'
                        +
                    '</td ></tr > ';

                });
                console.log(html);

                html += '</table>';

                $("#dvplan").append(html);
                $("#divLoader").css("display", "none");

               // window.location.href = webUrl + "/Home/Index?data=" + data;


            },
            failure: function (errMsg) {
                alert(errMsg);
            }
        });

    }

    ShowDetail = function (obj) {

        var PlanId = $(obj).attr('data-PlanId');
        var accountguid = $(obj).attr('data-AccountId');

        window.open('/Home/PurchaseSubscription?id=' + PlanId + '/' + accountguid);

        return false;


    }

    PurchseForCabOffice = function (obj) {

        var PlanId = $(obj).attr('data-PlanId');

        window.open('/Admin/AddSubscriptionByAdmin?id=' + PlanId );

        return false;


    }


    DeletePlan = function (obj) {

        var PlanId = $(obj).attr('data-PlanId');



        plan = {

            PlanId: PlanId
        };

        $.ajax({
            type: "POST",
            url: servicePath + "/Plan/DeletePlan",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(plan),
            dataType: "json",
            complete: function () {
            },

            success: function (response) {
                alert(response);
            },

            failure: function (response) {
                alert(response);
            }
        });


        location.reload();

        return false;


    }


});
