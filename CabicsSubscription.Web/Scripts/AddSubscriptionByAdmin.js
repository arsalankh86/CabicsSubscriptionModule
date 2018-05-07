
var addsubscriptionbyadmin = new function () {


    var ddlcaboofice = "#ddlcaboofice";
    var dvpayasyougo = "#dvpayasyougo";
    var chqueno = "#chqueno";
    var totalamount = "#totalamount";
    var lblprice = "#lblprice";
    var qty = "#qty";
    var btnsubmut = "#btnsubmut";
    var dvmonthly = "#dvmonthly";
    var plantype = "#plantype";

    $(qty).bind('input', function () {
        /* This will be fired every time, when textbox's value changes. */
        var credit = $(lblprice).text();
        $("#totalamount").val(this.value * credit);
        //alert(this.value);
    });


    var planid = GetParameterValues('id');
    //var myDropDownList = $('.myDropDownLisTId');

    // Load all cab office
    $.ajax({
        type: "GET",
        url: servicePath + "/Account/GetCabOffice",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            $(response).each(function (index) {
                var OptionValue = response[index].Id;
                var OptionText = response[index].FullName;
                var option = $("<option>" + OptionText + "</option>");
                option.attr("value", OptionValue);

                $(ddlcaboofice).append(option);
            });
        },
        failure: function () {
            alert("Failed!");
        }
    });


    // Load Plan Detail

    $.ajax({
        type: "GET",
        url: servicePath + "/Plan/GetPlanDetail?planId=" + planid,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            $(plantype).val(response.PlanTypeId);
            if (response.PlanTypeId == 1) {
                $(dvpayasyougo).css('display', 'block');
                $(lblprice).text(response.CreditPrice);
                var html = 'Buy Credit (' + response.Credit + ' Credit = £' + response.CreditPrice + ')';
                var price = response.CreditPrice;
            }
            else if (response.PlanTypeId == 2) {

                $(totalamount).val(response.CreditPrice);
             

                if (response.PlanTypeId == 2) {
                    var monthlyplandetail = 'No of Agents:' + response.NoOfAgents;
                    monthlyplandetail += '<br /> No of Driver:' + response.NoOfDrivers;
                    monthlyplandetail += '<br/> No of Vehicles:' + response.NoOfVehicles;
                    monthlyplandetail += '<br/> Price Per SMS:' + response.PerSMSPrice;

                    $(dvmonthly).append(monthlyplandetail);
                    $(totalamount).val(response.CreditPrice)
                    $(qty).css('display', 'none');

                }

                $(dvbuycredit).text(html);
                $(lblprice).text(price);
            }

        },
        failure: function () {
            alert("Failed!");
        }
    });

    function GetParameterValues(param) {
        var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < url.length; i++) {
            var urlparam = url[i].split('=');
            if (urlparam[0] == param) {
                return urlparam[1];
            }
        }
    } 

    $(btnsubmut).click(function () {

        if (ValidateControl($(chqueno)) == false)
            return false;
        //if ($(plantype).val() == 2) {
        //    if (ValidateControl($(qty)) == false)
        //        return false;
        //}
        if (ValidateControl($(totalamount)) == false)
            return false;

        if (isNumberKey($(totalamount)) == false)
            return false;
        if (isNumberKey($(qty)) == false)
            return false;

        

        subscriptionbyadmin = {

            planId: planid,
            qty: $(qty).val(),
            chequeNo: $(chqueno).val(),
            totalamount: $(totalamount).val(),
            cabofficeid: $(ddlcaboofice).val()
        }


        $.ajax({
            type: "POST",
            url: servicePath + "/Subscription/InsertSubscriptionbyAdmin",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(subscriptionbyadmin),
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

    });



}



