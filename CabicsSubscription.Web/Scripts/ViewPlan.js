﻿$(document).ready(function () {


    var account = GetParameterValues('data');

    localStorage.setItem("accounttoken", account);

    function GetParameterValues(param) {
        var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < url.length; i++) {
            var urlparam = url[i].split('=');
            if (urlparam[0] == param) {
                return urlparam[1];
            }
        }
    }  

    FetchPlanForView(account);

    function FetchPlanForView(account) {

        $.ajax({
            type: "Get",
            url: servicePath + "/Plan/GetAllPlanForCabOffice",
            //data: JSON.stringify(account),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                console.log(data);
                var html = '<table class="table table-bordered table-striped"> <thead><tr><td>Plan Name</td><td>Plan Code</td><td>Description</td><td>Amount</td><td>Credit</td><td>Purchase</td></tr></thead><tbody>'

                $.each(data, function (index, value) {
                    console.log(value);

                    var html1 =  "<div class='plan-item'>";
                    html1 += "<div class='plan-box'>";
                    html1 += "        <h2>" + value.Name + "</h2>";
                    html1 += "<div class='amount'>";
                    html1 += "  <h6>Amount</h6>";
                    html1 += "  <h3><b>£</b>" + value.CreditPrice +"</h3>";
                    html1 += "  <ul>";
                    html1 += "      <li>";
                    html1 += "          <span>Plan Code</span>";
                    html1 += "          " + value.PlanCode + "";
                    html1 += "  </li>";
                    html1 += "      <li>";
                    html1 += "          <span>Credit</span>";
                    html1 += "          " + value.Credit +"";
                    html1 += "  </li>";
                    html1 += "  </ul>";
                    html1 += " </div>";
                    html1 += "<div class='description'>";
                    html1 += "  <h4>Description</h4>";
                    html1 += "  <p>" + value.Description+"</p>";
                    html1 += "</div>";
                    html1 += " <div class='footer-btns'>";
                    html1 += "  <button class=''>Delete</button>";
                    html1 += "  <button class=''><a href='purchase-plan-01.html'>Purchase</a></button>";
                    html1 += "</div>";
                    html1 += "</div>";
                    html1 += "</div>";

                    html += '<tr><td>' + value.Name + '</td><td>' + value.PlanCode + '</td><td>' + value.Description + '</td><td>' + value.CreditPrice + '</td><td>' + value.Credit + '</td><td>'
                    +
                        '<a data-PlanId ="' + value.Id +
                        '" data-AccountId="' + account +
                        '" onclick="ShowDetail(this)">Select Plan(S)</a>'
                    +
                    '</td ></tr > ';

                });
                console.log(html);

                html += '</tbody></table>';

                $("#dvplan").append(html1);
                $("#divLoader").css("display", "none");

               // window.location.href = webUrl + "/Home/Index?data=" + data;


            },
            failure: function (errMsg) {
                alert(errMsg);
            }
        });

    }

    ShowDetail = function (obj) {

        var PlanId = $(obj).attr('data-PlanId');
        var accountguid = $(obj).attr('data-AccountId');

		window.location.href = webUrl + "/Home/PurchaseSubscription?id=" + PlanId + '&account=' + accountguid;
        //window.open('/Home/PurchaseSubscription?id=' + PlanId + '&account=' + accountguid);

        return false;
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


});
