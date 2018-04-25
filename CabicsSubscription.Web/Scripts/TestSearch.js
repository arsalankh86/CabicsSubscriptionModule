

$(function () {

    Search.InitalizeEvents();
})

var Search = new function () {

    var checkInDate = "#dtcheckIn";
    var checkOutDate = "#dtcheckOut";
    var txtAdult = "#txtAdult";
    var txtChildren = "#txtChildren";
    var txtDestination = "#txtDestination";
    var divSearchResults = "#divSearchResult";
    var txtHotelName = "#txtHotelName";
    var btnSearch = "#btnSearch";
    var divChildrenAges = "#divChildrenAges";
    var lblchildAges = "#lblchildAges";
    var txtNoOfRooms = "#txtNoOfRooms";
    this.longitude = 0;
    this.latitude = 0;
    var divModalPopUp = "#divModalPopUp";
    var divModalPopUpBody = "#divModalPopUpBody";
    var priceList = ".priceList";
    var divLoader = "#divLoader";
    this.currency = "";
    // working on children ages

    this.InitalizeEvents = function () {
        $(divLoader).css("display", "none");
        $(btnSearch).on("click", function () {

            $(divLoader).css("display", "block");
            var geocoder = new google.maps.Geocoder();
            geocoder.geocode({
                'address': $(txtDestination).val().trim()
            }, function (results, status) {
                if (status == google.maps.GeocoderStatus.OK) {

                    Search.latitude = results[0].geometry.location.lat();
                    Search.longitude = results[0].geometry.location.lng();

                    var children = new Array();
                    var paramHotel = {

                        Destination: $(txtDestination).val().trim(),
                        Name: $(txtHotelName).val().trim(),
                        CheckInDate: $(checkInDate).val().trim(),
                        CheckOutDate: $(checkOutDate).val().trim(),
                        NumberOfAdult: $(txtAdult).val().trim(),
                        NumberOfChildren: $(txtChildren).val().trim(),
                        NumberOfRooms: $(txtNoOfRooms).val().trim(),
                        Longitude: Search.longitude,
                        Latitude: Search.latitude,
                        Child: children
                    };

                    if ($(txtChildren).val() > 0) {

                        children = new Array();
                        for (var i = 1 ; i <= $(txtChildren).val().trim() ; i++) {

                           

                            var child = {
                                PersonType: 'CH',
                                Age: $("#ch" + i).val()
                            };

                            children.push(child);
                        }
                        if (children[0].Age > 0) {
                            paramHotel.Child = children;
                            paramHotel.IsChildren = true;
                        }
                        else {
                            alert("Please input children Ages");
                            return;
                        }
                    }
                    console.log("paramHotel", paramHotel);
                    console.log("children", children);
                    console.log(Configuration.GetHotelApiServerUrl() + "api/hotel/GetHotelData");
                    if (Search.ValidateField()) {
                        Search.SetBordersRedForEmptyFields();
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
                    else {
                        Search.SetBordersRedForEmptyFields();
                    }

                }
                else {
                    // alert("Something got wrong " + status);
                }
            });

        });

        $(txtChildren).focusout(function () {

            console.log($(txtChildren).val());
            $(divChildrenAges).html('');
            if ($(txtChildren).val() > 0) {

                var html = '';
                for (var i = 1 ; i <= $(txtChildren).val() ; i++) {

                    html = html + " <label>Child " + i + "</label>" +
                         " <input type='number'  min='0' value ='0' id='ch" + i + "' placeholder='Age'  class='childAge' /> </br />";
                }
                $(divChildrenAges).append(html);
            }
            $(divChildrenAges).css('display', 'block');
        });

        $(".childAgelabel").on('click', function () {


            if ($(divChildrenAges).css('display') == 'none')
                $(divChildrenAges).css('display', 'block');
            else
                $(divChildrenAges).css('display', 'none');
        });



        $(checkInDate).datepicker();
        $(checkOutDate).datepicker();





    }

    this.GetLongitudeAndLatitude = function () {


    }

    this.ValidateField = function () {


        if ($(checkInDate).val().trim() == "")
            return false;

        if ($(checkOutDate).val().trim() == "")
            return false;

        if ($(txtAdult).val().trim() == "")
            return false;

        if ($(txtDestination).val().trim() == "")
            return false;

        return true;
    }

    this.SetBordersRedForEmptyFields = function () {

        if ($(checkInDate).val().trim() == "")
            $(checkInDate).css('border-color', 'red');
        else
            $(checkInDate).css('border-color', '');

        if ($(checkOutDate).val().trim() == "")
            $(checkOutDate).css('border-color', 'red');
        else
            $(checkInDate).css('border-color', '');

        if ($(txtAdult).val().trim() == "")
            $(txtAdult).css('border-color', 'red');
        else
            $(checkInDate).css('border-color', '');

        if ($(txtDestination).val().trim() == "" && $(txtHotelName).val() == "") {
            $(txtDestination).css('border-color', 'red');
            $(txtHotelName).css('border-color', 'red');
        }
        else {
            $(txtDestination).css('border-color', '');
            $(txtHotelName).css('border-color', '');
        }
    }

    this.OnLoadSuccess = function (response) {
        console.log(response);
        Search.GenerateHtmlForHotelSearch(response.Hotels);
        $(divLoader).css("display", "none");
    }

    this.GenerateHtmlForHotelSearch = function (paramData) {
        console.log("Response Data" + paramData);
        $(divSearchResults).html('');

        for (var i = 0 ; i < paramData.length ; i++) {

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
            for (var i = 0 ; i < DailyRateArray.length - 1 ; i++) {

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
        for (var i = 0 ; i < paramData.length; i++) {

            html = html + "<table class='table table-bordered' style='background: #e7e8e8;'><tr> <td class='active' > <strong>  Room Code :</strong> " + Search.IsNullOrEmpty(paramData[i].RoomCode) + " </td>" + "</tr>" +
                           "<tr> <td class='active'> <strong>Person :</strong> " + " ( " + Search.IsNullOrEmpty(paramData[i].RoomRate[0].NumberOfAdults) + " - Adults, " + Search.IsNullOrEmpty(paramData[i].RoomRate[0].NumberOfChildren) + " - Children " + " ) " + " </td></tr>"+
            "<tr><td class='active'><strong> Room Name : </strong> " + Search.IsNullOrEmpty(paramData[i].RoomName) + " </td>  </tr>" +
            "<tr><td class='active'><strong> Room Category : </strong> " + Search.IsNullOrEmpty(paramData[i].RoomCategory) + " </td>  </tr>" +
            "</tr><td rowspan='3'><strong> Board Types Available : </strong>" + Search.GenerateRoomRateDataValues(paramData[i].RoomRate) + "</td> </tr></table> <br />";


        }

        return html;
    }




    this.GenerateRoomRateDataValues = function (paramData) {

        var html = "";

        for (var i = 0 ; i < paramData.length ; i++) {

            html = html + "<tr><td>" + Search.IsNullOrEmpty(paramData[i].BoardName) + "</td><td>" + Search.IsNullOrEmpty(paramData[i].RateClass) + "</td><td>" + Search.IsNullOrEmpty(Search.OfferHtmlGenereation(paramData[i].Offers)) + "</td><td><span class='name'>" + Search.IsNullOrEmpty(paramData[i].Net) + " " + Search.IsNullOrEmpty(Search.currency) + " <br /> </span><span class='subtext'>" + Search.IsNullOrEmpty(Search.TaxHtmlGeneration(paramData[i].Tax)) + "</span></td><td>" + Search.IsNullOrEmpty(Search.CancellationPolicyGenereation(paramData[i].CancellationPolicy)) + "</td><td><button class='btn-sm priceList'  data-BoardCode ='" + Search.IsNullOrEmpty(paramData[i].BoardCode) + "' " +
                          "data-BoardName='" + Search.IsNullOrEmpty(paramData[i].BoardName) + "' " +
                          "data-RateKey'" + Search.IsNullOrEmpty(paramData[i].RateKey) + "' ";

            var innerHtml = ""
            
            if (paramData[i].DailyRate != null) {
                for (var j = 0 ; j < paramData[i].DailyRate.length ; j++) {

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
            for (var i = 0 ; i < paramData.length; i++) {

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
            for (var i = 0 ; i < paramData.length; i++) {

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