var shared = new function () {

    var account = GetParameterValues('data');

    localStorage.setItem("accounttoken", account);

    var subs = "#subs";

    this.InitalizeEvents = function () {

        shared.ShowCurrentSubscription();
        
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

        if (account == 'undefined')
            return;

        $.ajax({
            type: "Get",
            url: servicePath + "/Subscription/ShowCurrentSubscription?userguid=" + account,
            //data: JSON.stringify(account),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                console.log(data);

                var html = "";
                if (data.IsActive == 1) {

                    html = '<h3> Your current subscription is Active. </h3>';
                    html += '<br /> <h2> Total Credit ' + data.TotalCredit + '  </h2>';
                    html += '<br /> <h2> Remaining Credit ' + data.RemainingCredit + '  </h2>';

                    localStorage.setItem("Subscription", 1);
                }
                else {
                    html = 'Your Current Subscription is Deactive';
                    localStorage.setItem("Subscription", 0);
                }

                $("#subs").append(html);

            },
            failure: function (errMsg) {
                alert(errMsg);
            }
        });
    }
}
