﻿
var addsubscriptionbyadmin = new function () {


    //var ddlcaboofice = "#ddlcaboofice";
    //var dvpayasyougo = "#dvpayasyougo";
    //var chqueno = "#chqueno";
    //var totalamount = "#totalamount";
    //var lblprice = "#lblprice";
    //var qty = "#qty";
    //var btnsubmut = "#btnsubmut";
    //var dvmonthly = "#dvmonthly";
    //var plantype = "#plantype";

    var ddlcaboofice = "#ddlcaboofice";
    var dvpayasyougo = "#dvpayasyougo";
    var chqueno = "#chqueno";
    var plantype = "#plantype";

    var dvbuycredit = "#dvbuycredit";
    var crdnumber = "#crdnumber";
    var expmonth = "#expmonth";
    var expyear = "#expyear";
    var add = "#add";
    var add2 = "#add2";
    var city = "#city";
    var state = "#state";
    var zip = "#zip";
    var qty = "#qty";
    var lblprice = "#lblprice";
    var lbltotal = "#lbltotal";
    var divLoader = "#divLoader";
    var btnSubmit = "#btnSubmit";
    var dvmonthly = "#dvmonthly";
    var totalamount = "#totalamount";
    var hdnplanid = "#hdnplanid";
    var hdnaccount = "#hdnaccount";
    var hdnamount = "#hdnamount";
    var lblsmscreditprice = "#lblsmscreditprice";
    var smscreditqty = "#smscreditqty";
    var dvcreditdeductiondetail = "#dvcreditdeductiondetail";
    var spnnoOfAgents = "#spnnoOfAgents";
    var spnnoOfDrivers = "#spnnoOfDrivers";
    var spnnoOfVehicles = "#spnnoOfVehicles";
    var spnpricePerSMS = "#spnpricePerSMS";
    var dvmonthlynew = "#dvmonthlynew";


    this.InitalizeEvents = function () {
        $(divLoader).css("display", "none");

        var planid = GetParameterValues('id');
        var account = GetParameterValues('data');

        $(hdnaccount).val(account);
        $(hdnplanid).val(planid);

        var caboffice = new Array();;


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

                    caboffice.push(response[index].FullName);


                });
                $("#caboffice").autocomplete({
                    source: caboffice
                });
            },
            failure: function () {
                alert("Failed!");
            }
        });

        $(qty).bind('input', function () {
            /* This will be fired every time, when textbox's value changes. */
            var totalprice = $(hdnamount).val();
            var credit = $(lblprice).text();

            $(totalamount).val(this.value * credit);
            $(hdnamount).val(this.value * credit);
            //alert(this.value);
        });



        $(smscreditqty).bind('input', function () {
            /* This will be fired every time, when textbox's value changes. */

            var totalprice = $(hdnamount).val();
            var lblsmscreditprice = $(hdnsmscreditamount).val();
            var smscreditqty = this.value;
            var totalsmscreditprice = lblsmscreditprice * smscreditqty;

            var amount = parseInt(totalprice) + parseInt(totalsmscreditprice);

            if (smscreditqty == "") {
                $(totalamount).val(totalprice);
                $("#hdnsmscreditotaltamount").val(0);
            }

            $(totalamount).val(amount);
            $("#hdnsmscreditotaltamount").val(totalsmscreditprice);

            //  $(hdnamount).val(amount);

        });


        $(document).on('change', 'input[Id="chkautorenewel"]', function (e) {
            //alert($(this).val());
            // $("#dvbillingcycle").css('display', 'block');
        });



        

    


            $.ajax({
                type: "Get",
                url: servicePath + "/Plan/GetPlanDetail?planId=" + planid,
                //data: JSON.stringify(account),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    console.log(data);


                    var html = 'Buy Credit (' + data.Credit + ' Credit = £' + data.CreditPrice + ')';
                    var price = data.CreditPrice;

                    if (data.PlanTypeId == 2) {
                        var monthlyplandetail = 'No of Agents:' + data.NoOfAgents;
                        monthlyplandetail += '<br /> No of Driver:' + data.NoOfDrivers;
                        monthlyplandetail += ' No of Vehicles:' + data.NoOfVehicles;
                        monthlyplandetail += ' SMS Credit Price :' + data.PerCreditSMSPrice;
                        html = 'Buy Credit (' + data.Credit + ' Month = £' + data.CreditPrice + ')';;

                        $(dvmonthly).append(monthlyplandetail);
                        $(dvmonthlynew).css('display', 'block');
                        $(spnnoOfAgents).text(data.NoOfAgents);
                        $(spnnoOfDrivers).text(data.NoOfDrivers);
                        $(spnnoOfVehicles).text(data.NoOfVehicles);
                        $(spnpricePerSMS).text(data.PerCreditSMSPrice);

                        $(hdnamount).val(data.CreditPrice);
                        $(totalamount).val(data.CreditPrice);
                        $(hdnsmscreditamount).val(data.PerCreditSMSPrice);
                        $(lblsmscreditprice).text(data.PerCreditSMSPrice);
                        $(qty).css('display', 'none');
                        $("#dvAutoRenewel").css('display', 'block');
                        $("#chkautorenewel").attr("checked", true);

                    }
                    else {

                        $(dvsmscreditqty).css('display', 'none');
                        $("#chkautorenewel").attr("checked", false);
                    }

                    $(dvbuycredit).text(html);
                    $(lblprice).text(price);

                },
                failure: function (errMsg) {
                    alert(errMsg);
                }
            });
        

            $.ajax({
                type: "Get",
                url: servicePath + "/Subscription/GetAllCreditDeductionDetail",
                //data: JSON.stringify(account),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    console.log(data);

                    var html = '<h3> Credit Deduction Detail is: </h3>';
                    $.each(data, function (i) {
                        html += '' + data[i].Name + ' ' + data[i].Credit;
                        html += '<br />';
                    });


                    $(dvcreditdeductiondetail).append(html);

                },
                failure: function (errMsg) {
                    alert(errMsg);
                }
            });
        


    };



    //// No Changed
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

        var planid = GetParameterValues('id');

        var chkautorenewelval = "off";
        if ($(chkautorenewel).is(':checked'))
            chkautorenewelval = "on";

        subscriptionbyadmin = {

            planId: planid,
            qty: $(qty).val(),
            chequeNo: $(chqueno).val(),
            totalamount: $(totalamount).val(),
            cabofficeid: $(ddlcaboofice).val(),
            hdnamount: $(totalamount).val(),
            smscreditqty: $(smscreditqty).val(),
            hdnsmscreditamount: $(hdnsmscreditamount).val(),
            chkautorenewel: chkautorenewelval
        }


        $.ajax({
            type: "POST",
            url: webUrl + "/Admin/InsertSubscriptionbyAdmin",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(subscriptionbyadmin),
            dataType: "json",
            complete: function () {

            },

            success: function (response) {
                //alert(response);
                window.location.href = "/Admin/Dashboard";
            },

            failure: function (response) {
                alert(response);
            }
        });

        
        

    });



}



