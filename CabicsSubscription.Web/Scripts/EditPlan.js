
var editplan = new function () {

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


    var dvpayasgo = "#dvpayasgo";
    var dvmonthly = "#dvmonthly";
    var btnsubmut = "#btnsubmut";



  

    this.InitalizeEvents = function () {
        



        $("#planexpirydate").datepicker();

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

            var planId = GetParameterValues('planid');

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

            };



            $.ajax({
                type: "POST",
                url: servicePath + "/Plan/EditPlan",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(plan),
                dataType: "json",               
                complete: function () {
                },

                success: function (response) {
                        window.location.href = webUrl + "/Admin/Viewplan";
                },

                failure: function (response) {
                    alert(response);
                }
            });


        });


    };

    this.GetPlanById = new function()
    {

        var planId = GetParameterValues('planid');

        $.ajax({
            type: "Get",
            url: servicePath + "/Plan/GetAllPlanByPlanId?planid="+planId,
            //data: JSON.stringify(account),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                console.log(data);

                $(txtplancode).val(data.PlanCode);
                $(txtplanname).val(data.Name);
                $(txtplandes).val(data.Description);
                $(txtcreditamount).val(data.CreditPrice);
                $(txtnoofagent).val(data.NoOfAgents);
                $(txtnoofdriver).val(data.NoOfDrivers);
                $(txtnoofvehicle).val(data.NoOfVehicles);
                $(txtpricepersms).val(data.PerCreditSMSPrice);

                $(planexpirydate).val(data.PlanExpiryDate);
                $(plantype).val(data.PlanTypeId);

                

            },
            failure: function (errMsg) {
                alert(errMsg);
            }
        });

    }

    function GetParameterValues(param) {
        var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < url.length; i++) {
            var urlparam = url[i].split('=');
            if (urlparam[0] == param) {
                return urlparam[1];
            }
        }
    }


}



