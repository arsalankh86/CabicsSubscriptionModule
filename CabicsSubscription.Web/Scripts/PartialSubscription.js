﻿var PartialSubscription = new function () {

    var dvbuycredit = "#dvbuycredit";
    var fname = "#fname";
    var lname = "#lname";
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
 
    this.InitalizeEvents = function () {
        $(divLoader).css("display", "none");

        PartialSubscription.SetPlanValue();

        $(btnSubmit).on("click", function () {

           

            if (PartialSubscription.SetBordersRedForEmptyFields()) {
                $.ajax({
                    type: "POST",
                    url: Configuration.GetHotelApiServerUrl() + "api/hotel/GetHotelData",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(paramHotel),
                    dataType: "json",
                    success: Search.OnLoadSuccess,
                    failure: function (response) {
                        alert(response.d);
                    }
                });
            }
        });

        };

    

    function GetParameterValues(param) {
        var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < url.length; i++) {
            var urlparam = url[i].split('=');
            if (urlparam[0] == param) {
                return urlparam[1];
            }
        }
    } 

    this.SetPlanValue = function () {
       
        var id = GetParameterValues('id');
        planid = id.split("/")[0];
        account = id.split("/")[1];

        $.ajax({
            type: "Get",
            url: servicePath + "/Plan/GetPlanDetail?planId=" + planid,
            //data: JSON.stringify(account),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                console.log(data);

                var html = 'Buy Credit (' + data.Credit + ' Credit = £' + data.CreditPrice+')';
                var price = data.CreditPrice;

                if (data.PlanTypeId == 2) {
                    var monthlyplandetail = 'No of Agents:' + data.NoOfAgents;
                    monthlyplandetail += '<br /> No of Driver:' + data.NoOfDrivers;
                    monthlyplandetail += '<br/> No of Vehicles:' + data.NoOfVehicles;
                    monthlyplandetail += '<br/> Price Per SMS:' + data.PerSMSPrice;
                                       
                    $(dvmonthly).append(monthlyplandetail);
                    $(totalamount).val(data.CreditPrice)
                    $(qty).css('display', 'none');

                }

                $(dvbuycredit).text(html);
                $(lblprice).text(price);
                
            },
            failure: function (errMsg) {
                alert(errMsg);
            }
        });
    }






    

}