$(document).ready(function () {


    var id = GetParameterValues('id');

    function GetParameterValues(param) {
        var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < url.length; i++) {
            var urlparam = url[i].split('=');
            if (urlparam[0] == param) {
                return urlparam[1];
            }
        }
    }  

    GetUserAllSubscriptionDetail(id);

    function GetUserAllSubscriptionDetail(id) {

        $.ajax({
            type: "Get",
            url: servicePath + "/Subscription/GetUserAllSubscriptionDetail?cabOfficeId="+id,
            //data: JSON.stringify(account),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                console.log(data);

               
                var html = '<table border="1"><tr><td>Plan Name</td><td>Subscription Type</td><td>No Of Sms Purchase</td><td>SMS Price</td><td>TotalPrice</td>';
                    html += '<td>TotalCredit</td><td>RemainingCredit</td><td>StartDate</td><td>EndDate</td><td>NoOfAgents</td><td>NoOfDrivers</td>';
                    html += '<td>NoOfVehicles</td><td>RemainingNoOfAgents</td><td>RemainingNoOfDrivers</td><td>RemainingNoOfVehicles</td><td>PerSMSPrice</td><td>ChequeNo</td><td>Created Date</td><td>Refund</td></tr>';


                    $.each(data, function (index, value) {
                        console.log(value);

                        var subscriptionname = "";
                        if (value.SubscriptionTypeId == 1)
                            subscriptionname = "Pay as you go";
                        else if (value.SubscriptionTypeId == 2)
                            subscriptionname = "Monthly";

                        html += '<tr><td>' + value.PlanName + '</td><td>' + subscriptionname + '</td><td>' + value.NoOfSmsPurchase + '</td><td>' + value.SMSPrice + '</td><td>';
                        html += '<td>' + value.TotalPrice + '</td><td>' + value.TotalCredit + '</td><td>' + value.RemainingCredit + '</td><td>' + value.StartDate + '</td><td>' + value.EndDate + '</td><td>';
                        html += '<td>' + value.NoOfAgents + '</td><td>' + value.NoOfDrivers + '</td><td>' + value.NoOfVehicles + '</td><td>' + value.RemainingNoOfAgents + '</td><td>' + value.RemainingNoOfDrivers + '</td><td>';
                        html += '<td>' + value.RemainingNoOfVehicles + '</td><td>' + value.PerSMSPrice + '</td><td>' + value.ChequeNo + '</td><td>' + value.CreatedDateTime + '</td><td><a data-TransactionId ="' + value.btTransactionId + '" onclick="RefudSubscription(this)">Refund Subscription(S)</a></td ></tr > ';

                    });


                    console.log(html);
                html += '</table>';

                $("#dvusrsubs").append(html);
                $("#divLoader").css("display", "none");

               // window.location.href = webUrl + "/Home/Index?data=" + data;


            },
            failure: function (errMsg) {
                alert(errMsg);
            }
        });

    }

    RefudSubscription = function (obj) {

        var transactionId = $(obj).attr('data-TransactionId');

        window.location.href = webUrl + "/Admin/RefundRequest?tid=" + transactionId;

        return false;

    }

    RefudSubscription_ = function (obj) {

        var transactionId = $(obj).attr('data-TransactionId');

        refundRequest = {
            TransactionId: transactionId,
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
