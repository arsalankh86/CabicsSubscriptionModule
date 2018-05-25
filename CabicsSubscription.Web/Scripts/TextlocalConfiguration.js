
var textlocalConfiguration = new function () {


    var txtApiKey = "#txtApiKey";
    var txtusername = "#txtusername";
    var txtHash = "#txtHash";
    var txtPassword = "#txtPassword";
    var cabofficeid = 0;
    var cabofficetoken = "";
    var token = "";
    var dvback = "#dvback";
 

    this.InitalizeEvents = function () {

        cabofficetoken = GetParameterValues('data');

        var linkhtml = "<h2><a href='Dashboard?data=" + cabofficetoken + "'> Back </a></h2>";
        $(dvback).append(linkhtml);

        $.ajax({
            type: "Get",
            url: servicePath + "/Configuration/GetCabOfficeTextlocalConfiguration?cabofficetoken=" + cabofficetoken,
            //data: JSON.stringify(account),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {

                if (data != null) {
                    $(txtApiKey).val(data.APIKey);
                    $(txtusername).val(data.Username);
                    $(txtHash).val(data.Hash);
                    $(txtPassword).val(data.Password);
                    cabofficeid = data.CabofficeId;
                }
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


    $(btnsubmut).click(function () {


        if (ValidateControl($("input[name=txtApiKey]")) == false)
            return false;
        if (ValidateControl($("input[name=txtusername]")) == false)
            return false;
        if (isNumberKey($("input[name=txtHash]")) == false)
            return false;
        if (isNumberKey($("input[name=txtPassword]")) == false)
            return false;
    
        var cabofficeid = localStorage.getItem("accountId");
        debugger;
        config = {

            ApiKey: $(txtApiKey).val(),
            Hash: $(txtHash).val(),
            Password: $(txtPassword).val(),
            UserName: $(txtusername).val(),
            CabofficeId: cabofficeid
        };



        $.ajax({
            type: "POST",
            url: servicePath + "/Configuration/AddOrUpdateTextLocalConfiguration",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(config),
            dataType: "json",
            complete: function () {
            },

            success: function (response) {
                alert("Updated Succesfully");

            },

            failure: function (response) {
                alert(response);
            }
        });


    });

}