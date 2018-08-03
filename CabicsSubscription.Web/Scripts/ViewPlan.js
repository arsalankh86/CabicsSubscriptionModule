$(document).ready(function () {


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
                var html = '<table class="table table-bordered table-striped"> <thead><tr><td>Plan Name</td><td>Plan Code</td><td>Description</td><td>Amount</td><td>Credit</td><td>Delete</td><td>Purchase For Cab Office</td><td>Edit Plan</td></tr></thead><tbody>'

                var html1 = "";
                var html2 = "";
                $.each(data, function (index, value) {
                    console.log(value);

                    if (value.PlanTypeId == 1) {
                        html1 += "<div class='plan-item'>";
                        html1 += "<div class='plan-box'>";
                        html1 += "        <h2>" + value.Name + "</h2>";
                        html1 += "<div class='amount'>";
                        html1 += "  <h6>Amount</h6>";
                        html1 += "  <h3><b>£</b>" + value.CreditPrice + "</h3>";
                        html1 += "  <ul>";
                        html1 += "      <li>";
                        html1 += "          <span>Plan Code</span>";
                        html1 += "          " + value.PlanCode + "";
                        html1 += "  </li>";
                        html1 += "      <li>";
                        html1 += "          <span>Credit</span>";
                        html1 += "          " + value.Credit + "";
                        html1 += "  </li>";
                        html1 += "  </ul>";
                        html1 += " </div>";
                        html1 += "<div class='description'>";
                        html1 += "  <h4>Description</h4>";
                        html1 += "  <p>" + value.Description + "</p>";
                        html1 += "</div>";
                        html1 += " <div class='footer-btns full-btn'>";
                        html1 += "  <button data-PlanId ='" + value.Id + "'  data-AccountId='" + account +"' onclick='PurchseForCabOffice(this)' class=''>Purchase Plan</button>";
                        html1 += "</div>";
                        html1 += "</div>";
                        html1 += "</div>";
                    }
                    else if (value.PlanTypeId == 2) {
                        html2 += "<div class='plan-item'>";
                        html2 += "<div class='plan-box'>";
                        html2 += "        <h2>" + value.Name + "</h2>";
                        html2 += "<div class='amount'>";
                        html2 += "  <h6>Amount</h6>";
                        html2 += "  <h3><b>£</b>" + value.CreditPrice + "</h3>";
                        html2 += "  <ul>";
                        html2 += "      <li>";
                        html2 += "          <span>Plan Code</span>";
                        html2 += "          " + value.PlanCode + "";
                        html2 += "  </li>";
                        html2 += "      <li>";
                        html2 += "          <span>Credit</span>";
                        html2 += "          Unlimited Credit";
                        html2 += "  </li>";
                        html2 += "  </ul>";
                        html2 += " </div>";
                        html2 += "<div class='description'>";
                        html2 += "  <h4>Description</h4>";
                        html2 += "  <p>" + value.Description + "</p>";
                        html2 += "</div>";
                        html2 += " <div class='footer-btns full-btn'>";
                        html2 += "  <button data-PlanId ='" + value.Id + "' data-AccountId='" + account +"' onclick='PurchseForCabOffice(this)' class=''>Purchase Plan</button>";
                        html2 += "</div>";
                        html2 += "</div>";
                        html2 += "</div>";
                    }

                });
                console.log(html);

                html += '  </tbody></table>';

                $("#dvplanPayAsYouGo").append(html1);
                $("#dvplanMonthly").append(html2);
                $("#divLoader").css("display", "none");

                // window.location.href = webUrl + "/Home/Index?data=" + data;


            },
            failure: function (errMsg) {
                alert(errMsg);
            }
        });

    }

    PurchseForCabOffice = function (obj) {

        var PlanId = $(obj).attr('data-PlanId');
        var accountguid = $(obj).attr('data-AccountId');

		window.location.href = webUrl + "/Home/PurchaseSubscription?id=" + PlanId + '&data=' + accountguid;
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
