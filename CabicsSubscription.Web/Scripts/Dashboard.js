var dashboard = new function()
{

    //bootbox.alert("Hello world!");

    var dvlink = "#dvlink";

    var data = GetParameterValues('data');

    function GetParameterValues(param) {
        var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < url.length; i++) {
            var urlparam = url[i].split('=');
            if (urlparam[0] == param) {
                return urlparam[1];
            }
        }
    }  


 
 
}