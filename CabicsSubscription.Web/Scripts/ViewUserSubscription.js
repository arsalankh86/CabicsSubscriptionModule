$(document).ready(function () {


    var id = GetParameterValues('id');
    var email = GetParameterValues('email');

    $("#dvcaboffice").append(email);

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

               
                var html = '<table class="table table-bordered table-striped">';
                html += '<thead><tr>';
                html += '<td> Plan Name</td >';
                html += '<td>Subscription Type</td>';
                html += '<td>No Of Sms Purchase</td>';
                html += '<td>SMS Price</td>';
                html += '<td>TotalPrice</td>';
                html += '<td>TotalCredit</td>';
                html += '<td>RemainingCredit</td>';
                html += '<td>StartDate</td>';
                html += '<td>EndDate</td>';
                html += '<td>NoOfAgents</td>';
                html += '<td>NoOfDrivers</td>';
                html += '<td>NoOfVehicles</td>';
                html += '<td>RemainingNoOfAgents</td>';
                html += '<td>RemainingNoOfDrivers</td>';
                html += '<td>RemainingNoOfVehicles</td>';
                html += '<td>PerSMSPrice</td>';
                html += '<td>ChequeNo</td>';
                html += '<td>PaymentTransactionId</td>';
                html += '<td>Payment Status</td>'
                html += '<td>Created Date</td>';
                html += '<td>Refund</td>'
                html += '<td>CreditUtilizationReport</td>'
                html += '</tr></thead>';


                    $.each(data, function (index, value) {
                        console.log(value);


                        var subscriptionname = "";
                        if (value.SubscriptionTypeId == 1)
                            subscriptionname = "Pay as you go";
                        else if (value.SubscriptionTypeId == 2)
                            subscriptionname = "Monthly";

                        html += '<tbody><tr>';
                        html += '<td> ' + value.PlanName + '</td>';
                        html += '<td>' + subscriptionname + '</td>';
                        html += '<td> ' + value.NoOfSmsPurchase + '</td >';
                        html += '<td> ' + value.SMSPrice + '</td >';
                        html += '<td>' + value.TotalPrice + '</td>';
                        html += '<td>' + value.TotalCredit + '</td>';
                        html += '<td>' + value.RemainingCredit + '</td>';
                        html += '<td>' + value.StartDate + '</td>';
                        html += '<td>' + value.EndDate + '</td>';
                        html += '<td>' + value.NoOfAgents + '</td>';
                        html += '<td> ' + value.NoOfDrivers + '</td >';
                        html += '<td>' + value.NoOfVehicles + '</td>';
                        html += '<td>' + value.RemainingNoOfAgents + '</td>';
                        html += '<td>' + value.RemainingNoOfDrivers + '</td>';
                        html += '<td>' + value.RemainingNoOfVehicles + '</td>';
                        html += '<td>' + value.PerSMSPrice + '</td>';						 
                        html += '<td> ' + value.ChequeNo + '</td >';
                        html += '<td> ' + value.btTransactionId + '</td >';
                        html += '<td>' + value.Status + '</td>';
                        html += '<td>' + value.CreatedDateTime + '</td>';
                       
                        if (value.ChequeNo == "") {
                            html += '<td><a data-TransactionId="' + value.btTransactionId + '" onclick="RefudSubscription(this)">Refund Subscription(S)</a></td >';
                        }
                        else {
                            html += '<td>Refund Not Available</td >';
                        }
                        html += '<td><a data-accountid="' + id + '" data-subscriptionid= "' + value.Id + '" onclick="CreditUtilizationReport(this)" >Credit Utilization Report</a></td >';
                        html += '</tr > </tbody> ';

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

    CreditUtilizationReport = function (obj) {

        var accountId = $(obj).attr('data-accountid');
        var subscriptionId = $(obj).attr('data-subscriptionid');

        window.location.href = webUrl + "/Admin/CreditUtilizationReport?accountId=" + accountId + "&subscriptionId=" + subscriptionId;

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
