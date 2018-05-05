
var addsubscriptionbyadmin = new function () {


    var ddlcaboofice = "#ddlcaboofice";
    //var myDropDownList = $('.myDropDownLisTId');

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

        var planid= GetParameterValues(id);


        if (ValidateControl($("input[name=txtplancode]")) == false)
            return false;
        if (ValidateControl($("input[name=txtplanname]")) == false)
            return false;
        if (ValidateControl($("input[name=txtplandes]")) == false)
            return false;

        //if ($(plantype).val() == 1) {
        if (ValidateControl($("input[name=txtcredit]")) == false)
            return false;
        if (ValidateControl($("input[name=txtcreditamount]")) == false)
            return false;

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




        subscription = {

            PlanCode: $(txtplancode).val(),
            Name: $(txtplanname).val(),
            PlanTypeId: $(plantype).val(),
            Description: $(txtplandes).val(),
            CreditPrice: $(txtcreditamount).val(),
            Credit: $(txtcredit).val(),
            NoOfAgents: $(txtnoofagent).val(),
            NoOfDrivers: $(txtnoofdriver).val(),
            NoOfVehicles: $(txtnoofvehicle).val(),
            PerSMSPrice: $(txtpricepersms).val()

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
                alert(response);
            },

            failure: function (response) {
                alert(response);
            }
        });


    });



}



