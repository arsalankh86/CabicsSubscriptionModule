﻿
var addplan = new function () {

    var plantype = "#plantype";
    var txtplancode = "#txtplancode";
    var txtplanname = "#txtplanname";
    var txtplandes = "#txtplandes";
    var txtcredit = "#txtcredit";
    var txtcreditamount = "#txtcreditamount";
    var txtnoofagent = "#txtnoofagent";
    var txtnoofvehicle = "#txtnoofvehicle";
    var txtnoofdriver = "#txtnoofdriver";
    var txtpricepersms = "#txtpricepersms";
    var planexpirydate = "#planexpirydate";
    var txtbraintreeplan = "#txtbraintreeplan";
    
    var dvpayasgo = "#dvpayasgo";
    var dvmonthly = "#dvmonthly";
    var btnsubmut = "#btnsubmut";


  

    this.InitalizeEvents = function () {
        
        $("#planexpirydate").datepicker({
            dateFormat: 'yy-mm-dd'
        });

        $(plantype).on("change", function () {

            var selectedplantype = $(plantype).val();
            if (selectedplantype == 2) {
                $(dvmonthly).css('display', 'block');
              //  $(dvpayasgo).css('display', 'none');
            }
            else {
                $(dvpayasgo).css('display', 'block');
                $(dvmonthly).css('display', 'none');
            }

        });


        $(btnsubmut).click(function () {


            if (ValidateControl($("input[name=txtplancode]")) == false)
                return false;
            if (ValidateControl($("input[name=txtplanname]")) == false)
                return false;
            if (ValidateControl($("input[name=txtplandes]")) == false)
                return false;

            //if ($(plantype).val() == 1) {
                //if (ValidateControl($("input[name=txtcredit]")) == false)
                //    return false;
                //if (ValidateControl($("input[name=txtcreditamount]")) == false)
                //    return false;

                if (isNumberKey($("input[name=txtcreditamount]")) == false)
                    return false;
                if (isNumberKey($("input[name=txtcredit]")) == false)
                    return false;
            //}

            if ($(plantype).val() == 2) {

                if (ValidateControl($("input[name=txtnoofdriver]")) == false)
                    return false;
                if (ValidateControl($("input[name=txtnoofagent]")) == false)
                    return false;
                if (ValidateControl($("input[name=txtnoofvehicle]")) == false)
                    return false;
                if (ValidateControl($("input[name=txtpricepersms]")) == false)
                    return false;

                if (isNumberKey($("input[name=txtnoofagent]")) == false)
                    return false;
                if (isNumberKey($("input[name=txtnoofvehicle]")) == false)
                    return false;
                if (isNumberKey($("input[name=txtnoofdriver]")) == false)
                    return false;
                if (isNumberKey($("input[name=txtpricepersms]")) == false)
                    return false;

            }
         



            plan = {

                PlanCode : $(txtplancode).val(),
                Name:$(txtplanname).val(),
                PlanTypeId: $(plantype).val(),
                Description:$(txtplandes).val(),
                CreditPrice:$(txtcreditamount).val(),
                Credit:1,
                NoOfAgents:$(txtnoofagent).val(),
                NoOfDrivers:$(txtnoofdriver).val(),
                NoOfVehicles:$(txtnoofvehicle).val(),
                PerSMSPrice: $(txtpricepersms).val(),
                PlanExpiryDate: $(planexpirydate).val(),
                BrainTreePlan: $(txtbraintreeplan).val()

            };



            $.ajax({
                type: "POST",
                url: servicePath + "/Plan/InsertPlan",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(plan),
                dataType: "json",               
                complete: function () {
                },

                success: function (response) {
                    if (response == -1001) { alert("Plan code alredy exist"); return false; }
                    if (response == -1002) { alert("Pay as you go plan already exist"); return false; }
                    else {
                        window.location.href = webUrl + "/Admin/Viewplan";
                    }

                },

                failure: function (response) {
                    alert(response);
                }
            });


        });


    };


}



