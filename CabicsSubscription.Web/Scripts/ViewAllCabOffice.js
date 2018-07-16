
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
                console.log(response);
                var html = '<table class="table"> <thead> <tr><td>Full Name</td><td>Email</td><td>Token</td><td>Subscriptions Popup</td>Subscriptions<td></td></tr></thead>'

                $.each(response, function (index, value) {

                    console.log(value);
                    html += '<tbody><tr><td>' + value.FullName + '</td><td>' + value.Email + '</td><td>' + value.Token + '</td><td>'
                    +
                        '<a data-AccountId ="' + value.Id +
                        '" data-AccountToken="' + value.Token +
                        '" data-email="' + value.Email +
                        '" onclick="ViewAllSubscription(this)">View All Sunscriptiond Popup(S)</a>'
                    +
					 
                    '</td ><td>'
					 +
                        '<a data-AccountId ="' + value.Id +                      
                        '" onclick="ViewAllSubscription2(this)">View All Sunscriptiond(S)</a>'
                    +
					'</td></tr ></tbody> ';

                });

                console.log(html);

                html += '</table>';

                $("#dvcaboffice").append(html);
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
    var email = $(obj).attr('data-email');

    window.open('ViewUserSubscription?id=' + accountId + '&token=' + accountguid + '&email=' + email);
   //window.location.href = 'ViewUserSubscription?id = ' + accountId + ' & token=' + accountguid + ' & email=' + email;



    return false;


}



ViewAllSubscription2 = function (obj) {

    var accountId = $(obj).attr('data-AccountId');
    
   window.location.href = 'ViewUserSubscription?id='+accountId;
}


