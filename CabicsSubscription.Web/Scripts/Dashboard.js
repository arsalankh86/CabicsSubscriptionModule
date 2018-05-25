var dashboard = new function()
{

    var planlink = "#dvlink";
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


    var html = "<h3><a href=ViewPlan?data=" + data + ">View Plan</a></h3> <br/>";
    $(planlink).append(html);
 
    
    var textLocalConfigurationHtml = "<h3><a href=TextLocalConfiguration?data=" + data + ">TextLocal Configuration</a></h3> <br/>";
    $(dvlink).append(textLocalConfigurationHtml);

    debugger;

if(typeof(EventSource) !== "undefined") {
    var source = new EventSource("https://www.w3schools.com/html/demo_sse.php");
        source.onmessage = function(event) {
            document.getElementById("result").innerHTML += event.data + "<br>";
        };
    } else {
    document.getElementById("result").innerHTML = "Sorry, your browser does not support server-sent events...";
}
    
   

}