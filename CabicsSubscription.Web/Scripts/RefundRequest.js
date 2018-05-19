var refundrequest = new function () {
    
    var hdntransactionid = "#hdntransactionid";
    var hdnmode = "#hdnmode";
    var dvpartialrefund = "#dvpartialrefund";



    function GetParameterValues(param) {
        var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < url.length; i++) {
            var urlparam = url[i].split('=');
            if (urlparam[0] == param) {
                return urlparam[1];
            }
        }
    }  

    this.InitalizeEvents = function () {

        $('.radio').click(function () {
           var value =$(this).val();

            $(hdnmode).val(value);

            if (value == 2)
                $(dvpartialrefund).css("display", "block");

        });

        var transactionId = GetParameterValues('tid');
        $(hdntransactionid).val(transactionId);

    }


    this.Validate = function () {

        if (ValidateControl($("input[name=txtamount]")) == false)
            return false;

        if (isNumberKey($("input[name=txtamount]")) == false)
            return false;
    }





    

}