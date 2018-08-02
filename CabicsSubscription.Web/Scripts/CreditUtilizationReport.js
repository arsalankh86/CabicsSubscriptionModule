$(document).ready(function () {


    var accountId = GetParameterValues('accountId');
    var subscriptionId = GetParameterValues('subscriptionId');


    function GetParameterValues(param) {
        var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < url.length; i++) {
            var urlparam = url[i].split('=');
            if (urlparam[0] == param) {
                return urlparam[1];
            }
        }
    }

    GetCredititUtilizationReport(accountId, subscriptionId);

    function GetCredititUtilizationReport(accountId, subscriptionId) {

        $.ajax({
            type: "Get",
            url: servicePath + "/Subscription/GetCredititUtilizationReport?cabOfficeId=" + accountId + "&subscriptionId=" + subscriptionId,
            //data: JSON.stringify(account),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                console.log(data);


                var html = '<table class="table table-bordered table-striped">';
                html += '<thead><tr>';
                html += '<td> Cab Office Name</td >';
                //html += '<td>Subscription Type</td>';
                html += '<td>Plan Name</td>';
                html += '<td>Credit Deduction Type</td>';
                html += '<td>No of Credit Used</td>';
                html += '<td>Job Id</td>';               
                html += '<td>Created Date</td>';               
                html += '</tr></thead>';


                $.each(data, function (index, value) {
                    console.log(value);

                    var date = value.CreatedDate.split('T')[0];

                 

                    html += '<tbody><tr>';
                    html += '<td> ' + value.FullName + '</td>';
                    //html += '<td>' + value.SubscriptionType + '</td>';
                    html += '<td> ' + value.PlanName + '</td >';
                    html += '<td> ' + value.Name + '</td >';
                    html += '<td>' + value.Credits + '</td>';
                    html += '<td>' + value.JobId + '</td>';
                    html += '<td>' + formatDate(date) + '</td>';              
                    html += '</tr > </tbody> ';

                });


                console.log(html);
                html += '</table>';

                $("#dvrpt").append(html);
                $("#divLoader").css("display", "none");

                // window.location.href = webUrl + "/Home/Index?data=" + data;


            },
            failure: function (errMsg) {
                alert(errMsg);
            }
        });

    }

    function formatDate(date) {
        var d = new Date(date),
            month = '' + (d.getMonth() + 1),
            day = '' + d.getDate(),
            year = d.getFullYear();

        if (month.length < 2) month = '0' + month;
        if (day.length < 2) day = '0' + day;

        return [year, month, day].join('-');
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
