$(function () {
    
    $("#txtSearch").autocomplete({
        source: function (request, response) {
            $.getJSON("/track/GetTrackTitles?query=" + request.term, function (data) {
                response(data);
            });
        },
        minLength: 3,
        delay: 500,
        focus: function () {
            return false;
        },
    });
});



