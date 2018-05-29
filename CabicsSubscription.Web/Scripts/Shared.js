var shared = new function () {

    var data = GetParameterValues('data');

    localStorage.setItem("accounttoken", data);

    var subs = "#subs";
    var dvlink = "#dvlink";

    this.InitalizeEvents = function () {

        shared.ShowCurrentSubscription();

        var dashboardLink = "<a class='active' href=Dashboard?data=" + data + ">Dashboard</a>";
        var viewPlanLink = "<a class='active' href=ViewPlan?data=" + data + ">View Plan</a>";
        var textLocalConfigurationHtml = "<a href=TextLocalConfiguration?data=" + data + ">TextLocal Configuration</a>";
        var menu = dashboardLink + " | " + viewPlanLink + " | " + textLocalConfigurationHtml;


        $(dvlink).append(menu);

        
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

    this.ShowCurrentSubscription = function () {

        if (data == 'undefined')
            return;

        $.ajax({
            type: "Get",
            url: servicePath + "/Subscription/ShowCurrentSubscription?userguid=" + data,
            //data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                console.log(data);

                var subscriptiongrid = '<table class="table table-bordered table-striped">';
                subscriptiongrid += '<thead>';
                subscriptiongrid += '<tr>';
                subscriptiongrid += '<th>Plan Name</th>';
                subscriptiongrid += '<th>Subscription Type</th>';
                subscriptiongrid += '<th>Total Price</th>';
                subscriptiongrid += '<th>Total Credit</th>';
                subscriptiongrid += '<th>Remaining Credit</th>';
                subscriptiongrid += '<th>Start Date</th>';
                subscriptiongrid += '<th>End Credit</th>';
                subscriptiongrid += '<th>Purchse Date</th>';
                subscriptiongrid += '</tr>';

               

                $.each(data, function (index, value) {
                 
                    console.log(value);

                    localStorage.setItem("accountId", value.AccountId);

                    var subscriptionname = "";
                    if (value.SubscriptionTypeId == 1)
                        subscriptionname = "Pay as you go";
                    else if (value.SubscriptionTypeId == 2)
                        subscriptionname = "Monthly";

                    subscriptiongrid += '<tbody><tr><td>' + value.PlanName + '</td><td>' + subscriptionname + '</td>';
                    subscriptiongrid += '<td>' + value.TotalPrice + '</td><td>' + value.TotalCredit + '</td><td>' + value.RemainingCredit + '</td><td>' + value.StartDate + '</td><td>' + value.EndDate + '</td>';
                    subscriptiongrid += '<td>' + value.CreatedDateTime + '</td></tr ></tbody> ';

                });

                $("#dvsubscriptiongrid").append(subscriptiongrid);
                $("#divLoader").css("display", "none");


                var html = "";
                if (data[0].IsActive == 1) {
                   
                    //html = '<h3> Your current subscription is Active. </h3>';
                    //html += '<br />  Total Credit ' + data.TotalCredit + '';
                    //html += '<br />  Remaining Credit ' + data.RemainingCredit + '';
                    // $("#subs").append(html);

                    var subshtml = "<a href='#' class='list-group-item'>Your current subscription is Active. <span class='badge'></span></a>";

                    var subsType = "Pay As You Go";
                    var isallow = true;
                    if (data[0].SubscriptionTypeId == 2) {

                        subshtml += "<a href='#' class='list-group-item'>Start Date <span class='badge'>" + data[0].StartDate + "</span></a>";
                        subshtml += "<a href='#' class='list-group-item'>End Date <span class='badge'>" + data[0].EndDate + "</span></a>";

                        subsType = "Monthly";

                    }
                    else {

                        subshtml += "<a href='#' class='list-group-item'>Total Credit <span class='badge'>" + data[0].TotalCredit + "</span></a>";
                        

                        if (data[0].RemainingCredit == 0) {
                            isallow = false;
                            subshtml += "<a href='#' class='list-group-item' style='color:red;'>Remaimimg Credit  <span class='badge' style='color:red;' >" + data[0].RemainingCredit + "</span></a>";

                        }
                        else {
                            subshtml += "<a href='#' class='list-group-item'>Remaimimg Credit  <span class='badge'>" + data[0].RemainingCredit + "</span></a>";

                        }

                       
                    }
                    $("#subsdetail").append(subshtml);

                    $("#dvsubstype").append(subsType);

                    localStorage.setItem("Subscription", 1);

                    if (isallow == false)
                          bootbox.alert("Your credit limit has finished!");

                    //var d = new Date();
                    //var strDate = d.getFullYear() + "/" + (d.getMonth() + 1) + "/" + d.getDate();

                    //alert(strDate);
                }
                else {
                    html = 'Your Current Subscription is Deactive';
                    localStorage.setItem("Subscription", 0);

                }


               

                var status = "Open";
                $("#dvstatus").append(status);

            },
            failure: function (errMsg) {
                alert(errMsg);
            }
        });
    }
}
