var shared = new function () {

    var data = GetParameterValues('data');

   // localStorage.setItem("accounttoken", data);

    var subs = "#subs";
    var dvlink = "#dvlink";

    this.InitalizeEvents = function () {

        shared.ShowCurrentSubscription();

        var dashboardLink = "<a class='active' href=Dashboard?data=" + data + ">Home</a>";
        var viewPlanLink = "<a class='active' href=ViewPlan?data=" + data + ">View Plan</a>";
        var textLocalConfigurationHtml = "<a href=TextLocalConfiguration?data=" + data + ">TextLocal Configuration</a>";
        var menu = dashboardLink + " | " + viewPlanLink + " | " + textLocalConfigurationHtml;


        $(dvlink).append(menu);

        $("#goBack").click(function () {
            window.history.back();
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

    this.ShowCurrentSubscription = function () {

        if (data == 'undefined')
            return;

        var status = "Open";
        $("#dvstatus").append(status);

        $.ajax({
            type: "Get",
            url: servicePath + "/Subscription/ShowCurrentSubscription?userguid=" + data,
            //data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (data == null)
                    return;
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
                subscriptiongrid += '<tbody>';
                var subshtml = "";
                var subsType = "";
                var isallow = true;
                $.each(data, function (index, value) {

                    console.log(value);

                    localStorage.setItem("accountId", value.AccountId);

                    var subscriptionname = "";
                    if (value.SubscriptionTypeId == 1)
                        subscriptionname = "Pay as you go";
                    else if (value.SubscriptionTypeId == 2)
                        subscriptionname = "Monthly";

                    var sdate = value.StartDate.split("T")[0];
                    var edate = value.EndDate.split("T")[0];
                    var cdate = value.CreatedDateTime.split("T")[0];


                    subscriptiongrid += '<tr>';
                    if (value.IsActive == true) {
                        subscriptiongrid += '<td style="background-color:#82f286;">' + value.PlanName + '</td><td style="background-color:#82f286;">' + subscriptionname + '</td>';
                        subscriptiongrid += '<td style="background-color:#82f286;">' + value.TotalPrice + '</td><td style="background-color:#82f286;">' + value.TotalCredit + '</td><td style="background-color:#82f286;">' + value.RemainingCredit + '</td><td style="background-color:#82f286;">' + sdate + '</td><td style="background-color:#82f286;">' + edate + '</td>';
                        subscriptiongrid += '<td style="background-color:#82f286;">' + cdate + '</td></tr>';

                    }
                    else {
                        subscriptiongrid += '<td ">' + value.PlanName + '</td><td ">' + subscriptionname + '</td>';
                        subscriptiongrid += '<td ">' + value.TotalPrice + '</td><td ">' + value.TotalCredit + '</td><td ">' + value.RemainingCredit + '</td><td ">' + sdate + '</td><td ">' + edate + '</td>';
                        subscriptiongrid += '<td ">' + cdate + '</td></tr>';
                    }
               
                    


                    if (value.IsActive == true) {

                        subshtml = "<a href='#' class='list-group-item'>Your current subscription is Active. <span class='badge'></span></a>";

                        subsType = "Pay As You Go";
                     
                        if (value.SubscriptionTypeId == 2) {

                            subshtml += "<a href='#' class='list-group-item'>Start Date <span class='badge'>" + sdate + "</span></a>";
                            subshtml += "<a href='#' class='list-group-item'>End Date <span class='badge'>" + edate + "</span></a>";
                            subsType = "Monthly";
                        }
                        else {

                            subshtml += "<a href='#' class='list-group-item'>Total Credit <span class='badge'>" + value.TotalCredit + "</span></a>";
                            if (value.RemainingCredit == 0) {
                                isallow = false;
                                subshtml += "<a href='#' class='list-group-item' style='color:red;'>Remaimimg Credit  <span class='badge' style='color:red;' >" + value.RemainingCredit + "</span></a>";

                            }
                            else {
                                subshtml += "<a href='#' class='list-group-item'>Remaimimg Credit  <span class='badge'>" + value.RemainingCredit + "</span></a>";
                            }
                        }
                    }
                });

                subscriptiongrid += '</tbody>';

                $("#dvsubscriptiongrid").append(subscriptiongrid);
                $("#divLoader").css("display", "none");


                var html = "";
                    $("#subsdetail").append(subshtml);

                    $("#dvsubstype").append(subsType);

                    localStorage.setItem("Subscription", 1);

                    if (isallow == false)
                          bootbox.alert("Your credit limit has finished!");
                    else {
                        html = 'Your Current Subscription is Deactive';
                        localStorage.setItem("Subscription", 0);

                    }

                 
                    //var d = new Date();
                    //var strDate = d.getFullYear() + "/" + (d.getMonth() + 1) + "/" + d.getDate();

                    //alert(strDate);
                }
        });
    }
}
