var PartialSubscription = new function () {

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
                var price = '£'+data.CreditPrice;

                $(dvbuycredit).text(html);
                $(lblprice).text(price);
                
            },
            failure: function (errMsg) {
                alert(errMsg);
            }
        });
    }

    this.SetBordersRedForEmptyFields = function () {

        if ($(fname).val().trim() == "") {
            $(fname).css('border-color', 'red');
            return false;
        }
        else
            $(fname).css('border-color', '');

        if ($(lname).val().trim() == "") {
            $(lname).css('border-color', 'red');
            return false;
        }
        else
            $(lname).css('border-color', '');

        if ($(crdnumber).val().trim() == "") {
            $(crdnumber).css('border-color', 'red');
            return false;
        }
        else
            $(crdnumber).css('border-color', '');

        if ($(expmonth).val().trim() == "") {
            $(expmonth).css('border-color', 'red');
            return false;
        }
        else
            $(expmonth).css('border-color', '');

        if ($(expyear).val().trim() == "") {
            $(expyear).css('border-color', 'red');
            return false;
        }
        else
            $(expyear).css('border-color', '');

        if ($(add).val().trim() == "") {
            $(add).css('border-color', 'red');
            return false;
        }
        else
            $(add).css('border-color', '');

        if ($(city).val().trim() == "") {
            $(city).css('border-color', 'red');
            return false;
        }
        else
            $(city).css('border-color', '');

        if ($(state).val().trim() == "") {
            $(state).css('border-color', 'red');
            return false;
        }
        else
            $(state).css('border-color', '');


        if ($(qty).val().trim() == "") {
            $(qty).css('border-color', 'red');
            return false;
        }
        else
            $(qty).css('border-color', '');


        if ($(zip).val().trim() == "") {
            $(zip).css('border-color', 'red');
            return false;
        }
        else
            $(zip).css('border-color', '');

        return true;
    }

    this.OnLoadSuccess = function (response) {
        console.log(response);
        Search.GenerateHtmlForHotelSearch(response.Hotels);
        $(divLoader).css("display", "none");
    }

    this.GenerateHtmlForHotelSearch = function (paramData) {
        console.log("Response Data" + paramData);
        $(divSearchResults).html('');

        for (var i = 0; i < paramData.length; i++) {

            Search.currency = paramData[i].Currency;
            var html = "<div><h3>" + Search.IsNullOrEmpty(paramData[i].HotelName) + "</h3>" + " <h5>" + Search.IsNullOrEmpty(paramData[i].CategoryName) + "</h5>" +
                "<p>" + Search.IsNullOrEmpty(paramData[i].DestinationName) + "</p>" +
                "<div class='container'>" +
                "<div class='row'>" +
                "<div class='col-md-12'>" +
                Search.GenerateHtmlForRoom(paramData[i].Room) +
                "</div>" +
                "</div>" +
                "</div></div>";


            $(divSearchResults).append(html);
        }

        $(priceList).on("click", function () {

            var BoardCode = $(this).attr("data-BoardCode");
            var BoardName = $(this).attr("data-BoardName");
            var DailyRate = $(this).attr("data-dailyRate");

            var DailyRateArray = DailyRate.split('!');

            var html = "";
            for (var i = 0; i < DailyRateArray.length - 1; i++) {

                var dailyRateObject = DailyRateArray[i].split(',');

                html = html + "<tr><td>" + dailyRateObject[1] + "</td><td class='price'>" + Search.IsNullOrEmpty(dailyRateObject[0]) + "</td></tr>";

            }

            html = "<table class='table'> <thead><tr> <th> <strong> Date </strong> </th> <th><strong> Amount  </strong></th> </tr></thead>" + html + "</table>";
            html = "<h3>" + Search.IsNullOrEmpty(BoardName) + "</h3> <br />" + html;

            $(divModalPopUpBody).html(html);
            $(divModalPopUp).modal('show');
            console.log($(this).attr("data-BoardCode"));
        })

    }

    this.GenerateHtmlForRoom = function (paramData) {

        var html = "";
        for (var i = 0; i < paramData.length; i++) {

            html = html + "<table class='table table-bordered' style='background: #e7e8e8;'><tr> <td class='active' > <strong>  Room Code :</strong> " + Search.IsNullOrEmpty(paramData[i].RoomCode) + " </td>" + "</tr>" +
                "<tr> <td class='active'> <strong>Person :</strong> " + " ( " + Search.IsNullOrEmpty(paramData[i].RoomRate[0].NumberOfAdults) + " - Adults, " + Search.IsNullOrEmpty(paramData[i].RoomRate[0].NumberOfChildren) + " - Children " + " ) " + " </td></tr>" +
                "<tr><td class='active'><strong> Room Name : </strong> " + Search.IsNullOrEmpty(paramData[i].RoomName) + " </td>  </tr>" +
                "<tr><td class='active'><strong> Room Category : </strong> " + Search.IsNullOrEmpty(paramData[i].RoomCategory) + " </td>  </tr>" +
                "</tr><td rowspan='3'><strong> Board Types Available : </strong>" + Search.GenerateRoomRateDataValues(paramData[i].RoomRate) + "</td> </tr></table> <br />";


        }

        return html;
    }




    this.GenerateRoomRateDataValues = function (paramData) {

        var html = "";

        for (var i = 0; i < paramData.length; i++) {

            html = html + "<tr><td>" + Search.IsNullOrEmpty(paramData[i].BoardName) + "</td><td>" + Search.IsNullOrEmpty(paramData[i].RateClass) + "</td><td>" + Search.IsNullOrEmpty(Search.OfferHtmlGenereation(paramData[i].Offers)) + "</td><td><span class='name'>" + Search.IsNullOrEmpty(paramData[i].Net) + " " + Search.IsNullOrEmpty(Search.currency) + " <br /> </span><span class='subtext'>" + Search.IsNullOrEmpty(Search.TaxHtmlGeneration(paramData[i].Tax)) + "</span></td><td>" + Search.IsNullOrEmpty(Search.CancellationPolicyGenereation(paramData[i].CancellationPolicy)) + "</td><td><button class='btn-sm priceList'  data-BoardCode ='" + Search.IsNullOrEmpty(paramData[i].BoardCode) + "' " +
                "data-BoardName='" + Search.IsNullOrEmpty(paramData[i].BoardName) + "' " +
                "data-RateKey'" + Search.IsNullOrEmpty(paramData[i].RateKey) + "' ";

            var innerHtml = ""

            if (paramData[i].DailyRate != null) {
                for (var j = 0; j < paramData[i].DailyRate.length; j++) {

                    innerHtml = innerHtml + Search.IsNullOrEmpty(paramData[i].DailyRate[j].Amount) + "," + Search.IsNullOrEmpty(paramData[i].DailyRate[j].Date) + "!";

                }
            }

            html = html + "data-dailyRate='" + innerHtml + "'> Price Details" + "</button></td></tr>";


        }
        return "<table><thead><tr><th> Board Type</th><th>Rate Type </th><th> Offers </th> <th> Net Amount</th><th> Cancellation Policy </th> <th> Detail </th> </tr><thead>" + html + "</table>";
    }

    this.TaxHtmlGeneration = function (paramData) {

        if (paramData != null) {

            var html = "";

            html = Search.IsNullOrEmpty(paramData.TaxAmount) + " " + Search.IsNullOrEmpty(paramData.TaxCurrency);
            if (paramData.IsTaxIncluded == true)
                return "Tax : " + html + " ( Included )";
            else
                return "Tax : " + html + " ( Not Included )";

        }
        else
            return " ";
    }

    this.CancellationPolicyGenereation = function (paramData) {
        if (paramData != null) {

            var html = "";
            for (var i = 0; i < paramData.length; i++) {

                html = html + "<strong> Canceltion Amount : </strong>" + Search.IsNullOrEmpty(paramData[i].CancellationAmount) + "<br />" + "<strong> Date From : </strong>" + Search.IsNullOrEmpty(paramData[i].Date) + "<br /><br />";
            }
            return html;
        }
        else {
            return " ";
        }
    }

    this.OfferHtmlGenereation = function (paramData) {

        if (paramData != null) {

            var html = "";
            for (var i = 0; i < paramData.length; i++) {

                html = html + "<strong>Offer Code : </strong>" + Search.IsNullOrEmpty(paramData[i].OfferCode) + "<br />" +
                    "<strong>Offer Name : </strong>" + Search.IsNullOrEmpty(paramData[i].OfferName) + "<br />" +
                    "<strong>Discount Amount : </strong>" + Search.IsNullOrEmpty(paramData[i].DiscountAmount) + " " + Search.IsNullOrEmpty(Search.currency) + " <br /><br />";

            }
            return html;
        }
        else {
            return " ";
        }
    }

    this.IsNullOrEmpty = function (value) {

        if (value != null)
            return value;

        else
            return " ";


    }

}