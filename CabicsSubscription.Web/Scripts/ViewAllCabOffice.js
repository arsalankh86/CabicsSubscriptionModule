
var ViewAllCabOffice = new function () {


    this.InitalizeEvents = function () {

        ViewAllCabOffice.GetAllCabOffice();


    }


    this.GetAllCabOffice = new function() {
        var caboffice = new Array();;

     
        $.ajax({
            type: "GET",
            url: servicePath + "/Account/GetCabOffice",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                console.log(data);
                var html = '<table border="1"><tr><td>Account Code</td><td>Full Name</td><td>Email</td><td>Token</td></tr>'

                $.each(response, function (index, value) {

                    console.log(value);
                    html += '<tr><td>' + value.AccountCode + '</td><td>' + value.FullName + '</td><td>' + value.Email + '</td><td>' + value.Token + '</td><td>'
                    +
                        '<a data-AccountId ="' + value.Id +
                        '" data-AccountToken="' + account +
                        '" onclick="ViewAllSubscription(this)">View All Sunscriptiond(S)</a>'
                    +
                    '</td ></tr > ';

                });

                console.log(html);

                html += '</table>';

                $("#dvplan").append(html);
                $("#divLoader").css("display", "none");

            
            },
            failure: function () {
                alert("Failed!");
            }
        });

    }

}


ViewAllSubscription = function (obj) {

    var accountId = $(obj).attr('data-AccountId');
    var accountguid = $(obj).attr('data-AccountToken');

    window.open('/Admin/ViewUserSubscription?id=' + accountId + '&token=' + accountguid);



    return false;


}


