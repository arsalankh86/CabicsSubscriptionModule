$(document).ready(function () {


    var account = GetParameterValues('id');

    function GetParameterValues(param) {
        var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < url.length; i++) {
            var urlparam = url[i].split('=');
            if (urlparam[0] == param) {
                return urlparam[1];
            }
        }
    }  

    GetUserAllSubscriptionDetail(account);

    function GetUserAllSubscriptionDetail(account) {

        $.ajax({
            type: "Get",
            url: servicePath + "/Subscription/GetUserAllSubscriptionDetail",
            //data: JSON.stringify(account),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                console.log(data);

               
                var html = '<table border="1"><tr><td>Plan Name</td><td>Subscription Type</td><td>Subscription Price</td><td>No Of Sms Purchase</td><td>SMS Price</td><td>TotalPrice</td>';
                    html += '<td>TotalCredit</td><td>RemainingCredit</td><td>StartDate</td><td>EndDate</td><td>NoOfAgents</td><td>NoOfDrivers</td>';
                    html += '<td>NoOfVehicles</td><td>RemainingNoOfAgents</td><td>RemainingNoOfDrivers</td><td>RemainingNoOfVehicles</td><td>PerSMSPrice</td><td>ChequeNo</td><td>Created Date</td><td>Ref</td></tr>';


                    $.each(data, function (index, value) {
                        console.log(value);

                        var subscriptionname = "";
                        if (value.SubscriptionTypeId == 1)
                            subscriptionname = "Pay as you go";
                        else if (value.SubscriptionTypeId == 2)
                            subscriptionname = "Monthly";

                        html += '<tr><td>' + value.PlanName + '</td><td>' + subscriptionname + '</td><td>' + value.SubscriptionPrice + '</td><td>' + value.NoOfSmsPurchase + '</td><td>' + value.SMSPrice + '</td><td>';
                        html += '<tr><td>' + value.TotalPrice + '</td><td>' + value.TotalCredit + '</td><td>' + value.RemainingCredit + '</td><td>' + value.StartDate + '</td><td>' + value.EndDate + '</td><td>';
                        html += '<tr><td>' + value.NoOfAgents + '</td><td>' + value.NoOfDrivers + '</td><td>' + value.NoOfVehicles + '</td><td>' + value.RemainingNoOfAgents + '</td><td>' + value.RemainingNoOfDrivers + '</td><td>';
                        html += '<tr><td>' + value.RemainingNoOfVehicles + '</td><td>' + value.PerSMSPrice + '</td><td>' + value.ChequeNo + '</td><td>' + value.CreatedDateTime  +
                            '<a data-TransactionId ="' + value.BtTransactionId +
                            '" onclick="RefudSubscription(this)">Refund Subscription(S)</a>'
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

    RefudSubscription = function (obj) {

        var TransactionId = $(obj).attr('data-TransactionId');

        refundRequest = {
            PlanId: TransactionId,
            Amount: 0
        };

        $.ajax({
            type: "POST",
            url: servicePath + "/Subscription/RefundSubscription",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(refundRequest),
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
                

        return false;


    }

});
