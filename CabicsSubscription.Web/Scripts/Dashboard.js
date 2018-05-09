var dashboard = new function()
{

    var planlink = "#dvlink";

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


    var html = "<h3><a href=ViewPlan?data=" + data + ">View Plan</a></h3> <br/>";
    $(planlink).append(html);
    
    
   

}